using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Js.Quest;

/*
    [Item의 상속구조]
    - Item
        - CountableItem
            - PortionItem : IUsableItem.Use() -> 사용 및 수량 1 소모
        - EquipmentItem
            - WeaponItem
            - ArmorItem

    [ItemData의 상속구조]
      (ItemData는 해당 아이템이 공통으로 가질 데이터 필드 모음)
      (개체마다 달라져야 하는 현재 내구도, 강화도 등은 Item 클래스에서 관리)

    - ItemData
        - CountableItemData
            - PortionItemData : 효과량(Value : 회복량, 공격력 등에 사용)
        - EquipmentItemData : 최대 내구도
            - WeaponItemData : 기본 공격력
            - ArmorItemData : 기본 방어력
*/

/*
    [API]
    - bool HasItem(int) : 해당 인덱스의 슬롯에 아이템이 존재하는지 여부
    - bool IsCountableItem(int) : 해당 인덱스의 아이템이 셀 수 있는 아이템인지 여부
    - int GetCurrentAmount(int) : 해당 인덱스의 아이템 수량
        - -1 : 잘못된 인덱스
        -  0 : 빈 슬롯
        -  1 : 셀 수 없는 아이템이거나 수량 1
    - ItemData GetItemData(int) : 해당 인덱스의 아이템 정보
    - string GetItemName(int) : 해당 인덱스의 아이템 이름

    - int Add(ItemData, int) : 해당 타입의 아이템을 지정한 개수만큼 인벤토리에 추가
        - 자리 부족으로 못넣은 개수만큼 리턴(0이면 모두 추가 성공했다는 의미)
    - void Remove(int) : 해당 인덱스의 슬롯에 있는 아이템 제거
    - void Swap(int, int) : 두 인덱스의 아이템 위치 서로 바꾸기
    - void SeparateAmount(int a, int b, int amount)
        - a 인덱스의 아이템이 셀 수 있는 아이템일 경우, amount만큼 분리하여 b 인덱스로 복제
    - void Use(int) : 해당 인덱스의 아이템 사용
    - void UpdateSlot(int) : 해당 인덱스의 슬롯 상태 및 UI 갱신
    - void UpdateAllSlot() : 모든 슬롯 상태 및 UI 갱신
    - void UpdateAccessibleStatesAll() : 모든 슬롯 UI에 접근 가능 여부 갱신
    - void TrimAll() : 앞에서부터 아이템 슬롯 채우기
    - void SortAll() : 앞에서부터 아이템 슬롯 채우면서 정렬
*/

// 날짜 : 2021-03-07 PM 7:33:52
// 작성자 : Rito

namespace Rito.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        /***********************************************************************
        *                               Public Properties
        ***********************************************************************/
        #region .
        /// <summary> 아이템 수용 한도 </summary>
        public int Capacity { get; private set; }

        // /// <summary> 현재 아이템 개수 </summary>
        //public int ItemCount => _itemArray.Count;

        #endregion
        /***********************************************************************
        *                               Private Fields
        ***********************************************************************/
        #region .

        // 초기 수용 한도
        [SerializeField, Range(8, 64)]
        private int _initalCapacity = 64;
        public int InitalCapacity => _initalCapacity;

        // 최대 수용 한도(아이템 배열 크기)
        [SerializeField, Range(8, 64)]
        private static int _maxCapacity = 64;
        public static int MaxCapacity => _maxCapacity;

        [SerializeField]
        private InventoryUI _inventoryUI; // 연결된 인벤토리 UI

        [SerializeField]
        private PlayerInventoryUI _playerInventoryUI; // 연결된 플레이어 인벤토리 UI

        /// <summary> 아이템 목록 </summary>
        [SerializeField]
        private Item[] _items = UserDataManager.items;
        public Item[] Items => _items;

        /// <summary> 업데이트 할 인덱스 목록 </summary>
        private readonly HashSet<int> _indexSetForUpdate = new HashSet<int>();

        /// <summary> 아이템 데이터 타입별 정렬 가중치 </summary>
        private readonly static Dictionary<Type, int> _sortWeightDict = new Dictionary<Type, int>
        {
            { typeof(QuestItemData),   10000 },
            { typeof(MaterialItemData),   20000 },
            { typeof(PortionItemData), 30000 },
            { typeof(BombItemData),  40000 },
        };
        private class ItemComparer : IComparer<Item>
        {
            public int Compare(Item a, Item b)
            {
                return (a.Data.ID + _sortWeightDict[a.Data.GetType()])
                     - (b.Data.ID + _sortWeightDict[b.Data.GetType()]);
            }
        }
        private static readonly ItemComparer _itemComparer = new ItemComparer();

        #endregion
        /***********************************************************************
        *                               Unity Events
        ***********************************************************************/
        #region .

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_initalCapacity > _maxCapacity)
                _initalCapacity = _maxCapacity;
        }
