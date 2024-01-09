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
        public List<SpecialQuest> SpecialQuestList => _specialQuestList;


        /*************************************************
         *                Private Fields
         *************************************************/
        private List<SpecialQuest> _specialQuestList;
        private int _questCreateCount = 3;              // 생성될 스페셜 퀘스트 갯수
        private Vector3[] _questCreatePositions =
        {
            new Vector3(7.01f, 21.641f, -2.436997f),
            new Vector3(7.01f, 21.641f, -6.120997f),
            new Vector3(7.01f, 21.641f, -4.299998f),
        };


        /*************************************************
         *                 Unity Events
         *************************************************/
        void Start()
        {
            // Init
            //Initialize();
        }


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize()
        {
            // Init
            for (int i = 0; i < _questCreateCount; i++)
            {
                CreateSpecialQuestStone();
            }
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        // 스페셜 퀘스트 비석을 생성
        private void CreateSpecialQuestStone()
        {
            // 퀘스트 생성
            GameObject prefab = Resources.Load<GameObject>("Special Quest");
            GameObject questStone = Instantiate(prefab, transform);
            SpecialQuest specialQuest = questStone.AddComponent<SpecialQuest>();
            specialQuest.Initialize();
            _specialQuestList.Add(specialQuest);
        }


        // 모든 자식을 순회해서 SpecialQuestText 컴포넌트를 추가
        private List<SpecialQuest> AddSpecialQuestTextComponentForChild()
        {
            List<SpecialQuest> specialQuestTextList = 
                new List<SpecialQuest>();
            foreach (Transform child in transform)
            {
                if (child.name.Equals("Special Quest"))
                {
                    // 컴포넌트 추가 & 반복 중단
                    SpecialQuest specialQuestText = child.Find("Text (TMP)")
                        .gameObject.AddComponent<SpecialQuest>();
                    specialQuestTextList.Add(specialQuestText);
                }
            }

            return specialQuestTextList;
        }
    }
}
