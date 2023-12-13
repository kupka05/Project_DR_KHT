using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMonster
{
    public class AttackState_8 : IState
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public int ID => _id;
        public BossData BossData => _bossData;
        public AttackStateData_8 StateData => _stateData;


        /*************************************************
         *                 Public Fields
         *************************************************/
        private int _id;
        private BossData _bossData;
        private AttackStateData_8 _stateData;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자
        public AttackState_8(int id, BossData bossData)
        {
            // 보스 정보 참조
            _bossData = bossData;

            // 공격 패턴 (1)에 해당하는 구글 시트 ID 할당
            _id = id;

            // 상태 정보 객체 생성 및 초기화
            _stateData = new AttackStateData_8(_id, bossData);
        }


        /*************************************************
         *               Interface Methods
         *************************************************/
        // 상태 진입시
        public void EnterState(Boss boss)
        {
        }

        // 상태 업데이트시
        public void UpdateState(Boss boss)
        {

        }

        // 상태에서 나갈시
        public void ExitState(Boss boss)
        {

        }
    }
}
