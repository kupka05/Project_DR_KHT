using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    public class Quest : ScriptableObject
    {
        /*************************************************
         *                 Public Fields
         *************************************************/

        public QuestData QuestData => _questData;               // 퀘스트 데이터
        public QuestState QuestState => _questState;            // 퀘스트 상태
        public QuestHandler QuestHandler => _questHandler;      // 퀘스트 핸들러


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private QuestData _questData;
        [SerializeField] private QuestState _questState;
        [SerializeField] private QuestHandler _questHandler;


        /*************************************************
         *                 Public Methods
         *************************************************/
        public Quest(int id)
        {
            // Init
            _questData = new QuestData(id);
            _questState = new QuestState(this);
            _questHandler = new QuestHandler(this);
        }

        // 퀘스트 클리어
        // 보상 지급: [클리어 보상] / [실패 보상]
        // 해당하는 이벤트 ID 반환
        public int[] ClearQuest()
        {
            // 퀘스트 클리어 후 클리어 / 실패 이벤트 ID 반환
            int[] eventIDs = QuestHandler.ClearQuest();
            // 오류 발생시 디버그 메세지 출력
            if (eventIDs.Length.Equals(0)) { GFunc.Log($"Quest.ClearQuest(): ID[{QuestData.ID}] 퀘스트의 클리어가 실패했습니다. " +
                $"진행중인 상태의 퀘스트만 클리어할 수 있습니다."); }
            return eventIDs;
        }

        // 퀘스트를 다음 상태로 변경
        // [시작불가] -> [시작가능] -> [진행중] -> [완료가능] -> [완료]
        public void ChangeToNextState()
        {
            QuestHandler.ChangeToNextState();
        }

        // 퀘스트 현재 값 증가
        public void AddCurrentValue(int value = 1)
        {
            QuestHandler.AddCurrentValue(value);
        }

        // 현재 퀘스트 달성 값 변경
        public void ChangeCurrentValue(int value)
        {
            QuestHandler.ChangeCurrentValue(value);
        }
    }
}
