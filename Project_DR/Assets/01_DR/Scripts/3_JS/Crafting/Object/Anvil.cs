using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito.InventorySystem;
using Js.Quest;

namespace Js.Crafting
{
    public class Anvil : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public enum State
        {
            DEFAULT = 0,
            FIRST_STEP = 1,
            TWO_STEP = 2
        }
        public Dictionary<int, List<GameObject>> AnvilStorageDictionary      // 모루 위 아이템 저장 딕셔너리
            => _anvilStorageDictionary;
        public Dictionary<int, ICraftingComponent> CraftingDictionary        // 크래프팅 조합식 딕셔너리 
            => CraftingManager.Instance.CraftingDictionary;                 
        public int NeedHammeringCount => _needHammeringCount;                // 필요 망치질 횟수
        public int CurrentHammeringCount => _currentHammeringCount;          // 현재 망치질 횟수
        public bool IsHammeringPossible => _isHammeringPossible;             // 망치질이 가능한지 여부
        public GameObject TempResultItem => _tempResultItem;                 // 결과 임시 아이템


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private Dictionary<int, List<GameObject>> _anvilStorageDictionary = 
            new Dictionary<int, List<GameObject>>();
        [SerializeField] private int _needHammeringCount;
        [SerializeField] private int _currentHammeringCount;
        [SerializeField] private int _currentConditionID;
        [SerializeField] private bool _isHammeringPossible = false;
        [SerializeField] private GameObject _resultItemBox;
        [SerializeField] private Dictionary<int, GameObject> _tempResultItemDictionary;
        [SerializeField] private GameObject _tempResultItem;
        [SerializeField] private CraftingHandler _tempResultItemHandler;
        [SerializeField] private State _currentState;


        /*************************************************
         *                 Unity Events
         *************************************************/
        // 모루 위에 아이템이 올라갔을 경우
        private void OnCollisionEnter(Collision collision)
        {
            // 망치질을 할 경우 횟수 추가
            AddCurrentHammeringCount(collision);

            // 아이템이 올라갔을 경우 동작 수행
            HandleItemColliderAction(collision, "Enter");
        }

