using System.Collections.Generic;
using UnityEngine;
using Rito.InventorySystem;
using System;

public class ItemManager : MonoBehaviour
{
    /*************************************************
    *                 Private Fields
    *************************************************/
    #region [+]
    // 싱글톤
    private static ItemManager _instance;
    public static ItemManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ItemManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ItemManager");
                    _instance = obj.AddComponent<ItemManager>();
                }
            }
            return _instance;
        }
    }

    [SerializeField] private Inventory _inventory;

    #endregion
    /*************************************************
    *                 Public Fields
    *************************************************/
    #region [+]
    public Inventory inventory => _inventory;

    #endregion
    /*************************************************
    *                 Unity Events
    *************************************************/
    #region [+]
    private void Awake()
    {
        // 파괴 방지
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        // 아이템 DB Init
        ItemDataManager.InitItemDB();

        // 테스트용 포션 생성
        CreateItem(5001, 10);
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

    // 자동으로 타입을 찾아서 인벤토리에 아이템을 생성
    public void InventoryCreateItem(int id, int amount = 1)
    {
        try
        {
            // 생성할 아이템이 Potion 타입일 경우
            if (ItemDataManager.SearchItemDB<PortionItemData>(id))
            {
                InventoryCreatePotionItem(id, amount);
            }

            // 생성할 아이템이 Bomb 타입일 경우
            else if (ItemDataManager.SearchItemDB<BombItemData>(id))
            {
                InventoryCreateBombItem(id, amount);
            }

            // 생성할 아이템이 Material 타입일 경우
            else if (ItemDataManager.SearchItemDB<MaterialItemData>(id))
            {
                InventoryCreateMaterialItem(id, amount);
            }

            // 생성할 아이템이 Quest 타입일 경우
            else 
            {
                InventoryCreateQuestItem(id, amount);
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"오류 발생! / ItemManager.InventoryCreateItem() {ex.Message}");
        }
    }

    // 포션 아이템 생성
    public void InventoryCreatePotionItem(int id, int amount = 1)
    {
        PortionItemData data = ItemDataManager.SearchItemDB<PortionItemData>(id);
        _inventory.Add(data, amount);
    }

    // 폭탄 아이템 생성
    public void InventoryCreateBombItem(int id, int amount = 1)
    {
        BombItemData data = ItemDataManager.SearchItemDB<BombItemData>(id);
        _inventory.Add(data, amount);
    }

    // 재료 아이템 생성
    public void InventoryCreateMaterialItem(int id, int amount = 1)
    {
        MaterialItemData data = ItemDataManager.SearchItemDB<MaterialItemData>(id);
        _inventory.Add(data, amount);
    }

    // 퀘스트 아이템 생성
    public void InventoryCreateQuestItem(int id, int amount = 1)
    {
        QuestItemData data = ItemDataManager.SearchItemDB<QuestItemData>(id);
        _inventory.Add(data, amount);
    }

    // 자동으로 타입을 찾아서 아이템을 생성
    public GameObject CreateItem(int id, int amount = 1)
    {
        /////////////////////////////////////////////
        // amount 값이 변해도 하나만 생성하도록 고정함
        /////////////////////////////////////////////
        try
        {
            GameObject item = default;
            // 생성할 아이템이 Potion 타입일 경우
            if (ItemDataManager.SearchItemDB<PortionItemData>(id))
            {
                item = CreatePotionItem(id, amount);
            }

            // 생성할 아이템이 Bomb 타입일 경우
            else if (ItemDataManager.SearchItemDB<BombItemData>(id))
            {
                item =CreateBombItem(id, amount);
            }

            // 생성할 아이템이 Material 타입일 경우
            else if (ItemDataManager.SearchItemDB<MaterialItemData>(id))
            {
                item =CreateMaterialItem(id, amount);
            }

            // 생성할 아이템이 Quest 타입일 경우
            else
            {
                item = CreateQuestItem(id, amount);
            }

            return item;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"오류 발생! / ItemManager.CreateItem() {ex.Message}");
            return new GameObject();
        }
    }

    public GameObject CreatePotionItem(int id, int amount = 1)
    {
        /////////////////////////////////////////////
        // amount 값이 변해도 하나만 생성하도록 고정함
        /////////////////////////////////////////////
        PortionItemData data = ItemDataManager.SearchItemDB<PortionItemData>(id);
        GameObject item = Instantiate(data.Prefab);
        item.name = data.Name;
        item.AddComponent<ItemColliderHandler>();
        // Monobehaviour을 상속받지 않아 ItemDataComponent<T>로 우회해서
        // 정보를 등록함
        ItemDataComponent itemData = 
            item.AddComponent<ItemDataComponent>();
        itemData.Initialize(data);
        //PortionItemData portionItemData = (PortionItemData)itemData.ItemData;

        return item;
    }

    public GameObject CreateBombItem(int id, int amount = 1)
    {
        /////////////////////////////////////////////
        // amount 값이 변해도 하나만 생성하도록 고정함
        /////////////////////////////////////////////
        BombItemData data = ItemDataManager.SearchItemDB<BombItemData>(id);
        GameObject item = Instantiate(data.Prefab);
        item.name = data.Name;
        item.AddComponent<ItemColliderHandler>();
        // Monobehaviour을 상속받지 않아 ItemDataComponent<T>로 우회해서
        // 정보를 등록함
        ItemDataComponent itemData =
            item.AddComponent<ItemDataComponent>();
        itemData.Initialize(data);
        //BombItemData bombItemData = (BombItemData)itemData.ItemData;

        return item;
    }

    public GameObject CreateMaterialItem(int id, int amount = 1)
    {
        /////////////////////////////////////////////
        // amount 값이 변해도 하나만 생성하도록 고정함
        /////////////////////////////////////////////
        MaterialItemData data = ItemDataManager.SearchItemDB<MaterialItemData>(id);
        GameObject item = Instantiate(data.Prefab);
        item.name = data.Name;
        item.AddComponent<ItemColliderHandler>();
        // Monobehaviour을 상속받지 않아 ItemDataComponent<T>로 우회해서
        // 정보를 등록함
        ItemDataComponent itemData =
            item.AddComponent<ItemDataComponent>();
        itemData.Initialize(data);
        //MaterialItemData materialItemData = (MaterialItemData)itemData.ItemData;

        return item;
    }

    public GameObject CreateQuestItem(int id, int amount = 1)
    {
        /////////////////////////////////////////////
        // amount 값이 변해도 하나만 생성하도록 고정함
        /////////////////////////////////////////////
        QuestItemData data = ItemDataManager.SearchItemDB<QuestItemData>(id);
        GameObject item = Instantiate(data.Prefab);
        item.name = data.Name;
        item.AddComponent<ItemColliderHandler>();
        // Monobehaviour을 상속받지 않아 ItemDataComponent<T>로 우회해서
        // 정보를 등록함
        ItemDataComponent itemData =
            item.AddComponent<ItemDataComponent>();
        itemData.Initialize(data);
        //QuestItemData questItemData = (QuestItemData)itemData.ItemData;

        return item;
    }

    #endregion
    /*************************************************
    *                 Private Methods
    *************************************************/
    #region [+]


    #endregion
}
