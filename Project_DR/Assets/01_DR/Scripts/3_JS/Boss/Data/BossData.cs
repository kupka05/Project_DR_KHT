using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMonster
{
    [System.Serializable]
    public class BossData
    {
        /*************************************************
         *                  Public Fields
         *************************************************/
        // 기본 스탯
        public Boss Boss => _boss;                                                      // 보스
        public int ID => _id;                                                           // 보스 아이디
        public int BossType => _bossType;                                               // 보스 타입
        public float HP => _hp;                                                         // 현재 체력
        public float MaxHP => _maxHP;                                                   // 최대 체력
        public float Atk => _atk;                                                       // 공격력
        public float Def => _def;                                                       // 방어력
        public float GiveEXP => _giveEXP;                                               // 경험치
        public int GiveGold => _giveGold;                                               // 보상 골드
        public float PatternInterval => _patternInterval;                               // 패턴 간격
        public int AttackAnimationRange => _attackAnimationRange;                       // 공격 애니메이션 범위
        public Rigidbody Rigidbody => _rigidBody;                                       // 리지드 바디
        public Damageable Damageable => _damageable;                                    // 데미지 관련 처리
        public Transform Target => _target;                                             // 공격 대상
        public Animator Animator => _animator;                                          // 애니메이터
        public int[] PhaseAttackPatternCounts => _phaseAttackPatternCounts;             // 각 페이즈 별 공격 패턴 갯수
        public int CurrentPatternCount => _currentPatternCount;                         // 현재 패턴 갯수      
        public List<int> AvailableAttackPatternsList => _availableAttackPatternsList;   // 사용 가능한 공격 패턴들
        public bool IsDead => _isDead;                                                  // 죽음 여부


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private Boss _boss;
        [SerializeField] private int _id;
        [SerializeField] private int _bossType;
        [SerializeField] private float _hp;
        [SerializeField] private float _maxHP;
        [SerializeField] private float _atk;
        [SerializeField] private float _def;
        [SerializeField] private float _giveEXP;
        [SerializeField] private int _giveGold;
        [SerializeField] private float _patternInterval = 3.0f;                 // 3.0f는 오류 대비 값, 실제 값은 GetData로 가져온다.
        [SerializeField] private int _attackAnimationRange;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private Damageable _damageable;
        [SerializeField] private Transform _target;
        [SerializeField] private Animator _animator;
        [SerializeField] private int[] _phaseAttackPatternCounts = new int[4];  // 페이즈는 4개로 고정.
        [SerializeField] private int _currentPatternCount;
        [SerializeField] private List<int> _availableAttackPatternsList = new List<int>();
        [SerializeField] private bool _isDead;


        /*************************************************
         *                 Public Methods
         *************************************************/
        // 생성자
        public BossData(Boss boss, int id)
        {
            _boss = boss;
            _id = id;
            _bossType = Data.GetInt(id, "Stage");
            _hp = Data.GetFloat(id, "HP");
            _maxHP = Data.GetFloat(id, "MaxHP");
            _atk = Data.GetFloat(id, "Atk");
            _def = Data.GetFloat(id, "Def");
            _giveGold = Data.GetInt(id, "GiveGold");
            _giveEXP = Data.GetFloat(id, "GiveEXP");
            _attackAnimationRange = Data.GetInt(id, "AttackAnimationRange");
            _patternInterval = Data.GetFloat(id, "AttackPatternInterval");
            for (int i = 0; i < _phaseAttackPatternCounts.Length; i++)
            {
                int index = i + 1;
                string category = GFunc.SumString("Phase", index.ToString(), "AttackPatternCount");
                _phaseAttackPatternCounts[i] = Data.GetInt(id, category);
            }

            // 현재 페이즈 카운트를 1 페이즈로 설정
            _currentPatternCount = _phaseAttackPatternCounts[0];
        }

        // 리지드 바디 할당
        public void SetRigidbody(Rigidbody rigidBody)
        {
            _rigidBody = rigidBody;
        }

        // Damageable 할당
        public void SetDamageable(Damageable damageable)
        {
            _damageable = damageable;
        }

        // 공격 대상 할당
        public void SetTarget(Transform target)
        {
            _target = target;
        }

        // 애니메이터 할당
        public void SetAnimator(Animator animator)
        {
            _animator = animator;
        }

        // 데미지 처리
        public void OnDamage(float damage)
        {
            _hp -= damage;
            if (_hp <= 0)
            {
                _isDead = true;
            }
        }

        // 사용 가능한 공격 패턴 초기화
        public void ResetAvailableAttackPatterns()
        {
            _availableAttackPatternsList.Clear();
        }

        // 현재 공격 패턴 갯수 변경
        public void SetCurrentPatternCount(int index)
        {
            _currentPatternCount = _phaseAttackPatternCounts[index];
        }

        // 랜덤으로 사용 가능한 공격 패턴을 설정
        public void ChooseRandomPattern()
        {
            // 사용 가능한 공격 패턴 초기화
            _availableAttackPatternsList.Clear();

            int patternCount = _currentPatternCount;
            GFunc.Log($"patternCount = {patternCount}");
            int i = 0;
            while (i < patternCount)
            {
                int randomNumber = UnityEngine.Random.Range(0, _boss.AttackStates.Length);

                // 패턴 중복 체크
                if (! _availableAttackPatternsList.Contains(randomNumber))
                {
                    // 중복이 아닐 경우 추가
                    _availableAttackPatternsList.Add(randomNumber);

                    i++;
                }

                // 중복일 경우
                else { continue; }
            }
        }
    }
}
