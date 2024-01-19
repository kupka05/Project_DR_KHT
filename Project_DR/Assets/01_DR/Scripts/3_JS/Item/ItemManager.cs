using System.Collections.Generic;
using UnityEngine;
using Rito.InventorySystem;
using System;
using System.Collections;
using Js.Quest;
using BNG;

public class ItemManager : MonoBehaviour
{
    /*************************************************
    *                 Public Fields
    *************************************************/
    #region 싱글톤
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
    private float overItemResetStateDelay = 5f;     // 초과된 아이템 상태 리셋(슬롯 가능) 딜레이
    #endregion
    public Inventory Inventory => _inventory;


    /*************************************************
    *                 Unity Events
    *************************************************/
    private void Awake()
    {
        // 파괴 방지
        //DontDestroyOnLoad(this);

        // 아이템 DB Init
        ItemDataManager.InitItemDB();
    }


    /*************************************************
    *                 Public Methods
    *************************************************/
    #region [+]
    public void ConnectInventory(Inventory inventory)
    {
        _inventory = inventory;
    }

    // ID로 인벤토리에 있는 아이템을 삭제
    public bool RemoveInventoryItemForID(int id, int amount)
    {
        return _inventory.RemoveInventoryItemForID(id, amount);
    }

    // 자동으로 타입을 찾아서 인벤토리에 아이템을 생성
    public void InventoryCreateItem(Vector3 handPos, int id, int amount = 1)
    {
        try
        {
            // 생성할 아이템이 Potion 타입일 경우
            if (ItemDataManager.SearchItemDB<PortionItemData>(id))
            {
                InventoryCreatePotionItem(handPos, id, amount);
            }

            // 생성할 아이템이 Bomb 타입일 경우
            else if (ItemDataManager.SearchItemDB<BombItemData>(id))
            {
                InventoryCreateBombItem(handPos, id, amount);
            }

            // 생성할 아이템이 Material 타입일 경우
            else if (ItemDataManager.SearchItemDB<MaterialItemData>(id))
            {
                InventoryCreateMaterialItem(handPos, id, amount);
            }

            // 생성할 아이템이 Quest 타입일 경우
            else
            {
                InventoryCreateQuestItem(handPos, id, amount);
            }

            // 인벤토리 정렬 및 PlayerInventoryUI 새로고침
            _inventory.SortAndUpdatePlayerInventoryUI();

            // 인벤토리 추가 효과음 출력
            AudioManager.Instance.AddSFX("SFX_Inventory_Acquirement");
            AudioManager.Instance.PlaySFX("SFX_Inventory_Acquirement");

        }
        catch (Exception ex)
        {
            GFunc.LogWarning($"오류 발생! / ItemManager.InventoryCreateItem() {ex.Message}");
        }
    }

    // 포션 아이템 생성
    public void InventoryCreatePotionItem(Vector3 handPos, int id, int amount = 1)
    {
        PortionItemData data = ItemDataManager.SearchItemDB<PortionItemData>(id);

        // 인벤토리에 아이템 추가 & 초과 수량 저장
        int overCount = _inventory.Add(data, amount);

        // 인벤토리가 가득 찼을 경우
        if (CheckOverInventorySlot(overCount))
        {
            // 초과분 만큼 아이템 생성
            CreateOverItem(handPos, id, overCount);
        }

        // 퀘스트 콜백 호출
        QuestCallback.OnInventoryCallback(id);
    }

    // 폭탄 아이템 생성
    public void InventoryCreateBombItem(Vector3 handPos, int id, int amount = 1)
    {
        BombItemData data = ItemDataManager.SearchItemDB<BombItemData>(id);

        // 인벤토리에 아이템 추가 & 초과 수량 저장
        int overCount = _inventory.Add(data, amount);

        // 인벤토리가 가득 찼을 경우
        if (CheckOverInventorySlot(overCount))
        {
            // 초과분 만큼 아이템 생성
            CreateOverItem(handPos, id, overCount);
        }

        // 퀘스트 콜백 호출
        QuestCallback.OnInventoryCallback(id);
    }

