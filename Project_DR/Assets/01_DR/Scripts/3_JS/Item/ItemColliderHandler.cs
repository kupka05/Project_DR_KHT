using Rito.InventorySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
public class ItemColliderHandler : MonoBehaviour
{
    /*************************************************
     *                Private Fields
     *************************************************/
    #region [+]
    private Rigidbody rigidBody = default;
    private float stateResetDelay = 5f;                     // 상태 초기화(슬롯에 보관)에 걸리는 시간
    //private GrabbableHaptics grabbableHaptics = default;    // 그립 여부를 파악하기 위해 객체 생성
    //public GrabbableHaptics GrabbableHaptics => grabbableHaptics;
    #endregion
    /*************************************************
     *                Public Fields
     *************************************************/
    #region [+]
    public enum State
    {
        Default = 0,
        Processing,
        Handed,
        Stop,
        Grabbed
    }
    public State state;

    #endregion
    /*************************************************
     *                 Unity Events
     *************************************************/
    #region [+]
    private void Awake()
    {
        //grabbableHaptics = GetComponent<GrabbableHaptics>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // 아이템이 특정 물체를 통과했을 경우
    private void OnTriggerEnter(Collider other)
    {
        ProcessItemInSlot(other);
    }

    // 슬롯이 아이템을 더 쉽게 감지하기 위해
    // OnTriggerStay에도 추가함
    private void OnTriggerStay(Collider other)
    {
        ProcessItemInSlot(other);
    }

    // 아이템이 바닥과 충돌 했을 경우
    //private void OnCollisionEnter(Collision collision)
    //{
    //    // 아이템을 슬롯에서 꺼낼 때 Stop 상태로 변경된다.
    //    // Stop 상태에서 바닥과 충돌했을 경우
    //    if (collision.collider.CompareTag("Floor") && state == State.Stop)
    //    {
    //        // 디버그
    //        //GFunc.Log("Floor");

    //        // n초 후에 상태 초기화 코루틴 실행
    //        Action func = ResetState;
    //        Coroutine(func, stateResetDelay);
    //    }
    //}
    #endregion

    /*************************************************
     *                Public Methods
     *************************************************/
    #region [+]
    // 함수를 코루틴으로 실행함
    public void Coroutine(Action func, float delay)
    {
        StartCoroutine(RunCoroutineFunction(func, delay));
    }

    // 물리 효과 토글
    public void ChangeKinematic(bool isKinematic)
    {
        rigidBody.isKinematic = isKinematic;
    }

    // 상태 초기화
    public void ResetState()
    {
        state = State.Default;
    }

    #endregion
    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    // 아이템이 슬롯에 들어왔을 때 처리하는 함수
    private void ProcessItemInSlot(Collider other)
    {
        // 대상이 아이템 슬롯일 경우
        if (other.CompareTag("ItemSlot") && state == State.Grabbed)
        {
            ItemSlotController itemSlot = other.GetComponent<ItemSlotController>();
            // 수납 가능한 경우에만 수납함
            if (itemSlot.IsStorageAvailable)
            {
                // 플레이어 개인 수납 슬롯이 아닌 경우
                if (itemSlot.IsPlayerStorage == false)
                {
                    // 콜라이더가 인벤토리 스크롤 패널 안에 있는지 체크
                    if (CheckColliderVisibility(
                        other.transform.parent.parent.parent.parent.parent.parent.GetComponent<RectTransform>(),
                        other.GetComponent<RectTransform>()) == false)
                    {
                        // 아닐 경우 예외처리
                        GFunc.Log("Out of range");
                        return;
                    }
                }
                // 작업 상태로 변경
                state = State.Processing;

                ItemDataComponent itemDataComponent = gameObject.GetComponent<ItemDataComponent>();
                //GFunc.Log($"GameObject {gameObject.GetComponent<ItemDataComponent>()}");
                // ItemDataComponent가 있는지 확인
                if (itemDataComponent != null)
                {
                    //Debug.Log(itemDataComponent.ItemData);
                    ItemData itemData = (ItemData)itemDataComponent.ItemData;
                    int id = itemData.ID;

                    // 만약 퀘스트 아이템일 경우, 경험치 또는 골드를 올려줌
                    if (itemData is QuestItemData qi)
                    {
                        UserData.AddItemScore(id);

                        Destroy(gameObject);
                        return;
                    }

                    ItemManager.instance.InventoryCreateItem(other.transform.position, id);
                }
                else
                {
                    // 디버그용
                    GFunc.LogWarning("Item Error!");
                }

     

                Destroy(gameObject);
            }
        }
    }

    private bool CheckColliderVisibility(RectTransform scrollPanel, RectTransform other)
    {
        // 현재 객체가 스크롤 패널 내에 있는지 여부 확인
        bool isVisible = RectTransformUtility.RectangleContainsScreenPoint(scrollPanel, other.position);

        return isVisible;
    }
    #endregion
    /*************************************************
     *                   Coroutines
     *************************************************/
    #region [+]
    // 코루틴에 함수를 넣어서 실행하는 코루틴 함수
    public IEnumerator RunCoroutineFunction(Action func, float delay)
    {
        // 대기
        yield return new WaitForSeconds(delay);

        // 받아온 함수 실행
        func();
    }

    #endregion
}
