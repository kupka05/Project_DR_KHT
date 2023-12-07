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

    #endregion
    /*************************************************
     *                Private Fields
     *************************************************/
    #region [+]
    [SerializeField] private int _id = default;
    [SerializeField] private ShopItemText _shopItemText;
    [SerializeField] private Collider _shopItemCollider;
    private Shop _shop;

    #endregion
    /*************************************************
     *                 Unity Events
     *************************************************/
    #region [+]
    private void Start()
    {
        // 자식 GameObject를 순회하면서 컴포넌트를 추가
        //AddComponentsToChildren();
    }

    #endregion
    /*************************************************
     *                 Public Methods
     *************************************************/
    #region [+]
    // ID를 초기화한다
    public void InitializeID(int id)
    {
        _id = id;

        // 콜백 호출
        OnIDChangeCallback();
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
            }
        }
    }

    #endregion
    /*************************************************
     *                Private Methods
     *************************************************/
    #region [+]

    // _id가 변경될 때 호출하는 콜백
    private void OnIDChangeCallback()
    {
        // _shopItemText에 변경된 ID 할당
        _shopItemText.SetID(_id);

        // _shopItemText 텍스트 갱신
        _shopItemText.GetDataAndSetText();
    }

    #endregion
}
