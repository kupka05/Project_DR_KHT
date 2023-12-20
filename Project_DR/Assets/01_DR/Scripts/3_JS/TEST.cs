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
            UserDataManager.QuestList[0].QuestState.PrintCurrentState();
            _questList = UserDataManager.QuestList;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Unit.AddInventoryItem(5001);
            UserDataManager.QuestList[0].ChangeToNextState();
            UserDataManager.QuestList[0].QuestState.PrintCurrentState();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UserDataManager.QuestList[0].ClearQuest();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            QuestCallback.OnInventoryCallback(5001);
        }
    }
}
