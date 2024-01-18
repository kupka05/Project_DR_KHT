using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class BossRoomPillar : MonoBehaviour
    {
        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private List<GameObject> _bossRoomPillars =
            GameManager.instance.bossRoomPillars;


        /*************************************************
         *                  Unity Events
         *************************************************/
        private void Start()
        {
            // Init
            Initialize();
        }


        /*************************************************
         *                 Private Methods
         *************************************************/
        private void Initialize()
        {
            // Init
            GameManager.instance.bossRoomPillars.Add(gameObject);
        }
    }
}
