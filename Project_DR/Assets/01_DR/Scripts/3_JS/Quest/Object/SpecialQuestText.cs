using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Js.Quest
{
    public class SpecialQuestText : MonoBehaviour
    {
        /*************************************************
         *                Private Fields
         *************************************************/
        public int QuestID => _quest.QuestData.ID;


        /*************************************************
         *                Private Fields
         *************************************************/
        private Quest _quest;
        private TMP_Text _text;


        /*************************************************
         *                 Unity Events
         *************************************************/
        private void Start()
        {
            // Init
            Initialize();
        }


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize()
        {
            // Init
            _text = GetComponent<TMP_Text>();
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        private void GetSpecialQuest()
        {
            // [시작가능] 상태의 스페셜 퀘스트 리스트 가져옴
            List<Quest> questList = Unit.GetCanStartSpeicalQuestForList();

            // 랜덤 스페셜 퀘스트 할당
            int randomIndex = Random.Range(0, questList.Count);
            _quest =  questList[randomIndex];


        }
    }
}
