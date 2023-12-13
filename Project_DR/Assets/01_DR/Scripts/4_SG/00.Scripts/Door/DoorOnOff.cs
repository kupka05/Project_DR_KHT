using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOnOff : MonoBehaviour
{


    public Vector3 defaultV3;
    public Vector3 targetV3;

    private bool compleateDoorOn;
    private bool compleateDoorOff;

    private float doorOnTime;
    private float doorOffTime;


    private void Awake()
    {
        AwakeInIt();
    }

    void Start()
    {
        StartInIt();
    }


    private void AwakeInIt()
    {
        defaultV3 = transform.position;
        targetV3 = new Vector3(defaultV3.x, defaultV3.y * 2, defaultV3.z);

        compleateDoorOn = false;
        compleateDoorOff = false;
        doorOnTime = 5f;
        doorOffTime = 8f;
    }       // AwakeInIt()

    private void StartInIt()
    {
        GameManager.instance.DoorOnEvent += OnDoor;
        GameManager.instance.DoorOffEvent += OffDoor;
    }       // StartInIt()

    public void OnDoor()
    {
        StartCoroutine(OnDoorCoroutine());
    }

    public void OffDoor()
    {
        StartCoroutine(OffDoorCoroutine());
    }


    private void OnDestroy()
    {
        GameManager.instance.DoorOnEvent -= OnDoor;
        GameManager.instance.DoorOffEvent -= OffDoor;
    }

    IEnumerator OnDoorCoroutine()
    {
        while (!compleateDoorOn)
        {
            transform.position = Vector3.Lerp(transform.position, targetV3, doorOnTime * Time.deltaTime);
            if (transform.position == targetV3)
            {
                compleateDoorOn = true;
            }
            yield return null;
        }
        compleateDoorOn = false;
    }       // OnDoorCoroutine()

    IEnumerator OffDoorCoroutine()
    {
        while (!compleateDoorOff)
        {
            transform.position = Vector3.Lerp(transform.position, defaultV3, doorOffTime * Time.deltaTime);
            if(transform.position == defaultV3)
            {
                compleateDoorOff = true;
            }
            yield return null;
        }
        compleateDoorOff = false;
    }       //  OffDoorCoroutine()

}
