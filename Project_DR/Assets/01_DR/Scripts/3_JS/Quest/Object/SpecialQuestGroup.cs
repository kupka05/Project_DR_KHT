using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private List<Quest> _questList;    // 퀘스트 리스트
        private List<SpecialQuest> _specialQuestList;       // 스페셜 퀘스트 리스트
        private string _prefabName = "Special Quest";       // 프리팹 이름
        private float _initializeDelay = 0.1f;              // Init 딜레이
        private int _questCreateCount = 3;                  // 생성될 스페셜 퀘스트 갯수
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
            // 0.1초 뒤 Init
            Invoke("Initialize", _initializeDelay);
        }


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize()
        {
            // Init
            _questList = new List<Quest>();
            _specialQuestList = new List<SpecialQuest>();
            for (int i = 0; i < _questCreateCount; i++)
            {
                _specialQuestList.Add(CreateSpecialQuestStone(_questCreatePositions[i]));
            }
            SetSpecialQuestList();
            foreach(var item in _questList)
            {
                GFunc.Log(item.QuestData.ID);
            }
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        // 스페셜 퀘스트 비석을 생성 후 컴포넌트 반환
        private SpecialQuest CreateSpecialQuestStone(Vector3 pos)
        {
            // 퀘스트 생성
            GameObject prefab = Resources.Load<GameObject>(_prefabName);
            if (prefab == null) { return default; }

            GameObject questStone = Instantiate(prefab, transform);
            questStone.transform.localPosition = pos;
            questStone.transform.name = _prefabName;
            SpecialQuest specialQuest = questStone.transform
                .GetChild(0).gameObject.AddComponent<SpecialQuest>();
            specialQuest.Initialize();

            return specialQuest;
        }

        // 랜덤한 스페셜 퀘스트를 가져와서 리스트에 할당
        private void SetSpecialQuestList()
        {
            // [시작가능] 상태의 스페셜 퀘스트 리스트 가져옴
            List<Quest> questList = Unit.GetCanStartSpeicalQuestForList();
            int index = default;
            while (_questList.Count < _questCreateCount)
            {
                // 퀘스트 리스트가 비어있을 경우 예외 처리
                if (questList.Count.Equals(0)) { break; }

                // 랜덤 스페셜 퀘스트 할당 & 예외 처리
                int randomIndex = UnityEngine.Random.Range(0, questList.Count);
                Quest randomQuest = questList[randomIndex];
                if (randomQuest != null)
                {
                    // 다른 비석과 중복됐을 경우 재할당
                    if (CheckSameQuest(randomQuest)) { continue; }

                    // 아닐 경우 리스트에 추가 & 스페셜 퀘스트 할당
                    _questList.Add(randomQuest);
                    _specialQuestList[index].SetCurrentQuest(randomQuest);

                    index++;
                }
            }
        }

        // 다른 스페셜 퀘스트 비석과 같은 퀘스트인지 확인
        private bool CheckSameQuest(Quest quest)
        {
            for (int i = 0; i < SpecialQuestList.Count; i++)
            {
                for (int j = 0; j < _questList.Count; j++)
                {
                    // 기존 퀘스트 와 같은 퀘스트일 경우
                    if (quest.Equals(_questList[j]))
                    {
                        return true;
                    }
                }
            }

            // 아닐 경우
            return false;
        }
    }
}
