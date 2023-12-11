using JetBrains.Annotations;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;

[System.Serializable]
public class ClearDatas
{
    public List<ClearData> list;
}
[System.Serializable]

public class ClearData
{
    public string MBTI;
    public string Date;
}


public class UserDataManager : MonoBehaviour
{
    // DB에서 가져온 유저의 데이터를 관리하는 클래스
    #region 싱글톤 패턴


    private static UserDataManager m_Instance = null; // 싱글톤이 할당될 static 변수    

    public static UserDataManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<UserDataManager>();
            if(m_Instance == null)
            {
                GameObject obj = new GameObject("UserDataManager");
                m_Instance = obj.AddComponent<UserDataManager>();
                DontDestroyOnLoad(obj);
            }
            return m_Instance;
        }
    }
    #endregion

    // 옵저버 패턴
    public delegate void UserDataUpdateDelegate();
    public event UserDataUpdateDelegate OnUserDataUpdate;
    public void UpdateUserData()
    {
        // 데이터가 변경될 때마다 호출
        OnUserDataUpdate?.Invoke();
    }

    #region 옵저버 패턴 private 유저 데이터
    private float _HP;
    private float _Exp;
    private int _Gold;

    #endregion
    #region 유저 데이터
    [Header("DB")]
    public bool dataLoadSuccess;    // 데이터 불러옴 여부

