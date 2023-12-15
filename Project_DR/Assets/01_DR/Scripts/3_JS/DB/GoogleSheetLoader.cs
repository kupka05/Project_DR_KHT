using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        "Item_Shop_Table", "BossMonster_Table", "AttackPattern_Table",

        // JH
        "Player_Table", "Drill_Table", "Skill_Table", "SkillEffect_Table", "MBTI_Table",
        "Upgrade_PC_HP_Table", "Upgrade_PC_GainEXP_Table", "Upgrade_PC_GainGold_Table",
        "Upgrade_Weapon_Atk_Table", "Upgrade_Weapon_CR_Table", "Upgrade_Weapon_CRD_Table", "Upgrade_Weapon_ATKSpeed_Table",

        //YS
        "Monster_Table", "Boss_Table", "Boss_projectile_Table",

        // SG
        "spawnNomalMonster_Table","spawnEliteMonster_Table","DungeonCreater_Table",
        "DungeonCreaterCustomRoom_Table","Floor1_MonsterSpawn_Table","Floor2_MonsterSpawn_Table",
        "Floor3_MonsterSpawn_Table","Floor4_MonsterSpawn_Table","Floor5_MonsterSpawn_Table",
        "BattleRoomObjectCreate_Table","EventRoomObjectCreate_Table","NullRoomObjectCreate_Table",
        "LightObject_Table","EnvObject_Table","MatObject_Table",


    };

    // 코루틴에서 데이터를 반환하고
    // 반환된 데이터를 저장하기 위한 콜백 변수
    //private Action<string> callBack;

    // 모든 데이터가 데이터 매니저에 등록되었는지
    // 상태를 알려주는 변수
    public static bool isDone = false;

    private void Start()
    {
        // 데이터 매니저를 설정하는 함수 호출
        SetDataManager();
    }

    // 데이터 매니저에 GoogleSheet 문서 데이터를
    // 저장하는 함수
    private void SetDataManager()
    {
        // sheetNames의 길이 만큼 순회
        for (int i = 0; i < sheetNames.Length; i++)
        {
            // 코루틴으로 구글 시트 데이터를 불러온다.
            // isCsvConert = true를 매개변수로 할당해서
            // Csv 데이터로 변환한다.
            int waitframe = (i + 1) * 3;
            StartCoroutine(GoogleSheetsReader.GetGoogleSheetsData(
                spreadsheetId, apiKey, sheetNames[i], true, waitframe, data =>
                {
                    // callBack 변수에서 받은 data를
                    // CSVReader.NewReadCSVFile()에
                    // 매개변수로 보내 데이터 타입을 변경
                    Dictionary<string, List<string>> dataDictionary =
                    CSVReader.NewReadCSVFile(data);
                    // dataDictionary를 데이터 매니저에 추가
                    DataManager.instance.SetData(dataDictionary);
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
        Debug.Log("isDOne");
    }
}