using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColliderHandler : MonoBehaviour
{
    /*************************************************
     *                Private Fields
     *************************************************/
    #region [+]
    private enum State
    {
        Default = 0,
        Processing
    }
    [SerializeField] private State _state;
    private Rigidbody rigidBody = default;
    #endregion
    /*************************************************
     *                 Unity Events
     *************************************************/
    #region [+]
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // 아이템이 특정 물체를 통과했을 경우
    private void OnTriggerEnter(Collider other)
    {
        // 대상이 아이템 슬롯일 경우
        if (other.CompareTag("ItemSlot") && _state == State.Default)
        {
            Debug.Log($"name: {other.transform.parent.name}");
            // 콜라이더가 인벤토리 스크롤 패널 안에 있을 경우
            if (CheckColliderVisibility(
                other.transform.parent.parent.parent.parent.parent.parent.GetComponent<RectTransform>(), 
                other.GetComponent<RectTransform>()) == true)
            {
                // 작업 상태로 변경
                _state = State.Processing;

                // 추후 콜라이더 정보 받아서 해당 ID에 해당하는 아이템 생성하게 변경하기
                ItemManager.instance.CreateItem(5102);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Out of range");
            }
        }
    }

    #endregion
    /*************************************************
     *                 Unity Events
     *************************************************/
    #region [+]
    private bool CheckColliderVisibility(RectTransform scrollPanel, RectTransform other)
    {
        // 현재 객체가 스크롤 패널 내에 있는지 여부 확인
        bool isVisible = RectTransformUtility.RectangleContainsScreenPoint(scrollPanel, other.position);

        return isVisible;
    }

    #endregion
}
