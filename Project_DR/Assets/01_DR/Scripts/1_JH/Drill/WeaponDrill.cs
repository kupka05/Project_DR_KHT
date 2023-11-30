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

    IEnumerator spinRoutine;


    void Start()
    {
        GetData();
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
        if(spinRoutine == null)
        {
            spinRoutine = DrillSpin();
            StartCoroutine(spinRoutine);
        }

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

            if(lerpSpeed <= 0.1f)
            {
                yield break;
            }
        }
    }
    public void ResetDrill()
    {
        transform.localRotation = Quaternion.identity;
        lerpSpeed = 0;
        StopAllCoroutines();
        if(spinRoutine !=null)
        {
            spinRoutine = DrillSpin();
            StartCoroutine(spinRoutine);
        }
    }
    private void GetData()
    {
        addSpeed = (float)DataManager.instance.GetData(1100, "SpiralForce", typeof(float));
        maxSpeed = (float)DataManager.instance.GetData(1100, "MaxSpiralSpeed", typeof(float));
    }
}
