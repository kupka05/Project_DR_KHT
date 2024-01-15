using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito.InventorySystem;

public class PlayerItemSlotController : MonoBehaviour
{
    /*************************************************
     *                 Private Fields
     *************************************************/
    #region [+]
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject itemSlot;
    private BoxCollider boxCollider;
    [SerializeField] private int _index;
    [SerializeField] private int _itemIndex;

    [SerializeField] private bool _isChangeSize = true;
    // 슬롯에 수납 가능 여부
    [SerializeField] private bool _isStorageAvailable = true;
    [SerializeField] private bool _isPlayerStorage = false;

    #endregion
    /*************************************************
     *                 Public Fields
     *************************************************/
    #region [+]
    public Inventory Inventory => _inventory;
    public bool IsStorageAvailable => _isStorageAvailable;
    public bool IsPlayerStorage => _isPlayerStorage;
    public int Index => _index;
    public int ItemIndex => _itemIndex;


    #endregion
    /*************************************************
     *                 Unity Events
     *************************************************/
    #region [+]
    void Start()
    {
        if (itemSlot == null && _isPlayerStorage == false)
        {
            itemSlot = GetParentGameObject(transform);
        }

        boxCollider = gameObject.GetComponent<BoxCollider>();

        if (_isChangeSize)
        {
            SetBoxColliderSize(GetSizeVector2(itemSlot), boxCollider);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GFunc.Log("Collision Enter");
    }

    #endregion
    /*************************************************
     *                Private Methods
     *************************************************/
    #region [+]
    public void SetIndex(int index)
    {
        _index = index;
    }

    public void SetItemIndex(int index)
    {
        _itemIndex = index;
    }

    #endregion
    /*************************************************
     *                Private Methods
     *************************************************/
    #region [+]
    // 부모의 게임 오브젝트를 가져온다
    private GameObject GetParentGameObject(Transform child)
    {
        Transform parent = child.parent;

        return parent.gameObject;
    }

    // 오브젝트의 Rect Transform Vector2 사이즈를 가져온다
    private Vector2 GetSizeVector2(GameObject gameObject)
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        return rect.sizeDelta;
    }

    // 박스 콜라이더의 사이즈(Vector2)를 변경한다.
    private void SetBoxColliderSize(Vector2 sizeDelta, BoxCollider boxCollider, float z = 0f)
    {
        Vector3 size = new Vector3(sizeDelta.x, sizeDelta.y, z);
        boxCollider.size = size;
    }

    #endregion
}
