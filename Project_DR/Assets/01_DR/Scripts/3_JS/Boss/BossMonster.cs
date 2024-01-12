using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class BossMonster : MonoBehaviour
    {
        /*************************************************
         *                 Private Fields
          *************************************************/
        private int _id;
        private Boss _boss;
        private WaitForSeconds _waitForSeconds;


        /*************************************************
         *               Public Methods
         *************************************************/
        public void Initialize(int id)
        {
            // Init 
            _id = id;

            // _boss 생성 및 초기화
            _boss = gameObject.AddComponent<Boss>();
            _boss.Initialize(id);

            // 패턴 간격으로 WaitForSeconds 캐싱
            _waitForSeconds = new WaitForSeconds(_boss.BossData.PatternInterval);

            // 공격 실행
            StartAttack();
        }


        /*************************************************
         *               Private Methods
         *************************************************/
        ///// TODO: 보스룸에서 Start를 했을 경우 아래의 함수를
        ///// 호출하도록 설정한다!
        // 패턴 간격에 따라 공격 패턴 실행
        private void StartAttack()
        {
            StartCoroutine(StartBossAttackPatternsCoroutine());
        }
        

        /*************************************************
         *                 Coroutines
         *************************************************/
        // 보스가 사용 가능한 공격 패턴들을 패턴 간격에 따라 실행한다.
        public IEnumerator StartBossAttackPatternsCoroutine()
        {
            int patternCount = _boss.BossData.AvailableAttackPatternsList.Count;
            for (int i = 0; i < patternCount; i++)
            {
                // 보스가 죽었을 경우
                if (_boss.BossData.IsDead) { break; }

                // 패턴 간격만큼 대기
                yield return _waitForSeconds;

                // 공격 패턴 변경
                _boss.DOAttackPattern(_boss.BossData.AvailableAttackPatternsList[i]);
                GFunc.Log($"사용하는 패턴: {_boss.BossData.AvailableAttackPatternsList[i]}");
            }

            // 보스가 살아있을 경우 재귀 호출
            if (! _boss.BossData.IsDead)
            {
                StartCoroutine(StartBossAttackPatternsCoroutine());
            }

        /////TODO: 보스가 죽었을 때 간혹 패턴이 실행되는 경우가 있음
        /////그러므로 죽었을 때 일정 시간동안(ex 3초) 플레이어를 무적으로 하는게 좋아보임
        /////
        }
    }
}
