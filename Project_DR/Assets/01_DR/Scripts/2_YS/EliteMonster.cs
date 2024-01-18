using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EliteMonster : Monster
{
    public MonsterBullet monsterBullet;

    [Header("몬스터 원거리 관련")]
    public Transform bulletPortLeft;
    public Transform bulletPortRight;
    public Transform bulletPort;
    public GameObject monsterBulletPrefab;

    [Header("정예 이펙트")]
    public GameObject FireEffect;
    public GameObject knockBackHeadEffect;

    [Header("이펙트 머리 위치")]
    public float headHeight = 2.0f;

    public override IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                // IDLE 상태 =======================================================
                case State.IDLE:
                    //GFunc.Log("IDLE state");
                    nav.isStopped = true;
                    anim.SetBool(hashRun, false);
                    anim.SetBool(hashidle, true);
                    //anim.SetBool(hashWalkingAttack, false);
                    anim.SetBool(hashAttack, false);
                    anim.SetBool(hashAttack2, false);
                    anim.SetBool(hashAttack3, false);
                    anim.SetBool(hashAttack4, false);
                    break;

                // TRACE 상태 =======================================================
                case State.TRACE:
                    //GFunc.Log("TRACE state");
                    nav.isStopped = false;
                    nav.SetDestination(playerTr.position);
                    anim.SetBool(hashRun, true);
                    anim.SetBool(hashWalkingAttack, false);
                    anim.SetBool(hashAttackRuning, true);
                    anim.SetBool(hashAttackRuning2, true);
                    anim.SetBool(hashAttackRuning3, true);
                    break;

                // ATTACK 상태 =======================================================
                case State.ATTACK:

                    //GFunc.Log("ATTACK state");

                    switch (monsterType)
                    {
                        case Type.HUMAN_ROBOTRED:

                            int humanRobotRed = Random.Range(0, 2);

                            switch (humanRobotRed)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);
                                    yield return new WaitForSeconds(1.3f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                                case 1:
                                    //anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack2, true);

                                    yield return new WaitForSeconds(1.3f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                            }
                            break;

                        case Type.HUMAN_GOLEMFIRE:

                            int humanGolemFire = Random.Range(0, 3);

                            switch (humanGolemFire)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);
                                    yield return new WaitForSeconds(1.3f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                                case 1:
                                    //anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack2, true);

                                    yield return new WaitForSeconds(1.3f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                                case 2:
                                    //anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack3, true);
                                    yield return new WaitForSeconds(1.2f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                            }
                            break;

                        case Type.BEAST_SCORPION:

                            int spider = Random.Range(0, 3);

                            switch (spider)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                                case 1:
                                    anim.SetBool(hashAttack2, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                                case 2:
                                    anim.SetBool(hashAttack3, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                            }
                            break;

                        case Type.BEAST_QUEENWORM:

                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                        case Type.SIMPLE_TOADSTOOL:

                            int toadStool = Random.Range(0, 3);

                            switch (toadStool)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);
                                
                                    yield return new WaitForSeconds(0.2f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                                case 1:
                                    anim.SetBool(hashAttack2, true);
                                    
                                    yield return new WaitForSeconds(0.2f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                                case 2:
                                    anim.SetBool(hashAttack3, true);

                                    yield return new WaitForSeconds(0.7f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;
                            }
                            break;

                        case Type.SIMPLE_PHANTOM:

                            int phantom = Random.Range(0, 3);

                            switch (phantom)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);

                                    yield return new WaitForSeconds(1.0f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    //anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                                case 1:
                                    anim.SetBool(hashAttack2, true);

                                    yield return new WaitForSeconds(1.0f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    //anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                                case 2:
                                    anim.SetBool(hashAttack3, true);

                                    yield return new WaitForSeconds(1.0f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;
                            }
                            break;
                    }
                    break;


                case State.DIE:
                    isDie = true;
                    nav.isStopped = true;
                    //GFunc.Log("nav.isStopped: " + nav.isStopped);
                    anim.SetTrigger(hashDie);
                    AudioManager.Instance.PlaySFX(dieSound);
                    Invoke(nameof(MonsterDestroy), 3.0f);
                    foreach (CapsuleCollider capsuleCollider in capsuleColliders)
                    {
                        capsuleCollider.isTrigger = true;
                    }
                    UserData.KillElite(exp);
                    //Destroy(this.gameObject, 1.3f); //damageable 쪽에서 처리
                    yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }


    }

    public override void OnDeal(float damage)
    {
        GameObject instantTakeDamage = Instantiate(takeDamageEffect, transform.position, Quaternion.identity);
        Destroy(instantTakeDamage, 2.0f);

        AudioManager.Instance.PlaySFX(takeDamageSound);

        // 죽지 않은 상태면 HP 바 업데이트
        if (damageable.Health > 0 && damageable.Health <= hp * 1.0f)
        {
            SetHealth(damageable.Health);
            GFunc.Log($"OnDeal체력:{damageable.Health}");
        }
        else if (damageable.Health <= 0)
        {
            SetHealth(0);
            GFunc.Log($"리턴 체력:{damageable.Health}");
            return;
        }


        //Debug.Log($"체력:{damageable.Health}");

        // 스턴 상태 또는 죽음 상태일 경우 리턴
        if (state == State.STUN || state == State.DIE)
            return;

        MonsterStun();  // 몬스터 스턴

        smashCount++;   // 분쇄 카운트 추가



        if (smashCount >= smashMaxCount)
        {
            smash.SetActive(true);
            //GFunc.Log("분쇄카운트 충족");

            smashCount = 0;
            //GFunc.Log($"분쇄 카운트:{smashCount}");

            smashFilled.fillAmount = 1;
            //GFunc.Log($"분쇄FillAmount:{smashFilled.fillAmount}");

            if (smashCoroutine != null)
            {
                StopCoroutine(smashCoroutine);
            }
            smashCoroutine = StartCoroutine(SmashTime());

            if (countNum <= 3)
            {
                smashCountNum.text = countNum.ToString();
                countNum++;
                //Debug.Log($"숫자:{countNum}");
            }
            else if (countNum == 5)
            {

            }

            //GFunc.Log($"숫자:{countNum}");

            ApplyStackDamage(damage);
            //GFunc.Log("스택 별 데미지 진입");

            //GFunc.Log("중첩 숫자 증가");

        }

        count++;
        GFunc.Log($"넉백 카운트:{count}");

        if (count >= maxCount)
        {
            count = 0;

            MonsterKnockBack();

            ////기존
            //Vector3 targetPosition = transform.position - transform.forward * 4.0f;
            //MoveWithSmoothTransition(targetPosition);

        }
    }

    public override void MonsterKnockBack()
    {
        //anim.SetTrigger(hashHit);

        rigid.WakeUp();

        if (rigid != null)
        {
            Vector3 headPositon = transform.position + Vector3.up * headHeight;

            // 넉백 힘이 적용되는 동안에도 이동하도록 Coroutine 사용
            StartCoroutine(MoveWithKnockBack(transform.position - transform.forward * 2.0f));

            AudioManager.Instance.PlaySFX(knockbackSound);

            Vector3 overlapSphereCenter = this.transform.position - transform.forward * 0.5f;
            overlapSphereCenter.z -= 0.5f;

            Collider[] colliders = Physics.OverlapSphere(overlapSphereCenter, damageRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Wall"))
                {
                    return;
                }
            }

            // 넉백 힘이 적용된 후에 이펙트를 머리 위치로 생성
            GameObject instantKnockbackHead = Instantiate(knockBackHeadEffect, headPositon, Quaternion.Euler(-90, 0, 0));
            GFunc.Log($"머리 쪽 이펙트:{instantKnockbackHead}");
            Destroy(instantKnockbackHead, 0.7f);
        }
    }

    // Coroutine으로 넉백 중에도 이동하도록 처리
    private IEnumerator MoveWithKnockBack(Vector3 targetPosition)
    {
        float duration = 1.0f; // 넉백 이동 시간 (조절 가능)

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 넉백 이동 중에 머리를 따라가도록 이동
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsed / duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 넉백 이동이 끝나면 최종 목적지로 위치 설정
        transform.position = targetPosition;
    }

    public void MonsterDestroy()
    {
        Destroy(this.gameObject);
    }


    public void GolemShoot(int index)
    {
        switch(index)
        {
            case 0:
                GameObject instantBulletLeft = Instantiate(monsterBulletPrefab, bulletPortLeft.position, bulletPortLeft.rotation);
                //MonsterBullet bulletLeft = instantBulletLeft.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr.position);
                //instantBulletLeft.transform.LookAt(playerTr);

                GameObject instantLeft = Instantiate(FireEffect, bulletPortLeft.position, bulletPortLeft.rotation);
                GFunc.Log($"이펙트 발생:{instantLeft}");

                GameObject instantBulletRight = Instantiate(monsterBulletPrefab, bulletPortRight.position, bulletPortRight.rotation);
                //MonsterBullet bulletRight = instantBulletRight.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr.position);
                //instantBulletRight.transform.LookAt(playerTr);
                
                GameObject instantRight = Instantiate(FireEffect, bulletPortRight.position, bulletPortRight.rotation);
                GFunc.Log($"이펙트 발생:{instantRight}");

                AudioManager.Instance.PlaySFX(attackSound);

                break;
        }
    }

    public void Shoot(int index)
    {
        switch(index)
        {
            case 0:
                GameObject instantBullet = Instantiate(monsterBulletPrefab, bulletPort.position, bulletPort.rotation);
                MonsterBullet bullet = instantBullet.GetComponent<MonsterBullet>();
                bulletPort.transform.LookAt(playerTr.position);
                //instantBullet.transform.LookAt(playerTr);

                GameObject instantFire = Instantiate(FireEffect, bulletPort.position, bulletPort.rotation);
                GFunc.Log($"이펙트 발생:{instantFire}");

                AudioManager.Instance.PlaySFX(attackSound);

                break;

                case 1:
                GameObject instantBulletRight = Instantiate(monsterBulletPrefab, bulletPortRight.position, bulletPortRight.rotation);
                MonsterBullet bulletRight = instantBulletRight.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr.position);
                //instantBulletRight.transform.LookAt(playerTr);

                GameObject instantFireRight = Instantiate(FireEffect, bulletPortRight.position, bulletPortRight.rotation);
                GFunc.Log($"이펙트 발생:{instantFireRight}");

                AudioManager.Instance.PlaySFX(attackSound);

                break;

                case 2:
                GameObject instantBulletLeft = Instantiate(monsterBulletPrefab, bulletPortLeft.position, bulletPortLeft.rotation);
                MonsterBullet bulletLeft = instantBulletLeft.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr.position);
                //instantBulletLeft.transform.LookAt(playerTr);

                GameObject instantFireLeft = Instantiate(FireEffect, bulletPortLeft.position, bulletPortLeft.rotation);
                GFunc.Log($"이펙트 발생:{instantFireLeft}");

                AudioManager.Instance.PlaySFX(attackSound);

                break;
        }
    }

   
}
