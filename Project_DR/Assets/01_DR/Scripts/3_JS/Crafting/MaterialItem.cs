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
        public Anvil Anvil => CraftingManager.Instance.Anvil;               // 모루
        public int ItemID => _itemID;                                       // 아이템 ID
        public int NeedAmount => _needAmount;                               // 필요한 갯수
        public int CurrentHammeringCount => Anvil.CurrentHammeringCount;    // 현재 망치질 횟수
        public int NeedHammeringCount => _needHammeringCount;               // 필요한 망치질 횟수
        public string ItemName => _itemName;                                // 아이템 이름
        

        /*************************************************
         *                 Private Fields
         *************************************************/
        private int _itemID;
        private int _needAmount;
        private int _needHammeringCount;
        private string _itemName;


        /*************************************************
         *                Public Methods
         *************************************************/
        public MaterialItem(int id, int amount, int count)
        {
            // Init
            _itemID = id;
            _needAmount = amount;
            _needHammeringCount = count;
            _itemName = Data.GetString(id, "Name");
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        public bool CheckCraft()
        {
            // itemID가 0으로 비어있을 경우 예외 처리
            if (_itemID.Equals(0)) { return true; }

            // 제작 조건을 충족하지 못할 경우 예외 처리
            if (! Anvil.CheckCanCraft(_itemID, _needAmount, _needHammeringCount)) { return false; }

            // 제작이 가능한 경우
            return true;
        }

        public void Craft()
        {
            // 모루 저장소에서 아이템을 삭제
            Anvil.RemoveItemFromStorage(_itemID, NeedAmount);
        }
    }
}
