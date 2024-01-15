using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class IdleState : IState
    {
        // 상태 진입시
        public void EnterState()
        {
            GFunc.Log("대기상태진입");
        }

        // 상태 업데이트시
        public void UpdateState()
        {
            GFunc.Log("대기상태업데이트");
        }

        // 상태에서 나갈시
        public void ExitState()
        {
            GFunc.Log("대기상태나가기");
        }
    }
}
