using System.Collections;
using UnityEngine;

public class HandColliderHandler : MonoBehaviour
{
    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    private enum State
    {
        Default = 0,
        ProcessingOne,
        ProcessingTwo
    }
    [SerializeField] private State _state;
    private Rigidbody rigidBody = default;
    #endregion
    /*************************************************
     *                 Unity Events
     *************************************************/
    #region [+]
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 감지된 콜라이더가 아이템 슬롯일 경우
        if (other.CompareTag("ItemSlot") && _state == State.Default)
        {
            // 콜라이더가 인벤토리 스크롤 패널 안에 있을 경우
            if (CheckColliderVisibility(
                other.transform.parent.parent.parent.parent.parent.parent.GetComponent<RectTransform>(),
                other.GetComponent<RectTransform>()) == true)
            {
                Debug.Log($"name: {other.transform.parent.name}");

                // 작업 상태로 변경
                _state = State.ProcessingOne;
            }
            else
            {
                Debug.Log("Out of range");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // 현재 작업 상태일 경우
        if (other.CompareTag("ItemSlot") && _state == State.ProcessingOne)
        {
            // 오른쪽 컨트롤러에서 트리거 키를 눌렀을 경우
            if (global::BNG.InputBridge.Instance.RightTriggerDown)
            {
                // 상태 변경
                _state = State.ProcessingTwo;

                // 디버그
                Debug.Log("Right Trigger Pressed");

                Transform slot = other.transform.parent;
                GameObject item = ItemManager.instance.CreateItem(5001);
                ItemColliderHandler itemColliderHandler = item.GetComponent<ItemColliderHandler>();
                Rigidbody itemRigid = item.GetComponent<Rigidbody>();

                // 슬롯에 넣을 수 없도록 아이템 상태 Stop으로 변경
                itemColliderHandler.state = ItemColliderHandler.State.Stop;

                // 아이템 물리 효과 정지
                itemRigid.isKinematic = true;

                // hand 위치로 포지션 이동
                item.transform.position = transform.position;

                // 플레이어가 아이템을 잡고 손을 떼엇을 경우 다시 들어가야 하므로,
                // n 초 후에 물리 효과 실행 및 아이템 슬롯에 들어가도록 설정
                itemColliderHandler.Coroutine(itemColliderHandler.ToggleKinematic, 3f);

            }

            // 왼쪽 컨트롤러에서 트리거 키를 눌렀을 경우
            if (global::BNG.InputBridge.Instance.LeftTriggerDown)
            {
                _state = State.ProcessingTwo;
                Debug.Log("Left Trigger Pressed");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //ChangeStateCoroutine(State.Default, 1f);
        if (_state == State.ProcessingOne || _state == State.ProcessingTwo)
        {
            _state = State.Default;
        }
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
}
