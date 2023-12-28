using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public class CompositeItem : ICraftingComponent
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public List<ICraftingComponent> Components => _components;      // 크래프팅 아이템 컴포넌트


        /*************************************************
         *                 Private Fields
         *************************************************/
        private List<ICraftingComponent> _components = new List<ICraftingComponent>();


        /*************************************************
         *                 Public Methods
         *************************************************/
        public CompositeItem(params ICraftingComponent[] components)
        {
            // Init
            for (int i = 0; i < components.Length; i++)
            {
                _components.Add(components[i]);
            }
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        public bool CheckCraft()
        {
            // 크래프팅 아이템 컴포넌트 순회
            // 제작에 필요한 아이템 보유량 체크
            foreach (var item in _components)
            {
                // 제작에 실패 했을 경우
                // 사유: 재료 부족
                if (item.CheckCraft().Equals(false))
                {
                    GFunc.Log($"CompositeItem.Craft(): 재료가 부족해 아이템 제작에 실패했습니다.");
                    return false;
                }
            }

            // 크래프트에 성공
            return true;
        }

        public void Craft() { }
    }
}
