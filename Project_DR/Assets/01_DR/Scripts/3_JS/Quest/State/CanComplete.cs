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
            GFunc.Log("ChangeToNextState");
            // [7] 증정 조건의 아이템 차감은
            // QuestHandler.GiveQuestReward()에서 처리한다.

            // [완료]으로 상태 변경
            questState.ChangeState(QuestState.StateQuest.COMPLETED);

            // 퀘스트 콜백 호출
            QuestCallback.OnQuestDataCallback();

            // 서브, 특수 퀘스트일 경우 콜백 호출
            if (quest.QuestData.Type.Equals(QuestData.QuestType.SUB)
                || quest.QuestData.Type.Equals(QuestData.QuestType.SPECIAL))
            {
                QuestCallback.OnSubspecialQuestCompletedCallback(quest);
            }

            // 효과음 재생
            AudioManager.Instance.AddSFX("SFX_Quest_UI_Complete_01");
            AudioManager.Instance.PlaySFX("SFX_Quest_UI_Complete_01");
        }
    }
}
