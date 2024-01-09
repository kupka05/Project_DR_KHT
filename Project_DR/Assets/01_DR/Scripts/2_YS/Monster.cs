using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;
using TMPro;
using BossMonster;

// 데미지를 체크하는 클래스
public class DamageChecker : MonoBehaviour
{
    public Monster monster;

    private void OnCollisionEnter(Collision collision)
    {
        // 공격 상태일 때 반환
        if (monster.isAttack)
        { return; }

        if (collision.gameObject.tag.Equals("Player"))
        {
            monster.isAttack = true;

            transform.GetComponent<DamageCollider>().OnCollisionEvent(collision);
            // 아닐 경우 모든 데미지 콜라이더를 꺼준다.
            for (int i = 0; i < monster.damageCollider.Length; i++)
            {
                monster.damageCollider[i].enabled = false;
            }
            monster.ResetDamageCollider();
        }
    }
}


public class Monster : MonoBehaviour
{
    public UnityEngine.UI.Slider monsterHpSlider;
    private TMP_Text hpText;

    //스턴 추가 - hit상태
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        STUN,
        DIE
    }

    public State state = State.IDLE;

    public enum Type
    {
        HUMAN_ROBOT,
        HUMAN_GOLEM,
        BEAST_SPIDER,
        BEAST_STING,
        SIMPLE_FUNGI,
        SIMPLE_SPOOK,

        HUMAN_ROBOTRED,
        HUMAN_GOLEMFIRE,
        BEAST_SCORPION,
        BEAST_QUEENWORM,
        SIMPLE_TOADSTOOL,
        SIMPLE_PHANTOM
    }
    public CapsuleCollider[] capsuleColliders;

    public int monsterId;

    public Type monsterType = Type.HUMAN_ROBOT;

    [Header("분쇄")]
    public GameObject smash;
    public UnityEngine.UI.Image smashFilled;
    public TMP_Text smashCountNum;
    public float skillTime = 10.0f;
    public int countNum = 1;

    [Header("분쇄 데미지 퍼센트")]
    public float smashOne = 0.03f;
    public float smashSecond = 0.15f;
    public float smashThird = 0.6f;

    [Header("분쇄 발동 카운트")]
    public int smashCount = 0;      //깍아냄
    public int smashMaxCount = 3;  //깍아냄 횟수 충족


    [Header("몬스터 테이블")]
    public float hp = default;       //체력이랑 damageble 보내준다
    public float attack = default;
    public float attDelay = default;   //몬스터 공격간격 
    public float hitDelay = default;   //몬스터 히트시 공격간격 
    public int exp = default;
    public float speed = default;      //몬스터 이동속도
    public float recRange = 30.0f;   //pc 인식범위
    public float attRange = 2.0f;   //pc 공격범위
    public float stunDelay = 1f;
    public float stunCount = default;  //경직 횟수, 일반 몬스터는 필요 없음
    public float stopDistance = default;
    //몬스터 이름도 추가될 예정

    public float lastDamageTime;

    [Header("트랜스폼")]
    public Transform monsterTr;
    public Transform playerTr;

    [Header("몬스터 컴포넌트")]
    public Animator anim;
    public Rigidbody rigid;
    public NavMeshAgent nav;

    IEnumerator actionRoutine;


    public DamageCollider[] damageCollider;

    public Damageable damageable;

    public readonly int hashRun = Animator.StringToHash("isRun");

    public readonly int hashWalkingAttack = Animator.StringToHash("isWalkingAttack");

    public readonly int hashAttack = Animator.StringToHash("isAttack");
    public readonly int hashAttack2 = Animator.StringToHash("isAttack2");
    public readonly int hashAttack3 = Animator.StringToHash("isAttack3");
    public readonly int hashAttack4 = Animator.StringToHash("isAttack4");

    public readonly int hashAttackRuning = Animator.StringToHash("isAttackRuning");
    public readonly int hashAttackRuning2 = Animator.StringToHash("isAttackRuning2");
    public readonly int hashAttackRuning3 = Animator.StringToHash("isAttackRuning3");

    public readonly int hashHit = Animator.StringToHash("isDamage");

    public readonly int hashDie = Animator.StringToHash("isDie");

    public readonly int hashidle = Animator.StringToHash("isIdle");

    public readonly int hashStun = Animator.StringToHash("isStun");

    [Header("조건")]
    public bool isDie = false;
    public bool isStun = false;
    public bool isStack = false;
    public bool isAttack = false;
    public bool isUpper = false;

    public IEnumerator stunRoutine; // 스턴 루틴

    [Header("Debug")]
    public float distanceDebug;
    public bool isDebug = false;

    [Header("DistanceFromGround")]
    public float distanceFromGround;            // 지면과의 거리
    public float distanceFromGroundOffset;      // 지면과의 거리 보정치 (크기 및 공중)
    public LayerMask GroundedLayers;            // 허용할 바닥 레이어

    private RaycastHit groundHit;
    IEnumerator knockbackRoutine;   // 넉백 루틴

    WaitForSeconds waitForSecond = new WaitForSeconds(0.1f);
    WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    [Header("몬스터 넉백 관련")]
    public int count = default;
    public int maxCount = default;
    private bool isMoving = false;
    private float moveDuration = 1.0f;
    private float moveTimer = 0.0f;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Awake()
    {
        GetData(monsterId);
    }

    // Start is called before the first frame update
    public void Start()
    {
        capsuleColliders = GetComponentsInChildren<CapsuleCollider>();
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().transform;

        damageable = GetComponent<Damageable>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();


        damageable.Health = hp;
        lastDamageTime = Time.time;

        foreach (DamageCollider damageCollider in damageCollider)
        {
            damageCollider.SetDamage(attack);

            //attack = damageCollider.Damage; // 지환 : attack은 시트에서 가져온 데이터 값
        }

        nav.speed = speed;

        nav.stoppingDistance = stopDistance;
        //nav.stoppingDistance = attRange - 0.5f;

        SetMaxHealth(damageable.Health); //hp
        //GFunc.Log($"초기 hp 설정 값:{damageable.Health}");

        InitMonster();

        SetDamageCollider();        // 데미지 콜라이더를 제어하는 클래스를 세팅
    }

    public void InitMonster()
    {
        isDie = false;
        nav.isStopped = false;
        StartCoroutine(MonsterState());

        actionRoutine = MonsterAction();
        StartCoroutine(actionRoutine);
    }

    public void Update()
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

    public void FixedUpdate()
    {
        if (state != State.DIE)
        {
            // Look At Y 각도로만 기울어지게 하기
            Vector3 targetPostition =
                new Vector3(playerTr.position.x, this.transform.position.y, playerTr.position.z);
            this.transform.LookAt(targetPostition);

        }

        if (playerTr == null)
            return;

        GroundCheck();  // 바닥과의 거리 체크
    }

    public void GetData(int id)
    {
        hp = Data.GetFloat(id, "MonHP");
        exp = Data.GetInt(id, "MonExp");
        attack = Data.GetFloat(id, "MonAtt");
        attDelay = Data.GetFloat(id, "MonDel");
        hitDelay = Data.GetFloat(id, "HitDel");
        speed = (float)DataManager.Instance.GetData(id, "MonSpd", typeof(float));
        attRange = (float)DataManager.Instance.GetData(id, "MonAtr", typeof(float));
        recRange = (float)DataManager.Instance.GetData(id, "MonRer", typeof(float));
        stunDelay = (float)DataManager.Instance.GetData(id, "MonSTFDel", typeof(float));

        stopDistance = (float)DataManager.Instance.GetData(id, "MonStd", typeof(float));

        if(isDebug)
        {
            hp = 100000;
            attack = 0;
            speed = 0;
        }

        maxCount = Data.GetInt(id, "MonKno");
    }

    public void SetMaxHealth(float newHealth)
    {
        hpText = monsterHpSlider.transform.GetChild(2).GetComponent<TMP_Text>();

        monsterHpSlider.maxValue = newHealth;
        monsterHpSlider.value = newHealth;
        hpText.text = newHealth.ToString();
    }

    public void SetHealth(float newHealth)
    {
        monsterHpSlider.value = newHealth;
        hpText.text = newHealth.ToString();
    }


    // 스테이트를 관리하는 코루틴
    // 역할 : 스테이트를 변환만 해준다. 다른건 없음.
    IEnumerator MonsterState()
    {
        while (!isDie)
        {
            //SetHealth(damageable.Health);

            // 체력이 0 이하면 죽은 상태로 전이
            if (damageable.Health <= 0)
            {
                SetHealth(0);
                state = State.DIE;

                if (actionRoutine != null)
                {
                    StopCoroutine(actionRoutine);
                    actionRoutine = null;
                }

                actionRoutine = MonsterAction();
                StartCoroutine(actionRoutine);
            }


            // 스턴을 맞을 경우 스턴 상태로 전이
            else if (isStun)
                state = State.STUN;

            else
            {
                float distance = Vector3.Distance(playerTr.position, monsterTr.position);
                distanceDebug = distance;
                if (distance <= attRange)
                {
                    state = State.ATTACK;
                }
                else if (distance <= recRange)
                {
                    state = State.TRACE;
                }
                else
                {
                    state = State.IDLE;
                }
            }
            yield return waitForSecond;
        }
    }

    // 몬스터의 상태에 따라 전이되는 액션
    public virtual IEnumerator MonsterAction()
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
                    if (nav.isOnNavMesh)
                    {
                        nav.isStopped = false;
                        nav.SetDestination(playerTr.position);
                    }
                    else
                        GFunc.Log("Missing NavMesh");

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
                        case Type.HUMAN_ROBOT:

                            anim.SetBool(hashWalkingAttack, true);
                            anim.SetBool(hashAttack, true);
                            yield return new WaitForSeconds(0.5f);
                            anim.SetBool(hashidle, true);
                            anim.SetBool(hashAttack, false);
                            anim.SetBool(hashRun, false);
                            yield return new WaitForSeconds(attDelay);
                            break;

                        case Type.HUMAN_GOLEM:

                            int humanGolem = Random.Range(0, 3);

                            switch (humanGolem)
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

                        case Type.BEAST_SPIDER:

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

                        case Type.BEAST_STING:

                            int sting = Random.Range(0, 3);

                            switch (sting)
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
                                    //case 3:
                                    //    anim.SetBool(hashAttack4, true);
                                    //    yield return new WaitForSeconds(0.8f);
                                    //    anim.SetBool(hashidle, true);
                                    //    anim.SetBool(hashAttack4, false);
                                    //    //anim.SetBool(hashWalkingAttack, false);
                                    //    anim.SetBool(hashRun, false);
                                    //    yield return new WaitForSeconds(0.3f);
                                    //    break;
                            }
                            break;

                        case Type.SIMPLE_FUNGI:

                            int fungi = Random.Range(0, 2);

                            switch (fungi)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);
                                    yield return new WaitForSeconds(0.2f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    //anim.SetBool(hashWalkingAttack, false);
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
                            }
                            break;

                        case Type.SIMPLE_SPOOK:

                            int spook = Random.Range(0, 3);

                            switch (spook)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);
                                    yield return new WaitForSeconds(0.7f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(attDelay);
                                    break;

                                case 1:

                                    anim.SetBool(hashAttack2, true);
                                    yield return new WaitForSeconds(0.7f);
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
                    }
                    break;


                case State.DIE:
                    isDie = true;
                    nav.isStopped = true;
                    //GFunc.Log("nav.isStopped: " + nav.isStopped);
                    anim.SetTrigger(hashDie);
                    foreach (CapsuleCollider capsuleCollider in capsuleColliders)
                    {
                        capsuleCollider.isTrigger = true;
                    }
                    UserData.KillMonster(exp);


                    Invoke(nameof(Explosion), 3f);

                    yield break;
            }

            yield return waitForSecond;
        }


    }

    public void OnDeal(float damage)
    {
        // 죽지 않은 상태면 HP 바 업데이트
        if (damageable.Health > 0)
        {
            SetHealth(damageable.Health);
        }
        else
        { 
            SetHealth(0);
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

            StartCoroutine(SmashTime());

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

        if (count >= maxCount)
        {
            count = 0;
            anim.SetTrigger(hashStun);

            Vector3 targetPosition = transform.position - transform.forward * 4.0f;

            MoveWithSmoothTransition(targetPosition);
        }
    }

    // 몬스터 스턴
    public void MonsterStun()
    {
        // 만약에 스턴루틴에 이미 다른 코루틴이 실행중인 경우
        if (stunRoutine != null)
        {
            StopCoroutine(stunRoutine);
            stunRoutine = null;
        }

        if (isUpper == false)
        {
            stunRoutine = StunDelay();
            StartCoroutine(stunRoutine);
        }
        isUpper = false;
    }

    public void ApplyStackDamage(float damage)
    {
        //Debug.Log($"countNum = {countNum}");

        if (countNum == 2)
        {
            //GFunc.Log("스택1진입");
            damageable.Health -= SmashDamageCalculate(damage, 1);
            // 갱신된 체력 값을 적용
            SetHealth(damageable.Health);

            // 남은 체력을 로그로 출력
            //Debug.Log($"추가 분쇄 데미지 1 : {SmashDamageCalculate(damage, 1)}, 남은체력:{damageable.Health}");

        }
        else if (countNum == 3)
        {
            damageable.Health -= SmashDamageCalculate(damage, 2);
            SetHealth(damageable.Health);

            //Debug.Log($"추가 분쇄 데미지 2 : {SmashDamageCalculate(damage, 2)}, 남은체력:{damageable.Health}");

        }
        else if (countNum == 4)
        {
            damageable.Health -= SmashDamageCalculate(damage, 3);
            SetHealth(damageable.Health);

            //Debug.Log($"남은체력:{damageable.Health}");

            //Debug.Log($"추가 분쇄 데미지 3 : {SmashDamageCalculate(damage, 3)}, 남은체력:{damageable.Health}");

        }

    }
    /// <summary> 분쇄 데미지를 계산하는 메서드 </summary>
    /// <param name="damage">플레이어의 최종 데미지</param>
    /// <param name="index">분쇄 단계</param>
    /// <returns></returns>
    public float SmashDamageCalculate(float damage, int index)
    {
        float _debuff = UserData.GetSmashDamage(index); ;
        return Mathf.RoundToInt(damage * (1 + _debuff)) - damage; 
    }

    public IEnumerator SmashTime()
    {
        while (smashFilled.fillAmount > 0)
        {
            //GFunc.Log($"남은 시간:{smashFilled.fillAmount * skillTime}");
            //GFunc.Log("분쇄 fill");
            smashFilled.fillAmount -= 1 * Time.smoothDeltaTime / skillTime;

            if (smashFilled.fillAmount <= 0)
            {
                smash.SetActive(false);
                smashCount = 0;
                countNum = 1;
                //GFunc.Log("사라지나");
            }
            yield return null;
        }


    }



    // 스턴 딜레이
    public IEnumerator StunDelay()
    {
        rigid.Sleep();
        //isStun = true;
        anim.SetTrigger(hashHit);
        //damageable.stun = true;
        yield return new WaitForSeconds(stunDelay);
        //isStun = false;
        rigid.WakeUp();
        //damageable.stun = false;
        yield break;
    }

    void OnDrawGizmos()
    {
        if (state == State.TRACE)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, recRange);
        }

        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attRange);
        }

    }
    public virtual void Explosion()
    {
        Destroy(this.gameObject);

    }


    // 플레이어에게 드릴 랜딩을 받을 시 몬스터 넉백 
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PlayerSkill"))
        {
            if (knockbackRoutine == null)
            {
                knockbackRoutine = KnockBackRoutine(other);
                StartCoroutine(knockbackRoutine);
            }
        }
    }
    /// <summary>
    /// 넉백 실행시 진행되는 코루틴
    /// </summary>
    /// <param name="other">플레이어 스킬 포지션</param>
    /// <returns></returns>
    IEnumerator KnockBackRoutine(Collider other)
    {
        // 스턴 실행 및 navMesh 꺼주기
        isStun = true;
        nav.enabled = false;

        // 스킬에서 해당 몬스터 넉백 실행
        other.GetComponent<SkillEvent>().ActiveDrillLanding(this.gameObject);
        //GFunc.Log(gameObject.name+" nav 해제");

        // 바로 스턴이 꺼지는 것을 방지하여 딜레이
        yield return waitForSeconds;
        while (true)
        {
            // 땅에 닿을 경우 스턴 해제 및 navMesh 켜주기
            if (distanceFromGround <= 0.1f)
            {
                //GFunc.Log(gameObject.name + " nav 재 실행");

                nav.enabled = true;
                yield return waitForSeconds;

                isStun = false;
                knockbackRoutine = null;
                yield break;
            }
            yield return waitForFixedUpdate;
        }
    }
    /// <summary>
    /// 넉백 시 몬스터가 띄워졌을 경우 바닥을 체크하는 메서드
    /// 바닥에 띄워지면 NavMesh가 꺼지고, 바닥에 다시 닿으면 NavMesh가 켜진다.
    /// </summary>
    public void GroundCheck()
    {
        if (Physics.Raycast(transform.position, -transform.up, out groundHit, 20, GroundedLayers, QueryTriggerInteraction.Ignore))
        {
            distanceFromGround = Vector3.Distance(transform.position, groundHit.point);


            // Round to nearest thousandth
            distanceFromGround = (float)Math.Round(distanceFromGround * 1000f) / 1000f;
        }
        else
        {
            //distanceFromGround = float.MaxValue;
        }


        if (distanceFromGround != float.MaxValue)
        {
            distanceFromGround -= distanceFromGroundOffset;
        }

        // Smooth floating point issues from thousandths
        if (distanceFromGround < 0.001f && distanceFromGround > -0.001f)
        {
            distanceFromGround = 0;
        }

    }
    // 데미지 콜라이더를 세팅해준다.
    public void SetDamageCollider()
    {
        // 데미지 콜라이더가 있을 경우
        if (damageCollider.Length == 0)
        {
            return;
        }

        for (int i = 0; i < damageCollider.Length; i++)
        {
            damageCollider[i].isMonster = true;
            damageCollider[i].AddComponent<DamageChecker>();
            damageCollider[i].GetComponent<DamageChecker>().monster = this;
        }
    }

    /// <summary>공격 시간 이후에 데미지 콜라이더 활성화 </summary>
    public void ResetDamageCollider()
    {
        Invoke(nameof(EnableDamageCollider), hitDelay);
    }
    /// <summary>데미지 콜라이더 활성화 </summary>
    private void EnableDamageCollider()
    {
        for (int i = 0; i < damageCollider.Length; i++)
        {
            damageCollider[i].enabled = true;
        }
        isAttack = false;
    }




}


