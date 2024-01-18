using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Js.Quest;

public class GoogleSheetLoader : MonoBehaviour
{
    [Header("GoogleAPI")]
    // 스프레드 시트 url에 있는 ID
    private const string spreadsheetId = "1QZiDv3heAHcoaWa91lvT2C_D9XFQmyUQ1ocWGwLJpBA";
    // API 접근 KEY
    private const string apiKey = "AIzaSyC3utQPnsLiJdh3AAAdmYJFQ4QCZ7ReV_A";
    // 불러올 문서의 시트 이름 배열
    // 불러올 시트 이름을 넣어주세요!!!
    private static string[] sheetNames =
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
        "NPC_Table","NPC_Comunication_Table","BossRoomObjectCreate_Table",
        "NPC_Sound_Table"

    };

    // 코루틴에서 데이터를 반환하고
    // 반환된 데이터를 저장하기 위한 콜백 변수
    //private Action<string> callBack;

    // 모든 데이터가 데이터 매니저에 등록되었는지
    // 상태를 알려주는 변수
    public static bool isDone = false;

    // 리로드를 위한 인스턴스
    public static GoogleSheetLoader instance;

    private void Start()
    {
        // 데이터 매니저를 설정하는 함수 호출
        StartCoroutine(SetDataManager());

        // Init
        instance = this;
    }

    // 데이터 매니저에 GoogleSheet 문서 데이터를
    // 저장하는 함수
    private IEnumerator SetDataManager()
    {
        yield return null;

        // sheetNames의 길이 만큼 순회
        for (int i = 0; i < sheetNames.Length; i++)
        {
            // 코루틴으로 구글 시트 데이터를 불러온다.
            // isCsvConert = true를 매개변수로 할당해서
            // Csv 데이터로 변환한다.
            int waitframe = (i + 1) * 30;
            StartCoroutine(GoogleSheetsReader.GetGoogleSheetsData(
                spreadsheetId, apiKey, sheetNames[i], true, waitframe, data =>
                {
                    // 데이터를 가져왔을 경우 값 저장
                    if (data.Equals("").Equals(false))
                    {
                        // callBack 변수에서 받은 data를
                        // CSVReader.NewReadCSVFile()에
                        // 매개변수로 보내 데이터 타입을 변경
                        Dictionary<string, List<string>> dataDictionary =
                        CSVReader.NewReadCSVFile(data);
                        // dataDictionary를 데이터 매니저에 추가
                        DataManager.Instance.SetData(dataDictionary);
                    }
                    else
                    {
                        GFunc.LogWarning($"GoogleSheetLoader.SetDataManager(): 가져온 데이터: [{data}] " +
                            $"데이터를 가져오지 못해 데이터를 저장하지 않았습니다.");
                    }
                }));
        }

        // 모든 데이터를 데이터 매니저에
        // 할당했다고 상태를 변경하는 코루틴 호출
        // 정확한 상태를 설정하기 위해 1초 대기
        StartCoroutine(WaitForChangeState(1f));
    }

    // isDone의 상태를 변경하는 코루틴 함수
    private IEnumerator WaitForChangeState(float t)
    {
        // 대기
        yield return new WaitForSeconds(t);

        // 로딩 완료 상태 변경
        isDone = true;
        GFunc.Log("isDOne");
    }

    // GoogleSheetLoader에서 데이터를 못 불러왔을 경우
    // 다시 로드한다.
    public void ReLoadGoogleSheetsData(string spreadsheetId, string apiKey, string sheetName, 
        bool isCsvConvert, int waitFrame, Action<string> callBack)
    {
        GFunc.LogWarning($"GoogleSheetLoader.ReLoadGoogleSheetsData(): [{sheetName}] 시트를 가져오지 못해서 다시 로드합니다.");
        waitFrame = 200;
        StartCoroutine(GoogleSheetsReader.GetGoogleSheetsData(
         spreadsheetId, apiKey, sheetName, true, waitFrame, data =>
         {
             GFunc.LogWarning($"GoogleSheetLoader.ReLoadGoogleSheetsData(): 시트 이름: [{sheetName}] 가져온 데이터: [{data}]");
             // 콜백 받은 데이터가 ""일 경우 재호출
             if (data.Equals(""))
             {
                 ReLoadGoogleSheetsData(spreadsheetId, apiKey, sheetName, true, waitFrame, callBack);
                 GFunc.LogWarning($"GoogleSheetLoader.ReLoadGoogleSheetsData(): 콜백받은 데이터가 [{data}]이므로 다시 로드합니다.");
                 return;
             }
             // callBack 변수에서 받은 data를
             // CSVReader.NewReadCSVFile()에
             // 매개변수로 보내 데이터 타입을 변경
             Dictionary<string, List<string>> dataDictionary = CSVReader.NewReadCSVFile(data);
             // dataDictionary를 데이터 매니저에 추가
             DataManager.Instance.SetData(dataDictionary);
         }));
    }
}