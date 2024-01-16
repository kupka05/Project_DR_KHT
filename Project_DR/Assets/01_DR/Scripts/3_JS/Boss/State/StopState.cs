using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class StopState : IState
    {
        // 상태 진입시
        public void EnterState()
        {
            GFunc.Log("정지상태 진입 ");
        }

        // 상태 업데이트시
        public void UpdateState()
        {

        }

        // 상태에서 나갈시
        public void ExitState()
        {

        }
    }
}