    // 재료 아이템 생성
    public void InventoryCreateMaterialItem(Vector3 handPos, int id, int amount = 1)
    {
        MaterialItemData data = ItemDataManager.SearchItemDB<MaterialItemData>(id);

        // 인벤토리에 아이템 추가 & 초과 수량 저장
        int overCount = _inventory.Add(data, amount);

        // 인벤토리가 가득 찼을 경우
        if (CheckOverInventorySlot(overCount))
        {
            // 초과분 만큼 아이템 생성
            CreateOverItem(handPos, id, overCount);
        }

        // 퀘스트 콜백 호출
        QuestCallback.OnInventoryCallback(id);
    }

    // 퀘스트 아이템 생성
    public void InventoryCreateQuestItem(Vector3 handPos, int id, int amount = 1)
    {
        QuestItemData data = ItemDataManager.SearchItemDB<QuestItemData>(id);

        // 인벤토리에 아이템 추가 & 초과 수량 저장
        int overCount = _inventory.Add(data, amount);

        // 인벤토리가 가득 찼을 경우
        if (CheckOverInventorySlot(overCount))
        {
            // 초과분 만큼 아이템 생성
            CreateOverItem(handPos, id, overCount);
        }

        // 퀘스트 콜백 호출
        QuestCallback.OnInventoryCallback(id);
    }

    // 자동으로 타입을 찾아서 아이템을 생성
    public GameObject CreateItem(int id, int amount = 1)
    {
        return CreateItem(Vector3.zero, id, amount);
    }

    // 크래프팅 출력용 임시 아이템 생성
    public GameObject CreateTempItem(Vector3 pos, int id, Transform parent, int amount = 1)
    {
        // 아이템 생성 & 컴포넌트 끄기
        GameObject item = CreateItem(pos, id, amount, true);
        item.transform.SetParent(parent);
        item.transform.localPosition = pos;
        ItemColliderHandler itemColliderHandler = item.GetComponent<ItemColliderHandler>();
        Grabbable grabbable = item.GetComponent<Grabbable>();
        if (itemColliderHandler != null) { itemColliderHandler.enabled = false; }
        if (grabbable != null) { grabbable.enabled = false; }

        return item;
    }

    // 상점용 구매 아이템 생성
    public GameObject CreateShopItem(Vector3 pos, int id, int amount = 1, bool isTempItem = false)
    {
        // 아이템 생성 & 아이템 사용 불가능하게 설정
        GameObject item = CreateItem(pos, id, amount, isTempItem);
        UseItem useItem = item.GetComponent<UseItem>();
        if (useItem != null)
        {
            Destroy(useItem);
        }

        return item;
    }

