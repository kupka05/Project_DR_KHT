using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPlatform : MonoBehaviour
{
    /*************************************************
     *                Public Fields
     *************************************************/
    public enum Type
    {
        OPEN = 0,       // 열리는 발판
        CLOSE = 1       // 닫히는 발판
    }
    [SerializeField] public ShopDoor.State CurrentState => _shopDoor.CurrentState;    // 현재 상태
    [SerializeField] public Animation Animation => _shopDoor.Animation;               // 애니메이션


    /*************************************************
     *                Private Fields
     *************************************************/
    [SerializeField] ShopDoor _shopDoor;                                               // 상점 문
    [SerializeField] private Type _type;                                               // 발판 타입
    [SerializeField] Action _action;                                                   // 액션


    /*************************************************
     *                 Unity Events
     *************************************************/
    void Start()
    {
        // Init
        Initialize();
    }

    // 상점 안에 들어올 경우 문을 닫는다.
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 들어왔을 경우
        if (other.CompareTag("Player"))
        {
            // n초 후 문을 열거나 닫는다.
            _shopDoor.StartFunctionForCoroutine(_action = UpdateDoor);
        }
    }


    /*************************************************
     *             Initialize Methods
     *************************************************/
    public void Initialize()
    {
        // Init
        _shopDoor = transform.parent.GetComponent<ShopDoor>();
    }


    /*************************************************
     *              Private Methods
     *************************************************/
    // 문을 연다
    private void UpdateDoor()
    {
        // 발판 타입이 [열림]이고
        // && 현재 상태가 닫힌 상태일 경우
        if (_type.Equals(Type.OPEN) && CurrentState.Equals(ShopDoor.State.CLOSE))
        {
            // 애니메이션 재생
            Animation.Play("ShopDoor_Open");

            // 발판에 맞는 상태로 변경
            _shopDoor.ChangeState((int)_type);
        }

        // 발판 타입이 [닫힘]이고
        // && 현재 상태가 열린 상태일 경우
        else if (_type.Equals(Type.CLOSE) && CurrentState.Equals(ShopDoor.State.OPEN))
        {
            // 애니메이션 재생
            Animation.Play("ShopDoor_Close");

            // 발판에 맞는 상태로 변경
            _shopDoor.ChangeState((int)_type);
        }
    }
}
