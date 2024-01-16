using BNG;
using Js.Crafting;
using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent : MonoBehaviour
{

    private void Start()
    {
        AudioManager.Instance.AddSFX("SFX_Shop_Purchase_01");
    }
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
            grabItem.GetComponent<ItemColliderHandler>().state = ItemColliderHandler.State.STOP;
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
        //GameObject itemaaa = ItemManager.instance.CreateItem(5001);
        //GameObject itemaaa2 = ItemManager.instance.CreateItem(5002);
        //GameObject itemaaa3 = ItemManager.instance.CreateItem(5003);
        //GameObject itemaaa24 = ItemManager.instance.CreateItem(5101);
        //GameObject itemaaa25 = ItemManager.instance.CreateItem(5102);

        // 아이템 슬롯일 경우 이벤트 실행
        ItemSlotEvent(grabItem);

        // 상점 아이템일 경우 이벤트 실행
        ShopItemSlotEvent(grabItem);

        // 크래프팅 인벤토리 상자일 경우 이벤트 실행
        CraftingChestEvent(grabItem);
    }


    // 아이템 놓는 상태
    public void ReleaseItem(Grabbable grabItem)
    {
        ItemColliderHandler itemColliderHandler = grabItem.GetComponent<ItemColliderHandler>();
        if (itemColliderHandler != null)
        {
            // 아이템 상태가 크래프팅일 경우 예외처리 
            if (itemColliderHandler.state.Equals(ItemColliderHandler.State.CRAFTING)) { return; }

            itemColliderHandler.state = ItemColliderHandler.State.GRABBED;
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

    // 아이템 슬롯 관련 이벤트

    private void ItemSlotEvent(Grabbable grabItem)
    {
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
            // 아이템 슬롯이 Inventory일 경우
            int slotIndex = default;
            if (grabItem.CompareTag("Inventory"))
            {
                slotIndex = grabItem.GetComponent<ItemSlotController>().Index; // 슬롯 인덱스
                GFunc.Log("Inventory ItemSlot");
            }

            // 플레이어 인벤토리일 경우
            else if (grabItem.CompareTag("PlayerInventory"))
            {
                slotIndex = grabItem.GetComponent<ItemSlotController>().ItemIndex; // 슬롯 보유 아이템 인덱스
                GFunc.Log("PlayerInventory ItemSlot");

            }

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
            itemColliderHandler.state = ItemColliderHandler.State.STOP;

            // 아이템 수량(1) 감소
            inventory.Use(slotIndex);

            // 들고있던 아이템 놔주기
            grabItem.DropItem(grabber.GetComponent<Grabber>(), true, false);

            // 생성한 아이템 다시 잡기
            grabber.GetComponent<Grabber>().TryRelease();
            grabber.GetComponent<Grabber>().GrabGrabbable(item.GetComponent<Grabbable>());
        }
    }

    // 상점 아이템 관련 이벤트
    private void ShopItemSlotEvent(Grabbable grabItem)
    {
        ShopItemColliderHandler shopItemCollider = 
            grabItem.GetComponent<ShopItemColliderHandler>();
        // 상점 아이템인지 확인
        if (shopItemCollider != null)
        {
            GameObject grabber = grabItem.GetComponent<ShopItemColliderHandler>().curGrabber;

            if (grabber == null)
            { return; }

            ShopItem shopItem = shopItemCollider.GetShopItem();
            int shopItemID = shopItem.ID;
            int itemID = (int)DataManager.Instance.GetData(shopItem.ID, "KeyID", typeof(int));
            Shop shop = shopItem.Shop;
            ShopItemPurchaseHandler shopItemPurchaseHandler = shopItem.Shop.ShopItemPurchaseHandler;

            // 패시브일 경우 이미 한 번 구매했다면 구매 못하게 예외 처리
            if (shopItem.IsItem.Equals(false) && shop.IsPurchasedPassiveSkill.Equals(true)) 
            {
                GFunc.Log($"이미 패시브 스킬을 구매했습니다."); return; 
            }

            // 아이템 구매 처리(골드 차감)
            if (shopItemPurchaseHandler.CheckAndDeductGoldForItemPurchase(shopItemID))
            {
                AudioManager.Instance.PlaySFX("SFX_Shop_Purchase_01");

                GFunc.Log("구매했슴당");
                // 아이템일 경우
                if (shopItem.IsItem)
                {
                    GFunc.Log(itemID);
                    GameObject item = Unit.AddShopItem(grabber.transform.position, itemID);
                    ItemColliderHandler itemColliderHandler = item.GetComponent<ItemColliderHandler>();
                    itemColliderHandler.state = ItemColliderHandler.State.STOP;

                    // 들고있던 아이템 놔주기
                    grabItem.DropItem(grabber.GetComponent<Grabber>(), true, false);

                    // 생성한 아이템 다시 잡기
                    grabber.GetComponent<Grabber>().TryRelease();
                    grabber.GetComponent<Grabber>().GrabGrabbable(item.GetComponent<Grabbable>());
                }

                // 패시브 스킬일 경우
                else
                {
                    // 패시브 스킬 적용 & 1번만 구매하게 설정
                    UserData.ActiveSkill(itemID);
                    shop.SetisPurchasedPassiveSkill(true);
                    GFunc.Log($"[{itemID}] 패시브 스킬 구매 완료");

                    // 패시브 아이템이 돌아가는 애니메이션 재생
                    shopItem.transform.parent.GetComponent<Animation>().Play("ShopPassive_Disable");
                }
            }

            // 구매 실패시
            else
            {
                GFunc.Log("돈이 업슴당");
            }
        }
    }

    // 크래프팅 상자 이벤트
    public void CraftingChestEvent(Grabbable grabItem)
    {
        InventoryChest inventoryChest = 
            grabItem.GetComponent<InventoryChest>();
        // 크래프팅 인벤토리 상자일 경우
        if (inventoryChest != null)
        {
            // 상자 초기화 & 토글
            inventoryChest.Initialize(gameObject);
            inventoryChest.ToggleChest();
        }
    }
}
