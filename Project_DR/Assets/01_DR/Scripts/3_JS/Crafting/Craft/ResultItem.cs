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
        public Vector3 SpawnPos => _spawnPos;           // 아이템이 생성될 위치


        /*************************************************
         *                 Private Fields
         *************************************************/
        private int _itemID;
        private int _giveAmount;
        private string _itemName;
        private Vector3 _spawnPos;


        /*************************************************
         *                Public Methods
         *************************************************/
        public ResultItem(int id, Vector3 pos, int amount = 1)
        {
            // Init
            _itemID = id;
            _spawnPos = pos;
            _giveAmount = amount;
            _itemName = Data.GetString(id, "Name");
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        public bool CheckCraft()
        {
            // 조건이 없으므로 true 반환
            return true;
        }

        public void Craft()
        {
            // 아이템 스폰
            Unit.AddFieldItem(_spawnPos, _giveAmount);

            // 디버그
            GFunc.Log($"[{_itemID}]아이템 [{_itemName}]이 [{_giveAmount}] 갯수만큼 제작되었습니다.");
        }
    }
}
