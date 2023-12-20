using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EliteMonster : Monster
{
    private bool isMoving = false;
    private float moveDuration = 1.0f;
    private float moveTimer = 0.0f;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    public MonsterBullet monsterBullet;

    [Header("몬스터 원거리 관련")]
    public Transform bulletPortLeft;
    public Transform bulletPortRight;
    public Transform bulletPort;
    public GameObject monsterBulletPrefab;

    [Header("넉백 카운터")]
    public float count = 0;
    public float maxCount = 3;

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
                    //GFunc.Log("nav.isStopped: " + nav.isStopped);
                    anim.SetTrigger(hashDie);
                    UserData.KillElite(0, exp);
                    //Destroy(this.gameObject, 1.3f); //damageable 쪽에서 처리
                    yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }


    }

    public override void OnDeal(float damage)
    {
        // 죽지 않은 상태면 HP 바 업데이트
        if (damageable.Health >= 0)
        {
            SetHealth(damageable.Health);
        }
        else
            return;

        Debug.Log($"체력:{damageable.Health}");

        // 스턴 상태 또는 죽음 상태일 경우 리턴
        if (state == State.STUN || state == State.DIE)
            return;

        MonsterStun();  // 몬스터 스턴

        smashCount++;   // 분쇄 카운트 추가

        if (smashCount >= smashMaxCount)
        {
            smash.SetActive(true);
            GFunc.Log("분쇄카운트 충족");

            //float smashTakeDamage = damageable.Health * smashOne;
            //SetHealth(damageable.Health - smashTakeDamage);
            //Debug.Log($"받는 데미지:{damageable.Health - smashTakeDamage}");

            smashCount = 0;
            //GFunc.Log($"분쇄 카운트:{smashCount}");

            smashFilled.fillAmount = 1;
            //GFunc.Log($"분쇄FillAmount:{smashFilled.fillAmount}");

            StartCoroutine(SmashTime());

            if (countNum <= 3)
            {
                smashCountNum.text = countNum.ToString();
                countNum++;
                Debug.Log($"숫자:{countNum}");
            }
            else if (countNum == 5)
            {

            }

            GFunc.Log($"숫자:{countNum}");

            ApplyStackDamage(damage);

            //넉백
            count++;

            if (count >= maxCount)
            {
                count = 0;
                anim.SetTrigger(hashStun);

                Vector3 targetPosition = transform.position - transform.forward * 4.0f;

                MoveWithSmoothTransition(targetPosition);
            }
        }
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
                GameObject instantBulletLeft = Instantiate(monsterBulletPrefab, bulletPortLeft.position, bulletPortLeft.rotation);
                //MonsterBullet bulletLeft = instantBulletLeft.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr.position);
                //instantBulletLeft.transform.LookAt(playerTr);

                GameObject instantBulletRight = Instantiate(monsterBulletPrefab, bulletPortRight.position, bulletPortLeft.rotation);
                //MonsterBullet bulletRight = instantBulletRight.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr.position);
                //instantBulletRight.transform.LookAt(playerTr);
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
                break;

                case 1:
                GameObject instantBulletRight = Instantiate(monsterBulletPrefab, bulletPortRight.position, bulletPortRight.rotation);
                MonsterBullet bulletRight = instantBulletRight.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr.position);
                //instantBulletRight.transform.LookAt(playerTr);
                break;

                case 2:
                GameObject instantBulletLeft = Instantiate(monsterBulletPrefab, bulletPortLeft.position, bulletPortLeft.rotation);
                MonsterBullet bulletLeft = instantBulletLeft.GetComponent<MonsterBullet>();
                bulletPortRight.transform.LookAt(playerTr.position);
                //instantBulletLeft.transform.LookAt(playerTr);
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
