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


    public void MonsterDestroy()
    {
        Destroy(this.gameObject);
    }

    public override void MonsterKnockBack()
    {
        rigid.WakeUp();

        if (rigid != null)
        {
            Vector3 knockbackDirection = -transform.forward * 3.0f;
            rigid.AddForce(knockbackDirection, ForceMode.Impulse);

            AudioManager.Instance.PlaySFX(knockbackSound);

            // 몬스터와 머리가 함께 뒤로 이동하도록 처리
            MoveWithSmoothTransition(transform.position + knockbackDirection);

            // 몬스터 머리에 이펙트 생성
            Vector3 headPosition = transform.position + Vector3.up * headHeight;
            GameObject instantKnockbackHead = Instantiate(knockBackHeadEffect, headPosition, Quaternion.Euler(-90, 0, 0));
            Destroy(instantKnockbackHead, 1.2f);

            // 몬스터 머리에 생성된 이펙트를 몬스터 머리와 함께 따라가도록 처리
            instantKnockbackHead.transform.parent = transform;

            //몬스터 벽 인식
            Vector3 overlapSphereCenter = this.transform.position - transform.forward * 2f;
            overlapSphereCenter.z += 1.5f;


            if (monsterType == Type.BEAST_QUEENWORM)
            {
                overlapSphereCenter = this.transform.position - transform.forward * 2f;
                overlapSphereCenter.z -= 0.5f;
            }

            Collider[] colliders = Physics.OverlapSphere(overlapSphereCenter, damageRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Wall"))
                {
                    return;
                }
            }
        }
    }

    public override void OnDrawGizmos()
    {
        Vector3 overlapSphereCenter = this.transform.position - transform.forward * 2f;
        overlapSphereCenter.z += 1.5f;

        if (monsterType == Type.BEAST_QUEENWORM)
        {
            overlapSphereCenter = this.transform.position - transform.forward * 2f;
            overlapSphereCenter.z -= 0.5f;
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(overlapSphereCenter, damageRadius);
    }


    public void GolemShoot(int index)
    {
        switch (index)
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
        switch (index)
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
