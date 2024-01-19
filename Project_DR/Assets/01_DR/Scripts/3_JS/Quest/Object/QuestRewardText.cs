using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Js.Quest
{
    public class QuestRewardText : MonoBehaviour
    {
        /*************************************************
         *                Private Fields
         *************************************************/
        private TMP_Text _text;
        private float _delay = 3.0f;
        private bool _isPrint = false;
        private Queue<int> _rewardQueue;
        private WaitForSeconds _waitForSeconds;


        /*************************************************
         *                 Unity Events
         *************************************************/
        private void Start()
        {
            // Init
            Initialize();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.U)) 
            {
                PrintText(32_3_024);

                // 큐에 있는 ID를 모두 출력
            }
        }

        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize()
        {
            // Init
            _text = GetComponent<TMP_Text>();
            _rewardQueue = new Queue<int>();
            _waitForSeconds = new WaitForSeconds(_delay);
        }

        // 텍스트를 출력한다.
        // n초 후에 숨김
        public void PrintText(params int[] ids)
        {
            // 출력 중일 경우 예외 처리
            if (_isPrint) { return; }

            // 출력 중
            _isPrint = true;

            // 리워드 ID[]를 큐에 넣기
            EnqueueReward(ids);

            // 큐에 있는 ID를 모두 출력
            DequeueReward();
        }


        /*************************************************
         *               Private Methods
         *************************************************/
        // 텍스트를 변경
        private void SetText(string text, int fontSize = 64)
        {
            _text.text = text;
            _text.fontSize = fontSize;
        }

        // 리워드 ID[]를 큐에 넣기
        private void EnqueueReward(int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                // ID가 0일 경우 예외 처리
                if (ids[i].Equals(0)) { continue; }

                // 큐에 요소 추가
                _rewardQueue.Enqueue(ids[i]);
            }
        }

        // 리워드 ID[]를 큐에서 빼기
        // 빼면서 텍스트를 출력한다.
        private void DequeueReward()
        {
            int[] ids = new int[_rewardQueue.Count];
            int index = default;
            int tempID = default;
            while (_rewardQueue.TryDequeue(out tempID))
            {
                ids[index] = tempID;
                index++;
            }

            // 텍스트 출력 & n초 후에 사라짐
            StartCoroutine(PrintTextCoroutine(ids));
        }

        // ID에 해당하는 보상 텍스트를 불러와서 변환 후 반환
        private string GetRewardText(int id, string category = "Name")
        {
            string rewardName = Data.GetString(id, category);
            rewardName = GFunc.CSVConversation(rewardName);

            // 리워드 이름이 공백일 경우 예외 처리
            if (rewardName.Equals("")) { return ""; }
            string text = GFunc.SumString("<", rewardName, ">");

            return text;
        }

        // 텍스트를 업데이트
        private void UpdateText(int id)
        {
            GFunc.Log(id);
            SetText(GetRewardText(id));
            EnableText();
            DisableText();
        }

        // 텍스트를 표시
        private void EnableText()
        {
            _text.DOFade(_delay, 0f);
        }

        // 텍스트를 숨김(사라지는 효과 출력)
        private void DisableText()
        {
            _text.DOFade(0f, 1f) // 텍스트 페이드 아웃
                .SetEase(Ease.Linear); // 애니메이션 자연스럽게 Easing
        }


        /*************************************************
         *               Private Methods
         *************************************************/
        // n초 후에 텍스트를 출력하는 코루틴
        // ids[] 갯수만큼 대기
        private IEnumerator PrintTextCoroutine(int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                if (i >= 1)
                {
                    // 대기
                    yield return _waitForSeconds;
                }

                // 텍스트 업데이트
                UpdateText(ids[i]);
            }

            // 출력 끝
            _isPrint = false;
        }
    }
}
