using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public class MaterialItem : ICraftingComponent
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public int ItemID => _itemID;                   // 아이템 ID
        public int NeedAmount => _needAmount;           // 필요한 갯수
        public string ItemName => _itemName;            // 아이템 이름


        /*************************************************
         *                 Private Fields
         *************************************************/
        private int _itemID;
        private int _needAmount;
        private string _itemName;


        /*************************************************
         *                Public Methods
         *************************************************/
        public MaterialItem(int id, int amount)
        {
            // Init
            _itemID = id;
            _needAmount = amount;
            _itemName = Data.GetString(id, "Name");
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        public bool Craft()
        {
            // itemID가 0으로 비어있을 경우 예외 처리
            if (_itemID.Equals(0)) { return true; }

            // TODO: 재료 체크하기
            // 맞을 경우 true

            // 재료가 없을 경우
            return false;
        }
    }
}
