using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Js.Quest
{
    public class Quest
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public QuestData QuestData => _questData;       // 퀘스트 데이터


        /*************************************************
         *                 Private Fields
         *************************************************/
        private QuestData _questData;


        /*************************************************
         *                 Public Methods
         *************************************************/
        public Quest(int id)
        {
            // Init
            _questData = new QuestData(id);
        }
    }
}
