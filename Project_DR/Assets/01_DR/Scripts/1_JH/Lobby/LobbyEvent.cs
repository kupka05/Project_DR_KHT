using Meta.WitAi.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;



// NPC 대사를 담을 클래스
[System.Serializable]
public class NpcDialogs
{
    public List<NpcDialog> logs;
}
[System.Serializable]
public class NpcDialog
{
    public int ID;
    public string[] _log;
    public Queue log;
    public int _event;
}

public class LobbyEvent : MonoBehaviour
{
    [Header("Data")]
    public Action dbRequest;
    //private StatData data;

    [Header("Main Display")]
    public GameObject mainDisplay;
    public GameObject mbtiDisplay;

    [Header("Main NPC")]
    public int questID;         // 시트에서 불러올 퀘스트 ID 
    public int targetQuestID;   // 디스플레이에 표시할 퀘스트 ID
    public TMP_Text npcDialog;  // 대사 텍스트

    [Header("NPC Dialog")]
    public NpcDialogs DialogData = new NpcDialogs();    // 대사 데이터
    public NpcDialog dialog;                            // 현재 대사

    [Header("ClearData")]
    public string[] clearDatas;             // 디버그용 클리어 데이터
    public GameObject clearDataObj;
    public Transform contentPos;

    [Header("Spawn Room Door")]
    public GameObject spawnroomDoor;
    public GameObject openDoorButton;

    [Header("Status Display")]
    public GameObject statusDisplay;        // 디스플레이 오브젝트
    public Transform statusPos;             // 디스플레이가 생성될 포지션
    private SphereCollider statusCollider;  // 디스플레이 트리거 콜라이더
    [Space(10f)]

    // ################################## 패널 ##############################################
    [Header("Status Pannel")]
    public GameObject selectStatusDis;      // 선택창
    public GameObject playerStatusDis;      // 플레이어 상태창
    public GameObject playerAcceptPannel;   // 확인창
    [Space(10f)]
    public GameObject weaponStatusDis;      // 무기 상태창
    public GameObject selectSkillStatusDis; // 스킬 선택창
    public GameObject skillStatusDis;       // 스킬 상태창
    [Space(10f)]
    public GameObject skillUpgrade1;       // 스킬 상태창
    public GameObject skillUpgrade2;       // 스킬 상태창
    public GameObject skillUpgrade3;       // 스킬 상태창
    public GameObject skillUpgrade4;       // 스킬 상태창

    // ################################## PC 업그레이드 ##############################################

    [Header("Player Status")]
    public TMP_Text playerLevel;
    public TMP_Text playerID;
    public TMP_Text playerGold;
    public TMP_Text playerExp;

    [Header("Player Upgrade Button")]
    public LobbyDisplayButton hpUpBtn;      // hp 증가 버튼
    public LobbyDisplayButton goldIncreBtn; // 골드 증가 버튼
    public LobbyDisplayButton expIncreBtn;  // 경험치 증가 버튼
    [Space(10f)]
    public TMP_Text curPlayerHp;
    public TMP_Text beforePlayerHp;
    public TMP_Text afterPlayerHp;

    private int playerSpend;

    [Header("Gold Upgrade")]
    public TMP_Text curGoldIncre;
    public TMP_Text beforeGoldIncre;
    public TMP_Text afterGoldIncre;

    [Header("Exp Upgrade")]
    public TMP_Text curExpIncre;
    public TMP_Text beforeExpIncre;
    public TMP_Text afterExpIncre;

    [Header("PC Spend Exp")]
    public TMP_Text pcCurExp;
    public TMP_Text pcSpendExp;
    public TMP_Text pcRemainExp;

    // ################################## 무기 업그레이드 ##############################################

    [Header("Weapon Upgrade")]
    public LobbyDisplayButton atkUpBtn;         // 공격력 버튼
    public LobbyDisplayButton critRateUpBtn;    // 치명타 확률 버튼
    public LobbyDisplayButton critDmgBtn;       // 치명타 데미지 버튼
    public LobbyDisplayButton atkRateBtn;       // 공격 간격 버튼


