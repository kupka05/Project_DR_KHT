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
        public enum Type
        {
            CRAFTING = 0,     // 아이템 조합
            ENHANCE = 1       // 아이템 강화
        }
        public Dictionary<int, ICraftingComponent> CraftingDictionary           // 아이템 조합 크래프팅 딕셔너리
           => _craftingDictionary;
        public readonly int[] CRAFTING_TABLE_INDEX =
        {
            3_0000_1, 3_2000_1                                                  // 크래프팅 테이블 색인 인덱스
        };
        public Anvil Anvil => _anvil;                                           // 아이템 조합 모루
        public bool IsEnhance => _isEnhance;                                    // 강화중인지 외부에서 체크하는 함수


        /*************************************************
         *                 Private Field
         *************************************************/
        private Dictionary<int, ICraftingComponent> _craftingDictionary
            = new Dictionary<int, ICraftingComponent>();
        private List<ICraftingComponent> _craftingList
            = new List<ICraftingComponent>();
        private List<ICraftingComponent> _enhanceList
            = new List<ICraftingComponent>();
        [SerializeField] private Anvil _anvil;
        private string _anvilPrefabName = "Crafting_Anvil";                     // 모루 프리팹 이름
        private string _enhancePrefabName = "Crafting_Enhance";                 // 강화소 프리팹 이름
        private bool _isEnhance = false;


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

        // 디버그용
        private void Update()
        {
            //if (Input.GetKeyUp(KeyCode.B))
            //{
            //    Vector3 pos = GameObject.Find("PlayerController").transform.position;
            //    CreateAnvil(pos);
            //}

            //if (Input.GetKeyUp(KeyCode.N))
            //{
            //    Vector3 pos = GameObject.Find("PlayerController").transform.position;
            //    CreateEnhance(pos);
            //}
        }

        /*************************************************
         *               Initialize Methods
         *************************************************/
        // 생성용 함수
        public void Create() { }

        public void Initialize()
        {
            // Init
            InitializeCrafting(Type.CRAFTING);
            InitializeCrafting(Type.ENHANCE);
        }

        // 아이템 [조합/강화] 크래프팅 Init
        private void InitializeCrafting(Type type)
        {
            // Init
            int tableID = GetTableIndex(type);
            List<int> idTable = DataManager.Instance.GetDataTableIDs(tableID);
            for (int i = 0; i < idTable.Count; i++)
            {
                // id 할당
                int id = idTable[i];

                // 크래프팅 아이템 & 마지막 컴포넌트 생성
                CraftingItem craftingItem = new CraftingItem(id);
                ICraftingComponent lastComponent = default;

                // 조건 검색 & 추가
                int[] conditions = GetCraftingConditions(id);
                for (int j = 0; j < conditions.Length; j++)
                {
                    // conditionID가 0일 경우 예외 처리
                    int conditionID = conditions[j];
                    if (conditionID.Equals(0)) { continue; }

                    //GFunc.Log($"ConditionID {conditionID}");
                    // 두 가지 조건의 조합식을 가진 컴포짓 아이템을 생성한다.
                    CompositeItem compositeItem = CreateCompositeItemWithConditions(craftingItem, conditionID);

                    // 크래프팅 아이템에 추가
                    craftingItem.AddComponent(compositeItem);
                }
                
                switch(type)
                {
                    // 타입이 크래프팅일 경우
                    case Type.CRAFTING:

                        // 결과 아이템 생성 & 리스트에 추가
                        lastComponent = CreateResultItem(id);
                        _craftingList.Add(craftingItem);
                        break;

                    // 타입이 강화일 경우
                    case Type.ENHANCE:

                        // 강화 핸들러 생성 & 리스트에 추가
                        lastComponent = CreateEnhanceHandler(id);
                        _enhanceList.Add(craftingItem);
                        break;
                }

                // 크래프팅 아이템 & 딕셔너리에 추가
                craftingItem.AddComponent(lastComponent);
                _craftingDictionary.Add(id, craftingItem);
            }
        }


        /*************************************************
         *                Public Methods
         *************************************************/
        // 지정한 위치에 모루를 생성한다.
        // 생성 후 _anvil에 할당
        public GameObject CreateAnvil(Vector3 pos)
        {
            GameObject prefab = Resources.Load<GameObject>(_anvilPrefabName);
            GameObject anvil = Instantiate(prefab);
            anvil.name = _anvilPrefabName;
            anvil.transform.position = pos;
            _anvil = anvil.transform.GetChild(0).GetComponent<Anvil>();

            return anvil;
        }

        // 지정한 위치에 강화소를 생성한다.
        public GameObject CreateEnhance(Vector3 pos)
        {
            GameObject prefab = Resources.Load<GameObject>(_enhancePrefabName);
            GameObject enhance = Instantiate(prefab);
            enhance.name = _anvilPrefabName;
            enhance.transform.position = pos;

            return enhance;
        }

        // ID로 딕셔너리에 있는 크래프팅 아이템을 반환
        public CraftingItem SearchDictionaryByID(int id)
        {
            return _craftingDictionary[id] as CraftingItem;
        }

        // 강화 크래프팅 아이템을 index로 검색 후 반환한다.
        public CraftingItem FindEnhanceByID(int index)
        {
            CraftingItem item = _enhanceList[index] as CraftingItem;

            GFunc.Log($"item {item}");
            return item;
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        // 두 가지 조합식을 가진 컴포짓 아이템을 생성한다.
        private CompositeItem CreateCompositeItemWithConditions(CraftingItem item, int id)
        {
            int material_1_KeyID = Data.GetInt(id, "Material_1_KeyID");
            int material_2_KeyID = Data.GetInt(id, "Material_2_KeyID");
            int material_1_Amount = Data.GetInt(id, "Material_1_Amount");
            int material_2_Amount = Data.GetInt(id, "Material_2_Amount");
            MaterialItem material_1 = new MaterialItem(item, material_1_KeyID);
            MaterialItem material_2 = new MaterialItem(item, material_2_KeyID);
            CompositeItem compositeItem = new CompositeItem(material_1, material_2);
            item.AddMaterialData(material_1_KeyID, material_1_Amount);
            item.AddMaterialData(material_2_KeyID, material_2_Amount);
            //GFunc.Log($"id: {material_1_KeyID} / amount: {material_1_Amount}");
            //GFunc.Log($"id: {material_2_KeyID} / amount: {material_2_Amount}");

            return compositeItem;
        }

        // 결과 아이템을 생성 후 반환한다.
        private ResultItem CreateResultItem(int id)
        {
            int resultKeyID = Data.GetInt(id, "Result_KeyID");
            int resultAmount = Data.GetInt(id, "Result_Amount");
            Vector3 itemSpawnPos = GetItemSpawnPos(id);
            ResultItem resultItem = new ResultItem(resultKeyID, itemSpawnPos, resultAmount);

            return resultItem;
        }

        // 강화 핸들러를 생성 후 반환한다.
        private EnhanceHandler CreateEnhanceHandler(int id)
        {
            int statKeyID = Data.GetInt(id, "Stat_KeyID");
            EnhanceHandler enhanceHandler = new EnhanceHandler(statKeyID);

            return enhanceHandler;
        }

        // 타입에 맞는 테이블 색인 인덱스를 반환한다.
        private int GetTableIndex(Type type)
        {
            return CRAFTING_TABLE_INDEX[(int)type];
        }

        // 크래프팅의 ID에 해당하는 모든 조건식[1 ~ 4]을 찾아서 반환한다.
        private int[] GetCraftingConditions(int id)
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

        // 아이템이 생성될 위치를 받아온다
        private Vector3 GetItemSpawnPos(int id)
        {
            Vector3 pos = new Vector3();
            pos.x = Data.GetFloat(id, "Spawn_Pos_X");
            pos.y = Data.GetFloat(id, "Spawn_Pos_Y");
            pos.z = Data.GetFloat(id, "Spawn_Pos_Z");

            return pos;
        }
    }
}
