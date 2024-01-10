using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour
{
    /*************************************************
     *                 Private Fields
     *************************************************/
    #region [+]
    [Header("Inventory")]
    [SerializeField] private Inventory _inventory;

    [Header("UI")]
    [SerializeField] private ItemTooltipUI _itemTooltip;
    [SerializeField] private GameObject itemSlotPanelPrefab;
    [SerializeField] private List<ItemSlotPanelUI> _itemSlotPanels;
    [SerializeField] private List<Item> _items;
    [SerializeField] private float _itemSlotInterval = 600f;

    private Vector2 _tooltipAnchorPos;
    private float _tooltipInterval = 300f;
    private string _panelName = "ItemSlotPanel";

    [SerializeField] private GameObject _content; // 스크롤 패널 범위 오브젝트
    private float _panelInterval = 180f; // 패널 간격

    #endregion
    /*************************************************
     *                  Unity Events
     *************************************************/
    #region [+]
    void Start()
    {
        _itemSlotPanels = new List<ItemSlotPanelUI>();
        _tooltipAnchorPos = _itemTooltip.GetComponent<RectTransform>().anchoredPosition;

        // 오브젝트 풀링
        CreateSlotPulling();

        // 슬롯 업데이트
        UpdatePlayerInventory();
    }

    #endregion
    /*************************************************a
     *                Public Methods
     *************************************************/
    #region [+]
    public void UpdatePlayerInventory()
    {
        // 아이템 슬롯 업데이트
        UpdateItemSlots();
    }

    private void Update()
    {
        // 디버그
        //UpdateItemSlots();
        // 현재 인벤토리에 아이템 추가할 때 마다
        // UpdateItemSlot() 호출하게 설정했음
    }

    // 아이템 슬롯(패널) 추가
    public GameObject AddItemSlot()
    {
        GameObject itemSlot = Instantiate(itemSlotPanelPrefab, 
            itemSlotPanelPrefab.transform.parent);
        ItemSlotPanelUI itemSlotPanelUI = itemSlot.GetComponent<ItemSlotPanelUI>();
        //itemSlotPanelUI.Initialize(id);
        itemSlot.SetActive(true);
        _itemSlotPanels.Add(itemSlotPanelUI);
        int index = _itemSlotPanels.Count - 1;
        if (index > 0)
        {
            RectTransform itemSlotRect = itemSlot.GetComponent<RectTransform>();
            Vector2 pos = itemSlotRect.anchoredPosition;
            // 야매로 (_itemSlotInterval / 2) * index로 수정함 왜냐면
            // 다르게 하면 간격이 틀어지는 오류가 발생
            // 나중에 방법 찾으면 해결할 예정
            pos.x += (_itemSlotInterval / 2) * index;
            itemSlotRect.anchoredPosition = pos;
        }

        itemSlot.name = _panelName + " (" + index + ")";
        itemSlotPanelUI.SetIndex(index);

        return itemSlot;
    }

    // 미사용: 툴팁 활성화
    public void ShowTooltip(int index)
    {
        //if (_itemSlotPanels[index])
        //{
        //    ItemSlotPanelUI itemSlotPanelUI = 
        //        _itemSlotPanels[index].GetComponent<ItemSlotPanelUI>(); 
        //    UpdateTooltipUI(itemSlotPanelUI.ItemData, index);
        //    _itemTooltip.Show();
        //}
    }

    // 미사용: 툴팁 비활성화
    public void HideToolTip()
    {
        //_itemTooltip.Hide();
    }

    // 미사용: 툴팁 업데이트
    private void UpdateTooltipUI(ItemData data, int index)
    {
        //// 툴팁 정보 갱신
        //_itemTooltip.SetItemInfo(data);

        //RectTransform tooltipRect = _itemTooltip.GetComponent<RectTransform>();
        //Vector2 tempAnchorPos = _tooltipAnchorPos;
        //tempAnchorPos.x += (_tooltipInterval * index);

        //// 툴팁 위치 조정
        //tooltipRect.anchoredPosition = tempAnchorPos;
    }

    #endregion
    /*************************************************
     *                Private Methods
     *************************************************/
    #region [+]
    // 초기 슬롯 오브젝트 풀링
    private void CreateSlotPulling()
    {
        for (int i = 0; i < _inventory.InitalCapacity; i++)
        {
            GameObject slot = AddItemSlot();
            // 기본 슬롯 3개 표시를 위해 예외처리
            if ((i < 3) == false)
            {
                slot.SetActive(false);
            }
        }
    }

    // 슬롯에 데이터 Init
    private void InitSlotData(int id, int amount, int maxAmount, int index, int itemIndex)
    {
        _itemSlotPanels[index].Initialize(id, amount, maxAmount, index, itemIndex);
        _itemSlotPanels[index].gameObject.SetActive(true);
    }

    // 지정 순번 이후의 모든 슬롯 초기화
    private void ResetSlotDatas(int startIndex, int id = default,
    int amount = default, int maxAmount = default, int index = default, int itemIndex = default)
    { 
        for (int i = startIndex; i < _itemSlotPanels.Count; i++)
        {
            _itemSlotPanels[i].Initialize(id, amount, maxAmount, index, itemIndex);
            // 기본 표시 슬롯 3개를 제외한 나머지 슬롯 비활성화
            if (i >= 3)
            {
                _itemSlotPanels[i].gameObject.SetActive(false);
            }
        }
    }

    // 아이템 슬롯 업데이트
    private void UpdateItemSlots()
    {
        // 인벤토리 슬롯 개수
        int count = _inventory.InitalCapacity;
        int latestIndex = -1;
        // 인벤토리에 있는 슬롯 개수만큼 순회
        for (int i = 0; i < count; i++)
        {            
            // 인벤토리에 슬롯이 비어있지 않을 경우
            if (_inventory.Items[i] != null)
            {
                // 해당 슬롯에 있는 아이템이 PortionItem일 경우
                if (_inventory.Items[i] is PortionItem pi)
                {
                    // 순차적으로 _itemSlotPanels을 순회
                    for (int j = latestIndex + 1; j < count; j++)
                    {
                        // 추가할 패널이 비활성화인 경우
                        if (_itemSlotPanels[j].gameObject.activeSelf == false)
                        {
                            // 활성화
                            _itemSlotPanels[j].gameObject.SetActive(true);
                        }
                        // 데이터 Init
                        int id = pi.Data.ID;
                        int amount = pi.Amount;
                        int maxAmount = pi.MaxAmount;
                        int itemIndex = i;
                        //GFunc.Log($"보유 아이템 인덱스: {itemIndex}");
                        // i는 실제 인벤토리 아이템 인덱스 저장을 위해 보냄
                        // itemIndex는 실제 보유 아이템 인덱스
                        InitSlotData(id, amount, maxAmount, j, itemIndex);

                        latestIndex = j;
                        break;
                    }
                }
                // 해당 슬롯에 있는 아이템이 BombItem일 경우
                else if (_inventory.Items[i] is BombItem bi)
                {
                    // 순차적으로 _itemSlotPanels을 순회
                    for (int j = latestIndex + 1; j < count; j++)
                    {
                        // 추가할 패널이 비활성화인 경우
                        if (_itemSlotPanels[j].gameObject.activeSelf == false)
                        {
                            // 활성화
                            _itemSlotPanels[j].gameObject.SetActive(true);
                        }
                        // 데이터 Init
                        int id = bi.Data.ID;
                        int amount = bi.Amount;
                        int maxAmount = bi.MaxAmount;
                        int itemIndex = i;
                        //GFunc.Log($"보유 아이템 인덱스: {itemIndex}");
                        // i는 실제 인벤토리 아이템 인덱스 저장을 위해 보냄
                        InitSlotData(id, amount, maxAmount, j, itemIndex);

                        latestIndex = j;
                        break;
                    }
                }
            }
        }

        // 슬롯 리셋
        ResetSlotDatas(latestIndex + 1);

        // 스크롤 패널 범위 설정
        SetScrollArea(latestIndex);
    }

    // 스크롤 패널 범위 설정
    private void SetScrollArea(int count)
    {
        // 슬롯(count)이 3개 미만일 경우 기본 범위(스크롤 불가)
        if (count < 3)
        {
            return;
        }

        RectTransform contentRect = _content.GetComponent<RectTransform>();
        RectTransform slotRect = _itemSlotPanels[count].GetComponent<RectTransform>();
        Vector2 slotAnchorPos = slotRect.anchoredPosition;
        contentRect.sizeDelta = slotAnchorPos;
    }

    #endregion
}
