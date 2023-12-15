
namespace Js.Quest
{
    public class QuestData
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public int ID => _id;                   // 퀘스트 ID
        public int Type => _type;               // 퀘스트 분류
        public string Desc => _desc;            // 퀘스트 설명


        /*************************************************
         *                 Private Fields
         *************************************************/
        private int _id;
        private int _type;
        private string _desc;


        /*************************************************
         *                 Public Methods
         *************************************************/
        public QuestData(int id)
        {
            // Init
            _id = Data.GetInt(id, "ID");
            _type = Data.GetInt(id, "Type");
            _desc = Data.GetString(id, "Desc");
        }
    }
}
