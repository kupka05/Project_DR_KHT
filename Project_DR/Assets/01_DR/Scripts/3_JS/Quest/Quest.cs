using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    public class Quest
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public QuestData QuestData => _questData;                                              // 퀘스트 데이터
        public QuestRewardData ClearRewardData => _questData.ClearReward.QuestRewardData;      // 클리어 보상 데이터
        public QuestRewardData FailRewardData => _questData.FailReward.QuestRewardData;        // 실패 보상 데이터


        /*************************************************
         *                 Private Fields
         *************************************************/
        private QuestData _questData;


        /*************************************************
         *                 Public Methods
         *************************************************/
        public Quest(int id)
        {
            // Init
            _questData = new QuestData(id);
        }

        // 퀘스트가 목표 수치에 달성했는지 체크
        // [true = 달성] / [false = 비달성]
        public bool IsQuestCompleted()
        {
            // 현재 달성 수치가 목표 수치 이상일 경우
            if (_questData.CurrentValue >= _questData.ClearValue)
            {
                return true;
            }

            // 아닐 경우
            return false;
        }
    }
}
