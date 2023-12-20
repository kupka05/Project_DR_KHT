using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    // [시작불가] 상태
    public class NotStartable : IState
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public List<Quest> QuestList => UserDataManager.QuestList;      // 플레이어가 보유한 퀘스트 리스트


        /*************************************************
         *                 Public Methods
         *************************************************/
        // 현재 상태를 출력
        public void PrintCurrentState()
        {
            GFunc.Log("현재 상태: [시작불가]");
        }

        // 다음 상태로 변경 {[시작불가] -> [시작가능] -> [진행중] -> [완료가능] -> [완료]}
        public void ChangeToNextState(Quest quest, QuestState questState)
        {
            // 선행 퀘스트가 있을 경우
            if (quest.QuestData.RequiredQuestID.Equals(0).Equals(false))
            {
                // 보유한 퀘스트 리스트를 순회
                foreach (var item in QuestList)
                {
                    // 보유 퀘스트에 선행 퀘스트가 있는지 확인
                    // && 선행 퀘스트의 상태가 [완료] 상태인지 확인
                    if (item.QuestData.ID.Equals(quest.QuestData.RequiredQuestID)
                        && item.QuestState.State.Equals(QuestState.StateQuest.COMPLETED))
                    {
                        // 디버그
                        GFunc.Log($"선행퀘스트: {quest.QuestData.RequiredQuestID}, 상태: {item.QuestState}");

                        // 일치할 경우 [시작가능]으로 상태 변경
                        questState.ChangeState(QuestState.StateQuest.CAN_STARTABLE);

                        // 종료
                        return;
                    }
                }
            }

            // 선행 퀘스트가 없을 경우
            else
            {
                // [시작가능]으로 상태 변경
                questState.ChangeState(QuestState.StateQuest.CAN_STARTABLE);
            }
        }
    }
}
