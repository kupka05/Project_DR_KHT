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
        public Anvil Anvil => CraftingManager.Instance.Anvil;                   // 모루
        public int NeedAmount => _craftingItem.MaterialDictionary[_itemID];     // 필요한 갯수


        /*************************************************
         *                 Private Fields
         *************************************************/
        private CraftingItem _craftingItem;                                     // 최상위 오브젝트
        private int _itemID;                                                    // 아이템 아이디


        /*************************************************
         *                Public Methods
         *************************************************/
        public MaterialItem(CraftingItem item, int id)
        {
            // Init
            _craftingItem = item;
            _itemID = id;
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        public bool CheckCraft()
        {
            // itemID가 0으로 비어있을 경우 예외 처리
            if (_itemID.Equals(0)) { return true; }

            // 제작 조건을 충족하지 못할 경우 예외 처리
            if (! Anvil.CheckCanCraft(_itemID, NeedAmount)) { return false; }

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
