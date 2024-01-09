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
        public QuestData QuestData => _currentQuest.QuestData;


        /*************************************************
         *                Private Fields
         *************************************************/
        private Quest _currentQuest;
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
            while (GetSpecialQuest()) { }
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        // 랜덤한 스페셜 퀘스트를 가져옴
        private bool GetSpecialQuest()
        {
            // [시작가능] 상태의 스페셜 퀘스트 리스트 가져옴
            List<Quest> questList = Unit.GetCanStartSpeicalQuestForList();

            // 퀘스트 리스트가 비어있을 경우 예외 처리
            if (questList.Count.Equals(0)) { return false; }

            // 랜덤 스페셜 퀘스트 할당 & 예외 처리
            int randomIndex = Random.Range(0, questList.Count);
            Quest randomQuest = questList[randomIndex];
            if (randomQuest != null)
            {
                // 다른 비석과 중복됐을 경우 재할당
                if (CheckSameQuest(randomQuest)) { return true; }

                // 아닐 경우 할당
                _currentQuest = randomQuest;
                
                // 반복 중단
                return false;
            }

            // 퀘스트를 할당하지 못했을 경우
            // 재할당을 위해 true 반환
            return true;
        }

        // 다른 스페셜 퀘스트 비석과 같은 퀘스트인지 확인
        private bool CheckSameQuest(Quest quest)
        {

            // 아닐 경우
            return false;
        }
    }
}
