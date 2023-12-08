using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BossMonster
{
    public class Boss : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public IState CurrentState => _currentState;
        public IState IdleState => _idleState;
        public IState HitState => _hitState;
        public IState DefendState => _defendState;
        public IState[] AttackStates => _attackStates;


        /*************************************************
         *                 Private Fields
         *************************************************/
        // 패턴에 따라 정의되는 상태
        [SerializeField] private BossData _bossData;     // 보스 데이터
        private IState _currentState;                    // 현재 상태
        private IState _idleState;                       // 대기 상태
        private IState _hitState;                        // 피격 상태
        private IState _defendState;                     // 방어 상태
        private IState[] _attackStates = new IState[10]; // 공격 상태 패턴(0 ~ 9)[10]        


        /*************************************************
         *                 Public Methods
         *************************************************/
        // Init
        public void Initialize(int id)
        {
            // 보스 데이터 가져옴
            _bossData = new BossData(id);
            StringBuilder stringBuilder = new StringBuilder();
            // 상태 초기화
            _idleState = new IdleState();
            _hitState = new HitState();
            _defendState = new DefendState();
            for (int i = 0; i < _attackStates.Length; i++)
            {
                // 타입을 찾을 때 네임스페이스명 + 찾을 타입명으로 검색해야 함
                // 연산을 최소화 하기 위해 string 대신 StringBuilder 사용
                stringBuilder.Clear();
                stringBuilder.Append("BossMonster.AttackState_");
                stringBuilder.Append(i);
                //string type = "BossMonster.AttackState_" + i;     //Legacy:
                // 타입 검색
                Type attackStateType = Type.GetType(stringBuilder.ToString());
                // 타입이 있을 경우
                if (attackStateType != null)
                {
                    // _인스턴스를 생성하여 _attackStates 배열에
                    // 할당 & 생성자 호출
                    _attackStates[i] = (IState)Activator.CreateInstance(attackStateType, id, _bossData);
                }

                // 없을 경우
                else
                {
                    Debug.LogWarning($"BossMonster.Boss.Initialize(): {stringBuilder} 타입을 찾을 수 없습니다.");
                }
            }

            // 초기 상태 지정
            _currentState = _idleState;
            // 대기 상태 진입
            _currentState.EnterState(this);
        }
    }

}
