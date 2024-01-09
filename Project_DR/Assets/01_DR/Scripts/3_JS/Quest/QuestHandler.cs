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

        // 퀘스트를 초기 상태로 변경한다.
        // 보유하고 있는 진행 값도 초기화한다.
        public void ResetQuest()
        {
            // 퀘스트를 [시작불가] 상태로 변경
            ChangeState(0);

            // 퀘스트의 현재 진행 값을 초기화
            ChangeCurrentValue(0);

            // 보유한 모든 퀘스트의 상태를 업데이트
            QuestManager.Instance.UpdateQuestStatesToNotStartable();
        }

        // 퀘스트가 [시작가능] 상태 조건을 충족하는지 체크
        public bool CheckStateForCanStartable()
        {
            return QuestState.CheckStateForCanStartable();
        }

        // 퀘스트 클리어
        public int[] ClearQuest()
        {
            switch(GiveQuestReward())
            {
                // 퀘스트를 클리어 했을 경우
                case 1:
                    int[] clearEventIDs = _quest.QuestData.ClearEventIDs;
                    GFunc.Log($"Quest.ClearQuest(): ID[{_quest.QuestData.ID}] 퀘스트를 클리어 완료");
                    return clearEventIDs;

                // 퀘스트를 실패 했을 경우
                case 2:
                    int[] failEventIDs = _quest.QuestData.FailEventIDs;
                    GFunc.Log($"Quest.ClearQuest(): ID[{_quest.QuestData.ID}] 퀘스트를 클리어 실패");
                    return failEventIDs;

                // 퀘스트를 클리어 할 수 없을 경우
                default:
                    GFunc.Log($"Quest.ClearQuest(): ID[{_quest.QuestData.ID}] 퀘스트를 클리어 할 수 없습니다.");
                    return default;
            }
        }

        // 퀘스트 상태를 다음 단계로 변경
        // 조건을 충족해야 다음 단계로 변경된다.
        public void ChangeToNextState()
        {
            QuestState.ChangeToNextState();
        }

        // 퀘스트를 특정 상태로 변경
        public void ChangeState(int state)
        {
            QuestState.StateQuest changeState = (QuestState.StateQuest)state;
            QuestState.ChangeState(changeState);
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


        /*************************************************
         *               Private Methods
         *************************************************/
        // 퀘스트 리워드 지급
        private int GiveQuestReward()
        {
            // 퀘스트 [성공!]
            // 퀘스트가 [완료가능] 상태일 경우
            if (_quest.QuestState.State.Equals(QuestState.StateQuest.CAN_COMPLETE))
            {
                // 조건이 [7]증정일 경우
                // && 아이템 차감시도 하고 실패할 경우
                if (QuestData.Condition.Equals(QuestData.ConditionType.GIVE_ITEM))
                {
                    if (! Unit.RemoveInventoryItemForID(QuestData.KeyID, QuestData.ClearValue))
                    {
                        // [2]'진행중'으로 상태 변경 & 처리 종료
                        QuestState.ChangeState(QuestState.StateQuest.IN_PROGRESS);
                        return 0;
                    }
                }

                // [완료] 상태 변경 & 클리어 보상 지급
                QuestState.ChangeToNextState();
                ClearReward.GetReward();      

                // 퀘스트 성공 보상 텍스트 출력
                Unit.PrintRewardText(ClearReward.QuestRewardData.ID);

                return 1;
            }

            // 퀘스트 [실패!]
            // 아닐 경우
            // 퀘스트가 [진행중] 상태일 경우
            else if (_quest.QuestState.State.Equals(QuestState.StateQuest.IN_PROGRESS))
            {
                // [실패] 상태 변경 & 실패 보상 지급
                QuestState.ChangeState(QuestState.StateQuest.FAILED);
                FailReward.GetReward();

                // 퀘스트 실패 보상 출력
                Unit.PrintRewardText(FailReward.QuestRewardData.ID);

                return 2;
            }

            // 아닐 경우 예외 처리용 0 반환
            return 0;
        }
    }
}