    [Header("Attack Upgrade")]
    public TMP_Text curAtk;
    public TMP_Text beforeAtk;
    public TMP_Text afterAtk;

    [Header("CritRate Upgrade")]
    public TMP_Text curCritRate;
    public TMP_Text beforeCritRate;
    public TMP_Text afterCritRate;

    [Header("Crit Damage Upgrade")]
    public TMP_Text curCritDamage;
    public TMP_Text beforeCritDamage;
    public TMP_Text afterCritDamage;

    [Header("Attack Rate Upgrade")]
    public TMP_Text curAtkRate;
    public TMP_Text beforeAtkRate;
    public TMP_Text afterAtkRate;

    [Header("Weapon Spend Exp")]
    public TMP_Text weaponCurExp;
    public TMP_Text weaponSpendExp;
    public TMP_Text weaponRemainExp;
    private int weaponSpend;


    // ################################## START ##################################

    public void Start()
    {
        
        dbRequest += GetDataFromDB;                     // DB 데이터 요청 성공 시 액션 추가
        UserDataManager.Instance.DBRequst(dbRequest);   // DB 데이터 요청

        // 메인 디스플레이 시작 시 세팅
        ChangeDisplayButton("Main");

        // PC 상태창 시작 시 세팅
        SetStatusDisplay();

        // 상태창 디스플레이 시작 시 세팅
        ChangeStatusDisplayButton("Main");    

        // 옵저버 등록
        UserDataManager.Instance.OnUserDataUpdate += UpdatePlayerStatusUI;
    }

    // ############################### 데이터 불러오기 ###############################
    #region 데이터 로드
    // DB에서 데이터 불러오기 완료 후 이벤트로 실행
    public void GetDataFromDB()
    {

        GetNPCDialog();              // NPC 대사 가져오고
        SetNpcDialog(targetQuestID); // NPC 대사 리스트 가져와서 퀘스트 진행 상황에 따라 대사, 이벤트 지정
        GetClearData();              // 클리어 데이터 가져오기

        // 메인 디스플레이의 클리어 데이터 업데이트
        UpdateClearDataUI(clearDatas);            

        // 상태창 : 플레이어 강화
        UpdatePlayerStatusUI();
        UpdatePlayerUpgradeUI();
        SetPlayerLevelBtn();

        // 상태창 : 무기 강화
        UpdateWeaponStateUI();
        UpdateWeaponUpgradeUI();
        SetWeaponLevelBtn();
    }

    // 클리어 데이터 불러오기
    public void GetClearData()
    {
        int index = 0;
        if (UserDataManager.Instance.clearDatas.list != null)
        {
            index = UserDataManager.Instance.clearDatas.list.Count;
        }

        // 클리어 카운트 가져와서 카운트만큼 배열 할당
        clearDatas = new string[index];
        for (int i = 0; i < clearDatas.Length; i++)
        {
            string Date = UserDataManager.Instance.clearDatas.list[i].Date;
            string MBTI = UserDataManager.Instance.clearDatas.list[i].MBTI;


            //TODO : 스트링빌더로 업데이트
            // 아래 형식으로 데이터 변환
            // 2023/11/21 08:23 0회차 MBTI INFP 
            string clearData = $"{Date} | {i + 1} 회차 | MBTI {MBTI}";

            // 배열에 담기
            clearDatas[i] = clearData;
        }
    }
    #endregion