    // 아이템 생성
    public GameObject CreateItem(Vector3 pos, int id, int amount = 1, bool isTempItem = false)
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
                item = CreatePotionItem(pos, id, amount);
            }

            // 생성할 아이템이 Bomb 타입일 경우
            else if (ItemDataManager.SearchItemDB<BombItemData>(id))
            {
                item = CreateBombItem(pos, id, amount);
            }

            // 생성할 아이템이 Material 타입일 경우
            else if (ItemDataManager.SearchItemDB<MaterialItemData>(id))
            {
                item = CreateMaterialItem(pos, id, amount);
            }

            // 생성할 아이템이 Quest 타입일 경우
            else if (ItemDataManager.SearchItemDB<QuestItemData>(id))
            {
                item = CreateQuestItem(pos, id, amount);
            }

            // 크래프팅 임시 결과 아이템이 아닐 경우 네임태그 추가
            if (isTempItem.Equals(false))
            {
                CreateNameTag(item);
            }

            return item;
        }
        catch (Exception ex)
        {
            GFunc.LogWarning($"오류 발생! / ItemManager.CreateItem() {ex.Message}");
            return new GameObject();
        }
    }

    public GameObject CreatePotionItem(Vector3 handPos, int id, int amount = 1)
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
        ItemDataComponent itemData = item.AddComponent<ItemDataComponent>();
        itemData.Initialize(data);
        SetOverItem(handPos, item);
        //PortionItemData portionItemData = (PortionItemData)itemData.ItemData;

        return item;
    }

    public GameObject CreateBombItem(Vector3 handPos, int id, int amount = 1)
    {
        /////////////////////////////////////////////
        // amount 값이 변해도 하나만 생성하도록 고정함
        /////////////////////////////////////////////
        BombItemData data = ItemDataManager.SearchItemDB<BombItemData>(id);
        GameObject item = Instantiate(data.Prefab);
        item.AddComponent<ItemColliderHandler>();
        // Monobehaviour을 상속받지 않아 ItemDataComponent<T>로 우회해서
        // 정보를 등록함
        ItemDataComponent itemData = item.AddComponent<ItemDataComponent>();
        item.AddComponent<ItemBombHandler>(); 
        itemData.Initialize(data);
        item.name = data.Name;
        SetOverItem(handPos, item);
        //BombItemData bombItemData = (BombItemData)itemData.ItemData;

        return item;
    }

    public GameObject CreateMaterialItem(Vector3 handPos, int id, int amount = 1)
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
        ItemDataComponent itemData = item.AddComponent<ItemDataComponent>();
        itemData.Initialize(data);
        SetOverItem(handPos, item);
        //MaterialItemData materialItemData = (MaterialItemData)itemData.ItemData;

        return item;
    }

    public GameObject CreateQuestItem(Vector3 handPos, int id, int amount = 1)
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
        ItemDataComponent itemData = item.AddComponent<ItemDataComponent>();
        itemData.Initialize(data);
        SetOverItem(handPos, item);
        //QuestItemData questItemData = (QuestItemData)itemData.ItemData;

        return item;
    }

    // 아이템에 네임택을 넣어주는 메서드
    public void CreateNameTag(GameObject item)
    {
        if(item.name.Equals(""))
        {
            GFunc.Log(item.name + " 해당 아이템에 이름이 없습니다.");
            return;
        }

        GameObject ItemTag = Resources.Load<GameObject>("Prefabs/Item_NameTag");
        GameObject itemTag = Instantiate(ItemTag, item.transform.position, item.transform.rotation);
        itemTag.transform.localScale =  Vector3.one;
        itemTag.transform.parent = item.transform;
        itemTag.GetComponent<ItemNameTag>().SetName(item.name);
        itemTag.GetComponent<ItemNameTag>().SetPosition(item.transform);
    }

    #endregion


    /*************************************************
    *                 Private Methods
    *************************************************/
    // 인벤토리가 가득 찼는지 체크하는 함수
    private bool CheckOverInventorySlot(int num)
    {
        // 가득 찼을 경우
        if (num > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 초과된 만큼 아이템을 생성하는 함수
    private void CreateOverItem(Vector3 handPos, int id, int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateItem(handPos, id);
        }
    }

    // 생성된 아이템의 설정을 변경하는 함수
    private void SetOverItem(Vector3 handPos, GameObject item)
    {
        ItemColliderHandler itemColliderHandler = item.GetComponent<ItemColliderHandler>();

        // 슬롯에 넣을 수 없도록 아이템 상태 Stop으로 변경
        itemColliderHandler.state = ItemColliderHandler.State.STOP;

        // hand 위치로 포지션 이동
        item.transform.position = handPos;

        // n초 후에 슬롯에 들어가도록 설정
        itemColliderHandler.Coroutine(itemColliderHandler.ResetState, overItemResetStateDelay);
    }
    private void NewInvoke(Action func, float t)
    {
        StartCoroutine(RunFuncToCoroutine(func, t));
    }


    /*************************************************
     *               Private Methods
     *************************************************/
    // 함수를 코루틴으로 실행하는 코루틴 함수
    private IEnumerator RunFuncToCoroutine(Action func, float t)
    {
        yield return new WaitForSeconds(t);
        func();
    }
}