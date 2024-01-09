using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{

    public class SpecialQuestGroup : MonoBehaviour
    {
        /*************************************************
         *                Private Fields
         *************************************************/
        public List<Quest> QuestList => _questList;


        /*************************************************
         *                Private Fields
         *************************************************/
        private List<Quest> _questList;


        /*************************************************
         *                 Unity Events
         *************************************************/
        void Start()
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

        }


        /*************************************************
         *                Private Methods
         *************************************************/
        // 모든 자식을 순회해서 SpecialQuestText 컴포넌트를 추가
        private List<SpecialQuestText> AddSpecialQuestTextComponentForChild()
        {
            List<SpecialQuestText> specialQuestTextList = 
                new List<SpecialQuestText>();
            foreach (Transform child in transform)
            {
                if (child.name.Equals("Special Quest"))
                {
                    // 컴포넌트 추가 & 반복 중단
                    SpecialQuestText specialQuestText = child.Find("Text (TMP)")
                        .gameObject.AddComponent<SpecialQuestText>();
                    specialQuestTextList.Add(specialQuestText);
                }
            }

            return specialQuestTextList;
        }
    }
}
