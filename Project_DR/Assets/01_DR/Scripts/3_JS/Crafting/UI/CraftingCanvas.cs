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
        public CraftingManager CraftingManager => CraftingManager.Instance;     // 크래프팅 매니저 인스턴스
        public Dictionary<int, ICraftingComponent> CraftingDictionary           // 크래프팅 딕셔너리
            => CraftingManager.Instance.CraftingDictionary;


        /*************************************************
         *                Private Fields
         *************************************************/
        [SerializeField] private int TABLE_ID = 3_0000_1;                               // 테이블 색인 ID
        [SerializeField] private Transform _content = default;                          // 스크롤 패널 Content
        [SerializeField] private GameObject _panelPrefab = default;                     // 기본 패널 프리팹
        [SerializeField] private List<CraftingPanelUI> _panelList;                      // 패널 리스트


        /*************************************************
         *                Unity Events
         *************************************************/
        void Start()
        {
            // Init
            Initialize();
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        private void Initialize()
        {
            // Init
            List<int> idTable = DataManager.Instance.GetDataTableIDs(TABLE_ID);
            for (int i = 0; i < idTable.Count; i++)
            {
                // 패널 생성 & 리스트에 추가
                _panelList.Add(CreatePanel(_content));
                _panelList[i].Initialize(idTable[i]);
            }
        }

        // 패널을 생성 후 반환
        private CraftingPanelUI CreatePanel(Transform parent)
        {
            GameObject panel = Instantiate(_panelPrefab);
            panel.transform.SetParent(parent);
            panel.name = "Panel";
            panel.transform.rotation = parent.rotation;

            return panel.GetComponent<CraftingPanelUI>();
        }
    }
}
