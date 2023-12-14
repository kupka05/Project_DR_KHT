using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMonster
{
    public class BossMonster : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public Boss Boss => _boss;


        /*************************************************
         *                 Private Fields
          *************************************************/
        [SerializeField] private int _id = default;     // 인스펙터에서 ID를 할당해야 합니다.
        private Boss _boss;


        /*************************************************
         *                 Unity Events
         *************************************************/
        void Start()
        {
            // _boss 생성 및 초기화
            _boss = gameObject.AddComponent<Boss>();
            _boss.Initialize(_id);

            Invoke("AttackAnimation", 3f);
        }


        /*************************************************
         *               Private Methods
         *************************************************/
        private void AttackAnimation()
        {
            _boss.BossAnimationHandler.AttackAnimation();
            Invoke("AttackAnimation", 3f);
        }


        /*************************************************
         *                 Coroutines
         *************************************************/


    }
}
