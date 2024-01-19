using Js.Quest;
using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClearDatas
{
    public List<ClearData> list;
}
[System.Serializable]

public class ClearData
{
    public MBTI MBTI;
    public string Date;
}

public partial class UserDataManager
{
    #region 유저 데이터
    [Header("DB")]
    private bool _dataLoad;
    public bool dataLoadSuccess
    {
        get { return _dataLoad; }
        set
        {
            _dataLoad = value;
            // TODO : 델리게이트 이벤트 추가하기
        }
    }    // 데이터 불러옴 여부

    [Header("User Data")]           // 유저 데이터
    public string PlayerID;

    [SerializeField]
    private int _Level;
    public int Level
    {
        get { return _Level; }
        set
        {
            _Level = value;
            OnUserDataUpdate?.Invoke();
        }
    }
    [SerializeField]
    private int _Exp;
    public int Exp // 플레이어 현재 경험치
    {
        get { return _Exp; }
        set
        {
            _Exp = value;
            OnUserDataUpdate?.Invoke();
        }
    }
    [SerializeField]
    private int _Gold;
    public int Gold  // 플레이어 현재 골드
    {
        get { return _Gold; }
        set
        {
            _Gold = value;
            OnUserDataUpdate?.Invoke();
        }
    }
    //public string mbti;
    public MBTI mbti = new MBTI();

    [Header("PC Data")]             // PC 데이터
    public float DefaultHP;         // 초기 체력
    public float MaxHP;             // 업그레이드 반영된 체력
    public float CurHP;             // 현재 체력
    public float GainGold;
    public float GainExp;

    [Header("PC Status Data")]      // PC 스탯 업그레이드 데이터
    [SerializeField]
    private int _HPLv;
    public int HPLv  // 플레이어 체력
    {
        get { return _HPLv; }
        set
        {
            _HPLv = value;
            OnUserDataUpdate?.Invoke();
        }
    }
    [SerializeField]
    private int _GainGoldLv;
    public int GainGoldLv        // 플레이어 골드 증가량
    {
        get { return _GainGoldLv; }
        set
        {
            _GainGoldLv = value;
            OnUserDataUpdate?.Invoke();
        }
    }
    [SerializeField]
    private int _GainExpLv;
    public int GainExpLv         // 플레이어 경험치 증가량
    {
        get { return _GainExpLv; }
        set
        {
            _GainExpLv = value;
            OnUserDataUpdate?.Invoke();
        }
    }
    [Header("Default Weapon Data")]
    public float _weaponAtk;
    public float _weaponCritRate;
    public float _weaponCritDamage;
    public float _weaponAtkRate;

    [Header("SkillEffect")]
    public float effectDamage;
    public float effectCritDamage;
    public float effectCritProbability;
    public float effectAttackRate;
    public float effectMaxHP;
    public float effectDrillSize;
    

    [Header("Weapon Data")]
    public float weaponAtk;
    public float weaponCritRate;
    public float weaponCritDamage;
    public float weaponAtkRate;

    [Space(10f)]

    public int WeaponAtkLv;           // 공격력
    public int WeaponCriRateLv;       // 치명타 확률
    public int WeaponCriDamageLv;     // 치명타 증가율
    public int WeaponAtkRateLv;       // 공격 속도

    [Header("Skill 1 Data")]
    public int Skill1Lv_1;                // 테라드릴 레벨
    public int Skill1Lv_2;                // 테라드릴 레벨

    [Header("Skill 2 Data")]
    public int Skill2Lv_1;                // 드릴연마 레벨
    public int Skill2Lv_2;                // 드릴연마 레벨
    public int Skill2Lv_3;                // 드릴연마 레벨

    [Header("Skill 3 Data")]
    public int Skill3Lv;                  // 드릴분쇄 레벨

    [Header("Skill 4 Data")]
    public int drillLandingCount;          // 잔여 드릴랜딩
    public int Skill4Lv_1;                 // 드릴랜딩 레벨
    public int Skill4Lv_2;                 // 드릴랜딩 레벨
    public int Skill4Lv_3;                 // 드릴랜딩 레벨

    [Header("Clear Data")]
    public int ClearCount;            // 클리어 횟수
    private string JsonData;          // Json을 담을 직렬화된 클리어 데이터
    private ClearDatas _clearDatas;
    public ClearDatas clearDatas      // 클리어 데이터 리스트
    {
        get { return _clearDatas; }
        set
        {
            _clearDatas = value;
            if (value == null)
            {
                _clearDatas = new ClearDatas();
            }
        }
    }
    private string decodedString;

    [Header("Setting Data")]          // 환경 설정
    public float rotationAmount = 45f;
    [Range(0, 100)]
    public float masterSound = 0.7f, sfx = 0.7f, backgroundSound = 0.7f;
    [Range(-5, 5)]
    public float brightness = 0;

    [Header("Inventory Data")]
    // 호출 순서 문제로 인해 static으로 설정
    public static Item[] items = new Item[Inventory.MaxCapacity];

    [Header("Quest Data")]
    private static List<Quest> _questList = new List<Quest>();                               
    private static Dictionary<int, Quest> _questDictionary = new Dictionary<int, Quest>();
    private static Dictionary<int, List<Quest>> _keyIDQuestDictionary = new Dictionary<int, List<Quest>>();
    public static List<Quest> QuestList => _questList;                                          // 보유 퀘스트 리스트
    public static Dictionary<int, Quest> QuestDictionary => _questDictionary;                   // 보유 퀘스트 딕셔너리화
    public static Dictionary<int, List<Quest>> KeyIDQuestDictionary => _keyIDQuestDictionary;   // Key ID가 키 값인 보유 퀘스트 딕셔너리화
    public string QuestMain => PlayerDataManager.QuestMain;                                     // 메인 퀘스트
    public string DebugQuest;
    public QuestRewardText QuestRewardText => _questRewardText;                                 // 퀘스트 리워드 텍스트
    private QuestRewardText _questRewardText; 

    [Header("Result Data")]
    public GameResult result = new GameResult();

    [Header("Reference Data")]
    private StatusData statusData = new StatusData();
    public StatData statData = new StatData();   // 업그레이드 스탯 정보가 담긴 데이터

    [Header("State")]
    public bool isClear;        // 보스를 클리어했는지 확인
    public bool isGameOver;     // 게임 오버 상태인지 확인

    #endregion

}