    // ################################# UI 업데이트 #################################
    #region 플레이어 업그레이드
    // 상태창 UI 업데이트 : 시작하고 바로 업데이트 해야하는 것들은 이곳에서 업데이트
    public void UpdatePlayerStatusUI()
    {
        if (UserDataManager.Instance.PlayerID == "")
        {
            UserDataManager.Instance.PlayerID = "Admin";
        }                     // 어드민 설정

        // 플레이어 스탯
        playerID.text = UserDataManager.Instance.PlayerID;                  // ID
        playerLevel.text = UserDataManager.Instance.Level.ToString();
        playerGold.text = UserDataManager.Instance.Gold.ToString();         // 골드
        playerExp.text = UserDataManager.Instance.Exp.ToString();           // 경험치

        curPlayerHp.text = UserDataManager.Instance.HP.ToString();
        beforePlayerHp.text = UserDataManager.Instance.HP.ToString();

        curGoldIncre.text = UserDataManager.Instance.GainGold.ToString();
        beforeGoldIncre.text = UserDataManager.Instance.GainGold.ToString();

        curExpIncre.text = UserDataManager.Instance.GainExp.ToString();
        beforeExpIncre.text = UserDataManager.Instance.GainExp.ToString();

        pcCurExp.text = UserDataManager.Instance.Exp.ToString();
        pcSpendExp.text = 0.ToString();
        pcRemainExp.text = 0.ToString();

    }

    public void SetPlayerLevelBtn()
    {
        hpUpBtn.level = UserDataManager.Instance.HPLv;
        goldIncreBtn.level = UserDataManager.Instance.GainGoldLv;
        expIncreBtn.level = UserDataManager.Instance.GainExpLv;
    }
    // 플레이어 업그레이드 상태창 UI 업데이트
    public void UpdatePlayerUpgradeUI()
    {        
        // 레벨은 0이하로 될 수 없음. 삼항연산자 활용
        afterPlayerHp.text = MinusCheck(hpUpBtn.newLevel - 1) ? (UserDataManager.Instance.DefaultHP + UserDataManager.Instance.statData.upgradeHp[hpUpBtn.newLevel - 1].sum).ToString() : beforePlayerHp.text;
        afterGoldIncre.text = MinusCheck(goldIncreBtn.newLevel - 1) ? (UserDataManager.Instance.statData.upgradeGainGold[goldIncreBtn.newLevel - 1].sum).ToString() : beforeGoldIncre.text;
        afterExpIncre.text = MinusCheck(expIncreBtn.newLevel - 1) ? (UserDataManager.Instance.statData.upgradeGainExp[expIncreBtn.newLevel - 1].sum).ToString() : beforeExpIncre.text;

        playerSpend = PlayerCalculator();        // 사용 금액
        pcSpendExp.text = playerSpend.ToString();
        pcRemainExp.text = (UserDataManager.Instance.Exp - playerSpend).ToString();       
    }
    // 플레이어 업그레이드
    public void PlayerUpgrade()
    {
        if(playerSpend == 0)
        {
            return;
        }

        // 구매했을 떄 일어나는 이벤트
        if (playerSpend <= UserDataManager.Instance.Exp)
        {
            Debug.Log("구매 완료");

            Debug.Log($"새 레벨 : {hpUpBtn.newLevel}, 기존 레벨 : {hpUpBtn.level}");
            // 업그레이드 레벨을 새 레벨로 적용
            hpUpBtn.level = hpUpBtn.newLevel;
            goldIncreBtn.level = goldIncreBtn.newLevel;
            expIncreBtn.level = expIncreBtn.newLevel;

            Debug.Log($"새 레벨 : {hpUpBtn.newLevel}, 기존 레벨 : {hpUpBtn.level}");
            // 경험치 소모
            UserDataManager.Instance.Exp -= playerSpend;
            // 레벨 업데이트
            UserDataManager.Instance.PlayerStatusUpgrade(hpUpBtn.level, goldIncreBtn.level, expIncreBtn.level);               
        }
        else
        {
            Debug.Log("경험치가 부족합니다.");
            return;
        }
        UpdatePlayerStatusUI();
        UpdatePlayerUpgradeUI();
        ChangeStatusDisplayButton("Player");
        UserDataManager.Instance.SavePlayerUpgrade();
    }

