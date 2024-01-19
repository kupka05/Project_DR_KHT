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

                // 서브, 특수 퀘스트일 경우 콜백 호출
                if (quest.QuestData.Type.Equals(QuestData.QuestType.SUB)
                    || quest.QuestData.Type.Equals(QuestData.QuestType.SPECIAL))
                {
                    QuestCallback.OnSubspecialQuestProgressCallback(quest);
                }

                // 현재 퀘스트가 특수 퀘스트일 경우
                if (quest.QuestData.Type.Equals(QuestData.QuestType.SPECIAL))
                {
                    // 효과음 재생
                    AudioManager.Instance.AddSFX("msfx_explosion_3_explode");
                    AudioManager.Instance.PlaySFX("msfx_explosion_3_explode");
                }

                // 그 외의 경우
                else
                {
                    // 효과음 재생
                    AudioManager.Instance.AddSFX("SFX_Quest_UI_message_01");
                    AudioManager.Instance.PlaySFX("SFX_Quest_UI_message_01");
                }
            }
        }
    }
}