        // 모루 위에 아이템이 사라졌을 경우
        private void OnCollisionExit(Collision collision)
        {
            // 아이템이 사라졌을 경우 동작 수행
            HandleItemColliderAction(collision, "Exit");
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                FirstStepCraft();
            }

        }

        private void Start()
        {
            // Init
            Initialize();
        }


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize()
        {
            // Init
            _resultItemBox = transform.Find("Result Item Box").gameObject;
            _tempResultItemDictionary = new Dictionary<int, GameObject>();
            AddTempResultItemDictionary();
        }

        // 모루 초기화
        public void ResetAnvil()
        {
            // 모루 위에 있는 아이템 전부 표시
            ActiveObjectFromAnvilStorage(true);

            // 초기화
            _anvilStorageDictionary.Clear();
            Destroy(_tempResultItem);
            _currentState = State.DEFAULT;
            _currentHammeringCount = 0;
            _needHammeringCount = 0;
            _tempResultItemHandler = null;
            _isHammeringPossible = false;
            _resultItemBox.SetActive(true);
        }

        // 아이템의 예상되는 투명한 결과물을 보여준다
        // [1단계]
        public void FirstStepCraft()
        {
            // 임시 결과 딕셔너리 안에 있는 모든 아이템을 비활성화
            SetActiveFalseInDictionary();

            List<int> craftingIDList = new List<int>();
            foreach (var item in CraftingDictionary)
            {
                // 조합식에 해당하는 재료가 모루위에 모두 있을 경우
                if (CraftingDictionary[item.Key].CheckCraft()
                    && CraftingDictionary[item.Key] is CraftingItem ci)
                {
                    // 마지막 컴포넌트에 ResultItem이 있는 조합 아이템일 경우 
                    if (ci.Components[ci.Components.Count - 1] is ResultItem)
                    {
                        // 크래프팅 아이디 리스트에 키 추가
                        craftingIDList.Add(item.Key);
                    }
                }
            }

            // 조합식에 해당하는 재료가 없을 경우 예외 처리
            if (craftingIDList.Count.Equals(0)) 
            {
                // 상태 변경
                _currentState = State.DEFAULT;
                return; 
            }

            // 결과 임시 아이템 생성 & 할당
            int index = craftingIDList.Count - 1;
            _currentConditionID = craftingIDList[index];

            // 해당하는 결과 아이템을 활성화
            _tempResultItemDictionary[_currentConditionID].SetActive(true);

            // 필요한 망치질 횟수 할당
            CraftingItem craftingItem = CraftingDictionary[_currentConditionID] as CraftingItem;
            _needHammeringCount = craftingItem.NeedHammeringCount;

            // 망치질 가능하게 활성화
            _isHammeringPossible = true;

            // 상태 변경
            _currentState = State.FIRST_STEP;
        }

        // 크래프트에 진입한다. 아이템을 회수해서 취소가 불가능하다.
        // [2단계]
        public void TwoStepCraft()
        {
            // 임시 결과 아이템 숨기기
            _tempResultItemDictionary[_currentConditionID].SetActive(false);
            _resultItemBox.SetActive(false);

            // 임시 결과 아이템 생성
            int itemID = Data.GetInt(_currentConditionID, "Result_KeyID");
            GameObject item = CreateTempResultItem(itemID, transform);
            Vector3 pos = Vector3.zero;
            pos.y = 0.6f;
            item.transform.localPosition = pos;
            item.SetActive(true);
            _tempResultItemHandler = item.AddComponent<CraftingHandler>();
            _tempResultItemHandler.Initialize(this);
            _tempResultItem = item;

            // 모루 위에 있는 아이템 전부 숨기기
            ActiveObjectFromAnvilStorage(false);

            // 상태 변경
            _currentState = State.TWO_STEP;
        }

        // 아이템을 실제로 조합하고 생성한다.
        // [3단계 최종단계]
        public void ThreeStepCraft()
        {
            // 현재 조건 ID가 0일 경우 예외처리
            if (_currentConditionID.Equals(0)) { return; }

            // 결과 아이템 생성
            CraftingDictionary[_currentConditionID].Craft();

            // 크래프팅 콜백 호출
            QuestCallback.OnCraftingCallback(_currentConditionID);

            // 모루 초기화
            ResetAnvil();

            // 모루위에 올라간 아이템 기준으로
            // 임시 결과물 출력
            FirstStepCraft();
        }

        // 저장소에서 아이템을 삭제한다
        public void RemoveItemFromStorage(int id, int amount)
        {
            GFunc.Log($"id: {id} / amount: {amount}");

            // id가 0일 경우 예외처리
            if (id.Equals(0)) { return; }

            // amount만큼 아이템 제거
            int removeAmount = default;
            for (int i = 0; i < _anvilStorageDictionary[id].Count; i++)
            {
                // 제거 완료시 반복문 종료
                if (removeAmount.Equals(amount)) { break; }

                // 아이템 삭제
                GameObject item = _anvilStorageDictionary[id][i];
                Destroy(item);

                removeAmount++;
            }

            // 디버그
            GFunc.Log($"남은갯수: {_anvilStorageDictionary[id].Count}");
        }

        // 조합 조건을 충족하는지 확인
        public bool CheckCanCraft(int id, int amount)
        {
            if (CheckStorageItem(id, amount))
            {
                // 충족할 경우
                return true;
            }

            // 아닐 경우
            return false;
        }

        // 현재 망치질 횟수를 초기화 한다.
        public void ResetCurrentHammeringCount()
        {
            _currentHammeringCount = 0;
        }

        // 망치질 가능 여부를 설정한다.
        public void SetIsHammeringPossible(bool canHammer)
        {
            _isHammeringPossible = canHammer;
        }


        /*************************************************
         *              Private Set Methods
         *************************************************/
        // 아이템 콜라이더와 관련된 동작을 처리
        public void HandleItemColliderAction(Collision collision, string command)
        {
            // ItemDataComponent 컴포넌트를 가지고 있을 경우
            ItemDataComponent item = collision.collider.GetComponent<ItemDataComponent>();
            if (item != null)
            {
                GameObject gameObject = collision.gameObject;

                // 저장소에 아이템 추가 / 삭제
                int itemID = item.ItemData.ID;
                switch (command)
                {
                    case "Enter":
                        // 아이템에 가해진 모든 물리효과 제거
                        // & 인벤토리에 들어가지 못하도록 함
                        RemoveAllPhysicsEffects(gameObject);
                        ChangeStateItem(gameObject, 5);

                        // 저장소에서 아이템 추가
                        AddAmountAnvilStorage(itemID, gameObject);
                        break;

                    case "Exit":
                        // 저장소에서 아이템 삭제
                        ChangeStateItem(gameObject, 0);
                        RemoveAmountAnvilStorage(itemID, gameObject);
                        break;
                }

                // 아이템 임시 결과물 출력
                FirstStepCraft();

                // 디버그
                GFunc.Log($"itemID[{itemID}] / count: [{_anvilStorageDictionary[itemID].Count}]");
            }
        }

        // 망치질 횟수를 추가한다.
        private void AddCurrentHammeringCount(Collision collision)
        {
            //GFunc.Log($"_isHammeringPossible {_isHammeringPossible}");
            // 현재 망치질이 불가능할 경우 예외 처리
            if (_isHammeringPossible.Equals(false)) { return; }

            // Hammer 태그를 가지고 있을 경우
            if (collision.collider.CompareTag("Hammer"))
            {
                // 현재 상태가 [1단계]일 경우 [2단계로] 변경
                if (_currentState.Equals(State.FIRST_STEP))
                {
                    TwoStepCraft();
                }

                // 망치질 횟수 추가
                _currentHammeringCount += 1;

                // 임시 결과 아이템 효과 업데이트
                _tempResultItemHandler.UpdateItemEffect();

                // 디버그
                GFunc.Log($"망치질 횟수 {_currentHammeringCount}");

                // 망치질 횟수가 필요한 횟수를 넘었을 경우 조합 시도
                TryCraftAfterHammeringRequirement();

                // 효과음 재생
                AudioManager.Instance.AddSFX("SFX_Craft_Hammer_01");
                AudioManager.Instance.PlaySFX("SFX_Craft_Hammer_01");
            }
        }

        // 망치질 횟수가 필요한 횟수를 넘었을 경우 조합 시도
        private void TryCraftAfterHammeringRequirement()
        {
            if (_currentHammeringCount >= _needHammeringCount)
            {
                ThreeStepCraft();
            }
        }

        // 모루 저장소에 있는 모든 아이템을 표시/숨김
        // 원할 경우 상태도 변경
        private void ActiveObjectFromAnvilStorage(bool isActive)
        {
            foreach (var item in AnvilStorageDictionary)
            {
                int count = AnvilStorageDictionary[item.Key].Count;
                for (int i = 0; i < count; i++)
                {
                    GameObject storageItem = AnvilStorageDictionary[item.Key][i];
                    storageItem.SetActive(isActive);
                }
            }
        }

        // 모루 저장소에 아이템을 추가
        private void AddAmountAnvilStorage(int id, GameObject gameObject)
        {
            // 저장소 체크 및 아이템 추가
            EnsureAnvilSpaceForID(id);
            _anvilStorageDictionary[id].Add(gameObject);
        }

        // 모루 저장소에 아이템을 삭제
        private void RemoveAmountAnvilStorage(int id, GameObject gameObject)
        {
            // 저장소 체크 및 아이템 삭제
            EnsureAnvilSpaceForID(id);
            _anvilStorageDictionary[id].Remove(gameObject);
        }

        // 크래프팅 결과 아이템 딕셔너리 할당
        private void AddTempResultItemDictionary()
        {
            int tableID = CraftingManager.Instance.CRAFTING_TABLE_INDEX[0];
            List<int> idTable = DataManager.Instance.GetDataTableIDs(tableID);
            for (int i = 0; i < idTable.Count; i++)
            {
                // 임시 결과 아이템 딕셔너리에 추가
                int craftingID = idTable[i];
                int ItemID = Data.GetInt(craftingID, "Result_KeyID");
                Transform parent = _resultItemBox.transform;
                GameObject tempResultItem = CreateTempResultItem(ItemID, parent);
                _tempResultItemHandler = tempResultItem.AddComponent<CraftingHandler>();
                _tempResultItemHandler.Initialize(this);

                _tempResultItemDictionary.Add(craftingID, tempResultItem);
            }
        }

        // 크래프팅 결과 임시 아이템 생성
        private GameObject CreateTempResultItem(int id, Transform parent)
        {
            // 결과 임시 아이템 생성 & 할당
            string itemName = Data.GetString(id, "Name");
            Vector3 pos = Vector3.zero;
            GameObject tempItem = Unit.AddFieldTempItem(pos, id, parent);
            Rigidbody tempItemRigidBody = tempItem.GetComponent<Rigidbody>();
            if (tempItemRigidBody != null) { tempItemRigidBody.isKinematic = true; }
            tempItem.name = GFunc.SumString("[임시] ", itemName);
            tempItem.SetActive(false);

            return tempItem;
        }

        // 크래프팅 임시 결과 딕셔너리에 있는 모든
        // 오브젝트를 비활성화 한다.
        private void SetActiveFalseInDictionary()
        {
            foreach (var item in _tempResultItemDictionary)
            {
                _tempResultItemDictionary[item.Key].SetActive(false);
            }
        }

        /*************************************************
         *              Private Get Methods
         *************************************************/
        /// <summary> 아이템의 상태를 변경함 [0]=디폴트, [5]=크래프팅 </summary>
        private void ChangeStateItem(GameObject gameObject, int state)
        {
            ItemColliderHandler.State changeState = (ItemColliderHandler.State)state;
            ItemColliderHandler item = gameObject.GetComponent<ItemColliderHandler>();
            if (item != null)
            {
                // 상태 변경
                item.state = changeState;
            }
        }

        // 저장소에 아이템이 있는지 확인
        private bool CheckStorageItem(int id, int amount)
        {
            // 모루 저장소에 ID에 해당하는 공간이 있는지 확인하고 없으면 초기화
            EnsureAnvilSpaceForID(id);
            if (_anvilStorageDictionary[id].Count >= amount)
            {
                // 있을 경우
                return true;
            }

            // 없을 경우
            return false;
        }

        // 망치질 횟수를 충족했는지 확인
        private bool CheckNeedHammeringCount(int needCount)
        {
            if (_currentHammeringCount >= needCount)
            {
                // 충족했을 경우
                return true;
            }

            // 아닐 경우
            return false;
        }

        // 모루 저장소에 ID에 해당하는 공간이 있는지 확인하고 없으면 초기화
        private void EnsureAnvilSpaceForID(int id)
        {
            // 비어있을 경우
            if (_anvilStorageDictionary.ContainsKey(id).Equals(false))
            {
                // ID에 해당하는 리스트 초기화
                _anvilStorageDictionary[id] = new List<GameObject>();
            }
        }


        /*************************************************
         *            Private Rigidbody Methods
         *************************************************/
        // 오브젝트에 가해진 모든 물리 효과를 없앤다.
        private void RemoveAllPhysicsEffects(GameObject gameObject)
        {
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.Sleep();
            rigidbody.WakeUp();
        }

    }
}
