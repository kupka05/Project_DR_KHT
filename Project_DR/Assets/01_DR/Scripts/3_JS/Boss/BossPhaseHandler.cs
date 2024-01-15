using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Js.Boss
{
    public class BossPhaseHandler
    {
        /*************************************************
         *                Private Fields
         *************************************************/
        private Boss _boss;                                 // 보스
        private float _phase2Threshold = 0.75f;             // 페이즈 2 체력 조건
        private float _phase3Threshold = 0.50f;             // 페이즈 3 체력 조건
        private float _phase4Threshold = 0.25f;             // 페이즈 4 체력 조건
        private BossSummoningStone.Phase _targetPhase;      // 외부에 할당 할 페이즈

        /*************************************************
         *                Public Methods
         *************************************************/
        // Init
        public BossPhaseHandler(Boss boss)
        {
            _boss = boss;
        }

        // 페이즈 조건 체크
        public void IsInPhase()
        {
            float bossCurrentHP = _boss.BossData.HP;
            float bossMaxHP = _boss.BossData.MaxHP;
            // 페이즈 2 체력 조건에 해당할 경우
            if (_targetPhase < BossSummoningStone.Phase.TWO &&
                bossCurrentHP <= (bossMaxHP * _phase2Threshold))
            {
                // CurrentPhase를 2로 변경
                _targetPhase = BossSummoningStone.Phase.TWO;
                _boss.BossSummoningStone.ChangeCurrentPhase(_targetPhase);
            }

            // 페이즈 3 체력 조건에 해당할 경우
            else if (_targetPhase < BossSummoningStone.Phase.THREE &&
                bossCurrentHP <= (bossMaxHP * _phase3Threshold))
            {
                // CurrentPhase를 3로 변경
                _targetPhase = BossSummoningStone.Phase.THREE;
                _boss.BossSummoningStone.ChangeCurrentPhase(_targetPhase);
            }

            // 페이즈 4 체력 조건에 해당할 경우
            else if (_targetPhase < BossSummoningStone.Phase.FOUR &&
                bossCurrentHP <= (bossMaxHP * _phase4Threshold))
            {
                // CurrentPhase를 4로 변경
                _targetPhase = BossSummoningStone.Phase.FOUR;
                _boss.BossSummoningStone.ChangeCurrentPhase(_targetPhase);
            }
        }
    }
}
