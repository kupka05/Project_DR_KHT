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

        // _shopItems를 Init
        InitializeShopItem();

        // 골드 텍스트 갱신
        UpdatePlayerGoldText();
    }   

    #endregion
    /*************************************************
     *                 Public Methods
     *************************************************/
    #region [+]
    // 플레이어 골드 안내 텍스트 갱신
    public void UpdatePlayerGoldText()
    {
        // null일경우 예외처리
        // 재시작 할 때마다, 기존에 등록된 이벤트가 null인
        // 텍스트를 호출해서 오류가 발생해 추가하였음
        if (_playerGoldText.Equals(null)) { return; }
        _playerGoldText.GetDataAndSetText();
    }

    #endregion
    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    // _shopItems를 Init한다
    private void InitializeShopItem()
    {
        for (int i = 0; i < _shopItems.Count; i++)
        {
            int id = _shopItems[i].ID;
            _shopItems[i].ShopItem.Initialize(id);
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
