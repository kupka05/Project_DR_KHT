using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour
{
    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    [Header("UI")]
    [SerializeField] private ItemTooltipUI _itemTooltip;
    [SerializeField] private GameObject itemSlotPanelPrefab;
    [SerializeField] private List<GameObject> _itemSlots;
    [SerializeField] private GameObject[] _defaultItemSlots = new GameObject[3];
    [SerializeField] private float _itemSlotInterval = 600f;
    private Vector2 _tooltipAnchorPos;
    private float _tooltipInterval = 300f;
    #endregion
    /*************************************************
     *                  Unity Events
     *************************************************/
    #region [+]
    void Start()
    {
        _itemSlots = new List<GameObject>();
        _tooltipAnchorPos = _itemTooltip.GetComponent<RectTransform>().anchoredPosition;
        for (int i = 0; i < 10; i++)
        {
            AddItemSlot(5001);
        }
    }

    #endregion
    /*************************************************a
     *                Public Methods
     *************************************************/
    #region [+]
    public void UpdatePlayerInventory()
    {
        // 빈 슬롯 활성화 관리
        int count = _itemSlots.Count >= 3 ? 3 : _itemSlots.Count;
        for (int i = 0; i < count; i++)
        {
            _defaultItemSlots[i].SetActive(false);
        }
    }

    public void AddItemSlot(int id)
    {
        GameObject itemSlot = Instantiate(itemSlotPanelPrefab, 
            itemSlotPanelPrefab.transform.parent);
        ItemSlotPanelUI itemSlotPanelUI = itemSlot.GetComponent<ItemSlotPanelUI>();
        itemSlotPanelUI.Initialize(id);
        itemSlot.SetActive(true);
        _itemSlots.Add(itemSlot);
        int index = _itemSlots.Count - 1;
        if (index > 0)
        {
            RectTransform itemSlotRect = itemSlot.GetComponent<RectTransform>();
            Vector2 pos = itemSlotRect.anchoredPosition;
            // 야매로 (_itemSlotInterval / 2) * index로 수정함 왜냐면
            // 다르게 하면 간격이 틀어지는 오류가 발생
            // 나중에 방법 찾으면 해결할 예정
            pos.x += (_itemSlotInterval / 2) * index;
            itemSlotRect.anchoredPosition = pos;
        }
        // 기본 슬롯 비활성화
        UpdatePlayerInventory();

        itemSlot.name = itemSlot.name + " (" + index + ")";
        itemSlotPanelUI.SetIndex(index);
    }

    public void ShowTooltip(int index)
    {
        if (_itemSlots[index])
        {
            ItemSlotPanelUI itemSlotPanelUI = 
                _itemSlots[index].GetComponent<ItemSlotPanelUI>(); 
            UpdateTooltipUI(itemSlotPanelUI.ItemData, index);
            _itemTooltip.Show();
        }
    }

    public void HideToolTip()
    {
        _itemTooltip.Hide();
    }

    private void UpdateTooltipUI(ItemData data, int index)
    {
        // 툴팁 정보 갱신
        _itemTooltip.SetItemInfo(data);

        RectTransform tooltipRect = _itemTooltip.GetComponent<RectTransform>();
        Vector2 tempAnchorPos = _tooltipAnchorPos;
        tempAnchorPos.x += (_tooltipInterval * index);
        // 툴팁 위치 조정
        tooltipRect.anchoredPosition = tempAnchorPos;
    }

    #endregion
    /*************************************************
     *                Private Methods
     *************************************************/
    #region [+]

    #endregion
}
