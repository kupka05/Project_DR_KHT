using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopItem : MonoBehaviour
{
    /*************************************************
     *                 Public Fields
     *************************************************/
    #region [+]
    public int ID => _id;
    public ShopItemText ShopItemText => _shopItemText;
    public Collider ShopItemCollider => _shopItemCollider;
    public bool IsItem => _isItem;
    public Shop Shop => _shop;

    #endregion
    /*************************************************
     *                Private Fields
     *************************************************/
    #region [+]
    [SerializeField] private int _id = default;
    [SerializeField] private ShopItemText _shopItemText;
    [SerializeField] private Collider _shopItemCollider;
    [SerializeField] private bool _isItem;
    private const int ITEM_ID_MAX_RANGE = 5999;
    private Shop _shop;

    #endregion
    /*************************************************
     *                 Public Methods
     *************************************************/
    #region [+]
    // Init
    public void Initialize(int id)
    {
        // id 할당
        _id = id;

        // _isItem 확인
        _isItem = CheckIsItem(_id);

        // _shopItemText Init
        _shopItemText.Initialize(_id);

        // _shopItemText 텍스트 갱신
        _shopItemText.GetDataAndSetText();
    }

    // 상점을 연결한다
    public void ConnectShop(Shop shop)
    {
        _shop = shop;
    }

    // 자식 GameObject를 순회하면서 컴포넌트를 연결한다.
    // 1 계층만 순회한다.
    public void AddComponentsToChildren()
    {
        // 부모
        Transform parent = transform;

        // 1계층 자식 순회
        for (int i = 0; i < parent.childCount; i++)
        {
            // [i]번째 자식 가져오기
            Transform child = parent.GetChild(i);

            // 자식이 ShopItemText 컴포넌트를 가지고 있는지 확인
            ShopItemText shopItemText = child.GetComponent<ShopItemText>();
            if (shopItemText != null)
            {
                // 가지고 있을 경우 할당
                _shopItemText = shopItemText;
            }

            // 자식이 Collider 컴포넌트를 가지고 있는지 확인
            Collider collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                // 가지고 있을 경우 할당
                _shopItemCollider = collider;

                // ShopItemColliderHandler 컴포넌트 추가
                child.gameObject.AddComponent<ShopItemColliderHandler>();

                // Grabbable 컴포넌트 추가 및 프리셋 설정
                Grabbable grabbable = child.gameObject.AddComponent<Grabbable>();
                grabbable.GrabbablePreset();
            }
        }
    }

    #endregion
    /*************************************************
     *                Private Methods
     *************************************************/
    #region [+]
    // 현재 ID에 해당하는 상점 아이템이
    // 실제 아이템인지 확인
    public bool CheckIsItem(int id)
    {
        id = (int)DataManager.Instance.GetData(id, "KeyID", typeof(int));
        if (id < ITEM_ID_MAX_RANGE)
        {
            return true;
        }

        return false;
    }
    #endregion
}
