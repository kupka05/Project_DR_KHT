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
        public List<ICraftingComponent> Components => _components;              // 크래프팅 아이템 컴포넌트
        public int NeedHammeringCount => _needHammeringCount;                   // 필요한 망치질 횟수
        public Dictionary<int, int> MaterialDictionary => _materialDictionary;  // 필요한 재료 정보 딕셔너리


        /*************************************************
         *                 Private Fields
         *************************************************/
        private List<ICraftingComponent> _components = new List<ICraftingComponent>();
        private Dictionary<int, int> _materialDictionary = new Dictionary<int, int>();
        private int _needHammeringCount;


        /*************************************************
         *               Initialize Methods
         *************************************************/
        // 추후 할당용 생성자
        public CraftingItem(int id)
        {
            // Init
            _needHammeringCount = Data.GetInt(id, "Need_Hammering_Count");
        }

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

        // 데이터 추가
        public void AddMaterialData(int id, int amount)
        {
            // 딕셔너리[ID]가 비어있을 경우
            if (! _materialDictionary.ContainsKey(id))
            {
                _materialDictionary.Add(id, amount);
            }

            // 있을 경우
            else
            {
                _materialDictionary[id] += amount;
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
                // 제작 조건을 충족하지 못할 경우
                // 사유: 재료 부족
                if (item.CheckCraft().Equals(false))
                {
                    GFunc.Log($"CraftingItem.CheckCraft(): 재료가 부족합니다.");
                    return false;
                }
            }

            // 모든 제작 조건 충족시
            return true;
        }

        public bool CheckEnhance()
        {
            // 크래프팅 아이템 컴포넌트 순회
            // 강화에 필요한 아이템 보유량 체크
            foreach (var item in _components)
            {
                // 제작 조건을 충족하지 못할 경우
                // 사유: 재료 부족
                if (item.CheckEnhance().Equals(false))
                {
                    GFunc.Log($"CraftingItem.CheckEnhance(): 재료가 부족합니다.");
                    return false;
                }
            }

            // 모든 제작 조건 충족시
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
