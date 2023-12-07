using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyEvent : MonoBehaviour
{
    [Header("Door")]
    public GameObject spawnroomDoor;
    public GameObject openDoorButton;

    [Header("Main Display")]
    public GameObject mainDisplay;
    public GameObject mbtiDisplay;

    [Header("ClearData")]
    public string[] clearDatas;             // 디버그용 클리어 데이터
    public GameObject clearDataObj;
    public Transform contentPos;

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
    public TMP_Text curPlayerHp;
    public TMP_Text beforePlayerHp;
    public TMP_Text afterPlayerHp;

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

    public void Start()
    {
        SetStatusDisplay();
        ChangeDisplayButton("Main");          // 메인 디스플레이 시작 시 메인 패널로
        ChangeStatusDisplayButton("Main");    // 상태창 시작 시 메인 패널로

        // 옵저버 등록
        UserDataManager.Instance.OnUserDataUpdate += UpdatePlayerStatusUI;
    }

    public void Update()
    {
        if(Input.GetKeyDown("."))
        {
            UserDataManager.Instance.Gold += 500;
        }
    }

    // ============================ 데이터 불러오기 ============================

    // DB에서 데이터 불러오기 완료 후 이벤트로 실행
    public void GetDataFromDB()
    {
        // ToDo. 데이터 불러와지면 삭제예정
        UserDataManager.Instance.Exp = 5000;

        GetClearData();
        UpdatePlayerStatusUI();
    }

    public void UpdatePlayerStatusUI()
    {
        Debug.Log("실행하나?");

        // ToDo : 테스트 값에 넣어야 할 데이터 세팅
        int testValue = 100;

        if(UserDataManager.Instance.PlayerID == "")
        {
            UserDataManager.Instance.PlayerID = "Admin";
        }

        playerID.text = UserDataManager.Instance.PlayerID;
        playerGold.text = UserDataManager.Instance.Gold.ToString();
        playerExp.text = UserDataManager.Instance.Exp.ToString();

        curPlayerHp.text = UserDataManager.Instance.HP.ToString();
        beforePlayerHp.text = UserDataManager.Instance.HP.ToString();
        afterPlayerHp.text = (UserDataManager.Instance.HP + testValue).ToString();

        curGoldIncre.text = UserDataManager.Instance.GoldIncrease.ToString();
        beforeGoldIncre.text = UserDataManager.Instance.GoldIncrease.ToString();
        afterGoldIncre.text = (UserDataManager.Instance.GoldIncrease + testValue).ToString();

        curExpIncre.text = UserDataManager.Instance.ExpIncrease.ToString();
        beforeExpIncre.text = UserDataManager.Instance.ExpIncrease.ToString();
        afterExpIncre.text = (UserDataManager.Instance.ExpIncrease + testValue).ToString();

        curExp.text = (UserDataManager.Instance.Exp).ToString();
        spendExp.text = (testValue).ToString();
        remainExp.text = (UserDataManager.Instance.Exp- testValue).ToString();
    }

    // 클리어 데이터 가져오기
    public void GetClearData()
    {
        // 클리어 카운트 가져와서 카운트만큼 배열 할당
        clearDatas = new string[UserDataManager.Instance.clearDatas.list.Count];
        for (int i = 0; i < clearDatas.Length; i++)
        {
            string Date = UserDataManager.Instance.clearDatas.list[i].Date;
            string MBTI = UserDataManager.Instance.clearDatas.list[i].MBTI;

            // 아래 형식으로 데이터 변환
            // 2023/11/21 08:23 0회차 MBTI INFP 
            string clearData = $"{Date} | {i + 1} 회차 | MBTI {MBTI}";

            // 배열에 담기
            clearDatas[i] = clearData;
        }
        SetClearData(clearDatas);
    }

    // 클리어 데이터 세팅
    public void SetClearData(string[] clearDataList)
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


    // ============================ 메인 디스플레이 ============================

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

    // ============================ 상태창 디스플레이 ============================
    
    // 상태창을 위한 세팅
    private void SetStatusDisplay()
    {
        statusCollider = GetComponent<SphereCollider>();
        statusCollider.center = statusPos.position;
        statusDisplay.transform.position = new Vector3(statusPos.position.x + 0.5f, statusPos.position.y + 0.6f, statusPos.position.z);
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





    // ========================= 상태창 디스플레이 트리거 =========================

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            statusDisplay.GetComponent<Animation>().Play("Status_On");
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            statusDisplay.GetComponent<Animation>().Play("Status_Off");
        }
    }


}
