using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Rito.InventorySystem;
using UnityEngine.UI;

public class ItemSlotPanelUI : MonoBehaviour
{
    /*************************************************
     *                Private Fields
     *************************************************/
    #region [+]
    [SerializeField] private ItemData _itemData;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _iconSprite;
    [SerializeField] private TMP_Text _count;
    [SerializeField] private int _index;

    #endregion
    /*************************************************
     *                Public Fields
     *************************************************/
    #region [+]
    public ItemData ItemData => _itemData;
    public int Index => _index;

    #endregion
    /*************************************************
     *                Public Methods
     *************************************************/
    #region [+]
    // Initalize
    public void Initialize(int id, int amount, int maxAmount, int index)
    {
        // id가 0일 경우 리셋
        if (id == 0)
        {
            _itemData = default;
            _name.text = default;
            _count.text = "0 / 0";
            _iconSprite.sprite = default;
            _index = default;

            return;
        }
        _itemData = ItemDataManager.SearchItemDB<ItemData>(id);
        _name.text = _itemData.Name;
        _iconSprite.sprite = _itemData.IconSprite;
        string countText = amount + " / " + maxAmount;
        UpdateCountText(countText);
        _index = index;
        //switch (ItemDataManager.GetItemType(id))
        //{
        //    // Potion일 경우
        //    case 0:
        //        PortionItemData portionItemData = ItemDataManager.SearchItemDB<PortionItemData>(id);
        //        InitUI(portionItemData);
        //        break;

        //    // Bomb일 경우
        //    case 1:
        //        BombItemData bombItemData = ItemDataManager.SearchItemDB<BombItemData>(id);
        //        InitUI(bombItemData);
        //        break;

        //    // Material일 경우
        //    case 2:
        //        MaterialItemData materialItemData = ItemDataManager.SearchItemDB<MaterialItemData>(id);
        //        InitUI(materialItemData);
        //        break;

        //    // Quest일 경우
        //    case 3:
        //        QuestItemData questItemData = ItemDataManager.SearchItemDB<QuestItemData>(id);
        //        InitUI(questItemData);
        //        break;

        //    default:
        //        Debug.Assert(false);
        //        break;
        //}
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    // 카운트 텍스트 갱신
    public void UpdateCountText(string text)
    {
        _count.text = text;
    }

    #endregion
    /*************************************************
     *                Private Methods
     *************************************************/
    //// UI 정보 Initialize
    //private void InitUI<T>(T data) where T : ItemData
    //{
    //    _name.text = data.Name;
    //    _iconSprite.sprite = data.IconSprite;
    //}
}