    // 플레이어 업그레이드를 위한 계산기
    public int PlayerCalculator()
    {
        int result, afterHP, curHP, afterGold, curGold, afterExp, curExp;

        afterHP = MinusCheck(hpUpBtn.newLevel - 1) ? UserDataManager.Instance.statData.upgradeHp[hpUpBtn.newLevel - 1].totalExp : 0;
        curHP = MinusCheck(hpUpBtn.level - 1) ? UserDataManager.Instance.statData.upgradeHp[hpUpBtn.level - 1].totalExp : 0;
        afterGold = MinusCheck(goldIncreBtn.newLevel - 1) ? UserDataManager.Instance.statData.upgradeGainGold[goldIncreBtn.newLevel - 1].totalExp : 0;
        curGold = MinusCheck(goldIncreBtn.level - 1) ? UserDataManager.Instance.statData.upgradeGainGold[goldIncreBtn.level - 1].totalExp : 0;
        afterExp = MinusCheck(expIncreBtn.newLevel - 1) ? UserDataManager.Instance.statData.upgradeGainExp[expIncreBtn.newLevel - 1].totalExp : 0;
        curExp = MinusCheck(expIncreBtn.level - 1) ? UserDataManager.Instance.statData.upgradeGainExp[expIncreBtn.level - 1].totalExp : 0;

        result = (afterHP - curHP) + (afterGold - curGold) + (afterExp - curExp);
        //Debug.Log($"{hpUpBtn.newLevel - 1} - {hpUpBtn.level - 1} + {goldIncreBtn.newLevel - 1} - {goldIncreBtn.level - 1} + {expIncreBtn.newLevel - 1} - {expIncreBtn.level - 1}");
        //Debug.Log($"{afterHP} - {curHP} + {afterGold} - {curGold} + {afterExp} - {curExp}");
        return result;
    }

    // 음수 값이면 거짓
    public bool MinusCheck(int value)
    {
        if (0 <= value )
        {
            return true;
        }
        else
            return false;
    }
    #endregion

    #region 무기 업그레이드
    // 무기 업그레이드 상태 UI 업데이트
    public void UpdateWeaponStateUI()
    {
        curAtk.text = UserDataManager.Instance.weaponAtk.ToString();
        beforeAtk.text = UserDataManager.Instance.weaponAtk.ToString();

        curCritRate.text = UserDataManager.Instance.weaponCritRate.ToString();
        beforeCritRate.text = UserDataManager.Instance.weaponCritRate.ToString();

        curCritDamage.text = UserDataManager.Instance.weaponCritDamage.ToString();
        beforeCritDamage.text = UserDataManager.Instance.weaponCritDamage.ToString();

        curAtkRate.text = UserDataManager.Instance.weaponAtkRate.ToString();
        beforeAtkRate.text = UserDataManager.Instance.weaponAtkRate.ToString();

        weaponCurExp.text = UserDataManager.Instance.Exp.ToString();
        weaponSpendExp.text = 0.ToString();
        weaponRemainExp.text = 0.ToString();
    }

