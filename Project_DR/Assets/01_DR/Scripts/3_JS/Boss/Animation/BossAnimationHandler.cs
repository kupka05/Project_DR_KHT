using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class BossAnimationHandler
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public enum Type
        {
            DEFAULT = 0,                                    
            MELONG = 1,     // 멜롱
            DRA = 2,        // 드라
            BOHEON = 3,     // 보헌
            REAPER = 4,     // 리퍼
            DVUSHI = 5      // 드뷔시
        }
        public Boss Boss => _boss;                                              // 보스
        public BossData BossData => _bossData;                                  // 보스 데이터
        public Transform Target => _bossData.Target;                            // 공격 대상
        public Type BossType => _bossType;                                      // 보스 타입
        public int AttackAnimationRange => _bossData.AttackAnimationRange;      // 공격 애니메이션 범위


        /*************************************************
         *                Private Fields
         *************************************************/
        private Boss _boss;
        private BossData _bossData;
        private Animator _animator;
        private Type _bossType;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자
        public BossAnimationHandler(Boss boss)
        {
            // Init
            _bossData = boss.BossData;
            _animator = boss.Animator;
            _bossType = (Type)_bossData.BossType;
        }


        /*************************************************
         *              Animation Methods
         *************************************************/
        // 죽음 애니메이션
        public void DieAnimation()
        {
            GFunc.Log("DieAnimation()");
            _animator.SetTrigger("isDie");
        }

        // 주문 시전 애니메이션
        public void CastSpellAnimation()
        {
            GFunc.Log("CastSpellAnimation()");
            _animator.SetTrigger("isCast");
        }

        // 공격 애니메이션
        public void AttackAnimation()
        {
            int randomNum = UnityEngine.Random.Range(1, AttackAnimationRange + 1);
            string animationType = randomNum == 1 ? "" : randomNum.ToString();
            _animator.SetTrigger(GFunc.SumString("isAttack", animationType));
            GFunc.Log($"AttackAnimation() {randomNum}");
        }


        /*************************************************
         *               Private Methods
         *************************************************/
    }
}
