using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    // [시작가능] 상태
    public class CanStartable : IState
    {
        /*************************************************
         *                 Public Methods
         *************************************************/
        // 현재 상태를 출력
        public void PrintCurrentState()
        {
            GFunc.Log("현재 상태: [시작가능]");
        }

        // 다음 상태로 변경 {[시작불가] -> [시작가능] -> [진행중] -> [완료가능] -> [완료]}
        public void ChangeToNextState(Quest quest, QuestState questState)
        {
            // 현재 상태가 [시작가능]일 경우
            if (questState.State.Equals(QuestState.StateQuest.CAN_STARTABLE))
            {
                // [진행중]으로 상태 변경
                questState.ChangeState(QuestState.StateQuest.IN_PROGRESS);
            }
        }
    }
}
