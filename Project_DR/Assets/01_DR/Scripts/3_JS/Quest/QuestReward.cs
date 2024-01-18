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
        private Quest _quest;


        /*************************************************
         *                 Public Methods
         *************************************************/
        public QuestReward(Quest quest, int id)
        {
            // Init
            _quest = quest;
            _questRewardData = new QuestRewardData(id);
        }

        // 퀘스트 보상을 획득한다
        public void GetReward()
        {
            // 퀘스트 보상 ID가 0일 경우 예외처리
            if (QuestRewardData.ID.Equals(0)) { return; }

            // 기본 보상(골드, EXP, 플레이어 체력) 지급
            UserData.AddQuestScore(_quest);
            UserData.SetCurrentHealth(_questRewardData.GiveHealth);

            // 퀘스트 타입에 따른 보상 획득
            GetQuestRewardByType();

            // 효과음 재생
            AudioManager.Instance.AddSFX("SFX_Quest_UI_Reward_01");
            AudioManager.Instance.PlaySFX("SFX_Quest_UI_Reward_01");
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        // 퀘스트 타입에 따른 보상 획득
        private void GetQuestRewardByType()
        {
            // 퀘스트 보상 타입
            switch (_questRewardData.Type)
            {
                // "아이템" 일 경우
                case QuestRewardData.TypeReward.ITEM:
                    // 퀘스트 보상 아이템(1 ~ 4) 획득
                    GetRewardItem();
                    break;

                // "MBTI" 일 경우
                case QuestRewardData.TypeReward.MBTI:
                    // 퀘스트 보상 MBTI 획득
                    GetRewardMBTI();
                    break;

                // "상태" 일 경우
                case QuestRewardData.TypeReward.STATE:
                    // 퀘스트 보상 스텟획득
                    GetRewardState();
                    break;
            }
        }

        // 퀘스트 보상 아이템(1 ~ 4) 획득
        private void GetRewardItem()
        {
            // 보상 아이템(1 ~ 4) 지급
            for (int i = 0; i < _questRewardData.RewardKeyIDs.Length; i++)
            {
                int keyID = _questRewardData.RewardKeyIDs[i];
                int amount = _questRewardData.RewardAmounts[i];
                int probability = _questRewardData.RewardProbabilitys[i];
                // 보상 아이템(1 ~ 4) 획득
                GetRewardItem(keyID, probability, amount);
            }
        }

        // 퀘스트 보상 아이템 획득
        private void GetRewardItem(int keyID, int probability, int amount = 1)
        {
            GFunc.Log($"KeyID: {keyID}, probability: {probability}, amount: {amount}");
            // 키 ID가 0이 아닐 경우 && 지정 확률 성공시
            if (! keyID.Equals(0) && GetRandomProbability(probability))
            {
                Unit.AddInventoryItem(keyID, amount);
            }
        }

        // 퀘스트 보상 MBTI 획득
        private void GetRewardMBTI()
        {
            // MBTI 보상 지급
            MBTI mbti = new MBTI();
            float[] values = _questRewardData.MBTIValues;
            mbti.SetMBTI(values[0], values[1], values[2], values[3]);
            MBTIManager.Instance?.ResultMBTI(mbti);
        }

        // 퀘스트 보상 상태 획득
        private void GetRewardState()
        {
            // TODO: 퀘스트 상태 보상 구현하기
            // 보상 아이템(1 ~ 4) 지급
            for (int i = 0; i < _questRewardData.RewardKeyIDs.Length; i++)
            {
                int keyID = _questRewardData.RewardKeyIDs[i];
                int probability = _questRewardData.RewardProbabilitys[i];
                // 보상 상태(1 ~ 4) 획득
                GetRewardState(keyID, probability);
            }
        }

        // 퀘스트 보상 상태 획득
        private void GetRewardState(int keyID, int probability)
        {
            // 키 ID가 0이 아닐 경우 && 지정 확률 성공시
            if (!keyID.Equals(0) && GetRandomProbability(probability))
            {
                UserData.ActiveSkill(keyID);
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
