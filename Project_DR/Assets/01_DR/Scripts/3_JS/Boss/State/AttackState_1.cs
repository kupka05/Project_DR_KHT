using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class AttackState_1 : IState
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public int ID => _id;
        public Boss Boss => _boss;
        public BossData BossData => _bossData;
        public AttackStateData_1 StateData => _stateData;


        /*************************************************
         *                 Public Fields
         *************************************************/
        private int _id;
        private Boss _boss;
        private BossData _bossData;
        private AttackStateData_1 _stateData;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자
        public AttackState_1(int id, Boss boss)
        {
            // 보스 및 보스 데이터 참조
            _boss = boss;
            _bossData = _boss.BossData;

            // 공격 패턴 (1)에 해당하는 구글 시트 ID 할당
            _id = id;

            // 상태 정보 객체 생성 및 초기화
            _stateData = new AttackStateData_1(_id, _bossData);
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        // 상태 진입시
        public void EnterState()
        {
            GFunc.Log("공격 상태 패턴 1 레이저 재생");
            // 공격 패턴 1 레이저 재생
            Old_Boss oldBoss = _boss.OldBoss;
            oldBoss.StartCoroutine(oldBoss.LazerCoroutine());
        }

        // 상태 업데이트시
        public void UpdateState()
        {
            GFunc.Log("공격 상태 패턴 1 업데이트");
        }

        // 상태에서 나갈시
        public void ExitState()
        {
            GFunc.Log("공격 상태 패턴 1 나가기");
        }
    }
}
