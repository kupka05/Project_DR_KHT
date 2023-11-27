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
    [SerializeField] int _index;

    #endregion
    /*************************************************
     *                Private Fields
     *************************************************/
    #region [+]
    public ItemData ItemData => _itemData;

    #endregion
    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    public int Index => _index;

    #endregion
    /*************************************************
     *                Public Methods
     *************************************************/
    #region [+]
    // Initalize
    public void Initialize(int id)
    {
        _itemData = ItemDataManager.SearchItemDB<ItemData>(id);
        _name.text = _itemData.Name;
        _iconSprite.sprite = _itemData.IconSprite;
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
