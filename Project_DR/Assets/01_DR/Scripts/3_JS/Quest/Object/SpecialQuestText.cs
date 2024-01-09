using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Js.Quest
{
    public class SpecialQuest : MonoBehaviour
    {
        /*************************************************
         *                Private Fields
         *************************************************/
        public QuestData QuestData => _currentQuest.QuestData;


        /*************************************************
         *                Private Fields
         *************************************************/
        [SerializeField] private Quest _currentQuest;
        [SerializeField] private TMP_Text _text;


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize()
        {
            // Init
            _text = GetTMPText();
        }

        // 현재 퀘스트 변경
        public void SetCurrentQuest(Quest quest)
        {
            _currentQuest = quest;

            // 퀘스트가 변경 될 때 텍스트도 변경
            SetTextForQuest();
        }

        public void isDead()
        {

        }
        

        /*************************************************
         *                Private Methods
         *************************************************/
        // 자식을 순회해서 TMP_Text를 찾아서 반환
        private TMP_Text GetTMPText()
        {
            TMP_Text tmpText = default;
            foreach (Transform child in transform)
            {
                if (child.name.Equals("Text (TMP)"))
                {
                    tmpText = child.GetComponent<TMP_Text>();
                    break;
                }
            }

            return tmpText;
        }

        // 현재 텍스트를 퀘스트에 맞게 변경
        private void SetTextForQuest()
        {
            string text = "<size=0.8>포션_(소) 3회\n\n사용하기</size>\n\n\n30 골드\n\n\n파괴 시\n\n퀘스트 수령";
            string questName = _currentQuest.QuestData.Desc;

        }
    }
}
