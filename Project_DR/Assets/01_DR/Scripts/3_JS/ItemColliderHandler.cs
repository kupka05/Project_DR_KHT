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
    private float stateResetDelay = 1f;     // 상태 초기화에 걸리는 시간
    private GrabbableHaptics grabbableHaptics = default;    // 그립 여부를 파악하기 위해 객체 생성

    #endregion
    /*************************************************
     *                Public Fields
     *************************************************/
    #region [+]
    public enum State
    {
        Default = 0,
        Processing,
        Stop
    }
    public State state;

    #endregion
    /*************************************************
     *                 Unity Events
     *************************************************/
    #region [+]
    private void Awake()
    {
        grabbableHaptics = GetComponent<GrabbableHaptics>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
    }

    // 아이템이 특정 물체를 통과했을 경우
    private void OnTriggerEnter(Collider other)
    {
        // 대상이 아이템 슬롯일 경우
        if (other.CompareTag("ItemSlot") && state == State.Default)
        {
            Debug.Log($"name: {other.transform.parent.name}");
            // 콜라이더가 인벤토리 스크롤 패널 안에 있을 경우
            if (CheckColliderVisibility(
                other.transform.parent.parent.parent.parent.parent.parent.GetComponent<RectTransform>(), 
                other.GetComponent<RectTransform>()) == true)
            {
                // 작업 상태로 변경
                state = State.Processing;

                ItemDataComponent itemDataComponent = gameObject.GetComponent<ItemDataComponent>();

                // ItemDataComponent가 있는지 확인
                if (itemDataComponent != null)
                {
                    ItemData itemData = (ItemData)itemDataComponent.ItemData;
                    int id = itemData.ID;
                    ItemManager.instance.InventoryCreateItem(id);
                    ItemManager.instance.CreatePotionItem(id);
                }
                else
                {
                    // 디버그용
                    Debug.LogWarning("Item Error!");
                    ItemManager.instance.InventoryCreateItem(5001);
                    ItemManager.instance.CreatePotionItem(5001);
                }
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Out of range");
            }
        }
    }

    // 아이템이 바닥과 충돌 했을 경우
    private void OnCollisionEnter(Collision collision)
    {
        // 아이템을 슬롯에서 꺼낼 때 Stop 상태로 변경된다.
        // Stop 상태에서 바닥과 충돌했을 경우
        if (collision.collider.CompareTag("Floor") && state == State.Stop)
        {
            // 디버그
            Debug.Log("Floor");

            // 1초 후에 상태 초기화 코루틴 실행
            Action func = ResetState;
            Coroutine(func, stateResetDelay);
        }
    }
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
