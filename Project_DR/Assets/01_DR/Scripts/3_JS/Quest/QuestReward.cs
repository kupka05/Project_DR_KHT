using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    public class QuestReward
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public QuestRewardData QuestRewardData => _questRewardData;       // 퀘스트 보상 데이터


        /*************************************************
         *                 Private Fields
         *************************************************/
        private QuestRewardData _questRewardData;


        /*************************************************
         *                 Public Methods
         *************************************************/
        public QuestReward(int id)
        {
            // Init
            _questRewardData = new QuestRewardData(id);
        }
    }
}
