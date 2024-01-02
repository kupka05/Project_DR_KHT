using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public class CompositeItem : ICraftingComponent
    {
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
                    return false;
                }
            }

            // 크래프팅 재료 보유시
            return true;
        }

        public bool CheckEnhance()
        {
            // 크래프팅 아이템 컴포넌트 순회
            // 강화에 필요한 아이템 보유량 체크
            foreach (var item in _components)
            {
                // 제작에 실패 했을 경우
                // 사유: 재료 부족
                if (item.CheckEnhance().Equals(false))
                {
                    return false;
                }
            }

            // 강화 재료 보유시
            return true;
        }

        public void Craft() 
        {
            // 모든 컴포넌트에게 Craft() 명령
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Craft();
            }
        }

        public void Enhance(int type)
        {
            // 모든 컴포넌트에게 Enhance() 명령
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Enhance(type);
            }
        }
    }
}
