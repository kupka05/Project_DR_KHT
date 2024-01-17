using Rito.InventorySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RandomRoom : MonoBehaviour
{       // 각 3종류의 방이 존재하는데 각 방Class는 RandomRoom을 상속받을것임
    
    public bool isClearRoom = default;    // 해당방 클리어했는지 여부

    public FloorMeshPos meshPos = default;        // 각방의 꼭지점Pos이 들어있는 Class
        
    private void Awake()
    {
        isClearRoom = false;
    }

    public bool BoolTest()
    {
        return isClearRoom;
    }

    /// <summary>
    /// FloorMeshPos 컴포넌트를 가져오는 함수 : 각 방의 꼭지점 V3 값이 들어있는 컴포넌트
    /// </summary>
    protected void GetFloorPos()
    {
       meshPos = this.GetComponent<FloorMeshPos>();
    }       // GetFloorPos()

    /// <summary>
    /// 상속해준 isClearRoom bool값을 리턴해주는 함수
    /// </summary>
    /// <returns>isClearRoom</returns>
    protected bool GetClearRoomBool()
    { 
        return isClearRoom;
    }       // GetClearRoomBool()

    /// <summary>
    /// clearRoom 변수를 true로 바꿔주는 함수
    /// </summary>
    protected void ClearRoomBoolSetTrue()
    {
        isClearRoom = true;
        GameManager.instance.IsClear = isClearRoom;
    }       // ClearRoomBoolSetTrue()

    /// <summary>
    /// clearRoom 변수를 false로 바꿔주는 함수
    /// </summary>
    protected void ClearRoomBoolSetFalse()
    {
        isClearRoom = false;
    }       // ClearRoomBoolSetFalse()


    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(isClearRoom == false)
            {
                GameManager.instance.IsClear = isClearRoom;
            }
        }        
    }




}       // ClassEnd
