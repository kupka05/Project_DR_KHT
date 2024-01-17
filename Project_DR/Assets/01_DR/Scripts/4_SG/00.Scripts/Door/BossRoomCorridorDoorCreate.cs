using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomCorridorDoorCreate : MonoBehaviour
{
public GameObject doorPrefab;

    private void Awake()
    {
        doorPrefab = Resources.Load<GameObject>("BossRoomDoor");
    }
    void Start()
    {
    
        InstantiateDoor();
    }


    /// <summary>
    /// 문을 인스턴스해주는함수
    /// </summary>
    private void InstantiateDoor()
    {
        FloorMeshPos cornerPos = this.GetComponent<FloorMeshPos>();
        // TODO : Vector3 Y포지션 수정 해야할수도 있음        
        Vector3 centerPos = new Vector3((cornerPos.bottomLeftCorner.x + cornerPos.bottomRightCorner.x) * 0.5f,
            doorPrefab.transform.localScale.y * 0.5f,
            (cornerPos.bottomLeftCorner.z + cornerPos.topLeftCorner.z) * 0.5f);

        GameObject doorClone = Instantiate(doorPrefab,centerPos,Quaternion.identity,this.transform);

        doorClone.AddComponent<BossRoomDoorOnOff>();

        SetRotation(doorClone);


    }       // InstantiateDoor()

    // 문의 로테이션을 조건에 맞게 바꿔줄 함수
    private void SetRotation(GameObject _doorClone)
    {
        float[] hitsDis = new float[4];
        
        RaycastHit hit;     // 레이를 맞은것
        // 아래 상,하,좌,우 레이를 쏘아서 먼곳을 바라보게 만들면 문의 역활을 할수 있을거같음
        if(Physics.Raycast(_doorClone.transform.position,Vector3.forward ,out hit,Mathf.Infinity))
        {       // 전방
            hitsDis[0] = hit.distance;
        }
        if(Physics.Raycast(_doorClone.transform.position,Vector3.back, out hit,Mathf.Infinity))
        {       // 후방
            hitsDis[1] = hit.distance;
        }
        if(Physics.Raycast(_doorClone.transform.position,Vector3.left, out hit,Mathf.Infinity))
        {       // 좌
            hitsDis[2] = hit.distance;
        }
        if(Physics.Raycast(_doorClone.transform.position,Vector3.right,out hit,Mathf.Infinity))
        {       // 우
            hitsDis[3] = hit.distance;
        }

        float highDis = 0;

        foreach(float dis in hitsDis)
        {
            if (highDis < dis)
            {
                highDis = dis;
            }
        }
        
        bool forward = false;
        bool back = false;
        bool left = false;
        bool right = false;

        if (highDis == hitsDis[0])
        {
            forward = true;
            //GFunc.Log($"{this.gameObject.name} : 은 forward가 true");
        }
        else if (highDis == hitsDis[1])
        {
            back = true;
            //GFunc.Log($"{this.gameObject.name} : 은 back이 true");
        }
        else if (highDis == hitsDis[2])
        {
            left = true;
            //GFunc.Log($"{this.gameObject.name} : 은 Left가 true");
        }
        else if (highDis == hitsDis[3])
        {
            right = true;
            //GFunc.Log($"{this.gameObject.name} : 은 Right가 true");
        }
        else { GFunc.Log("문이 쏜레이가 맞은 포인트가 맞지 않음"); }


        if (forward == true) 
        {
            _doorClone.transform.rotation = Quaternion.Euler(0f, 0f, 0f);                
        }
        else if (back == true)
        {
            _doorClone.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
        }
        else if (left == true)
        {
            _doorClone.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else if(right == true)
        {
            _doorClone.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

    }   // SetRotation()
}
