using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyDisplayButton : MonoBehaviour
{
    public enum ButtonType
    {
        Default,
        PlayerStatus
    }

    public ButtonType type = ButtonType.Default;
    private LobbyEvent lobbyEvent;

    [Header("Player Status")]

    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject acceptPannel;
    public GameObject beforeValue;
    public GameObject afterValue;

    public int index;                   // 아이템의 개수
    public int level;                   // 해당 아이템의 레벨
    public int newLevel;                 
    public Transform contentPos;        // 아이템이 들어갈 컨텐츠의 위치
    public GameObject item;             // 레벨에 따라 들어가는 아이템
    private GameObject[] items;

    public bool isActive=false;

    public void Start()
    {
        lobbyEvent = transform.root.GetComponent<LobbyEvent>();
    }

    public void OnEnable()
    {
        switch (type)
        {
            case ButtonType.Default:
                return;
            case ButtonType.PlayerStatus:
                leftButton.SetActive(false);
                rightButton.SetActive(false);
                if(beforeValue)
                beforeValue.SetActive(true);
                if (afterValue)
                afterValue.SetActive(false);
                acceptPannel.SetActive(false);

                newLevel = level;
                break;

        }

        isActive = false;

    }
    public void OnDisable()
    {
        if (items != null)
        {

            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }
        }
    }

    // 플레이어 스탯 업그레이드 버튼
    public void PlayerStatusUpgradeButton()
    {
        if (isActive)
            return;

        isActive = true;
        leftButton.SetActive(true);
        rightButton.SetActive(true);
        acceptPannel.SetActive(true);
        if (beforeValue)
        beforeValue.SetActive(false);
        if (afterValue)
        afterValue.SetActive(true);
        SetLevelItem(index, level);
    }

    // 레벨 생성
    private void SetLevelItem(int _index, int _level)
    {
        items = new GameObject[index];
        for (int i = 0; i < _index; i++)
        {

            items[i] = Instantiate(item, item.transform.position, item.transform.rotation, contentPos);      // 클리어 데이터 추가
            items[i].transform.localScale = Vector3.one;
            items[i].SetActive(true);

            if (i < _level)
            {
                items[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    // 레벨 조정 버튼
    public void SetLevelButton(int value)
    {
        newLevel += value;
        if(newLevel < 0 )
        {
            newLevel = 0;
            return;
        }
        else if (10 < newLevel)
        {
            newLevel = 10;
            return;
        }


        if (newLevel <= level)
        {
            newLevel = level;
        }
        else if(index < newLevel)
        {
            newLevel = index;
        }

        for (int i = 0; i < index; i++)
        {
            items[i].GetComponent<Image>().color = Color.black;
            if (i < newLevel)
            {
                items[i].GetComponent<Image>().color = Color.yellow;
            }

            if (i < level)
            {
                items[i].GetComponent<Image>().color = Color.white;
            }
        }

        lobbyEvent.UpdatePlayerUpgradeUI();
    }

}
