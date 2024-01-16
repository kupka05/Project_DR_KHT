using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFloor2 : Old_Boss
{
    public override IEnumerator ExecutePattern()
    {
        //GFunc.Log("코루틴이 한번만 실행이 되는지");

        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            if (!isPatternExecuting)
            {
                isPatternExecuting = true;

                //체력에 따라 랜덤으로 패턴 선택
                if (damageable.Health <= maxHp * 1.0f && damageable.Health > maxHp * 0.76f)
                {
                    //GFunc.Log("체력별 패턴 1 진입");

                    RandomPattern();
                    //GFunc.Log("랜덤 패턴1 발동");

                }
                else if (damageable.Health <= maxHp * 0.75f && damageable.Health > maxHp * 0.51f)
                {
                    GFunc.Log("체력별 패턴 2 진입");

                    RandomPattern();
                    //GFunc.Log("랜덤 패턴2 발동");

                    if (bossState && !isKnockBack)
                    {
                        PushPlayerBackward();
                        GFunc.Log("넉백");

                        bossState.GetComponent<BossState>().CastSpell();
                        //GFunc.Log("넉백 애니메이션 작동");
                        isKnockBack = true;

                    }
                }
                else if (damageable.Health <= maxHp * 0.5 && damageable.Health > maxHp * 0.26f)
                {
                    GFunc.Log("체력별 패턴 3 진입");

                    RandomPatternSecond();
                    GFunc.Log("랜덤 패턴 2개 발동");

                    if (bossState && !isKnockBackSecond)
                    {
                        PushPlayerBackward();
                        GFunc.Log("넉백");

                        bossState.GetComponent<BossState>().CastSpell();
                        //GFunc.Log("넉백 애니메이션 작동");
                        isKnockBackSecond = true;

                    }
                }
                else if (damageable.Health <= maxHp * 0.25f)
                {
                    GFunc.Log("체력별 패턴 4 진입");

                    RandomPatternThird();
                    GFunc.Log("랜덤 패턴 3개 발동");

                    if (bossState && !isKnockBackThird)
                    {
                        PushPlayerBackward();
                        GFunc.Log("넉백");

                        bossState.GetComponent<BossState>().CastSpell();
                        //GFunc.Log("넉백 애니메이션 작동");
                        isKnockBackThird = true;

                    }
                }

                yield return new WaitForSeconds(patternInterval);
                isPatternExecuting = false;

            }
        }
    }
}
