using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    // 유저의 데이터를 관리하는 클래스
    #region 싱글톤 패턴
    public static UserDataManager Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_Instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_Instance = FindObjectOfType<UserDataManager>();
            }
            // 싱글톤 오브젝트를 반환
            return m_Instance;
        }
    }
    private static UserDataManager m_Instance; // 싱글톤이 할당될 static 변수    
    #endregion

    #region 유저 데이터
    [Header("User Data")]
    private string userID;

    [Header("PC Data")]
    private float playerHp;         // 플레이어 체력
    private float playerExp;        // 플레이어 현재 경험치
    private float playerGold;       // 플레이어 현재 골드
    private float increExp;      // 플레이어 경험치 증가량
    private float increGold;     // 플레이어 골드 증가량

    [Header("Weapon Data")]

    private float attack;           // 공격력
    private float critProbability;  // 치명타 확률
    private float increaseCrit;     // 치명타 증가율
    private float attackRate;       // 공격 속도

    [Header("Skill Data")]
    private int teraDrillLv;      // 테라드릴 레벨
    private int grinderDrillLv;   // 드릴연마 레벨
    private int crashDrillLv;     // 드릴분쇄 레벨
    private int landingDrillLv;   // 드릴랜딩 레벨

    [Header("Quest Data")]
    private int curQuest;           // 현재 퀘스트

    [Header("Clear Data")]
    private int clearCount;         // 클리어 횟수
    private ClearData clearData;    // 클리어 데이터 클래스 MBTI, 클리어 날짜 시간
    #endregion

    // 클리어 데이터 클래스
    public class ClearData
    {
        public string mbti; //  MBTI
        public string date; //  클리어 날짜
    }

    #region 프로퍼티
    public string ID { get { return userID; } private set { userID = value; } }
    public float HP { get { return playerHp; } private set { playerHp = value; } }
    public float Exp { get { return playerExp; } private set { playerExp = value; } }
    public float Gold { get { return playerGold; } private set { playerGold = value; } }
    public float IncreaseEXP { get { return increExp; } private set { increExp = value; } }
    public float IncreaseGold { get { return increGold; } private set { increGold = value; } }
    public float Attack { get { return attack; } private set { attack = value; } }
    public float CritProbability { get { return critProbability; } private set { critProbability = value; } }
    public float IncreaseCrit { get { return increaseCrit; } private set { increaseCrit = value; } }
    public float AttackRate { get { return attackRate; } private set { attackRate = value; } }
    public int TeraLv { get { return teraDrillLv; } private set { teraDrillLv = value; } }
    public int GrinderLv { get { return grinderDrillLv; } private set { grinderDrillLv = value; } }
    public int CrashLv { get { return crashDrillLv; } private set { crashDrillLv = value; } }
    public int LandingLv { get { return landingDrillLv; } private set { landingDrillLv = value; } }
    public int CurrentQuest { get { return curQuest; } private set { curQuest = value; } }
    public int ClearCount { get { return clearCount; } private set { clearCount = value; } }
    public ClearData UserClearData { get { return clearData; } private set { clearData = value; } }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // 클리어 데이터를 저장하는 메서드
    public ClearData SaveClearData(string MBTI)
    {
        clearData = new ClearData();
        clearData.mbti = MBTI;
        clearData.date = GetCurrentDate();
        return clearData;
    }
    // 클리어 시간을 가져오는 함수
    public string GetCurrentDate()
    {
        return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
    }

}