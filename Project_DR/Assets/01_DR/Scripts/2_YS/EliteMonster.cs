using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EliteMonster : Monster
{
    public float count = 0;
    public float maxCount = 3;


    private bool isMoving = false;
    private float moveDuration = 1.0f;
    private float moveTimer = 0.0f;
    private Vector3 startPosition;
    private Vector3 targetPosition;


    void Update()
    {
        if (isMoving)
        {
            moveTimer += Time.deltaTime;

            float t = Mathf.Clamp01(moveTimer / moveDuration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            if (t >= 1.0f)
            {
                isMoving = false;
                moveTimer = 0.0f;
            }
        }
    }

    public void MoveWithSmoothTransition(Vector3 target)
    {
        if (!isMoving)
        {
            isMoving = true;
            startPosition = transform.position;
            targetPosition = target;
        }
    }

    public override IEnumerator MonsterAction()
    {
        while (true)
        {
            switch (state)
            {
                // IDLE 상태 =======================================================
                case State.IDLE:
                    //Debug.Log("IDLE state");
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
                    //Debug.Log("TRACE state");
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

                    //Debug.Log("ATTACK state");

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
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 1:
                                    //anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack2, true);

                                    yield return new WaitForSeconds(1.3f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
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
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 1:
                                    //anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack2, true);

                                    yield return new WaitForSeconds(1.3f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 2:
                                    //anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack3, true);
                                    yield return new WaitForSeconds(1.2f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
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
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 1:
                                    anim.SetBool(hashAttack2, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 2:
                                    anim.SetBool(hashAttack3, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
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
                                    yield return new WaitForSeconds(0.3f);
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
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 1:
                                    anim.SetBool(hashAttack2, true);
                                    
                                    yield return new WaitForSeconds(0.2f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 2:
                                    anim.SetBool(hashAttack3, true);

                                    yield return new WaitForSeconds(0.7f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
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
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 1:
                                    anim.SetBool(hashAttack2, true);

                                    yield return new WaitForSeconds(1.0f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    //anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 2:
                                    anim.SetBool(hashAttack3, true);

                                    yield return new WaitForSeconds(1.0f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;
                            }
                            break;
                    }
                    break;


                case State.DIE:
                    isDie = true;
                    nav.isStopped = true;
                    //Debug.Log("nav.isStopped: " + nav.isStopped);
                    anim.SetTrigger(hashDie);

                    //Destroy(this.gameObject, 1.3f); //damageable 쪽에서 처리
                    yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }


    }

    public override void OnDeal()
    {
        if (isStun)
            return;
        if (state != State.DIE && state != State.STUN)
        {
            if (damageable.Health >= 0)
            {
                // 만약에 스턴루틴에 이미 다른 코루틴이 실행중인 경우
                if (stunRoutine != null)
                {
                    StopCoroutine(stunRoutine);
                    stunRoutine = null;
                }

                stunRoutine = StunDelay();
                StartCoroutine(stunRoutine);

                Debug.Log($"state:{state}");

                count++;
               
                if (count >= maxCount)
                {
                    count = 0;
                    anim.SetTrigger(hashStun);
                   
                    Vector3 targetPosition = transform.position - transform.forward * 4.0f;
                    //transform.position = targetPosition;

                    MoveWithSmoothTransition(targetPosition);
                }
            }
        }
        Debug.Log($"hp:{damageable.Health}");
    }
    // 스턴 딜레이
    public override IEnumerator StunDelay()
    {
        isStun = true;
        anim.SetTrigger(hashHit);
        damageable.stun = true;
        yield return new WaitForSeconds(stunDelay);
        isStun = false;
        damageable.stun = false;
        yield break;
    }

    

    public void GolemShoot(int index)
    {
        switch(index)
        {
            case 0:
                GameObject instantBulletLeft = Instantiate(monsterBullet, bulletPortLeft.position, bulletPortLeft.rotation);
                MonsterBullet bulletLeft = instantBulletLeft.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr);
                instantBulletLeft.transform.LookAt(playerTr);

                GameObject instantBulletRight = Instantiate(monsterBullet, bulletPortRight.position, bulletPortRight.rotation);
                MonsterBullet bulletRight = instantBulletRight.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr);
                instantBulletRight.transform.LookAt(playerTr);
                break;
        }
    }

    public void Shoot(int index)
    {
        switch(index)
        {
            case 0:
                GameObject instantBullet = Instantiate(monsterBullet, bulletPort.position, bulletPort.rotation);
                MonsterBullet bullet = instantBullet.GetComponent<MonsterBullet>();
                bulletPort.transform.LookAt(playerTr);
                instantBullet.transform.LookAt(playerTr);
                break;

                case 1:
                GameObject instantBulletRight = Instantiate(monsterBullet, bulletPortRight.position, bulletPortRight.rotation);
                MonsterBullet bulletRight = instantBulletRight.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr);
                instantBulletRight.transform.LookAt(playerTr);
                break;

                case 2:
                GameObject instantBulletLeft = Instantiate(monsterBullet, bulletPortLeft.position, bulletPortLeft.rotation);
                MonsterBullet bulletLeft = instantBulletLeft.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr);
                instantBulletLeft.transform.LookAt(playerTr);
                break;
        }
    }

    public override void Explosion(int index)
    {
        switch(index)
        {
            case 0:
                Destroy(this.gameObject);
                break;
        }
        
    }
}
