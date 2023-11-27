using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    [Header("UI")]
    private GameObject _playerIntentoryUI;
    [SerializeField] ItemTooltipUI _itemTooltip;
    [SerializeField] private GameObject itemSlotPanelPrefab;
    [SerializeField] List<GameObject> _itemSlots;

    #endregion
    /*************************************************
     *                  Unity Events
     *************************************************/
    #region [+]
    void Start()
    {
        _playerIntentoryUI = gameObject;
        _itemSlots = new List<GameObject>();

        AddItemSlot(5001);
    }

    #endregion
    /*************************************************
     *                Public Methods
     *************************************************/
    #region [+]

    public void AddItemSlot(int id)
    {
        GameObject itemSlot = Instantiate(itemSlotPanelPrefab, 
            itemSlotPanelPrefab.transform.parent);
        ItemSlotPanelUI itemSlotPanelUI = itemSlot.GetComponent<ItemSlotPanelUI>();
        itemSlotPanelUI.Initialize(id);
        itemSlot.SetActive(true);
        _itemSlots.Add(itemSlot);
        int index = _itemSlots.Count - 1;
        itemSlotPanelUI.SetIndex(index);
    }

    public void ShowTooltip(int index)
    {
        if (_itemSlots[index])
        {
            ItemSlotPanelUI itemSlotPanelUI = 
                _itemSlots[index].GetComponent<ItemSlotPanelUI>(); 
            UpdateTooltipUI(itemSlotPanelUI.ItemData);
            _itemTooltip.Show();
        }
    }

    public void HideToolTip()
    {
        _itemTooltip.Hide();
    }

    private void UpdateTooltipUI(ItemData data)
    {
        // 툴팁 정보 갱신
        _itemTooltip.SetItemInfo(data);

        // 툴팁 위치 조정
        //_itemTooltip.SetRectPosition(slot.SlotRect, _horizontalSlotCount,
        //    _verticalSlotCount, slot.Index, _slotSize);
    }

    #endregion
    /*************************************************
     *                Private Methods
     *************************************************/
    #region [+]


    #endregion
}
