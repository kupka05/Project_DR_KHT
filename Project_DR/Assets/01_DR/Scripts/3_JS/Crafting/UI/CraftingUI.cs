using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public class CraftingUI : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public CraftingCanvas[] CraftingCanvases => _craftingCanvases;      // 크래프팅 캔버스[]


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField]private CraftingCanvas[] _craftingCanvases;


        /*************************************************
         *                  Unity Events
         *************************************************/
        void Start()
        {
            //Init
            Initialize();
        }


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize()
        {
            // Init
            for (int i = 0; i < _craftingCanvases.Length; i++)
            {
                CraftingCanvas.Type type = (CraftingCanvas.Type) i;
                _craftingCanvases[i].Initialize(this, type);
            }
        }

        // 캔버스 기본 상태로 표시 변경
        public void ResetCanvasActive()
        {
            _craftingCanvases[0].Parent.gameObject.SetActive(true);
            _craftingCanvases[1].Parent.gameObject.SetActive(false);
        }

        // 강화 캔버스 토글
        public void ToggleAllCanvas(int id)
        {
            for (int i = 0; i < _craftingCanvases.Length; i++)
            {
                GameObject canvas = _craftingCanvases[i].Parent.gameObject;
                canvas.SetActive(!canvas.activeSelf);
                _craftingCanvases[i].SetEnhanceID(id);
            }
        }

        // 모든 캔버스 비활성화
        public void DisableAllCanvas()
        {
            for (int i = 0; i < _craftingCanvases.Length; i++)
            {
                GameObject canvas = _craftingCanvases[i].Parent.gameObject;
                canvas.SetActive(false);
                GFunc.Log($"{canvas.name} {canvas.activeSelf}");
            }
        }
    }
}
