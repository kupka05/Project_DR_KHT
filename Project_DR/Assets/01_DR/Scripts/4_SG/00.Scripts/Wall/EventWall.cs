using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWall : MonoBehaviour
{

    private void Awake()
    {
        
    }


    void Start()
    {
        StartSetWallPosition();
    }


    // 오브젝트 생성시 Ray를 쏘아서 던전 안쪽으로 조금 튀어나오게 해줄 함수
    private void StartSetWallPosition()
    {
        // Ray맞은것
        RaycastHit hitInfo;
        if(Physics.Raycast(this.transform.position, Vector3.forward,out hitInfo,Mathf.Infinity))
        {
            //hitInfo.
        }
    }       // StartSetWallPosition()


}
