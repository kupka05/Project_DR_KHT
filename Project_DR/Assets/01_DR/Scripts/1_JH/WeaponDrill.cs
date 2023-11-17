using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class WeaponDrill : MonoBehaviour
{
    private bool isDrillOn;

    [SerializeField]
    [Range(0, 50)]
    private float lerpSpeed = 0f;    
    public float addSpeed;
    private float maxSpeed;



    void Start()
    {
        GetData();

        StartCoroutine("DrillSpin");
    }
    private void Update()
    {
        if(0 < lerpSpeed)
        {
            lerpSpeed -= 20 * Time.deltaTime;
        }
    }

    public void OnSpin()
    {
        lerpSpeed += addSpeed;
        if(maxSpeed < lerpSpeed)
            lerpSpeed = maxSpeed;
    }
    IEnumerator DrillSpin()
    {
        while (true)
        {
            float time = 0;

            while (time < 1)
            {
                transform.localRotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, -180, 0), time);
                time += Time.deltaTime * lerpSpeed;
                yield return null;
            }

            time = 0;
            while (time < 1)
            {
                transform.localRotation = Quaternion.Slerp(Quaternion.Euler(0, -180, 0), Quaternion.Euler(0, -360, 0), time);
                time += Time.deltaTime * lerpSpeed;
                yield return null;
            }
        }
    }
    public void ResetDrill()
    {
        transform.localRotation = Quaternion.identity;
        lerpSpeed = 0;
        StopAllCoroutines();
        StartCoroutine("DrillSpin");

    }
    private void GetData()
    {
        addSpeed = (float)DataManager.GetData(1100, "SpiralForce");
        maxSpeed = (float)DataManager.GetData(1100, "MaxSpiralSpeed");
    }
}
