using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    public interface IState
    {
        // 현재 상태를 출력
        void PrintCurrentState();

        // 다음 상태로 변경(시작불가 -> 시작가능 -> 진행중 -> 완료 가능 -> 완료)
        void ChangeToNextState(Quest quest, QuestState questState);
    }
}
