using JetBrains.Annotations;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;
using Rito.InventorySystem;
using static StatusData;
using OVR.OpenVR;
using System.Net.NetworkInformation;
using Js.Quest;
using System.Linq;

// DB에서 가져온 유저의 데이터를 관리하는 클래스

public partial class UserDataManager : MonoBehaviour
{
    #region 싱글톤 패턴


    private static UserDataManager m_Instance = null; // 싱글톤이 할당될 static 변수    

    public static UserDataManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<UserDataManager>();
            if (m_Instance == null)
            {
                GameObject obj = new GameObject("UserDataManager");
                m_Instance = obj.AddComponent<UserDataManager>();
                DontDestroyOnLoad(obj);
            }
            return m_Instance;
        }
    }
    #endregion

    #region 옵저버 패턴
    public delegate void UserDataUpdateDelegate();
    public event UserDataUpdateDelegate OnUserDataUpdate;
    public event UserDataUpdateDelegate OnWeaponDataUpdate;
    public event UserDataUpdateDelegate OnSkillDataUpdate;

    // 유저 데이터 업데이트
    public void UpdateUserData()
    {
        // 데이터가 변경될 때마다 호출
        OnUserDataUpdate?.Invoke();
    }
    // 무기 데이터 업데이트
    public void UpdateWeaponData()
    {
        // 데이터가 변경될 때마다 호출
        OnWeaponDataUpdate?.Invoke();
    }
    // 스킬 데이터 업데이트
    public void UpdateSkillData()
    {
        // 데이터가 변경될 때마다 호출
        OnSkillDataUpdate?.Invoke();
    }
    #endregion

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);

    // ####################### Awake #######################

    private void Awake()
    {
        // 싱글톤 패턴
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        { Destroy(gameObject); }

        //SetDebugData();
        GFunc.Log("데이터 요청 시간 : " + GetCurrentDate());
        GetReferenceData();
        PlayerDataManager.Update(true); // 데이터 요청
    }
    public void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            StartCoroutine(SetDebugData());
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            SaveClearData();
        }
    }

    // ####################### 데이터 로드 #######################

    // 참조 데이터 로드
    public void GetReferenceData()
    {
        statusData.GetData(statData);   // 스탯 데이터 로딩

        result.Initialize(); // 결과 점수 초기화
        InitMBTI();         // MBTI 초기화
    }

    // 로그인 후, DB에서 데이터 받아오기
    public void GetDataFromDB()
    {
        PlayerID = PlayerDataManager.PlayerID;
        Gold = PlayerDataManager.Gold;
        Exp = PlayerDataManager.Exp;

        // ######################### PC 업그레이드 #########################
        HPLv = PlayerDataManager.HP;
        GainGoldLv = PlayerDataManager.GoldIncrease;
        GainExpLv = PlayerDataManager.ExpIncrease;

        // HP 업그레이드 세팅
        DefaultHP = Data.GetFloat(1001, "Health");
        MaxHP = UserData.GetMaxHP();
        CurHP = MaxHP;

        // 골드 획득량 업그레이드 세팅
        if (GainGoldLv != 0)
        {
            GainGold = statData.upgradeGainGold[GainGoldLv - 1].sum;
        }
        // 경험치 획득량 업그레이드 세팅
        if (GainExpLv != 0)
        {
            GainExp = statData.upgradeGainExp[GainExpLv - 1].sum;
        }

        // 총 레벨
        Level = (HPLv + GainGoldLv + GainExpLv);

        // ######################### 무기 업그레이드 #########################

        WeaponAtkLv = PlayerDataManager.WeaponAtk;
        WeaponCriRateLv = PlayerDataManager.WeaponCriRate;
        WeaponCriDamageLv = PlayerDataManager.WeaponCriDamage;
        WeaponAtkRateLv = PlayerDataManager.WeaponAtkRate;

        _weaponAtk = Data.GetFloat(1100, "Damage");
        _weaponCritRate = Data.GetFloat(1100, "CritChance");
        _weaponCritDamage = Data.GetFloat(1100, "CritIncrease");
        _weaponAtkRate = Data.GetFloat(1100, "AttackSpeed");

        weaponAtk = _weaponAtk;
        weaponCritRate = _weaponCritRate;
        weaponCritDamage = _weaponCritDamage;
        weaponAtkRate = _weaponAtkRate;

        if (WeaponAtkLv != 0)
        {
            weaponAtk = _weaponAtk + statData.upgradeAtk[WeaponAtkLv - 1].sum1;
        }
        if (WeaponCriRateLv != 0)
        {
            weaponCritRate = _weaponCritRate + statData.upgradeCrit[WeaponCriRateLv - 1].sum1;
        }
        if (WeaponCriDamageLv != 0)
        {
            weaponCritDamage = _weaponCritDamage + statData.upgradeCritDmg[WeaponCriDamageLv - 1].sum1;
        }
        if (WeaponAtkRateLv != 0)
        {
            weaponAtkRate = _weaponAtkRate + statData.upgradeAtkSpd[WeaponAtkRateLv - 1].sum1;
        }

        // ######################### 스킬 업그레이드 #########################

        // TODO 데이터 매니저에 추가 테이블 필요
        Skill1Lv_1 = PlayerDataManager.SkillLevel1_1;
        Skill1Lv_2 = PlayerDataManager.SkillLevel1_2;

        Skill2Lv_1 = PlayerDataManager.SkillLevel2_1;
        Skill2Lv_2 = PlayerDataManager.SkillLevel2_2;
        Skill2Lv_3 = PlayerDataManager.SkillLevel2_3;

        Skill3Lv = PlayerDataManager.SkillLevel3;

        Skill4Lv_1 = PlayerDataManager.SkillLevel4_1;
        Skill4Lv_2 = PlayerDataManager.SkillLevel4_2;
        Skill4Lv_3 = PlayerDataManager.SkillLevel4_3;

        // 랜딩 스킬 사용횟수 갱신
        drillLandingCount = UserData.SetDrillLandingCount();

        // ######################### ETC #########################

        QuestMain = PlayerDataManager.QuestMain;
        ClearCount = PlayerDataManager.ClearCount;

        // ######################### 클리어 데이터 #########################

        JsonData = PlayerDataManager.ClearMBTIValue;

        // json으로 변환된 string은 .NET Framework 디코딩이 필요
        decodedString = System.Web.HttpUtility.UrlDecode(JsonData);

        clearDatas = JsonUtility.FromJson<ClearDatas>(decodedString);

        if (clearDatas.list == null) // 리스트가 없으면 새로 만들기
        {
            clearDatas.list = new List<ClearData>();
        }

        // 데이터를 불러오고 해야할 이벤트가 있다면 이벤트 실행
        // Ex. 플레이어 상태창, 상점의 현재 골드 등
        dataLoadSuccess = true;
        GFunc.Log("데이터 로드 시간 : " + GetCurrentDate());

        // 퀘스트가 생성안된 경우 데이터 테이블에 있는 퀘스트를 가져와서 생성
        // && 가져온 퀘스트 데이터에 따라 상태 변경
        if (_questList.Count.Equals(0))
        {
            QuestManager.Instance.CreateQuestFromDataTable();
        }
        QuestManager.Instance.LoadUserQuestDataFromDB();
    }

    // DB에 데이터를 요청하기 위한 메서드
    // 메서드를 action에 담아 호출하면, 데이터가 로드 시 코루틴은 멈춘다.
    public void DBRequst(Action action)
    {
        StartCoroutine(CheckData(action));
    }
    // 델리게이트에 추가로 변경하기
    IEnumerator CheckData(Action action)
    {
        yield return waitForSeconds;
        if (dataLoadSuccess)
        {
            //GFunc.Log(action + "데이터 로드 완료");
            action();
            yield break;
        }
        yield return null;
    }

    // ####################### 세이브 데이터 ####################### \\
    // 클리어 데이터 신규 저장
    public void SaveClearData()
    {
        // 넣을 데이터 생성
        ClearData newData = new ClearData();
        newData.Date = GetCurrentDate();             // 현재 시간
        newData.MBTI.mbti = mbti.GetMBTI();                         // 매개변수 MBTI

        clearDatas.list.Add(newData);                // 리스트에 추가
        ClearCount = clearDatas.list.Count;          // 클리어 데이터 리스트의 길이가 곧 클리어 카운트

        JsonData = JsonUtility.ToJson(clearDatas);   // json으로 변환

        // 저장 후 업데이트
        PlayerDataManager.Save("clear_mbti_value", JsonData);
        PlayerDataManager.Save("clear_count", ClearCount);
        PlayerDataManager.Update(true);
    }
    // 클리어 시간을 가져오는 함수
    private string GetCurrentDate()
    {
        return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }

    // 플레이어 업그레이드 세이브
    public void SavePlayerUpgrade()
    {
        PlayerDataManager.Save("exp", Exp);
        PlayerDataManager.Save("hp", HPLv);
        PlayerDataManager.Save("gold_increase", GainGoldLv);
        PlayerDataManager.Save("exp_increase", GainExpLv);
    }
    // 무기 업그레이드 세이브
    public void SaveWeaponUpgrade()
    {
        PlayerDataManager.Save("exp", Exp);
        PlayerDataManager.Save("weapon_atk", WeaponAtkLv);
        PlayerDataManager.Save("exp", WeaponAtkRateLv);
        PlayerDataManager.Save("weapon_cri_damage", WeaponCriDamageLv);
        PlayerDataManager.Save("weapon_cri_rate", WeaponCriRateLv);
    }
    // 스킬 업그레이드 세이브
    public void SaveSkillUpgrade()
    {
        PlayerDataManager.Save("skill_level_1_1", Skill1Lv_1);
        PlayerDataManager.Save("skill_level_1_2", Skill1Lv_2);
        PlayerDataManager.Save("skill_level_2_1", Skill2Lv_1);
        PlayerDataManager.Save("skill_level_2_2", Skill2Lv_2);
        PlayerDataManager.Save("skill_level_2_3", Skill2Lv_3);
        PlayerDataManager.Save("skill_level_3", Skill3Lv);
        PlayerDataManager.Save("skill_level_4_1", Skill4Lv_1);
        PlayerDataManager.Save("skill_level_4_2", Skill4Lv_2);
        PlayerDataManager.Save("skill_level_4_3", Skill4Lv_3);
    }

    // ####################### 디버그용 PC 데이터 리셋 ####################### \\
    // TODO 한번에 호출하면 저장 실패할 경우가 있음.
    public void ResetData()
    {
        StartCoroutine(SetDebugData());
    }
    public IEnumerator SetDebugData()
    {
        yield return null;
        PlayerDataManager.Save("hp", 0);
        yield return null;

        PlayerDataManager.Save("gold", 200000);
        yield return null;

        PlayerDataManager.Save("exp", 200000);
        yield return null;

        PlayerDataManager.Save("gold_increase", 0);
        yield return null;

        PlayerDataManager.Save("exp_increase", 0);
        yield return null;

        PlayerDataManager.Save("weapon_atk", 0);
        yield return null;

        PlayerDataManager.Save("weapon_cri_rate", 0);
        yield return null;

        PlayerDataManager.Save("weapon_cri_damage", 0);
        yield return null;

        PlayerDataManager.Save("weapon_atk_rate", 0);
        yield return null;

        int debugLv = 0;

        PlayerDataManager.Save("skill_level_1_1", debugLv);
        yield return null;

        PlayerDataManager.Save("skill_level_1_2", debugLv);
        yield return null;

        PlayerDataManager.Save("skill_level_2_1", debugLv);
        yield return null;

        PlayerDataManager.Save("skill_level_2_2", debugLv);
        yield return null;

        PlayerDataManager.Save("skill_level_2_3", debugLv);
        yield return null;


        PlayerDataManager.Save("skill_level_3", debugLv);
        yield return null;
        PlayerDataManager.Save("skill_level_4_1", debugLv);
        yield return null;

        PlayerDataManager.Save("skill_level_4_2", debugLv);
        yield return null;

        PlayerDataManager.Save("skill_level_4_3", debugLv);
        yield return null;

        PlayerDataManager.Update(true);
        yield break;
    }

    // #######################  MBTI  ####################### \\
    public void InitMBTI()
    {
        mbti.SetMBTI(50,50,50,50);
    }
    public MBTI GetMBTI()
    {
        return mbti;
    }
    public void SetMBTI(MBTI mbtiData)
    {
        mbti = mbtiData;
    }


    // #######################  PC 데이터 세팅  ####################### \\

    public void PlayerStatusUpgrade(int hpLv, int gainGoldLv, int gainExpLv)
    {
        int newHpLv = hpLv;
        int newGainGoldLv = gainGoldLv;
        int newgainExpLv = gainExpLv;

        HPLv = newHpLv;
        GainGoldLv = newGainGoldLv;
        GainExpLv = newgainExpLv;

        Level = HPLv + GainGoldLv + GainExpLv;

        MaxHP = DefaultHP;
        if (HPLv != 0)
        {
            MaxHP = DefaultHP + statData.upgradeHp[HPLv - 1].sum;
        }
        // 골드 획득량 업그레이드 세팅
        if (GainGoldLv != 0)
        {
            GainGold = statData.upgradeGainGold[GainGoldLv - 1].sum;
        }
        // 경험치 획득량 업그레이드 세팅
        if (GainExpLv != 0)
        {
            GainExp = statData.upgradeGainExp[GainExpLv - 1].sum;
        }
    }
    public void WeaponUpgrade(int atkLv, int critLv, int critRateLv, int atkRateLv)
    {
        int _atkLv = atkLv;
        int _critLv = critLv;
        int _critRateLv = critRateLv;
        int _atkRateLv = atkRateLv;

        WeaponAtkLv = _atkLv;
        WeaponCriDamageLv = _critLv;
        WeaponCriRateLv = _critRateLv;
        WeaponAtkRateLv = _atkRateLv;

        weaponAtk = _weaponAtk;
        weaponCritRate = _weaponCritRate;
        weaponCritDamage = _weaponCritDamage;
        weaponAtkRate = _weaponAtkRate;

        if (WeaponAtkLv != 0)
        {
            weaponAtk = _weaponAtk + statData.upgradeAtk[WeaponAtkLv - 1].sum1;
        }
        if (WeaponCriRateLv != 0)
        {
            weaponCritRate = _weaponCritRate + statData.upgradeCrit[WeaponCriRateLv - 1].sum1;
        }
        if (WeaponCriDamageLv != 0)
        {
            weaponCritDamage = _weaponCritDamage + statData.upgradeCritDmg[WeaponCriDamageLv - 1].sum1;
        }
        if (WeaponAtkRateLv != 0)
        {
            weaponAtkRate = _weaponAtkRate + statData.upgradeAtkSpd[WeaponAtkRateLv - 1].sum1;
        }
    }

    public void SkillUpgrade(int skill1_1, int skill1_2, int skill2_1, int skill2_2, int skill2_3,
        int skill3, int skill4_1, int skill4_2, int skill4_3)
    {
        Skill1Lv_1 = skill1_1;
        Skill1Lv_2 = skill1_2;

        Skill2Lv_1 = skill2_1;
        Skill2Lv_2 = skill2_2;
        Skill2Lv_3 = skill2_3;

        Skill3Lv = skill3;

        Skill4Lv_1 = skill4_1;
        Skill4Lv_2 = skill4_2;
        Skill4Lv_3 = skill4_3;     
    }


    // ######################### 퀘스트 #########################
    // 보유한 퀘스트를 삭제
    public static void RemoveQuest(int index)
    {
        _questList.RemoveAt(index);
    }

    // _questList 리스트에 퀘스트 추가
    public static void AddQuestToQuestList(Quest quest)
    {
        _questList.Add(quest);
    }

    // _questList 리스트를 딕셔너리로 변경 및 _questDictionary에 할당
    public static void AddQuestDictionary()
    {
        // 퀘스트 ID를 키 값으로 하는 <int, Quest> 딕셔너리 할당
        _questDictionary = _questList.ToDictionary(
            quest => quest.QuestData.ID, quest => quest);
    }

    // 퀘스트 정보(json)를 DB에 저장
    public void SaveQuestDatasToDB(string json)
    {
        PlayerDataManager.Save("quest_main", json);
    }
}