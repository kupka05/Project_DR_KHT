using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Rito.InventorySystem;

public class ButtonHighlightDetection : MonoBehaviour
{
    private Button _btn;
    private ItemSlotUI _itemSlotUI;
    private InventoryUI _inventoryUI;

    void Start()
    {
        _btn = GetComponent<Button>();
        _itemSlotUI = GetComponent<ItemSlotUI>();
        _inventoryUI = _itemSlotUI.publicInventoryUI;

        // 이벤트 트리거 컴포넌트 추가
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        // 호버 진입 이벤트에 대한 콜백 등록
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnHoverEnter(); });
        eventTrigger.triggers.Add(entryEnter);

        // 호버 해제 이벤트에 대한 콜백 등록
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { OnHoverExit(); });
        eventTrigger.triggers.Add(entryExit);
    }

    // 호버에 진입했을 경우
    void OnHoverEnter()
    {
        //Debug.Log("Hover Enter");
        _inventoryUI.ShowTooltip(_itemSlotUI.Index);
        // 호버 진입 시 실행할 동작 추가
    }

    // 호버에서 나갔을 경우
    void OnHoverExit()
    {
        //Debug.Log("Hover Exit");
        _inventoryUI.HideTooltip();
        // 호버 해제 시 실행할 동작 추가
    }
}
