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
    public GameObject selectStatusDis;      // 선택창
    public GameObject playerStatusDis;      // 플레이어 상태창
    public GameObject weaponStatusDis;      // 무기 상태창
    public GameObject selectSkillStatusDis; // 스킬 선택창
    public GameObject skillStatusDis;       // 스킬 상태창


    public void Start()
    {
        // 상태창 세팅
        statusCollider = GetComponent<SphereCollider>();   
        statusCollider.center = statusPos.position;
        statusDisplay.transform.position = new Vector3(statusPos.position.x + 0.5f, statusPos.position.y + 0.6f, statusPos.position.z);

        ChangeDisplayButton("Main");          // 시작 시 메인디스플레이로
        ChangeStatusDisplayButton("Main");    // 시작 시 메인디스플레이로
        SetClearData(clearDatas);             // 디버그용 클리어 데이터 세팅
    }


    // 디스플레이 화면 변경 버튼
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

    public void ChangeStatusDisplayButton(string name)
    {
        selectStatusDis.SetActive(false);
        playerStatusDis.SetActive(false);
        weaponStatusDis.SetActive(false);
        selectSkillStatusDis.SetActive(false);
        skillStatusDis.SetActive(false);

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
            case "Skill":
                skillStatusDis.SetActive(true);
                break;
        }

    }

    // 메인디스플레이와 상호작용 시 문 열리게하기
    public void OpenSpawnRoomDoor()
    {
        spawnroomDoor.GetComponent<Animation>().Play("SpawnRoom_Open");
        openDoorButton.SetActive(false);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 진입");
            statusDisplay.GetComponent<Animation>().Play("Status_On");
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 진입");
            statusDisplay.GetComponent<Animation>().Play("Status_Off");
        }
    }


}
