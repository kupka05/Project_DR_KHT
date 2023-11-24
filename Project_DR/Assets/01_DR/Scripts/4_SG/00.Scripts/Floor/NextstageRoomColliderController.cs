using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextstageRoomColliderController : MonoBehaviour
{

    // 플레이어가 밟을수 있는 땅(Collider)
    public BoxCollider stepOnCollider;
    // 플레이어가 바닥으로 갈수 있게할 콜라이더
    public BoxCollider stepOffCollider;    




    /// <summary>
    /// 호출시 매개변수에 따라서 콜라이더 각 변수에 넣어주는 함수
    /// </summary>   
    public void GetColliders(BoxCollider stepOnCollider_,BoxCollider stepOffCollider_)
    {
        stepOnCollider = stepOnCollider_;
        stepOffCollider = stepOffCollider_;
    }       // GetColliders()




}       // ClassEnd 
