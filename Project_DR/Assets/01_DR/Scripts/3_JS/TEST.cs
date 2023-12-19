using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Js.Quest;

public class TEST : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Quest _quest;
    void Start()
    {
        _quest = new Quest(1_000_000_1);
        Unit.AddInventoryItem(5001);
    }

    int index = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _quest.QuestState.ChangeToNextState();
            _quest.QuestState.PrintCurrentState();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _quest.QuestData.AddCurrentValue();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _quest.GiveQuestReward();
        }
    }
}
