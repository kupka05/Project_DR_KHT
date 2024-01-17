using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Js.Quest;

//! [DataManager의 사용법]
//! 0. CSVFile을 가져온다.
//! CSVReader.ReadCSVFile(CSVFiles/CSV파일명); 
//! *what? CSVFiles = CSV 파일이 저장된 디렉토리
//! *what? CSV파일명 = 가져올 CSV 파일명, 확장자는 생략한다.
//! *what? 반환타입 = Dictionary<string, List<string>> 이며,
//! 가져올 변수를 같은 타입으로 선언해야 한다.
//!
//! 1. DataManager에 데이터를 보관한다.
//! DataManager.SetData(데이터);
//! *what? 데이터 = 불러온 CSV파일
//!
//! 2. DataManager에 저장된 데이터를 불러온다.
//! 2-1. 하나의 데이터 값만 가져올 때
//! DataManager.Instance.GetData(아이디, 카테고리);
//! 값을 받을 때 int num = (int)DataManager.Instance.GetData();
//! 위와 같은 형태로 맞는 데이터 타입으로 형변환 해야 한다.
//! *what? 아이디 = CSV파일 참조
//! *what? 카테고리 = CSV파일의 행 참조
//!
//! 2-2. 아이디에 해당하는 줄의 모든 데이터를 가져올 떄
//! DataManager.Instance.GetData(아이디);
//! *what? 반환타입 = Dictionary<string, string> 이며,
//! 가져올 변수를 같은 타입으로 선언해야 한다.
//!SetIDTable
//! 아이디가 들어있는 CSV파일의 카운트를 가져온다.
//! DataManager.GetCount(아이디);
public class DataManager : MonoBehaviour
{
    #region 싱글톤 패턴


    private static DataManager m_Instance = null; // 싱글톤이 할당될 static 변수    

