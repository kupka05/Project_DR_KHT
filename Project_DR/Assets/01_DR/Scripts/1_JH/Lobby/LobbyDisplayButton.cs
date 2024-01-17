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
        PlayerStatus,
        Weapon,
        Skill
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
    public int level;
  
    public int newLevel;                 
    public Transform contentPos;        // 아이템이 들어갈 컨텐츠의 위치
    public GameObject item;             // 레벨에 따라 들어가는 아이템
    private GameObject[] items;

    public bool isActive=false;

    public void Start()
    {
        lobbyEvent = transform.root.GetComponent<LobbyEvent>();
        AudioManager.Instance.AddSFX("SFX_Stat_Upgrade_LevelUP_01");
        AudioManager.Instance.AddSFX("SFX_Stat_Upgrade_LevelDown_01");
        AudioManager.Instance.AddSFX("SFX_Stat_Upgrade_Choose_01");
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

                lobbyEvent?.UpdatePlayerUpgradeUI();
                lobbyEvent?.SetPlayerLevelBtn();
                newLevel = level;
                break;

            case ButtonType.Weapon:
                leftButton.SetActive(false);
                rightButton.SetActive(false);
                if (beforeValue)
                    beforeValue.SetActive(true);
                if (afterValue)
                    afterValue.SetActive(false);
                acceptPannel.SetActive(false);

                lobbyEvent?.UpdateWeaponUpgradeUI();
                lobbyEvent?.SetWeaponLevelBtn();
                newLevel = level;
                break;

            case ButtonType.Skill:
                leftButton.SetActive(false);
                rightButton.SetActive(false);
                acceptPannel.SetActive(false);
                lobbyEvent?.SetSkillLevelBtn();
                lobbyEvent?.UpdateSkillUpgradeUI();

                newLevel = level;
                break;
        }

        isActive = false;

    }
    public void OnDisable()
    {
        if (items != null)
        {

            foreach (var obj in items)
            { 
                if (obj)
                { Destroy(obj.gameObject); }
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

        AudioManager.Instance.PlaySFX("SFX_Stat_Upgrade_Choose_01");
    }

    // 레벨 생성
    private void SetLevelItem(int _index, int _level)
    {
        items = new GameObject[index+1];
        for (int i = 1; i <= _index; i++)
        {
            items[i] = Instantiate(item, item.transform.position, item.transform.rotation, contentPos);      // 클리어 데이터 추가
            items[i].transform.localScale = Vector3.one;
            items[i].SetActive(true);

            if (i <= _level)
            {
                items[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    // 레벨 조정 버튼
    public void SetLevelButton(int value)
    {
        newLevel += value;

        if (newLevel < level)
        {
            newLevel = level;
            return;
        }
        else if (index < newLevel)
        {
            newLevel = index;
            return;
        }

        if (0 < value)
        {
            AudioManager.Instance.PlaySFX("SFX_Stat_Upgrade_LevelUP_01");
        }
        else
        {
            AudioManager.Instance.PlaySFX("SFX_Stat_Upgrade_LevelDown_01");
        }
        SetLevel();
    }
    public void SetLevel()
    {
        if (newLevel == 0 && item != null)
        {
            for (int i = 1; i <= index; i++)
            {
                items[i].GetComponent<Image>().color = Color.black;
            }
        }

        else
        {
            for (int i = 1; i <= index; i++)
            {
                items[i].GetComponent<Image>().color = Color.black;
                if (i <= newLevel)
                {
                    items[i].GetComponent<Image>().color = Color.yellow;
                }

                if (i <= level)
                {
                    items[i].GetComponent<Image>().color = Color.white;
                }
            }
        }
        if (type == ButtonType.PlayerStatus)
            lobbyEvent?.UpdatePlayerUpgradeUI();
        else if (type == ButtonType.Weapon)
            lobbyEvent?.UpdateWeaponUpgradeUI();
        else if (type == ButtonType.Skill)
            lobbyEvent?.UpdateSkillUpgradeUI();
    }
}
