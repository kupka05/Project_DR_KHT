using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rito.InventorySystem;

namespace Js.Crafting
{
    public class Anvil : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public Dictionary<int, List<GameObject>> AnvilStorageDictionary      // 모루 위 아이템 저장 딕셔너리
            => _anvilStorageDictionary;                                 
        public int NeedHammeringCount => _needHammeringCount;                // 필요 망치질 횟수
        public int CurrentHammeringCount => _currentHammeringCount;          // 현재 망치질 횟수
        public bool IsHammeringPossible => _isHammeringPossible;              // 망치질이 가능한지 여부


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private Dictionary<int, List<GameObject>> _anvilStorageDictionary = 
            new Dictionary<int, List<GameObject>>();
        private int _needHammeringCount;
        private int _currentHammeringCount;
        private bool _isHammeringPossible = true;


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
            if (Input.GetKeyDown(KeyCode.G))
            {
                Craft(5001, 1, 1);
            }
        }


        /*************************************************
         *                Public Methods
         *************************************************/
        // 아이템 조합을 시도한다.
        public void Craft(int id, int amount, int hammeringAmount)
        {
            //// 아이템이 부족할 경우 예외 처리
            //if (! CheckStorageItem(id, amount)) { return; }

            //// 망치질 횟수 확인
            //if ()
            //// amount만큼 아이템 제거
            //int removeAmount = default;
            //for (int i = 0; i < _anvilStorageDictionary[id].Count; i++)
            //{
            //    // 제거 완료시 반복문 종료
            //    if (removeAmount.Equals(amount)) { break; }

            //    // 리스트에서 아이템 제거 & 오브젝트 파괴
            //    GameObject item = _anvilStorageDictionary[id][0];
            //    _anvilStorageDictionary[id].RemoveAt(0);
            //    Destroy(item);

            //    removeAmount++;
            //}

            GFunc.Log($"갯수: {_anvilStorageDictionary[id].Count}");
        }

        // 저장소에서 아이템을 삭제한다
        public void RemoveItemFromStorage(int id, int amount)
        {
            // amount만큼 아이템 제거
            int removeAmount = default;
            for (int i = 0; i < _anvilStorageDictionary[id].Count; i++)
            {
                // 제거 완료시 반복문 종료
                if (removeAmount.Equals(amount)) { break; }

                // 리스트에서 아이템 제거 & 오브젝트 파괴
                GameObject item = _anvilStorageDictionary[id][0];
                _anvilStorageDictionary[id].RemoveAt(0);
                Destroy(item);

                removeAmount++;
            }

            // 디버그
            GFunc.Log($"남은갯수: {_anvilStorageDictionary[id].Count}");
        }

        // 조합 조건을 충족하는지 확인
        public bool CheckCanCraft(int id, int amount, int count)
        {
            if (CheckStorageItem(id, amount) && CheckNeedHammeringCount(count))
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
                        BlockItemEntryToInventory(gameObject);

                        // 저장소에서 아이템 추가
                        AddAmountAnvilStorage(itemID, gameObject);
                        break;

                    case "Exit":
                        // 저장소에서 아이템 삭제
                        RemoveAmountAnvilStorage(itemID, gameObject);
                        break;
                }
                
                // 디버그
                GFunc.Log($"itemID[{itemID}] / count: [{_anvilStorageDictionary[itemID].Count}]");
            }
        }

        // 망치질 횟수를 추가한다.
        private void AddCurrentHammeringCount(Collision collision)
        {
            // 현재 망치질이 불가능할 경우 예외 처리
            if (_isHammeringPossible.Equals(false)) { return; }

            // Hammer 태그를 가지고 있을 경우
            if (collision.collider.CompareTag("Hammer"))
            {
                // 망치질 횟수 추가
                _currentHammeringCount += 1;
                GFunc.Log($"망치질 횟수 {_currentHammeringCount}");
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


        /*************************************************
         *              Private Get Methods
         *************************************************/
        // 아이템이 인벤토리에 들어가지 못하게 함
        private void BlockItemEntryToInventory(GameObject gameObject)
        {
            ItemColliderHandler item = gameObject.GetComponent<ItemColliderHandler>();
            if (item != null)
            {
                item.state = ItemColliderHandler.State.Default;
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
