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
            _boss.Initialize(this, id);

            // 패턴 간격으로 WaitForSeconds 캐싱
            _waitForSeconds = new WaitForSeconds(_boss.BossData.PatternInterval);

            // 디버그: 공격 실행
            //StartAttack();
        }

        ///// TODO: 보스룸에서 Start를 했을 경우 아래의 함수를
        ///// 호출하도록 설정한다!
        // 패턴 간격에 따라 공격 패턴 실행
        // 공격을 시작한다.
        public void StartAttack()
        {
            // 소환석 주위에 중복되는 오브젝트 삭제
            _boss.BossSummoningStone.gameObject.AddComponent<CheckDuplicateObject>();

            // 공격 패턴 실행
            StartCoroutine(StartBossAttackPatternsCoroutine());

            // 사운드 재생
            PlaySound();
        }

        // n초 후 공격 시작
        public void InvokeStartAttack(float delay)
        {
            // 디버그(테스트)
            // 클론 소환석 스폰
            //_boss.BossSummoningStone.SpawnCloneSummoningStones(5);

            // 공격 시작
            Invoke("StartAttack", delay);
        }

        // 보스 몬스터에게 할당된 사운드(BGM)을 재생한다.
        public void PlaySound()
        {
            string soundName = _boss.BossData.SoundName;
            AudioManager.Instance.AddBGM(soundName);
            AudioManager.Instance.PlayBGM(soundName);
        }

        // 랜덤한 공격 효과음 출력
        public void PlayAttackSFX()
        {
            int randomIndex = Random.Range(0, 2);
            string sfxName = _boss.BossData.AttackSFXNames[randomIndex];
            AudioManager.Instance.AddSFX(sfxName);
            AudioManager.Instance.PlaySFX(sfxName);
        }


        /*************************************************
         *               Private Methods
         *************************************************/
        // 공격을 실행한다.
        private void Attack(int attackPattern)
        {
            // 현재 상태가 정지 상태일 경우 예외 처리
            if (_boss.CurrentState.Equals(_boss.StopState)) { return; }

            // 공격 효과음 출력
            PlayAttackSFX();

            // 공격 애니메이션 실행
            _boss.BossAnimationHandler.AttackAnimation();

            // 공격 패턴 변경
            _boss.DOAttackPattern(attackPattern);
            GFunc.Log($"사용하는 패턴: {attackPattern}");
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
                // 공격을 실행
                Attack(_boss.BossData.AvailableAttackPatternsList[i]);

                // 패턴 간격만큼 대기
                yield return _waitForSeconds;
            }

            // 보스가 정지 상태가 아닐 경우 재귀 호출
            if (_boss.CurrentState != _boss.StopState)
            {
                GFunc.Log($"CurrentState: {_boss.CurrentState}");
                StartCoroutine(StartBossAttackPatternsCoroutine());
            }

        /////TODO: 보스가 죽었을 때 간혹 패턴이 실행되는 경우가 있음
        /////그러므로 죽었을 때 일정 시간동안(ex 3초) 플레이어를 무적으로 하는게 좋아보임
        /////
        }
    }
}
