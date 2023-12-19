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

        // 퀘스트 리워드 지급
        // [클리어 보상] / [실패 보상]
        public void GiveQuestReward()
        {
            QuestHandler.GiveQuestReward();
        }
    }
}
