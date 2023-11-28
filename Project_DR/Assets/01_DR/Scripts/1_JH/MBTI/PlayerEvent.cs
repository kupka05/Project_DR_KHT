using BNG;
using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent : MonoBehaviour
{


    // 그랩한 아이템을 체크하는 이벤트
    public void GrabCheck(Grabbable grabItem)
    {
        // MBTI 체크 아이템인지 확인
        if(grabItem.GetComponent<MBTIChecker>() != null)
        {
            grabItem.GetComponent<MBTIChecker>().GrabEvent();
        }

        // 인벤토리에 넣는 아이템인지 확인
        if (grabItem.GetComponent<ItemColliderHandler>() != null)
        {
            grabItem.GetComponent<ItemColliderHandler>().state = ItemColliderHandler.State.Stop;
        }

        // 인벤토리인지 확인
        if (grabItem.GetComponent<PlayerInventory>() != null)
        {
            grabItem.GetComponent<PlayerInventory>().OpenInventory();
        }
        Debug.Log("체크");

        // 아이템 슬롯인지 확인
        if (grabItem.GetComponent<ItemSlotController>() != null)
        {
            Debug.Log("아이템 슬롯");
            // 콜라이더가 인벤토리 스크롤 패널 안에 있을 경우 반환
            if (!CheckColliderVisibility(grabItem, grabItem.GetComponent<RectTransform>()) == true)
            { return; }

            Debug.Log($"아이템 ID: {grabItem.transform.parent.name}");

            GameObject grabber = grabItem.GetComponent<SpawnItemSlot>().curGrabber;
            if(grabber == null)
            { return; }
            Debug.Log($"핸드 : {grabber.name}");

            Transform slot = grabItem.transform.parent;
            GameObject item = ItemManager.instance.CreateItem(grabber.transform.position, 5001);
            //grabber.GetComponent<Grabber>().GrabGrabbable();
            grabItem.DropItem(grabber.GetComponent<Grabber>(),false, true);
            grabber.GetComponent<Grabber>().TryRelease();
            grabber.GetComponent<Grabber>().GrabGrabbable(item.GetComponent<Grabbable>());
            ItemColliderHandler itemColliderHandler = item.GetComponent<ItemColliderHandler>();
            itemColliderHandler.state = ItemColliderHandler.State.Stop;

        }
    }

    // 콜라이더 체크
    private bool CheckColliderVisibility(Grabbable grabItem, RectTransform other)
    {
        // TODO : 최적화 필요
        RectTransform scrollPanel = grabItem.transform.parent.parent.parent.parent.parent.parent.GetComponent<RectTransform>();
        // 현재 객체가 스크롤 패널 내에 있는지 여부 확인
        bool isVisible = RectTransformUtility.RectangleContainsScreenPoint(scrollPanel, other.position);

        return isVisible;
    }

    // 아이템 놓는 상태
    public void ReleaseItem(Grabbable grabItem)
    {
        if (grabItem.GetComponent<ItemColliderHandler>() != null)
        {
            grabItem.GetComponent<ItemColliderHandler>().state = ItemColliderHandler.State.Default;
        }
    }


   





}
