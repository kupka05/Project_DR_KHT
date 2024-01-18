using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public class EnhanceSlot : MonoBehaviour
    {
        /*************************************************
         *                 Private Fields
        *************************************************/
        [SerializeField] private CraftingUI _craftingUI;


        /*************************************************
         *                 Public Methods
         *************************************************/
        // 슬롯에 드릴이 들어올 떄 호출
        public void InSlot()
        {
            // 캔버스의 표시 상태를 초기 상태로 변경
            _craftingUI.ResetCanvasActive();
        }

        // 슬롯에 드릴이 나갈 떄 호출
        public void OutSlot()
        {
            GFunc.Log("OutSlot()");
            // 캔버스를 전부 끔
            _craftingUI.DisableAllCanvas();
        }
    }
}
