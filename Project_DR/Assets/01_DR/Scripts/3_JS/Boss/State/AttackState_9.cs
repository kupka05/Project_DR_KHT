using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class AttackState_9 : IState
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public int ID => _id;
        public Boss Boss => _boss;
        public BossData BossData => _bossData;
        public AttackStateData_9 StateData => _stateData;


        /*************************************************
         *                 Public Fields
         *************************************************/
        private int _id;
        private Boss _boss;
        private BossData _bossData;
        private AttackStateData_9 _stateData;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자
        public AttackState_9(int id, Boss boss)
        {
            // 보스 및 보스 데이터 참조
            _boss = boss;
            _bossData = _boss.BossData;

            // 공격 패턴 (9)에 해당하는 구글 시트 ID 할당
            _id = id;

            // 상태 정보 객체 생성 및 초기화
            _stateData = new AttackStateData_9(_id, _bossData);
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        // 상태 진입시
        public void EnterState()
        {
            GFunc.Log("공격 상태 패턴 9 진입");
        }

        // 상태 업데이트시
        public void UpdateState()
        {
            GFunc.Log("공격 상태 패턴 9 업데이트");
        }

        // 상태에서 나갈시
        public void ExitState()
        {
            GFunc.Log("공격 상태 패턴 9 나가기");
        }
    }
}
