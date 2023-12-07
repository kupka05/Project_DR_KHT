using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemColliderHandler : MonoBehaviour
{
    /*************************************************
     *               Public Fields
     *************************************************/
    #region [+]
    public GameObject curGrabber;

    #endregion
    /*************************************************
     *               Public Fields
     *************************************************/
    #region [+]
    private ShopItem _shopItem;

    #endregion
    /*************************************************
     *                Unity Events
     *************************************************/
    #region [+]
    private void OnTriggerEnter(Collider other)
    {
        Grabber grabber = other.GetComponent<Grabber>();
        if (grabber)
        {
            curGrabber = other.gameObject;

            // 잡은 손에 진동 처리
            VibrateManager.instance.Vibrate(grabber.HandSide);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == curGrabber)
            curGrabber = null;
    }

    #endregion
    /*************************************************
     *                Public Methods
     *************************************************/
    #region [+]
    public ShopItem GetShopItem()
    {
        return _shopItem ?? GetComponentInParent<ShopItem>();
    }

    #endregion
}