#endif
        private void Awake()
        {
            Capacity = _initalCapacity;
            _inventoryUI.SetInventoryReference(this);
        }

        private void Start()
        {
            UpdateAccessibleStatesAll();
        }

        #endregion
        /***********************************************************************
        *                               Private Methods
        ***********************************************************************/
        #region .
        /// <summary> 인덱스가 수용 범위 내에 있는지 검사 </summary>
        private bool IsValidIndex(int index)
        {
            return index >= 0 && index < Capacity;
        }

        /// <summary> 앞에서부터 비어있는 슬롯 인덱스 탐색 </summary>
        private int FindEmptySlotIndex(int startIndex = 0)
        {
            for (int i = startIndex; i < Capacity; i++)
                if (_items[i] == null)
                    return i;
            return -1;
        }

        /// <summary> 앞에서부터 개수 여유가 있는 Countable 아이템의 슬롯 인덱스 탐색 </summary>
        private int FindCountableItemSlotIndex(CountableItemData target, int startIndex = 0)
        {
            for (int i = startIndex; i < Capacity; i++)
            {
                var current = _items[i];
                if (current == null)
                    continue;

                // LEGACY:
                //if (current.Data == target && current is CountableItem ci)
                //{
                //    if (!ci.IsMax)
                //        return i;
                //}

                // LEGACY의 데이터 매칭 오류 발생으로
                // ID매칭으로 변경
                if (current.Data.ID == target.ID && current is CountableItem ci)
                {
                    if (!ci.IsMax)
                        return i;
                }
            }

            return -1;
        }

        /// <summary> 해당하는 인덱스의 슬롯 상태 및 UI 갱신 </summary>
        private void UpdateSlot(int index)
        {
            if (!IsValidIndex(index)) return;

            Item item = _items[index];
            // 1. 아이템이 슬롯에 존재하는 경우
            if (item != null)
            {
                // 아이콘 등록
                _inventoryUI.SetItemIcon(index, item.Data.IconSprite);
                // 1-1. 셀 수 있는 아이템
                if (item is CountableItem ci)
                {
                    // 1-1-1. 수량이 0인 경우, 아이템 제거
                    if (ci.IsEmpty)
                    {
                        _items[index] = null;
                        RemoveIcon();
                        return;
                    }
                    // 1-1-2. 수량 텍스트 표시
                    else
                    {
                        _inventoryUI.SetItemAmountText(index, ci.Amount);
                    }
                }
                // 1-2. 셀 수 없는 아이템인 경우 수량 텍스트 제거
                else
                {
                    _inventoryUI.HideItemAmountText(index);
                }

                // 슬롯 필터 상태 업데이트
                _inventoryUI.UpdateSlotFilterState(index, item.Data);
            }
            // 2. 빈 슬롯인 경우 : 아이콘 제거
            else
            {
                RemoveIcon();
            }

            // 로컬 : 아이콘 제거하기
            void RemoveIcon()
            {
                _inventoryUI.RemoveItem(index);
                _inventoryUI.HideItemAmountText(index); // 수량 텍스트 숨기기
            }

            _playerInventoryUI.UpdatePlayerInventory();
        }

        /// <summary> 해당하는 인덱스의 슬롯들의 상태 및 UI 갱신 </summary>
        private void UpdateSlot(params int[] indices)
        {
            foreach (var i in indices)
            {
                UpdateSlot(i);
            }
        }

        /// <summary> 모든 슬롯들의 상태를 UI에 갱신 </summary>
        public void UpdateAllSlot()
        {
            for (int i = 0; i < Capacity; i++)
            {
                UpdateSlot(i);
            }
        }

        #endregion
        /***********************************************************************
        *                               Check & Getter Methods
        ***********************************************************************/
        #region .

        /// <summary> 해당 슬롯이 아이템을 갖고 있는지 여부 </summary>
        public bool HasItem(int index)
        {
            return IsValidIndex(index) && _items[index] != null;
        }

        /// <summary> 해당 슬롯이 셀 수 있는 아이템인지 여부 </summary>
        public bool IsCountableItem(int index)
        {
            return HasItem(index) && _items[index] is CountableItem;
        }

        /// <summary> 
        /// 해당 슬롯의 현재 아이템 개수 리턴
        /// <para/> - 잘못된 인덱스 : -1 리턴
        /// <para/> - 빈 슬롯 : 0 리턴
        /// <para/> - 셀 수 없는 아이템 : 1 리턴
        /// </summary>
        public int GetCurrentAmount(int index)
        {
            if (!IsValidIndex(index)) return -1;
            if (_items[index] == null) return 0;

            CountableItem ci = _items[index] as CountableItem;
            if (ci == null)
                return 1;

            return ci.Amount;
        }

        /// <summary> 해당 슬롯의 아이템 정보 리턴 </summary>
        public ItemData GetItemData(int index)
        {
            if (!IsValidIndex(index)) return null;
            if (_items[index] == null) return null;

            return _items[index].Data;
        }

        /// <summary> 해당 슬롯의 아이템 이름 리턴 </summary>
        public string GetItemName(int index)
        {
            if (!IsValidIndex(index)) return "";
            if (_items[index] == null) return "";

            return _items[index].Data.Name;
        }

        #endregion
        /***********************************************************************
        *                               Public Methods
        ***********************************************************************/
        #region .
        /// <summary> 인벤토리 UI 연결 </summary>
        public void ConnectUI(InventoryUI inventoryUI)
        {
            _inventoryUI = inventoryUI;
            _inventoryUI.SetInventoryReference(this);
        }

        /// <summary> 인벤토리에 아이템 추가
        /// <para/> 넣는 데 실패한 잉여 아이템 개수 리턴
        /// <para/> 리턴이 0이면 넣는데 모두 성공했다는 의미
        /// </summary>
        public int Add(ItemData itemData, int amount = 1)
        {
            int index;
            // 1. 수량이 있는 아이템
            if (itemData is CountableItemData ciData)
            {
                bool findNextCountable = true;
                index = -1;

                while (amount > 0)
                {
                    // 1-1. 이미 해당 아이템이 인벤토리 내에 존재하고, 개수 여유 있는지 검사
                    if (findNextCountable)
                    {
                        index = FindCountableItemSlotIndex(ciData, index + 1);
                        GFunc.Log($"인덱스: {index}");
                      
                        // 개수 여유있는 기존재 슬롯이 더이상 없다고 판단될 경우, 빈 슬롯부터 탐색 시작
                        if (index == -1)
                        {
                            GFunc.Log("인벤토리에 없음");
                            findNextCountable = false;
                        }
                        // 기존재 슬롯을 찾은 경우, 양 증가시키고 초과량 존재 시 amount에 초기화
                        else
                        {
                            GFunc.Log("인벤토리에 있음");
                            CountableItem ci = _items[index] as CountableItem;
                            amount = ci.AddAmountAndGetExcess(amount);
                            UpdateSlot(index);
                        }
                    }
                    // 1-2. 빈 슬롯 탐색
                    else
                    {
                        index = FindEmptySlotIndex(index + 1);

                        // 빈 슬롯조차 없는 경우 종료
                        if (index == -1)
                        {
                            break;
                        }
                        // 빈 슬롯 발견 시, 슬롯에 아이템 추가 및 잉여량 계산
                        else
                        {
                            // 새로운 아이템 생성
                            CountableItem ci = ciData.CreateItem() as CountableItem;
                            ci.SetAmount(amount);

                            // 슬롯에 추가
                            _items[index] = ci;
                            // 남은 개수 계산
                            amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                            UpdateSlot(index);
                        }
                    }
                }
            }
            // 2. 수량이 없는 아이템
            else
            {
                // 2-1. 1개만 넣는 경우, 간단히 수행
                if (amount == 1)
                {
                    index = FindEmptySlotIndex();
                    if (index != -1)
                    {
                        // 아이템을 생성하여 슬롯에 추가
                        _items[index] = itemData.CreateItem();
                        amount = 0;

                        UpdateSlot(index);
                    }
                }

                // 2-2. 2개 이상의 수량 없는 아이템을 동시에 추가하는 경우
                index = -1;
                for (; amount > 0; amount--)
                {
                    // 아이템 넣은 인덱스의 다음 인덱스부터 슬롯 탐색
                    index = FindEmptySlotIndex(index + 1);

                    // 다 넣지 못한 경우 루프 종료
                    if (index == -1)
                    {
                        break;
                    }

                    // 아이템을 생성하여 슬롯에 추가
                    _items[index] = itemData.CreateItem();

                    UpdateSlot(index);
                }
            }

            GFunc.Log(amount);
            return amount;
        }

        /// <summary> 해당 슬롯의 아이템 제거 </summary>
        public void Remove(int index)
        {
            if (!IsValidIndex(index)) return;

            int itemID = _items[index].Data.ID;
            _items[index] = null;
            _inventoryUI.RemoveItem(index);

            // 퀘스트 콜백 호출
            QuestCallback.OnInventoryCallback(itemID);
        }

        /// <summary> ID로 인벤토리에 있는 아이템을 제거 </summary>
        public bool RemoveInventoryItemForID(int id, int amount)
        {
            GFunc.Log($"삭제할 ID: {id}");
            // ID가 0일 경우 예외처리
            if (id.Equals(0))
            { 
                GFunc.Log("Inventory.RemoveInventoryItemForID():" +
                "삭제할 ID가 0이라서 삭제할 수 없습니다.");
                return false;
            }
            List<int> itemIndexList = new List<int>();
            List<int> itemAmountList = new List<int>();
            int removeAmount = amount;
            int itemTotalAmount = default;
            for (int i = (_items.Length - 1); i > -1; i--)
            {
                // 아이템 보유량 체크 및 인덱스 보관
                // 인벤토리에 있는 아이템 ID와 받아온 ID가 일치할 경우
                if (id.Equals(Items[i]?.Data.ID))
                {
                    int itemCount = default;
                    // 셀 수 있는 아이템일 경우
                    if (Items[i] is CountableItem ci)
                    {
                        GFunc.Log($"ci.amount = {ci.Amount}");
                        // 아이템 총 보유량 & 보유량 리스트 추가
                        itemTotalAmount += ci.Amount;
                        itemCount = ci.Amount;
                    }

                    // 아닐 경우
                    else
                    {
                        itemTotalAmount++;
                        itemCount = 1;
                    }

                    // 인덱스 & 카운트 보관
                    itemIndexList.Add(i);
                    itemAmountList.Add(itemCount);
                    GFunc.Log($"아이템 Amount: {itemCount}");
                }
            }

            // 아이템 총 보유량이 삭제할 보유량 이상일 경우
            if (itemTotalAmount >= amount)
            {
                // 삭제할 아이템 갯수
                for (int i = 0; i < itemIndexList.Count; i++)
                {
                    GFunc.Log(i);
                    // 해당 슬롯의 아이템 보유량 만큼 차감 & 아이템 삭제
                    GFunc.Log($"ItemIndexList[{i}]: {itemIndexList[i]}");
                    int itemAmount = itemAmountList[i];
                    if ((removeAmount - itemAmount) > 0)
                    {
                        // 해당 슬롯 아이템 삭제
                        removeAmount -= itemAmount;
                        GFunc.Log($"보유갯수: {itemAmount}");
                        GFunc.Log($"아이디:{id} 삭제 갯수[{itemAmount} - {removeAmount}]");
                        Remove(itemIndexList[i]);
                    }

                    // 해당 슬롯의 아이템 보유량이 남을 경우
                    else
                    {
                        // 해당 슬롯 아이템 보유량 차감
                        itemAmount -= removeAmount;
                        CountableItem ci = Items[itemIndexList[i]] as CountableItem;
                        GFunc.Log($"아이디[{itemIndexList[i]}]:{id} 삭제 갯수[{itemAmount - removeAmount}]");

                        ci.SetAmount(itemAmount);

                        // 처리 종료
                        break;
                    }
                }

                // 모든 슬롯 업데이트
                UpdateAllSlot();

                // 아이템 정렬 및 PlayerInventoryUI 갱신
                SortAndUpdatePlayerInventoryUI();

                // 삭제 성공 반환
                return true;
            }

            // 아이템 보유량이 적을 경우
            else
            {
                GFunc.Log($"Inventory.RemoveInventoryItemForID(): 삭제할 아이템 ID[{id}]의 보유량[{itemTotalAmount}]이 " +
                    $"삭제할 갯수[{removeAmount}]보다 적어서 삭제에 실패했습니다.");

                // 삭제 실패 반환
                return false;
            }
        }

        /// <summary> ID로 인벤토리에 있는 아이템 보유량 확인 </summary>
        public int FindInventoryItemForID(int id)
        {
            List<int> itemIndexList = new List<int>();
            List<int> itemAmountList = new List<int>();
            int itemTotalAmount = default;
            for (int i = 0; i < Items.Length; i++)
            {
                // 아이템 보유량 체크 및 인덱스 보관
                // 인벤토리에 있는 아이템 ID와 받아온 ID가 일치할 경우
                if (id.Equals(Items[i]?.Data.ID))
                {
                    int itemCount = default;
                    // 셀 수 있는 아이템일 경우
                    if (Items[i] is CountableItem ci)
                    {
                        GFunc.Log($"ci.amount = {ci.Amount}");
                        // 아이템 총 보유량 & 보유량 리스트 추가
                        itemTotalAmount += ci.Amount;
                        itemCount = ci.Amount;
                    }

                    // 아닐 경우
                    else
                    {
                        itemTotalAmount++;
                        itemCount = 1;
                    }

                    // 인덱스 & 카운트 보관
                    itemIndexList.Add(i);
                    itemAmountList.Add(itemCount);
                }
            }

            // 아이템 갯수 반환
            return itemTotalAmount;
        }

        /// <summary> 두 인덱스의 아이템 위치를 서로 교체 </summary>
        public void Swap(int indexA, int indexB)
        {
            if (!IsValidIndex(indexA)) return;
            if (!IsValidIndex(indexB)) return;

            Item itemA = _items[indexA];
            Item itemB = _items[indexB];

            // 1. 셀 수 있는 아이템이고, 동일한 아이템일 경우
            //    indexA -> indexB로 개수 합치기
            if (itemA != null && itemB != null &&
                itemA.Data == itemB.Data &&
                itemA is CountableItem ciA && itemB is CountableItem ciB)
            {
                int maxAmount = ciB.MaxAmount;
                int sum = ciA.Amount + ciB.Amount;

                if (sum <= maxAmount)
                {
                    ciA.SetAmount(0);
                    ciB.SetAmount(sum);
                }
                else
                {
                    ciA.SetAmount(sum - maxAmount);
                    ciB.SetAmount(maxAmount);
                }
            }
            // 2. 일반적인 경우 : 슬롯 교체
            else
            {
                _items[indexA] = itemB;
                _items[indexB] = itemA;
            }

            // 두 슬롯 정보 갱신
            UpdateSlot(indexA, indexB);
        }

        /// <summary> 셀 수 있는 아이템의 수량 나누기(A -> B 슬롯으로) </summary>
        public void SeparateAmount(int indexA, int indexB, int amount)
        {
            // amount : 나눌 목표 수량

            if(!IsValidIndex(indexA)) return;
            if(!IsValidIndex(indexB)) return;

            Item _itemA = _items[indexA];
            Item _itemB = _items[indexB];

            CountableItem _ciA = _itemA as CountableItem;

            // 조건 : A 슬롯 - 셀 수 있는 아이템 / B 슬롯 - Null
            // 조건에 맞는 경우, 복제하여 슬롯 B에 추가
            if (_ciA != null && _itemB == null)
            {
                _items[indexB] = _ciA.SeperateAndClone(amount);

                UpdateSlot(indexA, indexB);
            }
        }

        /// <summary> 해당 슬롯의 아이템 사용 </summary>
        public void Use(int index, int amount = 1)
        {
            if (!IsValidIndex(index)) return;
            if (_items[index] == null) return;

            // 사용 가능한 아이템인 경우
            if (_items[index] is IUsableItem uItem)
            {
                // 아이템 사용
                bool succeeded = uItem.Use();

                if (succeeded)
                {
                    // 업데이트
                    UpdateSlot(index);

                    // 아이템 정렬 및 PlayerInventoryUI 갱신
                    SortAndUpdatePlayerInventoryUI();
                }
            }
        }

        public void SortAndUpdatePlayerInventoryUI()
        {
            // 아이템 정렬
            SortAll();

            // PlayerInventoryUI 갱신
            _playerInventoryUI.UpdatePlayerInventory();
        }

        /// <summary> 모든 슬롯 UI에 접근 가능 여부 업데이트 </summary>
        public void UpdateAccessibleStatesAll()
        {
            _inventoryUI.SetAccessibleSlotRange(Capacity);
        }

        /// <summary> 빈 슬롯 없이 앞에서부터 채우기 </summary>
        public void TrimAll()
        {
            // 가장 빠른 배열 빈공간 채우기 알고리즘

            // i 커서와 j 커서
            // i 커서 : 가장 앞에 있는 빈칸을 찾는 커서
            // j 커서 : i 커서 위치에서부터 뒤로 이동하며 기존재 아이템을 찾는 커서

            // i커서가 빈칸을 찾으면 j 커서는 i+1 위치부터 탐색
            // j커서가 아이템을 찾으면 아이템을 옮기고, i 커서는 i+1 위치로 이동
            // j커서가 Capacity에 도달하면 루프 즉시 종료

            _indexSetForUpdate.Clear();

            int i = -1;
            while (_items[++i] != null) ;
            int j = i;

            while (true)
            {
                while (++j < Capacity && _items[j] == null);

                if (j == Capacity)
                    break;

                _indexSetForUpdate.Add(i);
                _indexSetForUpdate.Add(j);

                _items[i] = _items[j];
                _items[j] = null;
                i++;
            }

            foreach (var index in _indexSetForUpdate)
            {
                UpdateSlot(index);
            }
            _inventoryUI.UpdateAllSlotFilters();
        }

        /// <summary> 빈 슬롯 없이 채우면서 아이템 종류별로 정렬하기 </summary>
        public void SortAll()
        {
            // 1. Trim
            int i = -1;
            while (_items[++i] != null) ;
            int j = i;

            while (true)
            {
                while (++j < Capacity && _items[j] == null) ;

                if (j == Capacity)
                    break;

                _items[i] = _items[j];
                _items[j] = null;
                i++;
            }

            // 2. Sort
            Array.Sort(_items, 0, i, _itemComparer);

            // 3. Update
            UpdateAllSlot();
            _inventoryUI.UpdateAllSlotFilters(); // 필터 상태 업데이트
        }

        #endregion
    }
}