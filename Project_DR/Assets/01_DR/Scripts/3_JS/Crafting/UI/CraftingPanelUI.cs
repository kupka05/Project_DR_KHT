using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Js.Crafting
{
    public class CraftingPanelUI : MonoBehaviour
    {

        /*************************************************
         *                Public Fields
         *************************************************/
        public TMP_Text Text => _text;                             // UI에 표시될 텍스트
        public CraftingCanvas.Type CurrentType => _craftingCanvas.CurrentType;

        /*************************************************
         *                Private Fields
         *************************************************/
        [SerializeField] private int _id;
        [SerializeField] private int _index;                       // 패널 순서 인덱스
        [SerializeField] private bool _isNormal = true;            // 텍스트 세팅 형태
        [SerializeField] private CraftingCanvas _craftingCanvas;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private bool _isEnhance;                  // 강화 패널인지 여부
        [SerializeField] BNG.SnapZone _enhanceSlot;                // 강화 슬롯


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize(int id, int index, CraftingCanvas craftingCanvas)
        {
            // Init
            _id = id;
            _index = index;
            _craftingCanvas = craftingCanvas;
            gameObject.SetActive(true);
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            _text.text = ConvertText(id);
            _button = GetComponent<Button>();
            _button?.onClick.AddListener(OnClick);
            Transform enhanceSlot = _craftingCanvas.CraftingUI.transform.FindChildRecursive("Enhance_Slot");
            _enhanceSlot = enhanceSlot?.GetComponent<BNG.SnapZone>();
        }


        /*************************************************
         *               Private Methods
         *************************************************/
        // 형태에 맞게 텍스트를 변경 후 반환한다.
        private string ConvertText(int id)
        {
            // 기본 형태일 경우
            if (_isNormal)
            {
                return ConvertTextForNormal(id);
            }

            // 보너스 형태일 경우
            else
            {
                return ConvertTextForBonus(id);
            }
        }

        // 보너스 스텟형 텍스트 세팅
        // 기본형 텍스트 세팅
        private string ConvertTextForNormal(int id)
        {
            string itemName = Data.GetString(id, "Name");
            string text = GFunc.SumString(itemName, " ▶");
            int[] conditionKeyIDs =
            {
               Data.GetInt(id, "Condition_1_KeyID"),
               Data.GetInt(id, "Condition_2_KeyID"),
               Data.GetInt(id, "Condition_3_KeyID"),
               Data.GetInt(id, "Condition_4_KeyID"),
            };
            for (int i = 0; i < conditionKeyIDs.Length; i++)
            {
                // 조건 ID가 0일 경우 무시하기
                if (conditionKeyIDs[i].Equals(0)) { continue; }

                int conditionID = conditionKeyIDs[i];
                int material_1_KeyID = Data.GetInt(conditionID, "Material_1_KeyID");
                int material_2_KeyID = Data.GetInt(conditionID, "Material_2_KeyID");
                int material_1_Amount = Data.GetInt(conditionID, "Material_1_Amount");
                int material_2_Amount = Data.GetInt(conditionID, "Material_2_Amount");
                string itemName_1 = Data.GetString(material_1_KeyID, "Name");
                string itemName_2 = Data.GetString(material_2_KeyID, "Name");
                text += AddTextIfIDNotZero(material_1_KeyID, itemName_1, material_1_Amount);
                text += AddTextIfIDNotZero(material_2_KeyID, itemName_2, material_2_Amount);
            }

            // 마지막 글자 삭제 후 반환
            return text.Substring(0, text.Length - 1);
        }

        // 기본형 텍스트 세팅
        private string ConvertTextForBonus(int id)
        {
            string itemName = Data.GetString(id, "Name");
            string text = GFunc.SumString(itemName, " ▶");
            string statAmount = Data.GetString(id, "Stat_Amount");
            string statProbaility = Data.GetString(id, "Success_Probability");
            text = GFunc.SumString(text, "증가량: ", statAmount, "% ", "성공 확률: ", statProbaility, "%");

            return text;
        }

        // id가 0이 아닐 경우 텍스트 추가
        private string AddTextIfIDNotZero(int id, string name, int amount)
        {
            string text = default;
            if (! id.Equals(0))
            {
                text = GFunc.SumString(" " , name, "<size=18>x", amount.ToString(), "</size> +");
            }

            return text;
        }


        /*************************************************
         *                Public Methods
         *************************************************/
        // 클릭시 관련 동작
        public void OnClick()
        {
            // 패널이 강화 전용일 경우
            if (_isEnhance)
            {
                switch (CurrentType)
                {
                    // 캔버스 타입이 [Type.ONE]일 경우
                    case CraftingCanvas.Type.ONE:
                        // 모든 캔버스 토글 & 강화 ID 전송
                        _craftingCanvas.CraftingUI.ToggleAllCanvas(_id);
                        // EMPTY //
                        break;

                    // 캔버스 타입이 [Type.TWO]일 경우
                    case CraftingCanvas.Type.TWO:
                        // TODO: 강화시도
                        GFunc.Log("강화시도!");
                        int id = _craftingCanvas.EnhanceID;
                        TryEnhance(id);

                        // 모든 캔버스 토글
                        break;
                }

                // 모든 캔버스 토글 & 강화 ID 전송
                //_craftingCanvas.CraftingUI.ToggleAllCanvas(_id);
            }
        }

        // 강화를 시도한다
        public void TryEnhance(int id)
        {
            CraftingItem craftingItem = 
                CraftingManager.Instance.CraftingDictionary[id] as CraftingItem;

            // 예외 처리
            if (craftingItem.Equals(null)) { return; }

            int type = _index;
            // 재료가 있을 경우
            if (craftingItem.CheckEnhance())
            {
                for (int i = 0; i < craftingItem.Components.Count; i++)
                {
                    // 순회하며 모든 재료 소비 & 강화 시도
                    craftingItem.Components[i].Enhance(type);
                }
            }
            
            // 재료가 없을 경우
            else
            {
                GFunc.Log("재료가 부족합니다.");
            }

            // 강화 슬롯 초기화
            _enhanceSlot.ResetEnhanceSlot();
        }
    }
}
