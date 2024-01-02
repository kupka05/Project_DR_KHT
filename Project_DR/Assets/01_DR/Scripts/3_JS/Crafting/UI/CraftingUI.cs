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

        // 모든 캔버스 토글
        public void ToggleAllCanvas(int id)
        {
            for (int i = 0; i < _craftingCanvases.Length; i++)
            {
                GameObject canvas = _craftingCanvases[i].Parent.gameObject;
                canvas.SetActive(!canvas.activeSelf);
                _craftingCanvases[i].SetEnhanceID(id);
            }
        }
    }
}
