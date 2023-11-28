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
        // 아이템 슬롯인지 확인
        if(grabItem.GetComponent<ItemSlotController>() != null)
        {
            // 콜라이더가 인벤토리 스크롤 패널 안에 있을 경우
            if (CheckColliderVisibility(
                grabItem.transform.parent.parent.parent.parent.parent.parent.GetComponent<RectTransform>(),
                grabItem.GetComponent<RectTransform>()) == true)
            {
                Debug.Log($"name: {grabItem.transform.parent.name}");

                // 상태 변경
            }
        }
    }
    private bool CheckColliderVisibility(RectTransform scrollPanel, RectTransform other)
    {
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
