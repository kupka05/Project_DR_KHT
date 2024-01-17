using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class BossNPCMeet : MonoBehaviour
    {
        /*************************************************
         *                Private Fields
         *************************************************/
        [SerializeField] private BossNPC _bossNPC;
        [SerializeField] private bool isMeet = false;


        /*************************************************
         *                 Unity Events
         *************************************************/
        private void OnTriggerEnter(Collider other)
        {
            // 태그가 플레이어 & isMeet이 False일 경우
            if (other.CompareTag("Player") && ! isMeet)
            {
                isMeet = true;
                GFunc.Log("BossNPCMeet.Trigger()");
                _bossNPC.BossMeet();
            }
        }


        /*************************************************
         *                Public Methods
         *************************************************/
        public void Initialize(BossNPC bossNPC)
        {
            // Init
            _bossNPC = bossNPC;
            Vector3 position = Vector3.zero;
            position.z = 79.0f;
            transform.position = position;
        }
    }
}
