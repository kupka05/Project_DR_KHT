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
            string[] questNames = SubStringFromEnd(_currentQuest.QuestData.Desc);
            int rewardID = _currentQuest.QuestData.ClearReward.QuestRewardData.ID;
            string reward = Data.GetString(rewardID, "Name");
            string text = "<size=0.8>" + questNames[0] + " " + questNames[1] + "\n\n" + questNames[2] +"</size>\n\n\n" + reward + "\n\n\n파괴 시\n\n퀘스트 수령";
            _text.text = text;
        }

        // 공백을 기준으로 문자열을 잘라서 배열로 반환
        private string[] SubStringFromEnd(string input, char symbol = ' ')
        {
            string[] texts = input.Split(symbol);
            return texts;
        }
    }
}
