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
        /// <summary>
        /// [시작불가] = 0 <br></br>
        /// [시작가능] = 1 <br></br>
        /// [진행중] = 2 <br></br>
        /// [완료가능] = 3 <br></br>
        /// [완료] = 4 <br></br>
        /// [실패] = 5 <br></br>
        /// </summary>
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
        public IState[] States => _states;                // 퀘스트 상태{[0]=시작불가, [1]=시작가능, [2]=진행중, [3]=완료가능, [4]=완료, [5]=실패}
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
            ChangeState(StateQuest.NOT_STARTABLE);

            // 선행 퀘스트를 다음 상태인 [시작가능]으로 진행
            // 조건 미충족(선행퀘스트 미완료)시 전환 불가
            //ChangeToNextState();

            // TODO: 
            // 1. DB에서 퀘스트 데이터를 가져온 후 호출 -> 콜백 연동해야 됨
            // [완료] 2. 퀘스트가 완료 될 때마다 한번씩 호출하게 한다. -> 퀘스트가 완료될 떄 콜백 연동

            // 현재 상태 출력
            //_currentState.PrintCurrentState();
        }

        // 현재 상태를 다음 상태로 변경
        public void ChangeToNextState()
        {
#if UNITY_EDITOR
            // 디버그
            _currentState = _states[(int)_state];
            // 디버그
#endif
            _currentState.ChangeToNextState(_quest, this);
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

        // 퀘스트가 [시작가능] 상태 조건을 충족하는지 체크
        public bool CheckStateForCanStartable()
        {
            // [시작가능] 상태 조건을 충족할 경우
            // 선행 퀘스트가 있을 경우
            int requiredQuestID = _quest.QuestData.RequiredQuestID;
            if (requiredQuestID.Equals(0).Equals(false))
            {
                // 선행 퀘스트의 상태가 [완료]일 경우
                if (UserDataManager.QuestDictionary[requiredQuestID].
                    QuestState.State.Equals(StateQuest.COMPLETED))
                {
                    return true;
                }
            }

            // 선행 퀘스트가 없을 경우
            else
            {
                return true;
            }

            // [시작가능] 상태 조건을 충족하지 못할 경우
            return false;
        }
    }
}

