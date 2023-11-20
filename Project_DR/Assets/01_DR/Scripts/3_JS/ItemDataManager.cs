using System;
using System.Collections.Generic;
using UnityEngine;
using Rito.InventorySystem;

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
                    potionItemDB.Add(id, InitData(id, new PortionItemData()));
                    Debug.Log($"ID: {potionItemDB[id].ID}");
                    break;

                case "Bomb":
                    bombItemDB.Add(id, InitData(id, new BombItemData()));
                    Debug.Log($"maxAmount: {bombItemDB[id]._maxAmount}");
                    break;

                case "Material":
                    materialItemDB.Add(id, InitData(id, new MaterialItemData()));
                    Debug.Log($"maxAmount: {materialItemDB[id]._maxAmount}");
                    break;

                case "Quest":
                    questItemDB.Add(id, InitData(id, new QuestItemData()));
                    Debug.Log($"maxAmount: {questItemDB[id]._maxAmount}");
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }

    private static PortionItemData InitData(int id, PortionItemData data)
    {
        // Init
        data._id = (int)DataManager.GetData(id, "ID");
        data._name = (string)DataManager.GetData(id, "Name");
        data._maxAmount = (int)DataManager.GetData(id, "MaxCount");
        data._value = (float)DataManager.GetData(id, "EffectAmount");
        data._duration = (float)DataManager.GetData(id, "Duration");
        data._effectDuration = (float)DataManager.GetData(id, "EffectDuration");
        data._desc = (string)DataManager.GetData(id, "Desc");
        // TODO: 아직 프리팹,스프라이트 등록안되서
        //       임시 예외처리
        //data._iconSprite = Resources.Load<Sprite>(
        //    (string)DataManager.GetData(id, "IconSprite"));
        //data._prefab = Resources.Load<GameObject>(
        //    (string)DataManager.GetData(id, "PrefabName"));

        return data;
    }

    private static BombItemData InitData(int id, BombItemData data)
    {
        data._id = (int)DataManager.GetData(id, "ID");
        data._name = (string)DataManager.GetData(id, "Name");
        data._maxAmount = (int)DataManager.GetData(id, "MaxCount");
        data._value = (float)DataManager.GetData(id, "EffectAmount");
        data._radius = (float)DataManager.GetData(id, "Radius");
        data._duration = (float)DataManager.GetData(id, "Duration");
        data._desc = (string)DataManager.GetData(id, "Desc");
        // TODO: 아직 프리팹,스프라이트 등록안되서
        //       임시 예외처리
        //data._iconSprite = Resources.Load<Sprite>(
        //    (string)DataManager.GetData(id, "IconSprite"));
        //data._prefab = Resources.Load<GameObject>(
        //    (string)DataManager.GetData(id, "PrefabName"));
        return data;
    }

    private static MaterialItemData InitData(int id, MaterialItemData data)
    {
        data._id = (int)DataManager.GetData(id, "ID");
        data._name = (string)DataManager.GetData(id, "Name");
        data._maxAmount = (int)DataManager.GetData(id, "MaxCount");
        data._desc = (string)DataManager.GetData(id, "Desc");
        // TODO: 아직 프리팹,스프라이트 등록안되서
        //       임시 예외처리
        //data._iconSprite = Resources.Load<Sprite>(
        //    (string)DataManager.GetData(id, "IconSprite"));
        //data._prefab = Resources.Load<GameObject>(
        //    (string)DataManager.GetData(id, "PrefabName"));
        return data;
    }
    private static QuestItemData InitData(int id, QuestItemData data)
    {
        data._id = (int)DataManager.GetData(id, "ID");
        data._name = (string)DataManager.GetData(id, "Name");
        data._maxAmount = (int)DataManager.GetData(id, "MaxCount");
        data._desc = (string)DataManager.GetData(id, "Desc");
        // TODO: 아직 프리팹,스프라이트 등록안되서
        //       임시 예외처리
        //data._iconSprite = Resources.Load<Sprite>(
        //    (string)DataManager.GetData(id, "IconSprite"));
        //data._prefab = Resources.Load<GameObject>(
        //    (string)DataManager.GetData(id, "PrefabName"));
        return data;
    }

    private static T SearchItemDB<T>(int id) where T : class
    {
        // Potion일 경우
        if (POTION_TYPE_ID <= id && id < BOMB_TYPE_ID)
        {
            return potionItemDB[id] as T;
        }

        // Bomb일 경우
        else if (BOMB_TYPE_ID <= id && id < MATERIAL_TYPE_ID)
        {
            return bombItemDB[id] as T;
        }

        // Material일 경우
        else if (MATERIAL_TYPE_ID <= id && id < QUEST_TYPE_ID)
        {
            return materialItemDB[id] as T;
        }

        // Quest일 경우
        else
        {
            return questItemDB[id] as T;
        }
    }
    #endregion

}
