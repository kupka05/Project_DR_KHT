using BNG;
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
        public int ID => _id;                               // 보스 아이디
        public float HP => _hp;                             // 현재 체력
        public float MaxHP => _maxHP;                       // 최대 체력
        public float Atk => _atk;                           // 공격력
        public float Def => _def;                           // 방어력
        public float GiveEXP => _giveEXP;                   // 경험치
        public int GiveGold => _giveGold;                   // 보상 골드
        public int PatternCount => _patternCount;           // 패턴 갯수
        public float PatternInterval => _patternInterval;   // 패턴 간격
        public Rigidbody Rigidbody => _rigidBody;           // 리지드 바디
        public Damageable Damageable => _damageable;        // 데미지 관련 처리
        public Transform Target => _target;                 // 공격 대상
        public Animator Animator => _animator;              // 애니메이터


        /*************************************************
         *                 Private Fields
         *************************************************/
        [SerializeField] private int _id;
        [SerializeField] private float _hp;
        [SerializeField] private float _maxHP;
        [SerializeField] private float _atk;
        [SerializeField] private float _def;
        [SerializeField] private float _giveEXP;
        [SerializeField] private int _giveGold;
        [SerializeField] private int _patternCount;
        [SerializeField] private float _patternInterval = 2.0f;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private Damageable _damageable;
        [SerializeField] private Transform _target;
        [SerializeField] private Animator _animator;


        /*************************************************
         *                 Public Methods
         *************************************************/
        // 생성자
        public BossData(int id)
        {
            _id = id;
            _hp = Data.GetFloat(id, "HP");
            _maxHP = Data.GetFloat(id, "MaxHP");
            _atk = Data.GetFloat(id, "Atk");
            _def = Data.GetFloat(id, "Def");
            _giveEXP = Data.GetFloat(id, "GiveEXP");
            _giveGold = Data.GetInt(id, "GiveGold");
            _patternCount = Data.GetInt(id, "Stage");
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
        }
    }
}
