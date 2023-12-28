using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public class CraftingHandler : MonoBehaviour
    {
        /*************************************************
         *                Public Fields
         *************************************************/
        public int NeedHammeringCount => _anvil.NeedHammeringCount;         // 필요한 망치질 횟수
        public int CurrentHammeringCount => _anvil.CurrentHammeringCount;   // 현재 망치질 횟수
        public Material ItemMaterial => _itemMeshRenderer.material;         // 임시 결과 아이템 머테리얼


        /*************************************************
         *                Private Fields
         *************************************************/
        [SerializeField] private Anvil _anvil;
        [SerializeField] private MeshRenderer _itemMeshRenderer;
        private bool _isCraft = false;


        /*************************************************
         *                 Unity Events
         *************************************************/
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize(Anvil anvil)
        {
            // Init
            _anvil = anvil;
            _itemMeshRenderer = GetComponent<MeshRenderer>();
            SetMaterial();
        }

        // 아이템 효과 업데이트
        public void UpdateItemEffect()
        {
            // 망치질 횟수 초과시 예외 처리
            if (IsOverRequiredHammeringCount()) { return; }

            // 망치질 횟수에 따른 머테리얼의 투명도를 조절
            SetMaterialAlpha(CalculateAlphaFromHammering());
        }


        /*************************************************
         *                Private Methods
         *************************************************/
        // 머테리얼 색상 변경
        private void SetMaterial()
        {
            if (_itemMeshRenderer != null)
            {
                // 머테리얼 색상을 변경
                Material material = Resources.Load<Material>("Crafting_WhiteMaterial");
                _itemMeshRenderer.material = material;
            }
        }

        // 머테리얼의 투명도를 조절
        private void SetMaterialAlpha(float alpha)
        {
            if (ItemMaterial != null)
            {
                // 투명도 설정 (Base Color 속성의 알파 채널)
                Color baseColor = ItemMaterial.GetColor("_BaseColor");
                baseColor.a = alpha;
                ItemMaterial.SetColor("_BaseColor", baseColor);
            }
        }

        // 망치질 횟수에 따른 투명도 계산
        private float CalculateAlphaFromHammering()
        {
            float alpha = (1.0f / NeedHammeringCount) * CurrentHammeringCount;
            return alpha;
        }

        // 망치질 횟수가 필요 망치질 횟수를 넘었는지 확인
        public bool IsOverRequiredHammeringCount()
        {
            if (CurrentHammeringCount >= NeedHammeringCount)
            {
                // 넘었을 경우
                return true;
            }

            // 아닐 경우
            return false;
        }

    }
}
