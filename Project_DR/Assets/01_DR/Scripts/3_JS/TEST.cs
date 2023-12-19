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
    }

    int index = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UserDataManager.quests[0].QuestState.PrintCurrentState();
            _questList = UserDataManager.quests;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Unit.AddInventoryItem(5001);
            UserDataManager.quests[0].ChangeToNextState();
            UserDataManager.quests[0].QuestState.PrintCurrentState();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UserDataManager.quests[0].GiveQuestReward();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            QuestCallback.OnInventoryCallback(5001);
        }
    }
}
