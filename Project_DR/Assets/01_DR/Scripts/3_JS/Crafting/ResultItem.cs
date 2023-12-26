using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public class ResultItem : ICraftingComponent
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public int ItemID => _itemID;                   // 아이템 ID
        public int GiveAmount => _giveAmount;           // 지급 아이템 갯수
        public string ItemName => _itemName;            // 아이템 이름


        /*************************************************
         *                 Private Fields
         *************************************************/
        private int _itemID;
        private int _giveAmount;
        private string _itemName;


        /*************************************************
         *                Public Methods
         *************************************************/
        public ResultItem(int id, int amount = 1)
        {
            // Init
            _itemID = id;
            _giveAmount = amount;
            _itemName = Data.GetString(id, "Name");
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        public bool Craft()
        {
            // TODO: 아이템 지급


            // 실패시
            return true;
        }
    }
}
