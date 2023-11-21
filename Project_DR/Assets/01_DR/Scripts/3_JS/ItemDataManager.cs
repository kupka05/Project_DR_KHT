using System;
using System.Collections.Generic;
using UnityEngine;
using Rito.InventorySystem;
using System.Reflection;

public static class ItemDataManager
{
    /*************************************************
    *                 Public Fields
    *************************************************/
    #region [+]
    public static Dictionary<int, PortionItemData> potionItemDB { get; private set; }
    public static Dictionary<int, BombItemData> bombItemDB { get; private set; }
    public static Dictionary<int, MaterialItemData> materialItemDB { get; private set; }
    public static Dictionary<int, QuestItemData> questItemDB { get; private set; }

    #endregion

    /*************************************************
     *                 Private Fields
     *************************************************/
    #region [+]
    private const int POTION_TYPE_ID = 5001;
    private const int BOMB_TYPE_ID = 5101;
    private const int MATERIAL_TYPE_ID = 5201;
    private const int QUEST_TYPE_ID = 5301;
    #endregion

    /*************************************************
    *                 Public Methods
    *************************************************/
    #region [+]
    public static void InitItemDB()
    {
        string data2 = (string)DataManager.GetData(50022221, "Dur222ation", typeof(string));
        Debug.Log($"{data2}");

        // DB 초기화
        potionItemDB = new Dictionary<int, PortionItemData>();
        bombItemDB = new Dictionary<int, BombItemData>();
        materialItemDB = new Dictionary<int, MaterialItemData>();
        questItemDB = new Dictionary<int, QuestItemData>();

        // ItemDB에 Init
        InitTable(POTION_TYPE_ID, "Potion");
        InitTable(BOMB_TYPE_ID, "Bomb");
        InitTable(MATERIAL_TYPE_ID, "Material");
        InitTable(QUEST_TYPE_ID, "Quest");

        // 디버그
        Debug.Log($"TEST: {SearchItemDB<MaterialItemData>(5201).Desc}");
        Debug.Log($"TEST: {SearchItemDB<MaterialItemData>(5206).Desc}");
    }

    #endregion

    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    private static void InitTable(int typeID, string category)
    {
        int size = DataManager.GetCount(typeID);
        for (int i = 0; i < size; i++)
        {
            int id = typeID + i;
            switch (category)
            {
                case "Potion":
                    potionItemDB.Add(id, InitData<PortionItemData>(id, new PortionItemData()));
                    Debug.Log($"ID: {potionItemDB[id].ID}");
                    //ScriptableObjectCreator.SaveScriptableObject(id, potionItemDB[id]);
                    break;

                case "Bomb":
                    bombItemDB.Add(id, InitData<BombItemData>(id, new BombItemData()));
                    Debug.Log($"maxAmount: {bombItemDB[id]._maxAmount}");
                    break;

                case "Material":
                    materialItemDB.Add(id, InitData<MaterialItemData>(id, new MaterialItemData()));
                    Debug.Log($"maxAmount: {materialItemDB[id]._maxAmount}");
                    break;

                case "Quest":
                    questItemDB.Add(id, InitData<QuestItemData>(id, new QuestItemData()));
                    Debug.Log($"maxAmount: {questItemDB[id]._maxAmount}");
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }

    private static T InitData<T>(int id, T data) where T : ItemData
    {
        // itemData가 가지고 있는 기본 프로퍼티
        data._id = (int)DataManager.GetData(id, "ID", typeof(int));
        data._name = (string)DataManager.GetData(id, "Name", typeof(string));
        data._desc = (string)DataManager.GetData(id, "Desc", typeof(string));

        // 자식 클래스에 해당 프로퍼티가 있는지 확인 후 데이터 추가
        if (CheckProperty<ItemData, int>(data, "_maxAmount"))
        {
            SetPropertyIfExists(data, "_maxAmount", (int)DataManager.GetData(id, "MaxCount", typeof(int)));
        }
        if (CheckProperty<ItemData, float>(data, "_effectAmount"))
        {
            SetPropertyIfExists(data, "_effectAmount", (float)DataManager.GetData(id, "EffectAmount", typeof(float)));
        }
        if (CheckProperty<ItemData, float>(data, "_effectAmount"))
        {
            SetPropertyIfExists(data, "_radius", (float)DataManager.GetData(id, "Radius", typeof(float)));
        }
        if (CheckProperty<ItemData, float>(data, "_duration"))
        {
            SetPropertyIfExists(data, "_duration", (float)DataManager.GetData(id, "Duration",typeof(float)));
        }

    //TODO: 아직 프리팹, 스프라이트 등록안되서
    //       임시 예외처리
    //    data._iconSprite = Resources.Load<Sprite>(
    //        (string)DataManager.GetData(id, "IconSprite"));
    //    data._prefab = Resources.Load<GameObject>(
    //        (string)DataManager.GetData(id, "PrefabName"));

        return data;
    }

    /// <summary> 부모 클래스와 상속 관계인 자식 클래스에
    /// 해당 프로퍼티(변수)가 있는지 확인하고 있을 경우
    /// 값을 넣는 함수. </summary>
    private static bool CheckProperty<T, TValue>(T target, string propertyName)
    {
        PropertyInfo property = typeof(T).GetProperty(propertyName);
        if (property?.PropertyType == typeof(TValue))
        {
            return true;
        }
        return false;
    }

    // 프로퍼티에 값을 넣는 함수
    private static void SetPropertyIfExists<T, TValue>(T target, string propertyName, TValue value)
    {
        PropertyInfo property = typeof(T).GetProperty(propertyName);
        property.SetValue(target, value);
    }

    /// <summary> itemDB에서 값을 검색하는 함수 </summary>
    private static T SearchItemDB<T>(int id) where T : class
    {
        // Potion일 경우
        if (POTION_TYPE_ID <= id && id < BOMB_TYPE_ID)
        {
            // db에 키 값이 있을 경우
            if (CheckIsValidKey(potionItemDB, id))
            {
                return potionItemDB[id] as T;
            }

            // 없을 경우
            return new PortionItemData() as T;
        }

        // Bomb일 경우
        else if (BOMB_TYPE_ID <= id && id < MATERIAL_TYPE_ID)
        {
            // db에 키 값이 있을 경우
            if (CheckIsValidKey(bombItemDB, id))
            {
                return bombItemDB[id] as T;
            }

            // 없을 경우
            return new BombItemData() as T;
        }

        // Material일 경우
        else if (MATERIAL_TYPE_ID <= id && id < QUEST_TYPE_ID)
        {
            // db에 키 값이 있을 경우
            if (CheckIsValidKey(materialItemDB, id))
            {
                return materialItemDB[id] as T;
            }

            // 없을 경우
            return new MaterialItemData() as T;
        }

        // Quest일 경우
        else
        {
            // db에 키 값이 있을 경우
            if (CheckIsValidKey(questItemDB, id))
            {
                return questItemDB[id] as T;
            }

            // 없을 경우
            return new QuestItemData() as T;
        }
    }

    // DB에 키 값이 정상적으로 존재하는지 확인하는 함수
    private static bool CheckIsValidKey<T> (Dictionary<int, T> data, int id)
    {
        // 딕셔너리에 id 키가 존재할 경우
        if (data.ContainsKey(id))
        {
            Debug.Log("111: Have Key");
            return true;
        }
        // 아닐 경우
        else
        {
            Debug.Log("111: Don't Have Key");
            return false;
        }
        return false;
    }
    #endregion

}