    public void UpdateWeaponUpgradeUI()
    {
        // 레벨은 0이하로 될 수 없음. 삼항연산자 활용
        afterAtk.text = MinusCheck(atkUpBtn.newLevel - 1) ? (UserDataManager.Instance.weaponAtk + UserDataManager.Instance.statData.upgradeAtk[atkUpBtn.newLevel - 1].sum1).ToString() : beforeAtk.text;
        afterCritRate.text = MinusCheck(critRateUpBtn.newLevel - 1) ? (UserDataManager.Instance.weaponCritRate + UserDataManager.Instance.statData.upgradeCrit[critRateUpBtn.newLevel - 1].sum1).ToString() : beforeCritRate.text;
        afterCritDamage.text = MinusCheck(critDmgBtn.newLevel - 1) ? (UserDataManager.Instance.weaponCritDamage + UserDataManager.Instance.statData.upgradeCritDmg[critDmgBtn.newLevel - 1].sum1).ToString() : beforeCritDamage.text;
        afterAtkRate.text = MinusCheck(atkRateBtn.newLevel - 1) ? (UserDataManager.Instance.weaponAtkRate + UserDataManager.Instance.statData.upgradeAtkSpd[atkRateBtn.newLevel - 1].sum1).ToString() : beforeAtkRate.text;

        weaponSpend = WeaponCalculator();        // 사용 금액
        weaponSpendExp.text = weaponSpend.ToString();
        weaponRemainExp.text = (UserDataManager.Instance.Exp - weaponSpend).ToString();
    }
    public void SetWeaponLevelBtn()
    {
        atkUpBtn.level = UserDataManager.Instance.WeaponAtkLv;
        critRateUpBtn.level = UserDataManager.Instance.WeaponCriDamageLv;
        critDmgBtn.level = UserDataManager.Instance.WeaponCriDamageLv;
        atkRateBtn.level = UserDataManager.Instance.WeaponAtkRateLv;
    }
    public int WeaponCalculator()
    {
        int result, afterAtk, curAtk, afterCritRate, curCritRate, afterCritDmg, curCritDmg, afterAtkRate ,curAtkRate;        // 계산식 필요

        afterAtk = MinusCheck(atkUpBtn.newLevel - 1) ? UserDataManager.Instance.statData.upgradeAtk[atkUpBtn.newLevel - 1].totalExp : 0;
        curAtk = MinusCheck(atkUpBtn.level - 1) ? UserDataManager.Instance.statData.upgradeAtk[atkUpBtn.level - 1].totalExp : 0;
        afterCritRate = MinusCheck(critRateUpBtn.newLevel - 1) ? UserDataManager.Instance.statData.upgradeCrit[critRateUpBtn.newLevel - 1].totalExp : 0;
        curCritRate = MinusCheck(critRateUpBtn.level - 1) ? UserDataManager.Instance.statData.upgradeCrit[critRateUpBtn.level - 1].totalExp : 0;
        afterCritDmg = MinusCheck(critDmgBtn.newLevel - 1) ? UserDataManager.Instance.statData.upgradeCritDmg[critDmgBtn.newLevel - 1].totalExp : 0;
        curCritDmg = MinusCheck(critDmgBtn.level - 1) ? UserDataManager.Instance.statData.upgradeCritDmg[critDmgBtn.level - 1].totalExp : 0;
        afterAtkRate = MinusCheck(atkRateBtn.newLevel - 1) ? UserDataManager.Instance.statData.upgradeAtkSpd[atkRateBtn.newLevel - 1].totalExp : 0;
        curAtkRate = MinusCheck(atkRateBtn.level - 1) ? UserDataManager.Instance.statData.upgradeAtkSpd[atkRateBtn.level - 1].totalExp : 0;

        result = (afterAtk - curAtk) + (afterCritRate - curCritRate) + (afterCritDmg - curCritDmg) + (afterAtkRate - curAtkRate);
        return result;
    }

    public void WEaponUpgrade()
    {
        if (weaponSpend == 0)
        {
            return;
        }
        //ToDo 업그레이드 작성 필요
        //// 구매했을 떄 일어나는 이벤트
        //if (weaponSpend <= UserDataManager.Instance.Exp)
        //{
        //    Debug.Log("구매 완료");

        //    Debug.Log($"새 레벨 : {hpUpBtn.newLevel}, 기존 레벨 : {hpUpBtn.level}");
        //    // 업그레이드 레벨을 새 레벨로 적용
        //    hpUpBtn.level = hpUpBtn.newLevel;
        //    goldIncreBtn.level = goldIncreBtn.newLevel;
        //    expIncreBtn.level = expIncreBtn.newLevel;

        //    Debug.Log($"새 레벨 : {hpUpBtn.newLevel}, 기존 레벨 : {hpUpBtn.level}");
        //    // 경험치 소모
        //    UserDataManager.Instance.Exp -= playerSpend;
        //    // 레벨 업데이트
        //    UserDataManager.Instance.PlayerStatusUpgrade(hpUpBtn.level, goldIncreBtn.level, expIncreBtn.level);
        //}
        //else
        //{
        //    Debug.Log("경험치가 부족합니다.");
        //    return;
        //}
        //UpdatePlayerStatusUI();
        //UpdatePlayerUpgradeUI();
        //ChangeStatusDisplayButton("Player");
        //UserDataManager.Instance.SavePlayerUpgrade();
    }

