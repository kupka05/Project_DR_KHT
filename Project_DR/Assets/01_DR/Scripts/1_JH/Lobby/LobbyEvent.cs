using Meta.WitAi.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LobbyEvent : MonoBehaviour
{
    [Header("Data")]
    public Action dbRequest;
    //private StatData data;

    [Header("Main Display")]
    public GameObject mainDisplay;
    public GameObject mbtiDisplay;

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

    [Header("Status Pannel")]
    public GameObject selectStatusDis;      // 선택창
    public GameObject playerStatusDis;      // 플레이어 상태창
    public GameObject playerAcceptPannel;   // 확인창
    public GameObject weaponStatusDis;      // 무기 상태창
    public GameObject selectSkillStatusDis; // 스킬 선택창
    public GameObject skillStatusDis;       // 스킬 상태창

    public GameObject skillUpgrade1;       // 스킬 상태창
    public GameObject skillUpgrade2;       // 스킬 상태창
    public GameObject skillUpgrade3;       // 스킬 상태창
    public GameObject skillUpgrade4;       // 스킬 상태창

    [Header("Player Status")]
    public TMP_Text playerLevel;
    public TMP_Text playerID;
    public TMP_Text playerGold;
    public TMP_Text playerExp;

    [Header("Player Upgrade")]
    public LobbyDisplayButton hpUpBtn;      // hp 증가 버튼
    public LobbyDisplayButton goldIncreBtn; // 골드 증가 버튼
    public LobbyDisplayButton expIncreBtn;  // 경험치 증가 버튼

    public TMP_Text curPlayerHp;
    public TMP_Text beforePlayerHp;
    public TMP_Text afterPlayerHp;

    // 강화 계산용 변수
    public int hpLv;
    public int goldLv;
    public int expLv;
    public int playerSpendExp;

    [Header("Gold Upgrade")]
    public TMP_Text curGoldIncre;
    public TMP_Text beforeGoldIncre;
    public TMP_Text afterGoldIncre;

    [Header("Exp Upgrade")]
    public TMP_Text curExpIncre;
    public TMP_Text beforeExpIncre;
    public TMP_Text afterExpIncre;

    [Header("Spend Exp")]
    public TMP_Text curExp;
    public TMP_Text spendExp;
    public TMP_Text remainExp;


    // ################################## START ##################################

    public void Start()
    {
        
        dbRequest += GetDataFromDB;                     // DB 데이터 요청 성공 시 액션 추가
        UserDataManager.Instance.DBRequst(dbRequest);   // DB 데이터 요청


        ChangeDisplayButton("Main");          // 메인 디스플레이 시작 시 메인 패널로
        SetStatusDisplay();
        ChangeStatusDisplayButton("Main");    // 상태창 시작 시 메인 패널로


        // 옵저버 등록
        UserDataManager.Instance.OnUserDataUpdate += UpdatePlayerStatusUI;
    }

    // ############################### 데이터 불러오기 ###############################

    // DB에서 데이터 불러오기 완료 후 이벤트로 실행
    public void GetDataFromDB()
    {
        GetClearData();

        UpdateClearDataUI(clearDatas);             // 메인 디스플레이의 클리어 데이터 업데이트
        UpdatePlayerStatusUI();
        UpdatePlayerUpgradeUI();
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

    // ################################# 플레이어 업그레이드 UI #################################

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

        curExp.text = UserDataManager.Instance.Exp.ToString();
        spendExp.text = 0.ToString();
        remainExp.text = 0.ToString();
    }
    // 플레이어 업그레이드 상태창 UI 업데이트
    public void UpdatePlayerUpgradeUI()
    {
        hpLv = hpUpBtn.newLevel-1;
        goldLv = goldIncreBtn.newLevel-1;
        expLv = expIncreBtn.newLevel-1;

        // 레벨은 0이하로 될 수 없음. 삼항연산자 활용
        afterPlayerHp.text = MinusCheck(hpLv) ? (UserDataManager.Instance.DefaultHP + UserDataManager.Instance.statData.upgradeHp[hpLv].sum).ToString() : beforePlayerHp.text;
        afterGoldIncre.text = MinusCheck(goldLv) ? (UserDataManager.Instance.statData.upgradeGainGold[goldLv].sum).ToString() : beforeGoldIncre.text;
        afterExpIncre.text = MinusCheck(expLv) ? (UserDataManager.Instance.statData.upgradeGainExp[expLv].sum).ToString() : beforeExpIncre.text;

        playerSpendExp = PlayerCalculator();        // 사용 금액
        spendExp.text = playerSpendExp.ToString();
        remainExp.text = (UserDataManager.Instance.Exp - playerSpendExp).ToString();

       
    }
    public void PlayerUpgrade()
    {
        if(playerSpendExp == 0)
        {
            return;
        }

        if (playerSpendExp <= UserDataManager.Instance.Exp)
        {
            Debug.Log("구매 완료");        
            hpUpBtn.level = hpUpBtn.newLevel;
            goldIncreBtn.level = goldIncreBtn.newLevel;
            expIncreBtn.level = expIncreBtn.newLevel;

            UserDataManager.Instance.Exp -= playerSpendExp;
            if (hpUpBtn.level > 1)
            { UserDataManager.Instance.HP = UserDataManager.Instance.DefaultHP + UserDataManager.Instance.statData.upgradeHp[hpUpBtn.level-1].sum; }
            if (goldIncreBtn.level > 1)
            { UserDataManager.Instance.GainGold = UserDataManager.Instance.statData.upgradeGainGold[goldIncreBtn.level-1].sum; }
            if (expIncreBtn.level > 1)
            { UserDataManager.Instance.GainExp = UserDataManager.Instance.statData.upgradeGainExp[expIncreBtn.level-1].sum; }

            UserDataManager.Instance.HPUpgrade = hpUpBtn.level;
            UserDataManager.Instance.GainGoldUpgrade = goldIncreBtn.level;
            UserDataManager.Instance.GainExpUpgrade = expIncreBtn.level;
            UserDataManager.Instance.Level = hpUpBtn.level + goldIncreBtn.level + expIncreBtn.level;
        }
        else
        {
            Debug.Log("경험치가 부족합니다.");
            return;
        }
        UpdatePlayerStatusUI();
        UpdatePlayerUpgradeUI();
        ChangeStatusDisplayButton("Player");
    }

    // 업그레이드를 위한 계산기
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
        hpUpBtn.level = UserDataManager.Instance.HPUpgrade;
        goldIncreBtn.level = UserDataManager.Instance.GainGoldUpgrade;
        expIncreBtn.level = UserDataManager.Instance.GainExpUpgrade;
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

}
