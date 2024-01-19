using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public class InventoryChest : MonoBehaviour
    {
        /*************************************************
         *                 Public Field
         *************************************************/
        public enum State
        {
            CLOSE = 0,                                          // 열림
            OPEN = 1                                            // 닫힘
        }


        /*************************************************
         *                 Private Field
         *************************************************/
        [SerializeField] private Animation _animtion;           // 애니메이션
        [SerializeField] private State _currentState;           // 현재 상태
        [SerializeField] private GameObject _player;            // 플레이어
        [SerializeField] private Canvas _canvasInventory;       // 크래프팅 인벤토리 캔버스
        [SerializeField] private bool _isInitalize = false;     // 초기화 여부


        /*************************************************
         *                 Public Methods
         *************************************************/
        public void Initialize(GameObject player)
        {
            // 초기화를 안했을 경우
            if (! _isInitalize)
            {
                // Init
                _animtion = GetComponent<Animation>();
                _player = player;
                _canvasInventory = player.transform.FindChildRecursive("Canvas_Inventory")
                    ?.GetComponent<Canvas>();
                // 인벤토리 캔버스를 찾지 못한 경우
                if (_canvasInventory == null)
                {
                    // 씬 내에 모든 오브젝트를 검색
                    Object[] objects = FindObjectsOfTypeAll(typeof(Canvas));

                    foreach (Object obj in objects)
                    {
                        // 인벤토리 캔버스를 찾을 경우
                        if (obj.name.Equals("Canvas_Inventory"))
                        {
                            _canvasInventory = (Canvas)obj;
                            break;
                        }
                    }
                }

                // 상자 설정 & 크래프팅 인벤토리 정렬
                SetChest();
                SortAllInventorySlot();
                _isInitalize = true;
            }

            // 초기화를 했을 경우
            return;
        }

        // 상자를 토글
        public void ToggleChest()
        {
            // 현재 상태에 따라 상자 토글
            switch (_currentState)
            {
                // 상자가 열려있을 경우
                case State.OPEN:
                    // 상자를 닫는다
                    CloseChest();
                    break;

                // 상자가 닫혀있을 경우
                case State.CLOSE:
                    // 상자를 연다
                    OpenChest();
                    break;
            }
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        // 상자를 연다
        private void OpenChest()
        {
            // 애니메이션 재생 & 상태 변경
            _animtion.Play("Chest_Open");
            _currentState = State.OPEN;

            // 크래프팅 인벤토리 표시
            _canvasInventory.gameObject.SetActive(true);

            // 효과음 재생
            AudioManager.Instance.AddSFX("SFX_Craft_BoxOpen_01");
            AudioManager.Instance.PlaySFX("SFX_Craft_BoxOpen_01");
        }

        // 상자를 닫는다
        private void CloseChest()
        {
            // 애니메이션 재생 & 상태 변경
            _animtion.Play("Chest_Close");
            _currentState = State.CLOSE;

            // 크래프팅 인벤토리 숨김
            _canvasInventory.gameObject.SetActive(false);

            // 효과음 재생
            AudioManager.Instance.AddSFX("SFX_Craft_BoxClose_01");
            AudioManager.Instance.PlaySFX("SFX_Craft_BoxClose_01");
        }

        // 인벤토리를 크래프팅 하위의 자식으로 변경하고
        // 위치를 상자 위로 변경한다.
        private void SetChest()
        {
            // 부모 & 트랜스폼 설정 변경
            Transform inventory = _canvasInventory.transform;
            Vector3 scale = inventory.localScale;
            Vector3 position = new Vector3(0f, 1.0f, 0.4f);
            Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);

            inventory.SetParent(transform);
            inventory.localPosition = position;
            inventory.localScale = scale;
            inventory.localRotation = rotation;

            inventory.GetComponentInChildren<ResetScroll>().ResetScrollPos();
        }

        // 인벤토리의 모든 슬롯을 정렬
        private void SortAllInventorySlot()
        {
            Inventory inventory = _canvasInventory.transform.Find("Inventory")
                .gameObject?.GetComponent<InventoryUI>()?.Inventory;
            if (inventory != null)
            {
                inventory.SortAndUpdatePlayerInventoryUI();
            }
        }
    }
}
