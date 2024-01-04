using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCComunicationRange : MonoBehaviour
{
    private NpcTriggerType triggerType;

    private bool isOnTheWay;            // 다가가는 중인지 체크
    private bool isShortDistance;       // 가까운 거리까지 왔는지 (Auto타입 체크)

    private bool isCompleateRange;      // 대화할 준비가 되었는지 (Trigger타입체크)

    private float goalDis;               // 목표거리
    private float moveSpeed;             // 이동속도

    private Transform playerTransform;  // Player의 Transform

    private void Awake()
    {
        AwakeInIt();
    }


    private void Update()
    {
        if (triggerType == NpcTriggerType.Auto &&isOnTheWay == true && isShortDistance == false)
        {
            float dis = Vector3.Distance(this.transform.position, playerTransform.position);
            if (dis > goalDis)
            {
                OnTheWayPlayer();
            }
            else
            {
                isShortDistance = true;
            }

        }

    }

    // 플레이어를 따라가는 함수
    private void OnTheWayPlayer()
    {
        this.gameObject.transform.LookAt(playerTransform, Vector3.up);
        this.gameObject.transform.position += Vector3.forward * moveSpeed * Time.deltaTime;        
    }       // OnTheWayPlayer()

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            playerTransform = other.transform.GetComponent<Transform>();
            triggerType = this.transform.parent.GetComponent<NPC>().npcTriggerType;

            CheckNPCType();
        }
    }
    
    // NPC의 타입을 체크하고 그 타입에 따라 bool값을 바꾸어줄예정
    private void CheckNPCType()
    {
        // TODO : 타입 체크 이후 변수값 변경
        isOnTheWay = true;
    }

    private void AwakeInIt()
    {
        isOnTheWay = false;
        isShortDistance = false;
        isCompleateRange = false;

        goalDis = 5f;
        moveSpeed = 10f;

    }       // AwakeInIt()

}       // ClassEnd
