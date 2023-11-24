using System.Collections.Generic;
using UnityEngine;
using Rito.InventorySystem;
using System;

public class ItemManager : MonoBehaviour
{
    /*************************************************
    *                 Public Fields
    *************************************************/
    #region [+]
    public Inventory inventory => _inventory;

    #endregion
    /*************************************************
    *                 Private Fields
    *************************************************/
    #region [+]
    [SerializeField] private Inventory _inventory;

    #endregion
    /*************************************************
    *                 Unity Events
    *************************************************/
    #region [+]
    private void Start()
    {
        // 아이템 DB Init
        ItemDataManager.InitItemDB();

        // 테스트용 포션 생성
        CreateItem(5001, 13);
    }

    #endregion
    /*************************************************
    *                 Public Methods
    *************************************************/
    #region [+]
    public void ConnectInventory(Inventory inventory)
    {
        _inventory = inventory;
    }

    // 자동으로 타입을 찾아서 아이템을 생성
    public void CreateItem(int id, int amount = 1)
    {
        try
        {
            // 생성할 아이템이 Potion 타입일 경우
            if (ItemDataManager.SearchItemDB<PortionItemData>(id))
            {
                CreatePotionItem(id, amount);
            }

            // 생성할 아이템이 Bomb 타입일 경우
            else if (ItemDataManager.SearchItemDB<BombItemData>(id))
            {
                CreateBombItem(id, amount);
            }

            // 생성할 아이템이 Material 타입일 경우
            else if (ItemDataManager.SearchItemDB<MaterialItemData>(id))
            {
                CreateMaterialItem(id, amount);
            }

            // 생성할 아이템이 Quest 타입일 경우
            else 
            {
                CreateQuestItem(id, amount);
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"오류 발생! / ItemManager.CreateItem() {ex.Message}");
        }
    }

    // 포션 아이템 생성
    public void CreatePotionItem(int id, int amount = 1)
    {
        PortionItemData data = ItemDataManager.SearchItemDB<PortionItemData>(id);
        _inventory.Add(data, amount);
    }

    // 폭탄 아이템 생성
    public void CreateBombItem(int id, int amount = 1)
    {
        BombItemData data = ItemDataManager.SearchItemDB<BombItemData>(id);
        _inventory.Add(data, amount);
    }

    // 재료 아이템 생성
    public void CreateMaterialItem(int id, int amount = 1)
    {
        MaterialItemData data = ItemDataManager.SearchItemDB<MaterialItemData>(id);
        _inventory.Add(data, amount);
    }

    // 퀘스트 아이템 생성
    public void CreateQuestItem(int id, int amount = 1)
    {
        QuestItemData data = ItemDataManager.SearchItemDB<QuestItemData>(id);
        _inventory.Add(data, amount);
    }

    #endregion
    /*************************************************
    *                 Private Methods
    *************************************************/
    #region [+]


    #endregion
}
