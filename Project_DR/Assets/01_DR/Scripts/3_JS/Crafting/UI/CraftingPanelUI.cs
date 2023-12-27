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
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            _text.text = Data.GetString(id, "Text");
        }


        /*************************************************
         *               Private Methods
         *************************************************/
        // 크래프팅에 맞게 텍스트를 변경 후 반환한다.
        private void SetText(int id)
        {
            
        }

        // 조건식
    }
}
