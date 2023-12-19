using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    // [진행중] 상태
    public class InProgress : IState
    {
        /*************************************************
         *                 Public Methods
         *************************************************/
        // 현재 상태를 출력
        public void PrintCurrentState()
        {
            GFunc.Log("현재 상태: [진행중]");
        }

        // 다음 상태로 변경(시작불가 -> 시작가능 -> 진행중 -> 완료 가능 -> 완료)
        public void ChangeToNextState(Quest quest)
        {
                GFunc.Log($"진행도: CurrentValue: {quest.QuestData.CurrentValue} >= ClearValue: {quest.QuestData.ClearValue}");
            // 현재 퀘스트 달성 값이 목표 값 이상일 경우
            if (quest.QuestData.CurrentValue >= quest.QuestData.ClearValue)
            {
                // [완료가능]으로 상태 변경
                quest.QuestState.ChangeState(QuestState.StateQuest.CAN_COMPLETE);
            }
        }
    }
}
