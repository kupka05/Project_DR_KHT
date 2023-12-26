using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public class CraftingManager : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        #region 싱글톤 패턴
        private static CraftingManager m_Instance = null; // 싱글톤이 할당될 static 변수
        public static CraftingManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<CraftingManager>();
                if (m_Instance == null)
                {
                    GameObject obj = new GameObject("CraftingManager");
                    m_Instance = obj.AddComponent<CraftingManager>();
                    DontDestroyOnLoad(obj);
                }
                return m_Instance;
            }
        }
        #endregion
        public Dictionary<int, ICraftingComponent> ItemCraftingDictionary           // 아이템 조합 크래프팅 딕셔너리
           => _itemCraftingDictionary;
        public const int TABLE_INDEX = 3_0000_1;                              // 테이블 색인 인덱스


        /*************************************************
         *                 Private Field
         *************************************************/
        [SerializeField] private Dictionary<int, ICraftingComponent> _itemCraftingDictionary 
            = new Dictionary<int, ICraftingComponent>();


        /*************************************************
         *                  Unity Events
         *************************************************/
        // TODO: 크래프팅은 던전에서만 나온다.
        // 추후 Start로 변경한다.
        // 로비씬에서 해당 인스턴스 호출해서 매니저 생성해야한다.
        void Awake()
        {
            // Init
            Initialize();
        }


        /*************************************************
         *                 Public Methods
         *************************************************/
        public void Initialize()
        {
            // Init
            List<int> idTable = DataManager.Instance.GetDataTableIDs(TABLE_INDEX);
            for (int i = 0; i < idTable.Count; i++)
            {
                // id 할당
                int id = idTable[i];

                // 크래프팅 아이템 생성
                CraftingItem craftingItem = new CraftingItem();

                // 조건 검색 & 추가
                int[] conditions = FindCraftingConditions(id);
                for (int j = 0; j < conditions.Length; j++)
                {
                    // conditionID가 0일 경우 예외 처리
                    int conditionID = conditions[j];
                    if (conditions[j].Equals(0)) { continue; }

                    // 두 가지 조건의 조합식을 가진 컴포짓 아이템을 생성한다.
                    CompositeItem compositeItem = CreateCompositeItemWithConditions(conditionID);

                    // 크래프팅 아이템에 추가
                    craftingItem.AddComponent(compositeItem);
                }

                // 결과 아이템 생성 & 크래프팅 아이템에 추가
                int resultKeyID = Data.GetInt(id, "Result_KeyID");
                int resultAmount = Data.GetInt(id, "Result_Amount");
                ResultItem resultItem = new ResultItem(resultKeyID, resultAmount);
                craftingItem.AddComponent(resultItem);

                // 딕셔너리에 크래프팅 아이템 추가
                _itemCraftingDictionary.Add(id, craftingItem);
            }
        }

        // 크래프팅 아이템(조합식+결과)을 딕셔너리에서 찾아서 반환
        public ICraftingComponent GetCraftingCondition(int id)
        {
            return _itemCraftingDictionary[id];
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        // 두 가지 조합식을 가진 컴포짓 아이템을 생성한다.
        private CompositeItem CreateCompositeItemWithConditions(int id)
        {
            int material_1_KeyID = Data.GetInt(id, "Material_1_KeyID");
            int material_2_KeyID = Data.GetInt(id, "Material_2_KeyID");
            int material_1_Amount = Data.GetInt(id, "Material_1_Amount");
            int material_2_Amount = Data.GetInt(id, "Material_2_Amount");
            MaterialItem material_1 = new MaterialItem(material_1_KeyID, material_1_Amount);
            MaterialItem material_2 = new MaterialItem(material_2_KeyID, material_2_Amount);
            CompositeItem compositeItem = new CompositeItem(material_1, material_2);

            return compositeItem;
        }

        // 크래프팅의 ID에 해당하는 모든 조건식[1 ~ 4]을 찾아서 반환한다.
        private int[] FindCraftingConditions(int id)
        {
            int[] conditions =
            {
                Data.GetInt(id, "Condition_1_KeyID"),
                Data.GetInt(id, "Condition_2_KeyID"),
                Data.GetInt(id, "Condition_3_KeyID"),
                Data.GetInt(id, "Condition_4_KeyID")
            };

            return conditions;
        }
    }
}
