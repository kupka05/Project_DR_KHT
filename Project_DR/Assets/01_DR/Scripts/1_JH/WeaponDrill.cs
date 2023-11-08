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


    void Start()
    {
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
        if(50 < lerpSpeed)
            lerpSpeed = 50;
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

}
