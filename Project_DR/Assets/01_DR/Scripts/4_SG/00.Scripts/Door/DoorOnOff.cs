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

    BoxCollider boxCollider;

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

        boxCollider = this.GetComponent<BoxCollider>();
        AudioManager.Instance.AddSFX("SFX_Stage_Door_Open_01");
        AudioManager.Instance.AddSFX("SFX_Stage_Door_Close_01");
        AudioManager.Instance.AddSFX("SFX_Stage_Door_Close_02");
    }       // AwakeInIt()

    private void StartInIt()
    {
        GameManager.instance.DoorOnEvent += OnDoor;
        GameManager.instance.DoorOffEvent += OffDoor;
    }       // StartInIt()

    public void OnDoor()
    {
        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(OnDoorCoroutine());
        }
        else { /*PASS*/ }
    }

    public void OffDoor()
    {
        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(OffDoorCoroutine());
        }
        else { /*PASS*/ }
    }


    private void OnDestroy()
    {
        StopAllCoroutines();
        GameManager.instance.DoorOnEvent -= OnDoor;
        GameManager.instance.DoorOffEvent -= OffDoor;
    }

    IEnumerator OnDoorCoroutine()
    {
        AudioManager.Instance.PlaySFXPoint("SFX_Stage_Door_Open_01", this.transform.position);
        //GFunc.Log($"문 열기 호출\n호출자 : {this.gameObject.name}");
        StopCoroutine(OffDoorCoroutine());
        boxCollider.isTrigger = true;
        int recallCount = 0;
        while (!compleateDoorOn)
        {
            recallCount++;
            if (recallCount >= 350)     // 350 = 임시
            {
                this.transform.position = targetV3;
            }
            if (transform.position == targetV3)
            {
                compleateDoorOn = true;
            }
            transform.position = Vector3.Lerp(transform.position, targetV3, doorOnTime * Time.deltaTime);
            yield return null;
        }
        boxCollider.isTrigger = false;
        compleateDoorOn = false;
    }       // OnDoorCoroutine()

    IEnumerator OffDoorCoroutine()
    {
        AudioManager.Instance.PlaySFXPoint("SFX_Stage_Door_Close_01", this.transform.position);
        //GFunc.Log($"문닫기 호출\n호출자 : {this.gameObject.name}");
        StopCoroutine(OnDoorCoroutine());
        boxCollider.isTrigger = true;
        int recallCount = 0;
        while (!compleateDoorOff)
        {
            recallCount++;
            if (recallCount >= 350)     // 350 = 임시
            {
                this.transform.position = defaultV3;
            }
            if (transform.position == defaultV3)
            {
                compleateDoorOff = true;
            }
            transform.position = Vector3.Lerp(transform.position, defaultV3, doorOffTime * Time.deltaTime);
            yield return null;
        }
        boxCollider.isTrigger = false;
        compleateDoorOff = false;
        AudioManager.Instance.PlaySFXPoint("SFX_Stage_Door_Close_02", this.transform.position);

    }       //  OffDoorCoroutine()

}