    public static DataManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<DataManager>();
            if (m_Instance == null)
            {
                GameObject obj = new GameObject("DataManager");
                m_Instance = obj.AddComponent<DataManager>();
                DontDestroyOnLoad(obj);
            }
            return m_Instance;
        }
    }
    #endregion

    [Header("Choi")]
    // 데이터를 보관하는 변수
    
    public Dictionary<int, Dictionary<string, List<string>>>
        dataTable = new Dictionary<int, Dictionary<string, List<string>>>();

    private Dictionary<int, Dictionary<string, List<string>>>
    dataTable2 = new Dictionary<int, Dictionary<string, List<string>>>();

    // 로컬용 dataTable
    private Dictionary<int, Dictionary<string, List<string>>>
    localDataTable = new Dictionary<int, Dictionary<string, List<string>>>();

    // 로컬에서 불러올 csv 파일 이름
    private string[] fileNames =
    {
        // JS
        "Item_Potion_Table", "Item_Bomb_Table", "Item_Material_Table", "Item_Quest_Table",
        "Item_Shop_Table", "BossMonster_Table", "AttackPattern_Table", "Quest_Table", "Quest_Reward_Table",
        "Crafting_Table", "Crafting_Condition_Table", "Enhance_Table", "Enhance_Condition_Table", "Bonus_Stat_Table",

        // JH
        "Player_Table", "Drill_Table", "Skill_Table", "SkillEffect_Table", "MBTI_Table",
        "Upgrade_PC_HP_Table", "Upgrade_PC_GainEXP_Table", "Upgrade_PC_GainGold_Table",
        "Upgrade_Weapon_Atk_Table", "Upgrade_Weapon_CR_Table", "Upgrade_Weapon_CRD_Table", "Upgrade_Weapon_ATKSpeed_Table",
        "Upgrade_Skill_1","Upgrade_Skill_2","Upgrade_Skill_3","Upgrade_Skill_4",
        "Debug_Table",

        // YS
        "Monster_Table", "Boss_Table", "Boss_projectile_Table",
        
        // SG
        "spawnNomalMonster_Table","spawnEliteMonster_Table","DungeonCreater_Table",
        "DungeonCreaterCustomRoom_Table","Floor1_MonsterSpawn_Table","Floor2_MonsterSpawn_Table",
        "Floor3_MonsterSpawn_Table","Floor4_MonsterSpawn_Table","Floor5_MonsterSpawn_Table",
        "BattleRoomObjectCreate_Table","EventRoomObjectCreate_Table","NullRoomObjectCreate_Table",
        "LightObject_Table","EnvObject_Table","MatObject_Table",
        "NPC_Table", "BossRoomObjectCreate_Table","NPC_Comunication_Table",
        "NPC_Sound_Table"
    };

    // dataTable에 ID로 접근하기 위해
    // ID에 해당하는 인덱스를 보관하는 변수
    // idTable[ID][DATA_KEY] = dataTable에 저장된 순서
    // idTable[ID][DATA_INDEX] = 실제 데이터의 인덱스
    public Dictionary<int, List<int>> 
        idTable = new Dictionary<int, List<int>>();

    // 로컬용 idTable
    private Dictionary<int, List<int>>
    localIDTable = new Dictionary<int, List<int>>();

    // dataTable의 고정 상수
    private const string ID_HEADER = "ID";
    private const int DATA_KEY = 0;
    private const int DATA_INDEX = 1;
    private enum State
    {
        Default = 0,    // 기본 상태
        Done            // CSV 파일 Init 완료
    };
    private static State state = State.Default;


    /*************************************************
     *                 Unity Events
     *************************************************/
    private void Awake()
    {
        // 로컬용 CSV 파일을 테이블에 Init
        InitLocalDataTable();
    }


    /*************************************************
     *                 Public Methods
     *************************************************/
    #region [+]
    // 데이터 테이블에 있는 ID들을 리스트로 받아옴
    // 가져올 테이블에 있는 ID중 하나를 매개변수로 받아야 한다
    public List<int> GetDataTableIDs(int id)
    {
        List<int> idList = default;
        // dataTable이 비어있지 않을 경우
        if (dataTable.Count != 0)
        {
            // 키 값을 사용해 아이디 리스트를 가져옴
            int key = GetDataKey(id);

            // List<string>을 List<int>로 변환함
            idList = ConvertStringListToIntList(dataTable[key][ID_HEADER]);

            return idList;
        }

        // 비어 있을 경우 로컬로 대체
        else
        {
            // 키 값을 사용해 아이디 리스트를 가져옴
            int key = GetDataKey(id);

            // List<string>을 List<int>로 변환함
            idList = ConvertStringListToIntList(localDataTable[key][ID_HEADER]);

            return idList;
        }
    }

    // 데이터 매니저에 데이터가 전부 불러와졌는지 확인
    public bool IsDataLoaded()
    {
        // 시트에 있는 테이블을 전부 가져왔을 경우
        if (dataTable.Count.Equals(fileNames.Length))
        {
            return true;
        }

        // 아닐 경우
        return false;
    }

    // CSV Reader로 불러온 Dictionary<string, List<string>를
    // dataTable에 데이터를 저장하는 함수
    public void SetData(Dictionary<string, List<string>> data)
    {
        // data 카운트가 0 또는 1일 경우 예외처리
        if (data.Count.Equals(0) || data.Count.Equals(1)) { GFunc.LogWarning($"DataManager.SetData(): 가져온 데이터에 문제가 있어 저장할 수 없습니다."); return; }

        // ID 값을 idTable에 저장하는 함수 호출
        // 실패(false)시 예외처리
        if (SetIDTable(data).Equals(false)) { GFunc.LogWarning($"DataManager.SetData(): IDTable을 저장하는데 오류가 발생했습니다. 예외처리"); return; };

        // dataTable에 값 추가
        int index = dataTable.Count;
        dataTable.Add(index, data);
    }

    // dataTable에 저장된 데이터를 가져오는 함수
    // 기본 반환 값은 string이다.
    public object GetData(int id, string category, Type castType)
    {
        try
        {
            // GoogleSheetLoader에서 모든 데이터를
            // 불러왔을 경우 
            if (GoogleSheetLoader.isDone)
            {
                // dataTable을 검색하는 함수 호출
                object data = FindDataTable(id, category, dataTable, idTable);

                // 데이터 타입을 가져오는 함수 호출
                //string type = GetDataType((string)data);  //Legacy:

                // 데이터 타입에 따라 형변환 하는 함수 호출
                data = ConvertDataType(castType, (string)data);

                // data가 null일 경우
                // 예외 처리를 위해 추가
                if (data == null)
                {
                    // 만약 castType이 string 일 경우
                    // 참조 타입이므로 예외처리 한다.
                    if (castType == typeof(string))
                    {
                        return string.Empty;
                    }

                    // castType에 맞는 default 인스턴스 반환
                    return Activator.CreateInstance(castType);
                }
                else
                {
                    return data;
                }
            }
            // 구글 스프레트 시트에서 데이터를 못가져왔을 경우
            // 로컬에 있는 CSV 데이터를 가져옴
            // CSV 데이터를 가져왔을 경우
            else if (state == State.Done)
            {
                GFunc.LogWarning("GetData(): GoogleSheetLoader에서 모든 데이터가" +
                " 로딩되지 않았습니다. GoogleSheetLoader.isDone = false");

                // dataTable을 검색하는 함수 호출
                object data = FindDataTable(id, category, localDataTable, localIDTable);

                // 데이터 타입을 가져오는 함수 호출
                //string type = GetDataType((string)data);   //Legacy:

                // 데이터 타입에 따라 형변환 하는 함수 호출
                data = ConvertDataType(castType, (string)data);

                // data가 null일 경우
                // 예외 처리를 위해 추가
                if (data == null)
                {
                    // 만약 castType이 string 일 경우
                    // 참조 타입이므로 예외처리 한다.
                    if (castType == typeof(string))
                    {
                        return string.Empty;
                    }

                    // castType에 맞는 default 인스턴스 반환
                    return Activator.CreateInstance(castType);
                }
                else
                {
                    return data;
                }
            }
            // 위의 두 사항에 해당하지 않을 경우
            else
            {
                // 만약 castType이 string 일 경우
                // 참조 타입이므로 예외처리 한다.
                if (castType == typeof(string))
                {
                    return string.Empty;
                }
                // castType에 맞는 default 인스턴스 반환
                return Activator.CreateInstance(castType);
            }
        }

        catch (Exception ex)
        {
            GFunc.LogWarning($"오류 강제 예외처리 / DataManager.Instance.GetData() {category} : {id} / Exception: {ex.Message}");

            // 만약 castType이 string 일 경우
            // 참조 타입이므로 예외처리 한다.
            if (castType == typeof(string))
            {
                return string.Empty;
            }

            // castType에 맞는 default 인스턴스 반환
            return Activator.CreateInstance(castType);
        }
    }

    // 매개 변수에 id만 넣을 경우 Dictionary<string, string>로 반환한다.
    public Dictionary<string, string> GetData(
        int id)
    {
        // dataTable을 검색하는 함수 호출
        Dictionary<string, string> temp_DataTable = FindDataTable(id);

        return temp_DataTable;
    }

    // 아이디를 매개변수로 받고 아이디가 들어 있는 CSV파일의
    // 열 갯수를 가져오는 함수.
    public int GetCount(int id)
    {
        // dataTable이 비어있지 않을 경우
        if (dataTable.Count != 0)
        {

            // 키 값과 키 값을 사용해 카운트를 가져온다.
            int key = GetDataKey(id);
            int count = dataTable[key][ID_HEADER].Count;

            // 찾은 카운트 반환
            return count;
        }
        // 비어 있을 경우 로컬로 대체
        else
        {
            // 키 값과 키 값을 사용해 카운트를 가져온다.
            int key = GetDataKey(id);
            
            int count = localDataTable[key][ID_HEADER].Count;

            // 찾은 카운트 반환
            return count;
        }
    }

    // 아이디가 위치한 실제 인덱스를 가져오는 함수
    public int GetIndex(int id)
    {
        // 아이디의 실제 인덱스를 가져오는 함수 호출
        id = GetDataKey(id);
        return id;
    }

    #endregion

    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    // 리스트를 순회해서 string인 데이터를 int로 변경
    private List<int> ConvertStringListToIntList(List<string> stringList)
    {
        List<int> intList = new List<int>();

        for (int i = 0; i < stringList.Count; i++)
        {
            // string 값을 int로 변환 후 리스트에 추가
            string strValue = stringList[i].RemoveUnderbar();
            int intValue = int.Parse(strValue);
            intList.Add(intValue);
        }

        return intList;
    }

    // 로컬 데이터 테이블을 설정하는 함수
    public void SetLocalData(Dictionary<string, List<string>> data)
    {
        // dataTable에 값 추가
        int index = localDataTable.Count;

        localDataTable.Add(index, data);

        // ID 값을 idTable에 저장하는 함수 호출
        SetLocalIDTable(data);
    }

    // CSV 파일을 Local 변수에 Init하는 함수
    public void InitLocalDataTable()
    {
        string directory = "";
        for (int i = 0; i < fileNames.Length; i++)
        {
            TextAsset data = Resources.Load<TextAsset>(directory + fileNames[i]);

            Dictionary<string, List<string>> dataDictionary =
            CSVReader.NewReadCSVFile(data.ToString());

            // dataDictionary를 데이터 매니저에 추가
            SetLocalData(dataDictionary);
        }

        // 작업 완료 상태 변경
        state = State.Done;
    }

    // ID 값을 idTable에 저장하는 함수
    // 성공시 true 반환
    private bool SetIDTable(Dictionary<string, List<string>> data)
    {

        // dataTable의 길이를 딕셔너리 접근 인덱스로 설정
        // SetIDTable의 경우 IDTable을 설정한 후 dataTable을
        // 추가하도록 변경했음
        // index = dataTable.Count -1 에서 dataTable.Count로
        // 최종 수정 [2023-12-20]
        int index = dataTable.Count ;
        try
        {
            // data[ID_HEADER]의 길이 만큼 순회
            for (int i = 0; i < data[ID_HEADER].Count; i++)
            {
                int id = int.Parse(data[ID_HEADER][i].Replace("_", ""));
                int index2 = i;
                // idTable에 있는 기존 ID와 현재 ID가 중복되었을 경우
                if (idTable.ContainsKey(id))
                {
                    GFunc.LogWarning($"SetIDTable(): 등록하는 ID: [{id}]는 " +
                        $"기존에 있는 ID와 중복됩니다. CSV 파일의 ID 값을 변경하시거나, " +
                        $"CSV파일이 중복으로 SetData()를 호출하는지 확인해주세요.");

                    // 예외처리
                    return false;
                }
                // 딕셔너리의 키값으로 ID를 설정하고, 내부 List에 실제 인덱스를 저장한다.
                // index는 딕셔너리의 위치, index2는 실제 데이터 열이 저장된 위치
                idTable.Add(id, new List<int>());
                idTable[id].Add(index); // [0] 딕셔너리의 위치
                idTable[id].Add(index2); // [1] 실제 데이터 열이 저장된 위치
            }

            return true;
        }

        catch (Exception ex)
        {
            GFunc.LogWarning($"ID테이블 오류 발생 {data}, index:{index}");
            GFunc.Log($"DataManager.SetIDTable()[{fileNames[index]}] ID 테이블 오류 발생 + {ex.Message}");

            return false;
        }

    }

    // ref로 주소 값 전달해서 값 변경하려는데 안돼서
    // 이걸로 대체
    private void SetLocalIDTable(Dictionary<string, List<string>> data)
    {
        // dataTable의 길이 - 1 를 딕셔너리 접근 인덱스로 설정
        int index = localDataTable.Count - 1;
        try
        {
            // data[ID_HEADER]의 길이 만큼 순회
            for (int i = 0; i < data[ID_HEADER].Count; i++)
            {
                int id = int.Parse(data[ID_HEADER][i].Replace("_", ""));
                int index2 = i;
                // idTable에 있는 기존 ID와 현재 ID가 중복되었을 경우
                if (localIDTable.ContainsKey(id))
                {
                    GFunc.LogWarning($"SetIDTable(): 등록하는 ID: [{id}]는 " +
                        $"기존에 있는 ID와 중복됩니다. CSV 파일의 ID 값을 변경하시거나, " +
                        $"CSV파일이 중복으로 SetData()를 호출하는지 확인해주세요.");
                }
                // 딕셔너리의 키값으로 ID를 설정하고, 내부 List에 실제 인덱스를 저장한다.
                // index는 딕셔너리의 위치, index2는 실제 데이터 열이 저장된 위치
                localIDTable.Add(id, new List<int>());
                localIDTable[id].Add(index); // [0] 딕셔너리의 위치
                localIDTable[id].Add(index2); // [1] 실제 데이터 열이 저장된 위치
            }          
        }
        catch (Exception ex)
        {
            GFunc.Log($"DataManager.SetLocalIDTable() [{fileNames[index]}] Local ID 테이블 오류 발생 + {ex.Message}");
        }
    }

    // dataTable를 검색하는 함수
    private object FindDataTable(int id, string category,
        Dictionary<int, Dictionary<string, List<string>>> dataTable,
        Dictionary<int, List<int>> idTable)
    {
        try
        {
            object data = default;

            // idTable에 정상적으로 접근했을 경우
            if (idTable.ContainsKey(id))
            {
                // dataTable에서 데이터를 찾아서 반환한다.
                int key = idTable[id][DATA_KEY];
                int index = idTable[id][DATA_INDEX];
                data = dataTable[key][category][index];
            }

            // 접근하지 못했을 경우
            else
            {
                // 디버그 메세지 출력
                GFunc.LogWarning($"FindDataTable({id}, {category}): 데이터를 찾지 못했습니다. " +
                    $"ID와 Category를 확인해 주세요.");
            }
        return data;
        }

        catch (Exception ex)
        {
            // 예외가 발생했을 때 실행할 코드 블록
            GFunc.LogWarning($"오류 강제 예외처리 id:{id} category: {category} / DataManager.FindDataTable() Exception: {ex.Message}");

            return new object();
        }
    }

    // 매개 변수에 id만 넣을 경우 Dictionary<string, string>로 반환한다.
    private Dictionary<string, string> FindDataTable(int id)
    {
        Dictionary<string, string> temp_DataTable = new Dictionary<string, string>();
        // idTable에 정상적으로 접근했을 경우
        if (idTable.ContainsKey(id))
        {
            int key = idTable[id][DATA_KEY];
            int count = dataTable[key].Count;
            string[] categorys = new string[count];
            int temp_Index = 0;
            // dataTable[key]를 모두 순회
            foreach (var pair in dataTable[key])
            {
                // 키 값 저장
                categorys[temp_Index] = pair.Key;
                temp_Index++;
            }

            // ID에 해당하는 실제 데이터의 인덱스를 가져옴
            int index = idTable[id][DATA_INDEX];
            // count 만큼 순회
            for (int i = 0; i < count; i++)
            {
                // temp_DataTable에 dataTable에 있는 모든 카테고리 & 값 등록
                string category = categorys[i];
                string data = dataTable[key][category][index];
                temp_DataTable.Add(category, data);
            }
        }

        else
        // 정상적으로 접근하지 못했을 경우
        {
            // 디버그 메세지 출력
            GFunc.LogWarning($"FindDataTable({id}): 데이터를 찾지 못했습니다. " +
                $"ID를 확인해 주세요.");
        }

        // temp_DataTable 반환
        return temp_DataTable;
    }

    // string으로 저장된 data 값의 데이터 타입을
    // 조건식을 통해 찾아내는 함수
    public string GetDataType(string data)
    {
        // 데이터 타입이 int 일 경우(언더바 제거)
        if (int.TryParse(data.Replace("_", ""), out int tempIntValue))
        {
            // "int" 반환
            return "int";
        }

        // 데이터 타입이 float 일 경우
        else if (float.TryParse(data, out float tempFloatValue))
        {
            // "float" 반환
            return "float";
        }

        // 데이터 타입이 bool 일 경우
        else if (bool.TryParse(data, out bool tempBoolValue))
        {
            // "bool" 반환
            return "bool";
        }

        // 모두 일치 하지 않을 경우 "string" 반환
        return "string";
    }

    // 데이터 타입을 형변환 해주는 함수
    private object ConvertDataType(Type type, string data)
    {
        // 받아온 type에 따라 데이터 타입을 형변환
        switch (Type.GetTypeCode(type))
        {
            // "int"일 경우
            case TypeCode.Int32:
                // 언더바 제거
                return int.Parse(data.Replace("_", ""));

            // "float"일 경우
            case TypeCode.Single:
                return float.Parse(data);

            // "bool"일 경우
            case TypeCode.Boolean:
                return bool.Parse(data);

            // "string"일 경우
            default:
                return data;
        }
    }

    // 아이디로 해당 아이디가 위치한 CSV파일의 위치를 받아오는 함수
    private int GetDataKey(int id)
    {
        // dataTable이 비어있지 않을 경우
        if (dataTable.Count != 0)
        {
            int index = idTable[id][DATA_KEY];
        
            // 찾은 위치 인덱스 반환
            return index;
        }
        // 비어있을 경우
        else
        {
            int index = localIDTable[id][DATA_KEY];

            // 찾은 위치 인덱스 반환
            return index;
        }
    }

    #endregion
}
