using BNG;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Js.Boss
{
    public class Boss : MonoBehaviour
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public BossMonster BossMonster => _bossMonster;
        public Old_Boss OldBoss => _bossSummoningStone.OldBoss;
        public BossData BossData => _bossData;
        public GameObject BossStone => _bossStone;
        public BossSummoningStone BossSummoningStone => _bossSummoningStone;
        public BossAnimationHandler BossAnimationHandler => _bossAnimationHandler;
        public IState CurrentState => _currentState;
        public IState IdleState => _idleState;
        public IState StopState => _stopState;
        public IState[] AttackStates => _attackStates;
        public List<int> AvailableAttackPatternsList => _bossData.AvailableAttackPatternsList;  // 사용 가능한 공격 패턴(0 ~ 9)[10]
        public Rigidbody Rigidbody => _bossData.Rigidbody;                                      // 리지드 바디
        public Damageable Damageable => _bossData.Damageable;                                   // 데미지 관련 처리
        public Transform Target => _bossData.Target;                                            // 공격 대상
        public Animator Animator => _bossData.Animator;                                         // 애니메이터


        /*************************************************
         *                 Private Fields
         *************************************************/
        // 보스 관련
        [SerializeField] private BossMonster _bossMonster;                          // 보스 몬스터
        [SerializeField] private BossData _bossData;                                // 보스 데이터
        [SerializeField] private GameObject _bossStone;                             // 보스 소환석 게임 오브젝트
        [SerializeField] private BossSummoningStone _bossSummoningStone;            // 보스 소환석 스크립트
        [SerializeField] private BossAnimationHandler _bossAnimationHandler;        // 보스 애니메이션 핸들러

        // 패턴에 따라 정의되는 상태
        private IState _currentState;                                               // 현재 상태
        private IState _idleState;                                                  // 대기 상태
        private IState _stopState;                                                  // 정지 상태
        private IState[] _attackStates = new IState[4];                             // 공격 상태 패턴(0 ~ 3)[4]


        /*************************************************
         *                 Public Methods
         *************************************************/
        // Init
        public void Initialize(BossMonster bossMonster, int id)
        {
            // 보스 관련 데이터 할당
            _bossMonster = bossMonster;
            _bossData = new BossData(this, id);                         // 보스 데이터 생성
            _bossData.SetRigidbody(GetComponent<Rigidbody>());          // 리지드 바디 할당 
            _bossData.SetTarget(FindTarget("Player"));                  // 플레이어를 타겟으로 설정
            _bossData.SetAnimator(GetComponent<Animator>());            // 애니메이터 할당
            _bossAnimationHandler = new BossAnimationHandler(this);     // 보스 애니메이션 핸들러 생성

            // 상태 초기화 및 할당
            _idleState = new IdleState();
            _stopState = new StopState();
            SetAttackStates(id);

            // 초기 상태 변경
            _currentState = _idleState;
            // 대기 상태 진입
            _currentState.EnterState();
            // 상태 업데이트
            _currentState.UpdateState();

            // 보스 소환석 생성 및 Init
            CreateSummoningStone();

            // 데미지 관련 처리 할당 및 OnDamaged 이벤트 추가
            _bossData.SetDamageable(_bossSummoningStone.Damageable);
            _bossData.Damageable.onDamaged = new FloatEvent();
            _bossData.Damageable.onDamaged.AddListener(OnDamage);

            // 공격 패턴 랜덤 설정
            _bossData.ChooseRandomPattern();
        }

        // 보스의 공격 패턴 상태를 실행
        public void DOAttackPattern(int index)
        {
            // 인덱스로 받은 공격 패턴을
            ChangeState(_attackStates[index]);
        }

        // 데미지 처리
        public void OnDamage(float damage)
        {
            // 피격 효과음 재생
            AudioManager.Instance.AddSFX("poly_explosion_ice");
            AudioManager.Instance.PlaySFX("poly_explosion_ice");

            // 소환석에 데미지 처리
            _bossSummoningStone.OnDamage(OldBoss.OtherOnDeal(damage));
            GFunc.Log(damage + ": 보스 데미지");
        }

        // 보스 오브젝트 삭제
        public void Dead()
        {
            // 죽음 처리
            _bossData.SetIsDead(true);

            // 정지 상태로 변경
            _currentState = StopState;

            // 죽음 애니메이션 재생
            _bossAnimationHandler.DieAnimation();

            // 3초 후 오브젝트 삭제
            Destroy(gameObject, _bossData.DestroyDelay);

            // 죽음 효과음 출력
            AudioManager.Instance.AddSFX(_bossData.DieSFXName);
            AudioManager.Instance.PlaySFX(_bossData.DieSFXName);

            // 배경음악 변경
            AudioManager.Instance.AddBGM("BGM_Stage_InStage");
            AudioManager.Instance.PlayBGM("BGM_Stage_InStage");
        }

        // NPC 트리거 설정
        public void SetNPCTrigger()
        {
            Transform npcTrigger = transform.FindChildRecursive("GameStart");
            npcTrigger?.gameObject?.AddComponent<BossNPCMeet>()
                ?.Initialize(_bossSummoningStone.BossNPC);
        }


        /*************************************************
         *                 Unity Methods
         *************************************************/
        // 초당 60프레임 간격으로 고정해서 실행
        private void FixedUpdate()
        {
            // 공격 대상으로 LookAt
            LookAtTarget(Target);
        }


        /*************************************************
         *                 State Methods
         *************************************************/
        // 현재 상태 변경
        public void ChangeState(IState state)
        {
            if (_currentState != null)
            {
                // 상태 나가기
                _currentState.ExitState();
            }

            // 상태 변경
            _currentState = state;

            // 상태 진입
            _currentState.EnterState();

            // 상태 업데이트
            _currentState.UpdateState();
        }


        /*************************************************
         *                 Private Methods
         *************************************************/
        // 공격 상태 패턴들을 할당
        private void SetAttackStates(int id)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _attackStates.Length; i++)
            {
                // 타입을 찾을 때 네임스페이스명 + 찾을 타입명으로 검색해야 함
                // 연산을 최소화 하기 위해 string 대신 StringBuilder 사용
                stringBuilder.Clear();
                stringBuilder.Append("Js.Boss.AttackState_");
                stringBuilder.Append(i);
                // 타입 검색
                Type attackStateType = Type.GetType(stringBuilder.ToString());
                // 타입이 있을 경우
                if (attackStateType != null)
                {
                    // _인스턴스를 생성하여 _attackStates 배열에
                    // 할당 & 생성자 호출
                    _attackStates[i] = (IState)Activator.CreateInstance(attackStateType, id, this);
                }

                // 없을 경우
                else
                {
                    GFunc.LogWarning($"BossMonster.Boss.Initialize(): {stringBuilder} 타입을 찾을 수 없습니다.");
                }
            }
        }

        // 보스 소환석 생성
        private void CreateSummoningStone()
        {
            string stonePrefabName = _bossData.StonePrefabName;
            // 프리팹에 등록된 보스 소환석 생성
            GameObject bossStonePrefab = Resources.Load<GameObject>(stonePrefabName);
            if (bossStonePrefab != null)
            {
                GameObject bossStone = Instantiate(bossStonePrefab);
                bossStone.name = stonePrefabName;
                _bossStone = bossStone;
                // 보스 소환석 Init
                // 기존 AddComponent에서 GetComponent로 변경함
                // 사유: 단시간 내에 기존 boss와 결합하기 위함
                _bossSummoningStone = bossStone.GetComponent<BossSummoningStone>();
                _bossSummoningStone.Initialize(this);
                _bossSummoningStone.SetParentAndPosition(transform);
            }

            // 프리팹이 없을 경우
            else
            {
                GFunc.Log($"Boss.CreateSummoningStone(): Prefab[{stonePrefabName}]을 가져올 수 없습니다.");
            }
        }

        // 공격 대상 검색
        private Transform FindTarget(string targetName)
        {
            GameObject target = GameObject.FindWithTag(targetName);
            if (target != null)
            {
                // 타겟을 찾았을 경우
                if (target.GetComponent<PlayerPosition>() != null)
                {
                    Transform targetTransform = target.GetComponent<PlayerPosition>().playerPos;
                    return targetTransform;
                }
            }

            // 타겟을 못 찾았을 경우
            return default;
        }

        // 공격 대상으로 LookAt
        public void LookAtTarget(Transform target)
        {
            // 정지 상태일 경우 예외 처리
            if (CurrentState.Equals(StopState)) { return; }

            if (target != null)
            {
                // Look At Y 각도로만 기울어지게 하기
                Vector3 targetPosition =
                    new Vector3(target.position.x, transform.position.y, target.position.z);
                transform.LookAt(targetPosition);
            }
        }

        // 오브젝트 숨기기
        public void HideObject()
        {
            gameObject.transform.localScale = Vector3.zero;
        }
    }
}
