using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 날짜 : 2021-04-01 PM 8:33:22
// 작성자 : Rito

namespace Rito.InventorySystem
{
    /// <summary> 슬롯 내의 아이템 아이콘에 마우스를 올렸을 때 보이는 툴팁 </summary>
    public class ItemTooltipUI : MonoBehaviour
    {
        /***********************************************************************
        *                           Inspector Option Fields
        ***********************************************************************/
        #region .
        [SerializeField]
        private Text _titleText;   // 아이템 이름 텍스트

        [SerializeField]
        private Text _contentText; // 아이템 설명 텍스트

        #endregion
        /***********************************************************************
        *                               Private Fields
        ***********************************************************************/
        #region .
        private RectTransform _rt;
        private CanvasScaler _canvasScaler;

        private static readonly Vector2 LeftTop = new Vector2(0f, 1f);
        private static readonly Vector2 LeftBottom = new Vector2(0f, 0f);
        private static readonly Vector2 RightTop = new Vector2(1f, 1f);
        private static readonly Vector2 RightBottom = new Vector2(1f, 0f);

        #endregion
        /***********************************************************************
        *                               Unity Events
        ***********************************************************************/
        #region .
        private void Awake()
        {
            Init();
            Hide();
        }

        #endregion
        /***********************************************************************
        *                               Private Methods
        ***********************************************************************/
        #region .
        private void Init()
        {
            TryGetComponent(out _rt);
            _rt.pivot = LeftTop;
            _canvasScaler = GetComponentInParent<CanvasScaler>();

            DisableAllChildrenRaycastTarget(transform);
        }

        /// <summary> 모든 자식 UI에 레이캐스트 타겟 해제 </summary>
        private void DisableAllChildrenRaycastTarget(Transform tr)
        {
            // 본인이 Graphic(UI)를 상속하면 레이캐스트 타겟 해제
            tr.TryGetComponent(out Graphic gr);
            if(gr != null)
                gr.raycastTarget = false;

            // 자식이 없으면 종료
            int childCount = tr.childCount;
            if (childCount == 0) return;

            for (int i = 0; i < childCount; i++)
            {
                DisableAllChildrenRaycastTarget(tr.GetChild(i));
            }
        }

        #endregion
        /***********************************************************************
        *                               Public Methods
        ***********************************************************************/
        #region .
        /// <summary> 툴팁 UI에 아이템 정보 등록 </summary>
        public void SetItemInfo(ItemData data)
        {
            _titleText.text = data.Name;
            _contentText.text = data.Tooltip;
        }

        /// <summary> 툴팁의 위치 조정 </summary>
        public void SetRectPosition(RectTransform slotRect, int widthSlots, int heightSlots, int index, float slotSize)
        {
            //// 캔버스 스케일러에 따른 해상도 대응
            //float wRatio = Screen.width / _canvasScaler.referenceResolution.x;
            //float hRatio = Screen.height / _canvasScaler.referenceResolution.y;
            //float ratio = 
            //    wRatio * (1f - _ca
            //    nvasScaler.matchWidthOrHeight) +
            //    hRatio * (_canvasScaler.matchWidthOrHeight);

            //float slotWidth = slotRect.rect.width * ratio;
            //float slotHeight = slotRect.rect.height * ratio;

            //// 툴팁 초기 위치(슬롯 우하단) 설정
            //_rt.position = slotRect.position + new Vector3(slotWidth, -slotHeight);
            //Vector2 pos = _rt.position;

            //// 툴팁의 크기
            //float width = _rt.rect.width * ratio;
            //float height = _rt.rect.height * ratio;

            //// 우측, 하단이 잘렸는지 여부
            //bool rightTruncated = pos.x + width > Screen.width;
            //bool bottomTruncated = pos.y - height < 0f;

            //ref bool R = ref rightTruncated;
            //ref bool B = ref bottomTruncated;

            //// 오른쪽만 잘림 => 슬롯의 Left Bottom 방향으로 표시
            //if (R && !B)
            //{
            //    _rt.position = new Vector2(pos.x - width - slotWidth, pos.y);
            //}
            //// 아래쪽만 잘림 => 슬롯의 Right Top 방향으로 표시
            //else if (!R && B)
            //{
            //    _rt.position = new Vector2(pos.x, pos.y + height + slotHeight);
            //}
            //// 모두 잘림 => 슬롯의 Left Top 방향으로 표시
            //else if (R && B)
            //{
            //    _rt.position = new Vector2(pos.x - width - slotWidth, pos.y + height + slotHeight);
            //}
            //// 잘리지 않음 => 슬롯의 Right Bottom 방향으로 표시
            //// Do Nothing

            // 현재 인벤토리 기준으로 툴팁 위치 계산
            // 인벤토리 패널 사이즈가 변할 경우 위치가 틀어질 수 있음
            int xIndex = (index + 1) % widthSlots == 0 ? widthSlots - 1 : ((index + 1) % widthSlots) - 1;
            int yIndex = index / widthSlots;
            Debug.Log(yIndex);
            float defaultWidth = -277f;
            float defaultHeight = 327f;
            float minWidth = -277f;
            float maxWidth = 15f;
            float widthRange = Math.Abs(minWidth) + Math.Abs(maxWidth);
            float adjustWidth = (widthRange / (widthSlots - 1)) * xIndex;
            float adjustHeight = slotSize * yIndex;
            _rt.anchoredPosition3D = new Vector3((defaultWidth + adjustWidth), (defaultHeight - adjustHeight), 0f);
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        #endregion
    }
}