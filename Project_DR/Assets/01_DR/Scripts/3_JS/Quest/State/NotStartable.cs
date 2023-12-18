using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    // [시작불가] 상태
    public class NotStartable : IState
    {
        /*************************************************
         *                 Public Methods
         *************************************************/
        // 현재 상태를 출력
        public void PrintCurrentState()
        {
            GFunc.Log("현재 상태: [시작불가]");
        }

        // 다음 상태로 변경(시작불가 -> 시작가능 -> 진행중 -> 완료 가능 -> 완료)
        public void ChangeToNextState(Quest quest)
        {
            // TODO: 시작가능한지 선행 퀘스트 체크
            // [시작가능]으로 상태 변경
            quest.QuestState.ChangeState(QuestState.StateQuest.CAN_STARTABLE);
        }
    }
}