    #endregion

    // 클리어 UI 업데이트 가져오기
    public void UpdateClearDataUI(string[] clearDataList)
    {
        foreach (var item in clearDataList)
        {
            GameObject clearData;
            clearData = Instantiate(clearDataObj, clearDataObj.transform.position, clearDataObj.transform.rotation, contentPos);      // 클리어 데이터 추가
            clearData.transform.localScale = Vector3.one;

            clearData.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = item;
            clearData.SetActive(true);
        }
    }

    // 클리어 데이터 저장 테스트
    public void SaveTest()
    {
        string[] mbti = { "ISTJ", "ISTP", "ISFJ", "ISFP", "INTJ", "INTP", "INFJ", "INFP", "ESTJ", "ESTP", "ESFJ", "ESFP", "ENTJ", "ENTP", "ENTJ", "ENFP" };
        int rand = Random.Range(0, 16);
        UserDataManager.Instance.SaveClearData(mbti[rand]);

        GameObject clearData;
        clearData = Instantiate(clearDataObj, clearDataObj.transform.position, clearDataObj.transform.rotation, contentPos);      // 클리어 데이터 추가
        clearData.transform.localScale = Vector3.one;

        string Date = UserDataManager.Instance.clearDatas.list[clearDatas.Length].Date;
        string MBTI = UserDataManager.Instance.clearDatas.list[clearDatas.Length].MBTI;


        //TODO : 스트링빌더로 업데이트
        // 아래 형식으로 데이터 변환
        // 2023/11/21 08:23 0회차 MBTI INFP 
        string txt = $"{Date} | {clearDatas.Length} 회차 | MBTI {MBTI}";

        clearData.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = txt;
        clearData.SetActive(true);
    }

    // ############################### 메인 디스플레이 ###############################
    #region 메인 디스플레이
    // 메인 디스플레이 패널 변경 버튼
    public void ChangeDisplayButton(string name)
    {
        mainDisplay.SetActive(false);
        mbtiDisplay.SetActive(false);

        switch (name)
        {
            case "Main":
                mainDisplay.SetActive(true);
                break;
            case "MBTI":
                mbtiDisplay.SetActive(true);
                break;
        }
    }

    // 메인디스플레이와 상호작용 시 문 열림
    public void OpenSpawnRoomDoor()
    {
        spawnroomDoor.GetComponent<Animation>().Play("SpawnRoom_Open");
        openDoorButton.SetActive(false);
    }
    #endregion

    // ############################## 상태창 디스플레이 ##############################
    #region 상태창 디스플레이
    // 상태창을 위한 세팅
    private void SetStatusDisplay()
    {
        statusCollider = GetComponent<SphereCollider>();
        statusCollider.center = statusPos.position;
        statusDisplay.transform.position = new Vector3(statusPos.position.x + 0.5f, statusPos.position.y + 0.6f, statusPos.position.z);

        // PC 업그레이드 레벨 업데이트
        hpUpBtn.level = UserDataManager.Instance.HPLv;
        goldIncreBtn.level = UserDataManager.Instance.GainGoldLv;
        expIncreBtn.level = UserDataManager.Instance.GainExpLv;
    }

