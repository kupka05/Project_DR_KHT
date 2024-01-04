using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemColliderHandler : MonoBehaviour
{
    /*************************************************
     *               Public Fields
     *************************************************/
    public GameObject curGrabber;


    /*************************************************
     *               Public Fields
     *************************************************/

    private ShopItem _shopItem;


    /*************************************************
     *                Unity Events
     *************************************************/
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


    /*************************************************
     *                Public Methods
     *************************************************/
    public ShopItem GetShopItem()
    {
        return _shopItem ?? GetComponentInParent<ShopItem>();
    }
}
