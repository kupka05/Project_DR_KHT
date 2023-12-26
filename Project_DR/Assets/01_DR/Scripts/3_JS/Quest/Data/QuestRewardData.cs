using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    public class QuestRewardData : ScriptableObject
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public enum TypeReward
        {
            NONE = 0,       // 비어있음
            ITEM = 1,       // 아이템
            STATE = 2,      // 상태
            MBTI = 3        // MBTI
        }
        public int ID => _id;                                       // 퀘스트 보상 ID
        public TypeReward Type => _type;
        public string Name => _name;                                // 퀘스트 보상 이름
        public int GiveGold => _giveGold;                           // 퀘스트 지급 골드
        public int GiveEXP => _giveEXP;                             // 퀘스트 지급 경험치
        public float GiveHealth => _giveHealth;                     // 퀘스트 지급 체력
        public int StateKeyID => _stateKeyID;                       // 상태 보상 키 ID
        public int StateProbability => _stateProbability;           // 상태 보상 확률
        public int[] RewardKeyIDs => _rewardKeyIDs;                 // 퀘스트 보상 (1 ~ 4) 키 ID
        public int[] RewardAmounts => _rewardAmounts;               // 퀘스트 보상 (1 ~ 4) 지급 갯수
        public int[] RewardProbabilitys => _rewardProbabilitys;     // 퀘스트 보상 (1 ~ 4) 확률
        public float[] MBTIValues => _mbtiValues;                   // MBTI(I, N, F, P) 보상


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private int _id;
        [SerializeField] private TypeReward _type;
        [SerializeField] private string _name;
        [SerializeField] private int _giveGold;
        [SerializeField] private int _giveEXP;
        [SerializeField] private float _giveHealth;
        [SerializeField] private int _stateKeyID;
        [SerializeField] private int _stateProbability;
        [SerializeField] private int[] _rewardKeyIDs = new int[4];
        [SerializeField] private int[] _rewardAmounts = new int[4];
        [SerializeField] private int[] _rewardProbabilitys = new int[4];
        [SerializeField] private float[] _mbtiValues = new float[4];


        /*************************************************
         *                 Public Methods
         *************************************************/
        public QuestRewardData(int id)
        {
            // Init
            _id = id;
            // id가 0일 경우 예외 처리
            if (id.Equals(0)) { return; }
            _type = (TypeReward)Data.GetInt(id, "Type");
            _name = Data.GetString(id, "Name");
            _giveGold = Data.GetInt(id, "GiveGold");
            _giveEXP = Data.GetInt(id, "GiveEXP");
            _giveHealth = Data.GetFloat(id, "GiveHealth");
            _stateKeyID = Data.GetInt(id, "StateKeyID");
            _stateProbability = Data.GetInt(id, "StateProbability");
            string[] mbtiNames = { "I", "N", "F", "P" };
            for (int i = 0; i < _rewardKeyIDs.Length; i ++)
            {
                // 보상 데이터 Init
                int index = i + 1;
                string rewardKeyID = GFunc.SumString("Reward_", index.ToString(), "_KeyID");
                string rewardAmount = GFunc.SumString("Reward_", index.ToString(), "_Amount");
                string rewardProbability = GFunc.SumString("Reward_", index.ToString(), "_Probability");
                _rewardKeyIDs[i] = Data.GetInt(id, rewardKeyID);
                _rewardAmounts[i] = Data.GetInt(id, rewardAmount);
                _rewardProbabilitys[i] = Data.GetInt(id, rewardProbability);
                _mbtiValues[i] = Data.GetFloat(id, GFunc.SumString("MBTI_VALUE_", mbtiNames[i]));
            }
        }
    }
}
