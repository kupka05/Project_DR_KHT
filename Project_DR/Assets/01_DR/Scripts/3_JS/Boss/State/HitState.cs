using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMonster
{
    public class HitState : IState
    {
        // 상태 진입시
        public void EnterState(Boss boss)
        {
            GFunc.Log("피격상태 진입 ");
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
