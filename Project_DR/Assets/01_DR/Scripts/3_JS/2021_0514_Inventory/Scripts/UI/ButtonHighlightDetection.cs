using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Rito.InventorySystem;

public class ButtonHighlightDetection : MonoBehaviour
{
    private Button _btn;
    private ItemSlotUI _itemSlotUI;
    private InventoryUI _inventoryUI;
    [SerializeField] private ItemSlotPanelUI _itemSlotPanelUI;
    [SerializeField] private PlayerInventoryUI _playerInventoryUI;
    [SerializeField] private bool isPlayerInventory = false;

    void Start()
    {
        _btn = GetComponent<Button>();
        if (isPlayerInventory == false)
        {
            _itemSlotUI = GetComponent<ItemSlotUI>();
            _inventoryUI = _itemSlotUI.publicInventoryUI;
        }
        else
        {
            _itemSlotPanelUI = gameObject.transform.parent.GetComponent<ItemSlotPanelUI>();
        }

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
        if (isPlayerInventory == false)
        {
            _inventoryUI.ShowTooltip(_itemSlotUI.Index);
        }
        else
        {
            _playerInventoryUI.ShowTooltip(_itemSlotPanelUI.Index);
        }
        // 호버 진입 시 실행할 동작 추가
    }

    // 호버에서 나갔을 경우
    void OnHoverExit()
    {
        //Debug.Log("Hover Exit");
        if (isPlayerInventory == false)
        {
            _inventoryUI.HideTooltip();
        }
        else
        {
            _playerInventoryUI.HideToolTip();
        }
        // 호버 해제 시 실행할 동작 추가
    }
}
