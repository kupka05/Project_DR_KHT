using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Quest
{
    public class QuestState : ScriptableObject
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public enum StateQuest
        {
            NOT_STARTABLE = 0,      // 시작불가
            CAN_STARTABLE = 1,      // 시작가능
            IN_PROGRESS = 2,        // 진행중
            CAN_COMPLETE = 3,       // 완료가능
            COMPLETED = 4,          // 완료
            FAILED = 5              // 실패
        }
        public StateQuest State => _state;
        public IState[] States => _states;                // 퀘스트 상태([0]=시작불가, [1]=시작가능, [2]=진행중, [3]=완료가능, [4]=완료, [5]=실패)
        public IState CurrentState => _currentState;      // 현재 상태
        public QuestData QuestData => _quest.QuestData;   // 퀘스트 데이터


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private Quest _quest;
        [SerializeField] private StateQuest _state;
        [SerializeField] private IState[] _states = new IState[6];
        [SerializeField] private IState _currentState;


        /*************************************************
         *                 Public Methods
         *************************************************/
        public QuestState(Quest quest)
        {
            // Init
            _quest = quest;
            _states[0] = new NotStartable();    // [시작불가]
            _states[1] = new CanStartable();    // [시작가능]
            _states[2] = new InProgress();      // [진행중]
            _states[3] = new CanComplete();     // [완료가능]
            _states[4] = new Completed();       // [완료]
            _states[5] = new Failed();          // [실패]

            // 현재 상태를 [시작불가] 상태로 변경
            _state = StateQuest.NOT_STARTABLE;
            _currentState = _states[0];

            // 현재 퀘스트가 메인 퀘스트일 경우
            if (QuestData.Type.Equals(QuestData.QuestType.MAIN))
            {
                // TODO: 메인 퀘스트의 경우 플레이어의 퀘스트 데이터를 불러와서
                // 해당 상태로 변경해야 함
            }

            // 아닐 경우(서브 퀘스트일 경우)
            else
            {
                // 현재 상태를 다음 단계인 [시작가능]으로 변경
                // 단 시작가능 조건에 해당해야 한다.
                ChangeToNextState();
            }

            // 현재 상태 출력
            _currentState.PrintCurrentState();
        }

        // 현재 상태를 다음 상태로 변경
        public void ChangeToNextState()
        {
            _currentState.ChangeToNextState(_quest);
        }

        // 현재 상태를 변경
        public void ChangeState(StateQuest state)
        {
            _state = state;
            _currentState = _states[(int)state];
        }

        // 현재 상태를 출력
        public void PrintCurrentState()
        {
            _currentState.PrintCurrentState();
        }
    }
}

