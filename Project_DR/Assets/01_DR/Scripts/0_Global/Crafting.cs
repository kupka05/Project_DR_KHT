using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public static class Crafting
    {
        /*************************************************
         *                 Public Methods
         *************************************************/
        // 크래프팅 아이템을 가져온다(조합식 + 결과 포함)
        public static CraftingItem GetItem(int id)
        {
            return CraftingManager.Instance.GetCraftingCondition(id) as CraftingItem;
        }
    }
}
