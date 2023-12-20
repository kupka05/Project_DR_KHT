using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    public class QuestHandler : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public QuestData QuestData => _quest.QuestData;                     // 퀘스트 데이터
        public QuestReward ClearReward => QuestData.ClearReward;            // 클리어 보상 데이터
        public QuestReward FailReward => QuestData.FailReward;              // 실패 보상 데이터
        public QuestState QuestState => _quest.QuestState;                  // 퀘스트 상태


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private Quest _quest;                              // 퀘스트


        /*************************************************
         *                 Public Methods
         *************************************************/
        public QuestHandler(Quest quest)
        {
            // Init
            _quest = quest;
        }

        // 퀘스트 리워드 지급
        public void GiveQuestReward()
        {
            // 퀘스트가 [완료가능] 상태일 경우
            if (_quest.QuestState.State.Equals(QuestState.StateQuest.CAN_COMPLETE))
            {
                // [완료] 상태 변경 & 클리어 보상 지급
                QuestState.ChangeToNextState();
                ClearReward.GetReward();
            }

            // 아닐 경우
            // 퀘스트가 [진행중] 상태일 경우
            else if (_quest.QuestState.State.Equals(QuestState.StateQuest.IN_PROGRESS))
            {
                // [실패] 상태 변경 & 실패 보상 지급
                QuestState.ChangeState(QuestState.StateQuest.FAILED);
                FailReward.GetReward();
            }
        }

        // 퀘스트 상태를 다음 단계로 변경
        // 조건을 충족해야 다음 단계로 변경된다.
        public void ChangeToNextState()
        {
            QuestState.ChangeToNextState();
        }

        // 퀘스트 상태를 출력
        public void PrintCurrentState()
        {
            QuestState.PrintCurrentState();
        }

        // 현재 퀘스트 달성 값 추가
        public void AddCurrentValue(int value = 1)
        {
            QuestData.AddCurrentValue(value);
        }

        // 현재 퀘스트 달성 값 변경
        public void ChangeCurrentValue(int value)
        {
            QuestData.ChangeCurrentValue(value);
        }
    }
}
