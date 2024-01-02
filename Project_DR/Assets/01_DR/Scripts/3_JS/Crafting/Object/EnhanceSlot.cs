using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Crafting
{
    public class EnhanceSlot : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/


        /*************************************************
         *                 Private Fields
        *************************************************/
        [SerializeField] private CraftingUI _craftingUI;


        /*************************************************
         *                  Unity Events
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
         *                 Public Methods
         *************************************************/
        public void OnSlot(bool IsDrillInserted)
        {
            // 슬롯에 드릴이 들어갔을 경우
            if (IsDrillInserted)
            {

            }

            // 슬롯에서 드릴이 빠져나왔을 경우
            else
            {

            }
        }
    }
}
