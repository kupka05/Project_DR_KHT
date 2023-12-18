using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    public class QuestRewardData
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public int ID => _id;                                       // 퀘스트 보상 ID
        public int Type => _type;                                   // 퀘스트 보상 타입
        public string Name => _name;                                // 퀘스트 보상 이름
        public int Reward1KeyID => _reward1KeyID;                   // 퀘스트 보상 (1) 키 ID
        public int Reward1Probability => _reward1Probability;       // 퀘스트 보상 (1) 확률
        public int Reward2KeyID => _reward2KeyID;                   // 퀘스트 보상 (2) 키 ID
        public int Reward2Probability => _reward2Probability;       // 퀘스트 보상 (2) 확률
        public int Reward3KeyID => _reward3KeyID;                   // 퀘스트 보상 (3) 키 ID
        public int Reward3Probability => _reward3Probability;       // 퀘스트 보상 (3) 확률
        public int Reward4KeyID => _reward4KeyID;                   // 퀘스트 보상 (4) 키 ID
        public int Reward4Probability => _reward4Probability;       // 퀘스트 보상 (4) 확률


        /*************************************************
         *                 Private Fields
         *************************************************/
        private int _id;
        private int _type;
        private string _name;
        private int _reward1KeyID;
        private int _reward1Probability;
        private int _reward2KeyID;
        private int _reward2Probability;
        private int _reward3KeyID;
        private int _reward3Probability;
        private int _reward4KeyID;
        private int _reward4Probability;


        /*************************************************
         *                 Public Methods
         *************************************************/
        public QuestRewardData(int id)
        {
            // Init
            _id = id;
            _type = Data.GetInt(id, "Type");
            _name = Data.GetString(id, "Name");
            _reward1KeyID = Data.GetInt(id, "Reward_1_KeyID");
            _reward1Probability = Data.GetInt(id, "Reward_1_Probability");
            _reward2KeyID = Data.GetInt(id, "Reward_2_KeyID");
            _reward2Probability = Data.GetInt(id, "Reward_2_Probability");
            _reward3KeyID = Data.GetInt(id, "Reward_3_KeyID");
            _reward3Probability = Data.GetInt(id, "Reward_3_Probability");
            _reward4KeyID = Data.GetInt(id, "Reward_4_KeyID");
            _reward4Probability = Data.GetInt(id, "Reward_4_Probability");
        }
    }
}
