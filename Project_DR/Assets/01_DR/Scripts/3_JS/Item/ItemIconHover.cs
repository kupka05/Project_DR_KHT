using Rito.InventorySystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemIconHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _btn;
    [SerializeField] PlayerInventoryUI playerInventoryUI;
    [SerializeField] ItemSlotPanelUI itemSlotPanelUI;
    private void Start()
    {
        _btn = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 호버 진입시
        playerInventoryUI.ShowTooltip(itemSlotPanelUI.Index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //호버 나갈 경우
        playerInventoryUI.HideToolTip();
    }
}
