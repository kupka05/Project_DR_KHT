using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    // [완료가능] 상태
    public class CanComplete : IState
    {
        /*************************************************
         *                 Public Methods
         *************************************************/
        // 현재 상태를 출력
        public void PrintCurrentState()
        {
            GFunc.Log("현재 상태: [완료가능]");
        }

        // 다음 상태로 변경 {[시작불가] -> [시작가능] -> [진행중] -> [완료가능] -> [완료]}
        public void ChangeToNextState(Quest quest, QuestState questState)
        {
            // [7] 증정 조건의 아이템 차감은
            // QuestHandler.GiveQuestReward()에서 처리한다.

            // [완료]으로 상태 변경
            questState.ChangeState(QuestState.StateQuest.COMPLETED);

            // 퀘스트 콜백 호출
            QuestCallback.OnQuestDataCallback();
        }
    }
}
