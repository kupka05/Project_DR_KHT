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
        public TMP_Text Text => _text;          // UI에 표시될 텍스트


        /*************************************************
         *                Private Fields
         *************************************************/
        [SerializeField] private TMP_Text _text;


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize(int id)
        {
            // Init
            gameObject.SetActive(true);
            RectTransform rectTransform = this.GetComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            _text.text = ConvertText(id);
        }


        /*************************************************
         *               Private Methods
         *************************************************/
        // 크래프팅에 맞게 텍스트를 변경 후 반환한다.
        private string ConvertText(int id)
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
    }
}