    // 상태창 패널 변경 버튼
    public void ChangeStatusDisplayButton(string name)
    {
        selectStatusDis.SetActive(false);
        playerStatusDis.SetActive(false);
        playerAcceptPannel.SetActive(false);

        weaponStatusDis.SetActive(false);
        selectSkillStatusDis.SetActive(false);

        skillUpgrade1.SetActive(false);
        skillUpgrade2.SetActive(false);
        skillUpgrade3.SetActive(false);
        skillUpgrade4.SetActive(false);

        switch (name)
        {
            case "Main":
                selectStatusDis.SetActive(true);
                break;
            case "Player":
                playerStatusDis.SetActive(true);
                break;
            case "Weapon":
                weaponStatusDis.SetActive(true);
                break;
            case "SelectSkill":
                selectSkillStatusDis.SetActive(true);
                break;
            case "Skill1":
                skillUpgrade1.SetActive(true);
                break;
            case "Skill2":
                skillUpgrade2.SetActive(true);
                break;
            case "Skill3":
                skillUpgrade3.SetActive(true);
                break;
            case "Skill4":
                skillUpgrade4.SetActive(true);
                break;
        }
    }
    #endregion

    // ########################### 상태창 디스플레이 트리거 ###########################
    #region 디스플레이 트리거
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            statusDisplay.GetComponent<Animation>().Play("Status_On");
            ChangeStatusDisplayButton("Main");
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            statusDisplay.GetComponent<Animation>().Play("Status_Off");
        }
    }
    #endregion

    // ########################### 메인 디스플레이 NPC 대화 ###########################


    // NPC 대사를 세팅하는 함수
    public void SetNpcDialog(int id)
    {
        int target = BinarySearch(DialogData.logs, 0, DialogData.logs.Count-1, id);
        // 대사 데이터에서 ID에 맞는 데이터 찾아오기

        if (target != -1)
        {
            // 탐색 완료 시 데이터 변경
            dialog = DialogData.logs[target];

            // 대화창 업데이트 후 디큐
            npcDialog.text = dialog.log.Peek().ToString();
            dialog.log.Dequeue();
        }
        else
            Debug.Log("대사 ID를 찾지 못했습니다." + target);
    }

    // 이진 탐색 트리 ID 찾아오기
    int BinarySearch(List<NpcDialog> list, int first, int last, int target)
    {
        int mid;       
        // 중간 값 초기화
        if (first > last)                // 만약 첫 숫자가 마지막 숫자보다 작을 경우
        {   
            return -1;                   // -1을 반환 : 탐색 실패를 의미
        }
        mid = (first + last) / 2;        // 탐색 영역을 반으로 나누고 탐색을 진행
        Debug.Log($"타겟 : {target}, 탐색 위치 : {list[mid].ID}");

        if (list[mid].ID == target)      // 타겟과 같다면
        {
            return mid;                  // 타겟 값 반환
        }
        else if (list[mid].ID < target)  // 만약 타겟이 크면
        {
            // mid 또한 탐색범위에 넣기 위해, +1
            return BinarySearch(list, mid + 1, last, target);
        }
        else
            // mid 또한 탐색범위에 넣기 위해, -1
            return BinarySearch(list, first, mid - 1, target);
       
    }

        // NPC와 대화/보상 수락 등을 하는 디스플레이 버튼
        public void DisplayButton()
    {
        if(dialog.log.Count != 0)
        {
            npcDialog.text = dialog.log.Peek().ToString();
            dialog.log.Dequeue();            
        }
        else
        OpenSpawnRoomDoor();
    }


    // NPC 대사 데이터를 가져오는 메서드
    public void GetNPCDialog()
    {
        DialogData.logs = new List<NpcDialog>();

        for (int i = 0; i <= 6; i++)
        {
            NpcDialog newDialog = new NpcDialog();
            newDialog.ID = i + questID;

            string log = Data.GetString(questID + i, "OutPutText");

            log = log.Replace("\\n", "\n");      // 두줄짜리는 한줄로 치환
            log = log.Replace("#", ",");         // "#" 은 ","
            log = log.Replace("\\", "");         // 슬래시가 있을 경우 삭제 

            newDialog._log = log.Split("\n");
            newDialog.log = new Queue(log.Split("\n"));

            DialogData.logs.Add(newDialog);
        }
    }

}
