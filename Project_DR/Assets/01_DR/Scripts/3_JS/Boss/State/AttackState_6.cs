using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class AttackState_6 : IState
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public int ID => _id;
        public Boss Boss => _boss;
        public BossData BossData => _bossData;
        public AttackStateData_6 StateData => _stateData;


        /*************************************************
         *                 Public Fields
         *************************************************/
        private int _id;
        private Boss _boss;
        private BossData _bossData;
        private AttackStateData_6 _stateData;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자
        public AttackState_6(int id, Boss boss)
        {
            // 보스 및 보스 데이터 참조
            _boss = boss;
            _bossData = _boss.BossData;

            // 공격 패턴 (6)에 해당하는 구글 시트 ID 할당
            _id = id;

            // 상태 정보 객체 생성 및 초기화
            _stateData = new AttackStateData_6(_id, _bossData);
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        // 상태 진입시
        public void EnterState()
        {
            GFunc.Log("공격 상태 패턴 6 진입");
        }

        // 상태 업데이트시
        public void UpdateState()
        {
            GFunc.Log("공격 상태 패턴 6 업데이트");
        }

        // 상태에서 나갈시
        public void ExitState()
        {
            GFunc.Log("공격 상태 패턴 6 나가기");
        }
    }
}
