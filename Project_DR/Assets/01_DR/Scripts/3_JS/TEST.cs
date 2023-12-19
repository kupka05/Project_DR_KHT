using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Js.Quest;
using System;

public class TEST : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private List<Quest> _questList;
    void Start()
    {
        QuestManager.Instance.CreateQuest(1_000_000_1);
        _questList = UserDataManager.quests;
    }

    int index = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _questList[0].QuestState.ChangeToNextState();
            _questList[0].QuestState.PrintCurrentState();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Unit.AddInventoryItem(5001);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _questList[0].GiveQuestReward();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            QuestCallback.OnCallbackInventory(5001);
        }
    }
}
