using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    /// <summary>
    /// [구조]: MaterialItem -> CompositeItem + ResultItem -> CraftingItem <br></br>
    /// ResultItem은 리스트의 마지막에 추가되어야 한다.
    /// </summary>
    public class CraftingItem : ICraftingComponent
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
         *               Initialize Methods
         *************************************************/
        // 추후 할당용 생성자
        public CraftingItem() {}

        // 초기 할당용 생성자
        public CraftingItem(params ICraftingComponent[] components)
        {
            // Init
            for (int i = 0; i < components.Length; i++)
            {
                _components.Add(components[i]);
            }
        }


        /*************************************************
         *                 Public Methods
         *************************************************/
        // 컴포넌트 추가
        public void AddComponent(ICraftingComponent component)
        {
            _components.Add(component);
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        public bool Craft()
        {
            // 크래프팅 아이템 컴포넌트 순회
            // 제작에 필요한 아이템 보유량 체크
            foreach (var item in _components)
            {
                // 제작에 실패 했을 경우
                // 사유: 재료 부족
                if (item.Craft().Equals(false))
                {
                    GFunc.Log($"CraftingItem.Craft(): 재료가 부족해 아이템 제작에 실패했습니다.");
                    return false;
                }
            }

            // TODO: 아이템 지급 및 보유 아이템 차감
            int index = _components.Count - 1;
            if (_components[index] is ResultItem resultItem)
            {
                string itemName = resultItem.ItemName;
                GFunc.Log($"결과: {itemName}");

                // 크래프트에 성공
                return true;
            }

            // 크래프팅 오류 발생시
            return false;
        }
    }
}
