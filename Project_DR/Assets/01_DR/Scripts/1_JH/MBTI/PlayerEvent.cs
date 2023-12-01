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
    }

    // 그랩 사이클이 끝나고 체크
    public void AfterGrabCheck(Grabbable grabItem)
    {
        //아이템 생성 디버그
        GameObject itemaaa = ItemManager.instance.CreateItem(5001);
        GameObject itemaaa2 = ItemManager.instance.CreateItem(5002);
        GameObject itemaaa3 = ItemManager.instance.CreateItem(5003);
        GameObject itemaaa24 = ItemManager.instance.CreateItem(5101);
        GameObject itemaaa25 = ItemManager.instance.CreateItem(5102);


       
        // 아이템 슬롯인지 확인
        if (grabItem.GetComponent<ItemSlotController>() != null)
        {

            // 콜라이더가 인벤토리 스크롤 패널 안에 있을 경우 반환
            if (!CheckColliderVisibility(grabItem, grabItem.GetComponent<RectTransform>()) == true)
            { return; }

            GameObject grabber = grabItem.GetComponent<SpawnItemSlot>().curGrabber;

            if (grabber == null)
            { return; }


        ////// TODO: 아이템 생성을 슬롯에 있는 데이터를 받아와서 생성되게 변경함
        ///
            // 인벤토리 참조
            Inventory inventory = grabItem.GetComponent<ItemSlotController>().Inventory;
            int slotIndex = grabItem.GetComponent<ItemSlotController>().Index; // 슬롯 인덱스
            // 슬롯이 비어있을 경우
            if (inventory.HasItem(slotIndex) == false)
            {
                return;
            }

            // 아이템 생성 (슬롯에서 꺼낼 때 1개씩 꺼내는걸로 고정되서 아래와 같이 구현했다.)
            int itemID = inventory.GetItemData(slotIndex).ID;
            int itemAmount = inventory.GetCurrentAmount(slotIndex);
            GameObject item = ItemManager.instance.CreateItem(grabber.transform.position,
                itemID, itemAmount);
            ItemColliderHandler itemColliderHandler = item.GetComponent<ItemColliderHandler>();
            itemColliderHandler.state = ItemColliderHandler.State.Stop;

            // 아이템 수량(1) 감소
            inventory.Use(slotIndex);

            // 들고있던 아이템 놔주기
            grabItem.DropItem(grabber.GetComponent<Grabber>(), true, false);

            // 생성한 아이템 다시 잡기
            grabber.GetComponent<Grabber>().TryRelease();
            grabber.GetComponent<Grabber>().GrabGrabbable(item.GetComponent<Grabbable>());
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
            grabItem.GetComponent<ItemColliderHandler>().state = ItemColliderHandler.State.Grabbed;
        }
    }
}
