using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class CloneBossSummoningStone : MonoBehaviour
    {
        /*************************************************
         *                Public Fields
         *************************************************/
        private BossSummoningStone BossSummoningStone => _boss.BossSummoningStone;   // 최상위 소환석


        /*************************************************
         *                Private Fields
         *************************************************/
        private Boss _boss;                                                          // 보스 참조


        /*************************************************
         *                 Unity Events
         *************************************************/
        private void Start()
        {

        }


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize(Boss boss)
        {
            // Init
            _boss = boss;
        }


        /*************************************************
         *                Private Methods
         *************************************************/

    }
}
