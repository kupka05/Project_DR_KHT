using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    public class QuestReward : IReward
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

        // 퀘스트 보상(1 ~ 4)을 획득한다
        public void GetReward()
        {
            // 보상(1 ~ 4) 획득
            for (int i = 0; i < _questRewardData.RewardKeyIDs.Length; i++)
            {
                int keyID = _questRewardData.RewardKeyIDs[i];
                int amount = _questRewardData.RewardAmounts[i];
                int probability = _questRewardData.RewardProbabilitys[i];
                // 퀘스트 보상 획득
                GetReward(keyID, probability, amount);
            }
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        // 퀘스트 보상 획득
        private void GetReward(int keyID, int probability, int amount = 1)
        {
            GFunc.Log($"KeyID: {keyID}, probability: {probability}, amount: {amount}");
            // 키 ID가 0이 아닐 경우 && 지정 확률 성공시
            if (! keyID.Equals(0) && GetRandomProbability(probability))
            {
                // 퀘스트 보상 타입
                switch(_questRewardData.Type)
                {
                    // "아이템" 일 경우
                    case QuestRewardData.TypeReward.ITEM:
                        // 인벤토리에 아이템 지급
                        Unit.AddInventoryItem(keyID, amount);
                        break;
                }
            }
        }

        // 랜덤 확률 돌리기
        // [true = 성공] / [false = 실패]
        private bool GetRandomProbability(int probability)
        {
            int randomprobability = Random.Range(0, 101);
            if (randomprobability <= probability)
            {
                // 성공할 경우
                return true;
            }

            // 실패할 경우
            return false;
        }
    }
}