[Header("User Data")]
    public string PlayerID;

    public float HP  // 플레이어 체력
    {
        get  { return _HP; }
        set 
        { 
            _HP = value; 
            OnUserDataUpdate?.Invoke();
        }
    }
    public float Exp // 플레이어 현재 경험치
    {
        get { return _Exp; }
        set
        {
            _Exp = value;
            OnUserDataUpdate?.Invoke();
        }
    }                 
    public int Gold  // 플레이어 현재 골드
    {
        get { return _Gold; }
        set
        {
            _Gold = value;
            OnUserDataUpdate?.Invoke();
        }
    }
    [Header("PC Data")]
    public float ExpIncrease;         // 플레이어 경험치 증가량
    public float GoldIncrease;        // 플레이어 골드 증가량

    [Header("Weapon Data")]

    public float WeaponAtk;          // 공격력
    public float WeaponCriRate;      // 치명타 확률
    public float WeaponCriDamage;    // 치명타 증가율
    public float WeaponAtkRate;      // 공격 속도

    [Header("Skill Data")]
    public int TeraLv;        // 테라드릴 레벨
    public int GrinderLv;     // 드릴연마 레벨
    public int CrashLv;       // 드릴분쇄 레벨
    public int LandingLv;     // 드릴랜딩 레벨

    [Header("Quest Data")]
    public string QuestMain;           // 현재 퀘스트

    [Header("Clear Data")]
    public int ClearCount;         // 클리어 횟수
    private string JsonData;       // Json을 담을 직렬화된 클리어 데이터
    private ClearDatas _clearDatas;

    public ClearDatas clearDatas  // 클리어 데이터 모음
    {
        get { return _clearDatas; }
        set
        {
            _clearDatas = value;
            if (value == null)
            {
                Debug.Log("신규 데이터 생성");
                _clearDatas = new ClearDatas();
            }
        }
    }
    [Header("Setting Data")]
    public float rotationAmount = 45f;
    [Range(0, 100)]
    public float masterSound, sfx, backgroundSound;
    [Range(-5, 5)]
    public float brightness = 0;



    #endregion


    // 로드되면 이벤트 호출
    public UnityEvent LoadDataEvent;

    private void Awake()
    {

        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(this.gameObject);            
        }
        else
        { Destroy(gameObject); }
        

        Debug.Log("데이터 요청 시간 : " + GetCurrentDate());
        PlayerDataManager.Update(true);
    }
    public void Start()
    {
    }
    // ============================ 데이터 로드 ============================


    // 로그인 후, DB에서 데이터 받아오기
    public void GetDataFromDB()
    {
        PlayerID = PlayerDataManager.PlayerID;
        HP = PlayerDataManager.HP;
        Gold = PlayerDataManager.Gold;
        Exp = PlayerDataManager.Exp;
        GoldIncrease = PlayerDataManager.GoldIncrease;
        ExpIncrease = PlayerDataManager.ExpIncrease;

        WeaponAtk = PlayerDataManager.WeaponAtk;
        WeaponCriRate = PlayerDataManager.WeaponCriRate;
        WeaponCriDamage = PlayerDataManager.WeaponCriDamage;
        WeaponAtkRate = PlayerDataManager.WeaponAtkRate;

        TeraLv = PlayerDataManager.SkillLevel1;
        GrinderLv = PlayerDataManager.SkillLevel2;
        CrashLv = PlayerDataManager.SkillLevel3;
        LandingLv = PlayerDataManager.SkillLevel4;

        QuestMain = PlayerDataManager.QuestMain;
        ClearCount = PlayerDataManager.ClearCount;

        JsonData = PlayerDataManager.ClearMBTIValue;

        // json으로 변환된 string은 .NET Framework 디코딩이 필요
        string decodedString = System.Web.HttpUtility.UrlDecode(JsonData);

        clearDatas = JsonUtility.FromJson<ClearDatas>(decodedString);

        if(clearDatas.list == null)
        {
            clearDatas.list = new List<ClearData>();
        }

        // 데이터를 불러오고 해야할 이벤트가 있다면 이벤트 실행
        // Ex. 플레이어 상태창, 상점의 현재 골드 등
        dataLoadSuccess = true;
        Debug.Log("데이터 로드 시간 : " + GetCurrentDate());

       
    }

    // DB에 데이터를 요청하기 위한 메서드
    // 메서드를 action에 담아 호출하면, 데이터가 로드 시 코루틴은 멈춘다.
    public void DBRequst(Action action)
    {
        StartCoroutine(CheckData(action));
    }
    IEnumerator CheckData(Action action)
    {
        if (dataLoadSuccess)
        {
            Debug.Log(action + "데이터 로드 완료");
            action();
            yield break;
        }
        Debug.Log("데이터 로드를 시도하나?");
        yield return null;
    }

    // =========================== 세이브 데이터 ===========================
    // 클리어 데이터 신규 저장
    public void SaveClearData(string MBTI)
    {
        // 넣을 데이터 생성
        ClearData newData = new ClearData();
        newData.Date = GetCurrentDate();             // 현재 시간
        newData.MBTI = MBTI;                         // 매개변수 MBTI
        Debug.Log(clearDatas);
        Debug.Log(clearDatas.list);
        Debug.Log(clearDatas.list.Count);

        clearDatas.list.Add(newData);                // 리스트에 추가
        ClearCount = clearDatas.list.Count;          // 클리어 데이터 리스트의 길이가 곧 클리어 카운트

        JsonData = JsonUtility.ToJson(clearDatas);   // json으로 변환

        // 저장 후 업데이트
        PlayerDataManager.Save("clear_mbti_value", JsonData);
        PlayerDataManager.Save("clear_count", ClearCount);
        PlayerDataManager.Update(true);
    }

    // 클리어 저장
    // TODO : 플레이어 클리어시 저장하는 유저 데이터
    public void SavePlayerData()
    {

    }

    // 클리어 시간을 가져오는 함수
    private string GetCurrentDate()
    {
        return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }



    // ====================== 디버그용 PC 데이터 세팅 ======================
    public void SetDebugData()
    {
        HP = (float)DataManager.instance.GetData(1001, "Health", typeof(float));
        WeaponAtk = (float)DataManager.instance.GetData(1100, "Damage", typeof(float));
        WeaponCriRate = (float)DataManager.instance.GetData(1100, "CritChance", typeof(float));
        WeaponCriDamage = (float)DataManager.instance.GetData(1100, "CritIncrease", typeof(float));
        WeaponAtkRate = (float)DataManager.instance.GetData(1100, "AttackSpeed", typeof(float));

        PlayerDataManager.Save("hp", HP);
        PlayerDataManager.Save("gold", 5000);
        PlayerDataManager.Save("exp", 5000);
        PlayerDataManager.Save("weapon_atk", WeaponAtk);
        PlayerDataManager.Save("weapon_cri_rate", WeaponCriRate);
        PlayerDataManager.Save("weapon_cri_damage", WeaponCriDamage);
        PlayerDataManager.Save("weapon_atk_rate", WeaponAtkRate);
    }

    public void AddGold(int num)
    {
        Gold += num;
    }
}