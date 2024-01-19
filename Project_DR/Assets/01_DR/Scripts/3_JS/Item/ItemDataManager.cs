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
        try
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
            //string data2 = (string)DataManager.Instance.GetData(50022221, "Dur222ation", typeof(string));
            //GFunc.Log($"TEST: {SearchItemDB<MaterialItemData>(5201).Desc}");
            //GFunc.Log($"TEST: {SearchItemDB<MaterialItemData>(5206).Desc}");
        }
        // DB에 데이터를 저장할 수 없을 경우
        catch (Exception ex)
        {
            // 예외가 발생했을 때 실행할 코드 블록
            GFunc.LogWarning($"오류 강제 예외처리 / ItemDataManager.InitItemDB() Exception: {ex.Message}");
        }
    }

    /// <summary> itemDB에서 값을 검색하는 함수 </summary>
    public static T SearchItemDB<T>(int id) where T : class
    {
        // Potion일 경우
        if (GetItemType(id) == 0)
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
        else if (GetItemType(id) == 1)
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
        else if (GetItemType(id) == 2)
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
    #endregion

    // ID로 아이템의 타입을 찾는 함수
    public static int GetItemType(int id)
    {
        int type = 0;   // 0 = Potion, 1 = Bomb, 2 = Material, 3 = Quest

        if (POTION_TYPE_ID <= id && id < BOMB_TYPE_ID)
        {
            type = 0;
        }

        // Bomb일 경우
        else if (BOMB_TYPE_ID <= id && id < MATERIAL_TYPE_ID)
        {
            type = 1;
        }

        // Material일 경우
        else if (MATERIAL_TYPE_ID <= id && id < QUEST_TYPE_ID)
        {
            type = 2;
        }

        // Quest일 경우
        else
        {
            type = 3;
        }

        return type;
    }

    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    private static void InitTable(int typeID, string category)
    {
        int size = DataManager.Instance.GetCount(typeID);
        for (int i = 0; i < size; i++)
        {
            int id = typeID + i;
            switch (category)
            {
                case "Potion":
                    potionItemDB.Add(id, InitData(id, new PortionItemData()));
                    break;

                case "Bomb":
                    bombItemDB.Add(id, InitData(id, new BombItemData()));
                    break;

                case "Material":
                    materialItemDB.Add(id, InitData(id, new MaterialItemData()));
                    break;

                case "Quest":
                    questItemDB.Add(id, InitData(id, new QuestItemData()));
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
        data._id = Data.GetInt(id, "ID");
        data._name = Data.GetString(id, "Name");
        data._desc = Data.GetString(id, "Desc");
        string iconSpriteName = Data.GetString(id, "IconSprite").Trim();
        string prefabName = Data.GetString(id, "PrefabName").Trim();
        data._iconSprite = Resources.Load<Sprite>(iconSpriteName);
        data._prefab = Resources.Load<GameObject>(prefabName);
        // 참고
        // 간헐적으로 데이터매니저 호출 사용시 간혹 가져온 데이터에 공백이 생기는
        // 현상이 있어 프리팹을 못 불러오는 현상이 있음 / 공백 제거 함수 추가 후
        // 해당 현상은 해결되었으나 참고바람

        // 자식 클래스에 해당 프로퍼티가 있는지 확인 후 데이터 추가
        if (CheckField<T, int>(data, "_maxAmount"))
        {
            SetFieldIfExists(data, "_maxAmount", (int)DataManager.Instance.GetData(id, "MaxCount", typeof(int)));
        }
        if (CheckField<T, float>(data, "_effectAmount"))
        {
            SetFieldIfExists(data, "_effectAmount", (float)DataManager.Instance.GetData(id, "EffectAmount", typeof(float)));
        }
        if (CheckField<T, float>(data, "_radius"))
        {
            SetFieldIfExists(data, "_radius", (float)DataManager.Instance.GetData(id, "Radius", typeof(float)));
        }
        if (CheckField<T, float>(data, "_duration"))
        {
            SetFieldIfExists(data, "_duration", (float)DataManager.Instance.GetData(id, "Duration", typeof(float)));
        }
        if (CheckField<T, float>(data, "_maxDuration"))
        {
            SetFieldIfExists(data, "_maxDuration", (float)DataManager.Instance.GetData(id, "MaxDuration", typeof(float)));
        }
        if (CheckField<T, float>(data, "_effectDuration"))
        {
            SetFieldIfExists(data, "_effectDuration", (float)DataManager.Instance.GetData(id, "EffectDuration", typeof(float)));
        }

        return data;
    }

    /// <summary> 부모 클래스와 상속 관계인 자식 클래스에
    /// 해당 필드(변수(멤버))가 있는지 확인하고 있을 경우
    /// 값을 넣는 함수. </summary>
    // ㅇㄴ 프로퍼티랑 필드랑 헷갈려서 반나절날림
    // 멤버(변수) = 필드 / 데이터를 저장하는 것만 함
        // ex) public int num;
    // 프로퍼티 = 필드처럼 보이나 메서드로 선언 하는것
               // 데이터에 접근하거나 수정하는 동작에 중점을 둠
        // ex) public int num => _num;
        // ex2) public int num
        //      {
        //          get { return _num; } // 프로퍼티
        //          set { _num = value; }
        //      }
    private static bool CheckField<T, TValue>(T target, string fieldName)
    {
        if (target == null)
        {
            //GFunc.Log("Target is null");
            return false;
        }

        FieldInfo field = typeof(T).GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);

        if (field == null)
        {
            //GFunc.Log($"{typeof(T)} Field {fieldName} not found");
            return false;
        }

        //GFunc.Log($"isField = {field}");
        return field.FieldType == typeof(TValue);
    }


    // 필드에 값을 넣는 함수
    private static void SetFieldIfExists<T, TValue>(T target, string fieldName, TValue value)
    {
        FieldInfo field = typeof(T).GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
        if (field != null && field.FieldType == typeof(TValue))
        {
            field.SetValue(target, value);
        }
    }


    // DB에 키 값이 정상적으로 존재하는지 확인하는 함수
    private static bool CheckIsValidKey<T> (Dictionary<int, T> data, int id)
    {
        try
        {
            // 딕셔너리에 id 키가 존재할 경우
            if (data.ContainsKey(id))
            {
                return true;
            }
            // 아닐 경우
            else
            {
                return false;
            }
        }
        // 키 값을 확인할 수 없을 경우
        catch (Exception ex)
        {
            // 예외가 발생했을 때 실행할 코드 블록
            GFunc.LogWarning($"오류 강제 예외처리 / ItemDataManager.CheckIsValidKey() Exception: {ex.Message}");
            return false;
        }
    }
    #endregion

}
