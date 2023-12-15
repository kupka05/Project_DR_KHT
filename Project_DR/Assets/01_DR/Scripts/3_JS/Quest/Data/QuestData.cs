
namespace Js.Quest
{
    public class QuestData
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
        public int ID => _id;                               // 퀘스트 ID
        public int RequiredQuestID => _requiredQuestID;     // 선행 퀘스트 ID
        public int Condition => _condition;                 // 퀘스트 달성 조건
        public string Desc => _desc;                        // 퀘스트 설명
        public QuestType Type => _type;                     // 퀘스트 분류


        /*************************************************
         *                 Private Fields
         *************************************************/
        private int _id;
        private int _requiredQuestID;
        private int _condition;
        private string _desc;
        private QuestType _type;
     /////TODO: Reward 클래스 구현 및 추가


        /*************************************************
         *                 Public Methods
         *************************************************/
        public QuestData(int id)
        {
            // Init
            _id = Data.GetInt(id, "ID");
            _requiredQuestID = Data.GetInt(id, "RequiredQuestID");
            _condition = Data.GetInt(id, "Condition");
            _desc = Data.GetString(id, "Desc");
            _type = (QuestType)Data.GetInt(id, "Type");
        }
    }
}
