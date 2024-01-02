using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Js.Crafting
{
    public class CraftingCanvas : MonoBehaviour
    {
        /*************************************************
         *                Public Fields
         *************************************************/
        public enum Type
        {
            ONE = 0,
            TWO = 1
        }
        public Type CurrentType => _currentType;                                // 현재 타입[0 ~ 1]
        public int EnhanceID => _enhanceID;                                     // 강화 ID                     
        public CraftingManager CraftingManager => CraftingManager.Instance;     // 크래프팅 매니저 인스턴스
        public Dictionary<int, ICraftingComponent> CraftingDictionary           // 크래프팅 딕셔너리
            => CraftingManager.Instance.CraftingDictionary;
        public CraftingUI CraftingUI => _craftingUI;                            // 크래프팅 UI
        public Canvas Canvas => _canvas;                                        // 크래프팅 캔버스
        public Transform Parent => _parent;                                     // 부모


        /*************************************************
         *                Private Fields
         *************************************************/
        [SerializeField] private int TABLE_ID = 3_0000_1;                       // 테이블 색인 ID
        [SerializeField] private int _enhanceID;
        [SerializeField] private Type _currentType;
        [SerializeField] private CraftingUI _craftingUI;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _content = default;                  // 스크롤 패널 Content
        [SerializeField] private Transform _parent;
        [SerializeField] private GameObject _panelPrefab = default;             // 기본 패널 프리팹
        [SerializeField] private List<CraftingPanelUI> _panelList;              // 패널 리스트


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize(CraftingUI craftingUI, Type type)
        {
            // Init
            _craftingUI = craftingUI;
            _currentType = type;
            _canvas = gameObject.GetComponent<Canvas>();
            _parent = transform.parent;
            List<int> idTable = DataManager.Instance.GetDataTableIDs(TABLE_ID);
            for (int i = 0; i < idTable.Count; i++)
            {
                // 패널 생성 & 리스트에 추가
                _panelList.Add(CreatePanel(_content));
                _panelList[i].Initialize(idTable[i], i, this);
            }
        }

        // 패널을 생성 후 반환
        public CraftingPanelUI CreatePanel(Transform parent)
        {
            GameObject panel = Instantiate(_panelPrefab);
            panel.transform.SetParent(parent);
            panel.name = "Panel";
            panel.transform.rotation = parent.rotation;

            return panel.GetComponent<CraftingPanelUI>();
        }

        // EnhanceID를 설정
        public void SetEnhanceID(int id)
        {
            _enhanceID = id;
        }
    }
}
