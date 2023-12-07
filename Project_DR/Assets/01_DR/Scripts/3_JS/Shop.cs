using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    /*************************************************
     *                 Public Fields
     *************************************************/
    #region [+]
    public ShopItemPurchaseHandler ShopItemPurchaseHandler => _shopItemPurchaseHandler;

    #endregion
    /*************************************************
     *                 Private Fields
     *************************************************/
    #region [+]
    [SerializeField] private ShopItemPurchaseHandler _shopItemPurchaseHandler
        = new ShopItemPurchaseHandler();
    [SerializeField] private ShopPlayerGoldTextController _playerGoldText = default;
    [SerializeField] private List<ShopItemReference> _shopItems = default;

    // 인스펙터에서 ShopItem과 ID를 설정하기 위해
    // 직렬화 클래스 생성
    [System.Serializable]
    private class ShopItemReference
    {
        public ShopItem ShopItem => _shopItem;
        public int ID => _id;

        [SerializeField] private ShopItem _shopItem = default;
        [SerializeField] private int _id = default;
    }

    #endregion
    /*************************************************
     *                  Unity Events
     *************************************************/
    #region [+]
    void Start()
    {
        // 옵저버 등록
        UserDataManager.Instance.OnUserDataUpdate += UpdatePlayerGoldText;

        // _shopItems에 컴포넌트를 연결
        ConnectItemsToComponent();

        // _shopItems에 Shop을 연결
        ConnectItemsToShop();

        // _shopItems에 ID를 Init
        InitializeShopItemIDs();
    }   

    #endregion
    /*************************************************
     *                 Public Methods
     *************************************************/
    #region [+]
    // 플레이어 골드 안내 텍스트 갱신
    public void UpdatePlayerGoldText()
    {
        Debug.Log("UpdatePlayerGoldText()");
        _playerGoldText.GetDataAndSetText();
    }

    #endregion
    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    // _shopItems에 ID를 Init한다
    private void InitializeShopItemIDs()
    {
        for (int i = 0; i < _shopItems.Count; i++)
        {
            int id = _shopItems[i].ID;
            _shopItems[i].ShopItem.InitializeID(id);
        }
    }

    // _shopItems에 컴포넌트를 연결한다
    private void ConnectItemsToComponent()
    {
        for (int i = 0; i < _shopItems.Count; i++)
        {
            _shopItems[i].ShopItem.AddComponentsToChildren();
        }
    }

    // _shopItems에 Shop을 연결한다
    private void ConnectItemsToShop()
    {
        for (int i = 0; i < _shopItems.Count; i++)
        {
            _shopItems[i].ShopItem.ConnectShop(this);
        }
    }

    #endregion
}
