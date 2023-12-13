using System;
using System.Text;
using UnityEngine;

namespace BossMonster
{
    public class Boss : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public BossData BossData => _bossData;
        public GameObject BossStone => _bossStone;
        public BossSummoningStone BossSummoningStone => _bossSummoningStone;
        public IState CurrentState => _currentState;
        public IState IdleState => _idleState;
        public IState DieState => _dieState;
        public IState[] AttackStates => _attackStates;


        /*************************************************
         *                 Private Fields
         *************************************************/
        // 패턴에 따라 정의되는 상태
        [SerializeField] private BossData _bossData;                                // 보스 데이터
        [SerializeField] private GameObject _bossStone;                             // 보스 소환석 게임 오브젝트
        [SerializeField] private BossSummoningStone _bossSummoningStone;            // 보스 소환석 스크립트
        [SerializeField] private string _bossStoneName = "BossSummoningStone";      // 가져올 소환석 프리팹 이름
        private IState _currentState;                                               // 현재 상태
        private IState _idleState;                                                  // 대기 상태
        private IState _dieState;                                                   // 죽음 상태
        private IState[] _attackStates = new IState[10];                            // 공격 상태 패턴(0 ~ 9)[10]        


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
            _dieState = new DieState();
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

            // 보스 소환석 생성 및 Init
            CreateSummoningStone();

            // 초기 상태 변경
            _currentState = _idleState;
            // 대기 상태 진입
            _currentState.EnterState(this);
            // 상태 업데이트
            _currentState.UpdateState(this);
        }

        // 데미지 처리
        public void OnDamage(float damage)
        {
            // 소환석에 데미지 처리
            _bossSummoningStone.OnDamage(damage);
        }


        /*************************************************
         *                 State Methods
         *************************************************/
        // 현재 상태 변경
        public void ChangeState(IState state)
        { 
            // 상태 나가기
            _currentState.ExitState(this);

            // 상태 변경
            _currentState = state;

            // 상태 진입
            _currentState.EnterState(this);

            // 상태 업데이트
            _currentState.UpdateState(this);
        }


        /*************************************************
         *                 Private Methods
         *************************************************/
        // 보스 소환석 생성
        private void CreateSummoningStone()
        {
            // 프리팹에 등록된 보스 소환석 생성
            GameObject bossStonePrefab = Resources.Load<GameObject>(_bossStoneName);
            GameObject bossStone = Instantiate(bossStonePrefab, transform);
                // 디버그용
                Vector3 position = new Vector3(0f, 1.013f, 4.42f);
                bossStone.transform.position = position;
                // 디버그용
            bossStone.name = _bossStoneName;
            _bossStone = bossStone;

            // 보스 소환석 Init
            _bossSummoningStone = bossStone.AddComponent<BossSummoningStone>();
            _bossSummoningStone.Initialize(_bossData);
        }
    }
}
