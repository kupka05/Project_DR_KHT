using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossMonster
{
    public class BossAnimationHandler
    {
        /*************************************************
         *                 Public Fields
         *************************************************/
        public enum Type
        {
            DEFAULT = 0,                                    // 기본 타입
            MELONG = 1,                                     // 멜롱
            DRA = 2,                                        // 드라
            BOHEON = 3,                                     // 보헌
            REAPER = 4,                                     // 리퍼
            DVUSHI = 5                                      // 드뷔시
        }
        public Boss Boss => _boss;                          // 보스
        public BossData BossData => _bossData;              // 보스 데이터
        public Transform Target => _bossData.Target;        // 공격 대상
        public Type BossType => _bossType;                  // 보스 타입


        /*************************************************
         *                Private Fields
         *************************************************/
        private Boss _boss;
        private BossData _bossData;
        private Animator _animator;
        private Type _bossType = Type.DEFAULT;


        /*************************************************
         *                Public Methods
         *************************************************/
        // 생성자
        public BossAnimationHandler(Boss boss)
        {
            // Init
            _bossData = boss.BossData;
            _animator = boss.Animator;
            int bossType = (int)DataManager.instance.GetData(_bossData.ID, "Stage", typeof(int));
            _bossType = (Type)bossType;
        }

        /*************************************************
         *              Animation Methods
         *************************************************/
        // 죽음 애니메이션
        public void DieAnimation()
        {
            Debug.Log("DieAnimation()");
            _animator.SetTrigger("isDie");
        }

        // 주문 시전 애니메이션
        public void CastSpellAnimation()
        {
            Debug.Log("CastSpellAnimation()");
            _animator.SetTrigger("isCast");
        }

        // 공격 애니메이션
        public void AttackAnimation()
        {
            //switch (bossType)
            //{
            //    case Type.DEATH_MAGE:
            //        int deathMage = UnityEngine.Random.Range(0, 5);
            //        switch (deathMage)
            //        {
            //            case 0:
            //                anim.SetTrigger("isAttack");
            //                break;
            //            case 1:
            //                anim.SetTrigger("isAttack2");
            //                break;
            //            case 2:
            //                anim.SetTrigger("isAttack3");
            //                break;
            //            case 3:
            //                anim.SetTrigger("isAttack4");
            //                break;
            //            case 4:
            //                anim.SetTrigger("isAttack5");
            //                break;
            //        }
            //        break;

            //    case Type.BAT_LORD:
            //        int batLord = UnityEngine.Random.Range(0, 4);
            //        switch (batLord)
            //        {
            //            case 0:
            //                anim.SetTrigger("isAttack");
            //                break;
            //            case 1:
            //                anim.SetTrigger("isAttack2");
            //                break;
            //            case 2:
            //                anim.SetTrigger("isAttack3");
            //                break;
            //            case 3:
            //                anim.SetTrigger("isAttack4");
            //                break;
            //        }
            //        break;

            //    case Type.GOLEM_ICE:
            //        int golemIce = UnityEngine.Random.Range(0, 5);
            //        switch (golemIce)
            //        {
            //            case 0:
            //                anim.SetTrigger("isAttack");
            //                break;
            //            case 1:
            //                anim.SetTrigger("isAttack2");
            //                break;
            //            case 2:
            //                anim.SetTrigger("isAttack3");
            //                break;
            //            case 3:
            //                anim.SetTrigger("isAttack4");
            //                break;
            //            case 4:
            //                anim.SetTrigger("isAttack5");
            //                break;
            //        }
            //        break;

            //    case Type.SHADOW:
            //        int shadow = UnityEngine.Random.Range(0, 3);
            //        switch (shadow)
            //        {
            //            case 0:
            //                anim.SetTrigger("isAttack");
            //                break;
            //            case 1:
            //                anim.SetTrigger("isAttack2");
            //                break;
            //            case 2:
            //                anim.SetTrigger("isAttack3");
            //                break;
            //        }
            //        break;

            //    case Type.WRAITH:
            //        int wraith = UnityEngine.Random.Range(0, 4);
            //        switch (wraith)
            //        {
            //            case 0:
            //                anim.SetTrigger("isAttack");
            //                break;
            //            case 1:
            //                anim.SetTrigger("isAttack2");
            //                break;
            //            case 2:
            //                anim.SetTrigger("isAttack3");
            //                break;
            //            case 3:
            //                anim.SetTrigger("isAttack4");
            //                break;
            //        }
            //        break;
            }
        }


        /*************************************************
         *               Private Methods
         *************************************************/
  
}
