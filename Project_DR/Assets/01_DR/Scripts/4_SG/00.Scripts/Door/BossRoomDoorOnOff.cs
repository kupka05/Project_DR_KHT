using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomDoorOnOff : MonoBehaviour
{
    public Vector3 defaultV3;
    public Vector3 targetV3;

    // 문이 열렸는지 닫혔는지 확인하는 변수
    private bool compleateDoorOn;
    private bool compleateDoorOff;

    // 문 열리고 닫히는 시간관련 변수
    private float doorOnTime;
    private float doorOffTime;

    // 소리관련 변수
    private string openSound = default;
    private string closeSound = default;
    private string compleatCloseSound = default;

    BoxCollider boxCollider;

    private void Awake()
    {
        AwakeInIt();
        openSound = "SFX_Stage_Door_Open_01";
        closeSound = "SFX_Stage_Door_Close_01";
        compleatCloseSound = "SFX_Stage_Door_Close_02";
    }

    void Start()
    {
        StartInIt();
        DoorSoundAdd();
    }

    /// <summary>
    /// 문의 소리를 사운드 메니저에 추가해주는 함수
    /// </summary>
    private void DoorSoundAdd()
    {
        AudioManager.Instance.AddSFX(openSound);
        AudioManager.Instance.AddSFX(closeSound);
        AudioManager.Instance.AddSFX(compleatCloseSound);        
    }       // DoorSoundAdd()

    private void AwakeInIt()
    {
        defaultV3 = transform.position;
        targetV3 = new Vector3(defaultV3.x, defaultV3.y * 2, defaultV3.z);

        compleateDoorOn = false;
        compleateDoorOff = false;
        doorOnTime = 5f;
        doorOffTime = 8f;

        boxCollider = this.GetComponent<BoxCollider>();
    }       // AwakeInIt()

    private void StartInIt()
    {
        GameManager.instance.BossRoomDoorOnEvent += OnDoor;

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
        GameManager.instance.BossRoomDoorOnEvent -= OnDoor;
    }

    IEnumerator OnDoorCoroutine()
    {
        boxCollider.isTrigger = true;
        AudioManager.Instance.PlaySFX(openSound);
        while (!compleateDoorOn)
        {
            transform.position = Vector3.Lerp(transform.position, targetV3, doorOnTime * Time.deltaTime);
            if (transform.position == targetV3)
            {
                compleateDoorOn = true;
            }
            yield return null;
        }
        boxCollider.isTrigger = false;
        compleateDoorOn = false;
    }       // OnDoorCoroutine()

    IEnumerator OffDoorCoroutine()
    {        
        boxCollider.isTrigger = true;
        AudioManager.Instance.PlaySFX(closeSound);
        while (!compleateDoorOff)
        {
            transform.position = Vector3.Lerp(transform.position, defaultV3, doorOffTime * Time.deltaTime);
            if (transform.position == defaultV3)
            {
                compleateDoorOff = true;
                AudioManager.Instance.PlaySFX(compleatCloseSound);
            }
            yield return null;
        }
        boxCollider.isTrigger = false;
        compleateDoorOff = false;
    }       //  OffDoorCoroutine()

}       // ClassEnd
