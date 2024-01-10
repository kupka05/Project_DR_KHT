using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Rito.InventorySystem;
using UnityEngine.UI;

public class ItemSlotPanelUI : MonoBehaviour
{
    /*************************************************
     *                Public Fields
     *************************************************/

    public ItemData ItemData => _itemData;
    public int Index => _index;
    public int ItemIndex => _itemIndex;


    /*************************************************
     *                Private Fields
     *************************************************/

    [SerializeField] private ItemSlotController _itemSlot;
    [SerializeField] private ItemData _itemData;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _iconSprite;
    [SerializeField] private TMP_Text _count;
    [SerializeField] private int _index = -1;       // 패널 슬롯 인덱스
    [SerializeField] private int _itemIndex = -1;   // 보관중인 아이템 인덱스
   

    /*************************************************
     *                Public Methods
     *************************************************/
    public void Initialize(int id, int amount, int maxAmount, int index, int itemIndex)
    {
        // Init
        // id가 0일 경우 리셋
        if (id == 0)
        {
            _itemData = default;
            _name.text = default;
            _count.text = "0 / 0";
            _iconSprite.sprite = default;
            _index = -1;
            _itemSlot.SetIndex(_index);

            return;
        }
        _itemData = ItemDataManager.SearchItemDB<ItemData>(id);
        _name.text = _itemData.Name;
        _iconSprite.sprite = _itemData.IconSprite;
        string countText = amount + " / " + maxAmount;
        UpdateCountText(countText);
        _index = index;
        _itemSlot.SetIndex(_index);
        _itemSlot.SetItemIndex(itemIndex);
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void SetItemIndex(int index)
    {
        _itemIndex = index;
    }

    // 카운트 텍스트 갱신
    public void UpdateCountText(string text)
    {
        _count.text = text;
    }


    /*************************************************
     *                Private Methods
     *************************************************/

}
