
using UnityEngine;

namespace Js.Quest
{
    public class QuestData : ScriptableObject
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public enum QuestType
        {
            ERROR = 0,      // 0일 경우 예외 처리 -> 퀘스트 클리어 및 저장 불가
            MAIN = 1,       // 메인 퀘스트
            SUB = 2,        // 서브 퀘스트
            SPECIAL = 3     // 특수 퀘스트
        }
        public enum ConditionType
        {
            ERROR = 0,
            BOSS_MEET = 1,
            BOSS_KILL = 2,
            USE_ITEM = 3,
            MONSTER_KILL = 4,
            CRAFTING = 5,
            OBJECT = 6,
            GIVE_ITEM = 7,
            NPC_TALK = 8
        }
        public int ID => _id;                               // 퀘스트 ID
        public int RequiredQuestID => _requiredQuestID;     // 선행 퀘스트 ID
        public ConditionType Condition => _condition;       // 퀘스트 달성 조건(1. 보스 조우, 2. 보스 처치, 3. 소비, 4. 처치, 5. 크래프팅, 6. 오브젝트, 7. 증정, 8. 대화)
        public int KeyID => _keyID;                         // 퀘스트 달성 조건에 해당하는 키 ID
        public int ClearValue => _clearValue;               // 퀘스트 목표 값(완료에 해당하는 값)
        public int CurrentValue => _currentValue;           // 현재 퀘스트 달성 값(초기: 0)
        public string Desc => _desc;                        // 퀘스트 설명
        public QuestType Type => _type;                     // 퀘스트 분류
        public int[] ClearEventIDs => _clearEventIDs;       // 클리어 이벤트 ID
        public int[] FailEventIDs => _failEventIDs;         // 실패 이벤트 ID
        public QuestReward ClearReward => _clearReward;     // 클리어 보상
        public QuestReward FailReward => _failReward;       // 실패 보상


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private Quest _quest;
        [SerializeField] private int _id;
        [SerializeField] private int _requiredQuestID;
        [SerializeField] private ConditionType _condition;
        [SerializeField] private int _keyID;
        [SerializeField] private int _clearValue;
        [SerializeField] private int _currentValue;
        [SerializeField] private string _desc;
        [SerializeField] private QuestType _type;
        [SerializeField] private int[] _clearEventIDs;
        [SerializeField] private int[] _failEventIDs;
        [SerializeField] private QuestReward _failReward;
        [SerializeField] private QuestReward _clearReward;
        // 디버그
        [SerializeField] private QuestRewardData _clearRewardData;
        [SerializeField] private QuestRewardData _failRewardData;
        // 디버그


        /*************************************************
         *                 Public Methods
         *************************************************/
        public QuestData(Quest quest, int id)
        {
            // Init
            _quest = quest;
            _id = id;
            _requiredQuestID = Data.GetInt(id, "RequiredQuestID");
            _condition = (ConditionType)Data.GetInt(id, "Condition");
            _keyID = Data.GetInt(id, "KeyID");
            _clearValue = Data.GetInt(id, "ClearValue");
            _desc = Data.GetString(id, "Desc");
            _type = (QuestType)Data.GetInt(id, "Type");
            _clearEventIDs = GFunc.SplitIds(Data.GetString(id, "ClearEventID"));
            _failEventIDs = GFunc.SplitIds(Data.GetString(id, "FailEventID"));
            _clearReward = new QuestReward(quest, Data.GetInt(id, "ClearRewardKeyID"));
            _failReward = new QuestReward(quest, Data.GetInt(id, "FailRewardKeyID"));

            // 디버그
            _clearRewardData = ClearReward.QuestRewardData;
            _failRewardData = _failReward.QuestRewardData;
            // 디버그
        }

        // 현재 퀘스트 진행 값 추가
        public void AddCurrentValue(int value = 1)
        {
            _currentValue += value;

            // 현재 퀘스트가 서브, 특수 퀘스트일 경우 콜백 호출
            if (Type.Equals(QuestType.SUB) || Type.Equals(QuestType.SPECIAL))
            {
                GFunc.Log("AddCurrentValue");
                QuestCallback.OnSubspecialQuestValueChangedCallback(_quest);
            }
        }

        // 현재 퀘스트 진행 값 변경
        public void ChangeCurrentValue(int value)
        {
            _currentValue = value;

            // 현재 퀘스트가 서브, 특수 퀘스트일 경우 콜백 호출
            if (Type.Equals(QuestType.SUB) || Type.Equals(QuestType.SPECIAL))
            {
                GFunc.Log("ChangeCurrentValue");
                QuestCallback.OnSubspecialQuestValueChangedCallback(_quest);
            }
        }
    }
}
