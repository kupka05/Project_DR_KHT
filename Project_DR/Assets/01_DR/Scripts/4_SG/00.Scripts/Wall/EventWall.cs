using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWall : MonoBehaviour
{
    public bool isLeft, isRight, isUp, isDown = false;
    private FloorMeshPos floorMesh = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (floorMesh == null)
            {
                floorMesh = collision.gameObject.GetComponent<FloorMeshPos>();
                EventWallPosExamine();
            }
        }


    }       // OnCollisioinEnter()


    // 이벤트벽 위치 검수
    private void EventWallPosExamine()
    {
        // 이벤트 벽이 왼쪽인지 오른쪽인지 검수
        if (this.transform.position.x == floorMesh.bottomLeftCorner.x)
        {       // 왼쪽 하단 꼭지점과 포지션이 같을경우
            isLeft = true;
        }
        else if (this.transform.position.x == floorMesh.bottomRightCorner.x)
        {       // 오른쪽 하단 꼭지점과 포지션이 같을경우
            isRight = true;
        }
        if (this.transform.position.z == floorMesh.topLeftCorner.z)
        {       // 상단의 높이와 같을경우
            isUp = true;
        }
        else if (this.transform.position.z == floorMesh.bottomLeftCorner.z)
        {       // 하단의 높이와 같을경우
            isDown = true;
        }
        EventWallPosSet();

    }       // EventWallPosExamine()

    private void EventWallPosSet()
    {
        Vector3 setPos;
        setPos = this.transform.position;
        if (isLeft == true)
        {
            setPos.x += 0.5f;
            this.transform.position = setPos;
        }
        if (isRight == true)
        {
            setPos.x -= 0.5f;
            this.transform.position = setPos;
        }
        if (isUp == true)
        {
            setPos.z -= 0.5f;
            this.transform.position = setPos;
        }
        if (isDown == true)
        {
            setPos.z += 0.5f;
            this.transform.position = setPos;
        }        
        EventWallScaleSet();
    }       // EventWallPosSet()


    // 자신의 위치에 따라서 Sclae을 바꿔주는 함수
    private void EventWallScaleSet()
    {
        Vector3 setSclae;
        if (isLeft == true)
        {
            setSclae = this.transform.localScale;
            setSclae.z = 3f;
            this.transform.localScale = setSclae;
        }
        if (isRight == true)
        {
            setSclae = this.transform.localScale;
            setSclae.z = 3f;
            this.transform.localScale = setSclae;
        }
        if (isUp == true)
        {
            setSclae = this.transform.localScale;
            setSclae.x = 3f;
            this.transform.localScale = setSclae;
        }
        if (isDown == true)
        {
            setSclae = this.transform.localScale;
            setSclae.x = 3f;
            this.transform.localScale = setSclae;
        }
        MakeChildrenObjects();

    }       // EventWallScaleSet()

    // 양쪽 벽 obj를 자신의 자식오브젝트로 만드는 함수
    private void MakeChildrenObjects()
    {
        RaycastHit hit;
        float maxDis = 1f;
        if (isLeft == true || isRight == true)
        {       // 이벤트벽이 왼쪽 혹은 오른쪽에 있다면 -> 전방,후방 레이를 쏨
            if (Physics.Raycast(this.transform.position,Vector3.forward,out hit, maxDis))
            {             
                hit.collider.transform.parent = this.transform;
            }
            if (Physics.Raycast(this.transform.position, Vector3.back, out hit, maxDis))
            {             
                hit.collider.transform.parent = this.transform;
            }            
        }
        else if(isUp == true || isDown == true)
        {       // 이벤트벽이 위쪽 혹은 아래쪽에 있다면 -> 좌,우 레이를쏨
            if (Physics.Raycast(this.transform.position, Vector3.left, out hit, maxDis))
            {             
                hit.collider.transform.parent = this.transform;
            }
            if (Physics.Raycast(this.transform.position, Vector3.right, out hit, maxDis))
            {                
                hit.collider.transform.parent = this.transform;
            }
        }

    }       // MakeChildrenObjects()
}       // ClassEnd
