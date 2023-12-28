using BNG;
using Js.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    public UnityEngine.UI.Slider bossHPSlider;

    public GameObject bossState;

    public Rigidbody rigid;

    public Damageable damageable;

    public UnityEngine.UI.Image targetImage;

    GameObject instantLazer;
    GameObject instantLazerFire;

    public float turnSpeed = 15.0f;

    public float missileCount = 6.0f;

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


    [Header("보스 및 투사체 테이블 관련")]
    public int bossId;  //보스 테이블
    public int bossProjectileId;  //투사체 테이블
    public int bossProjectileID;
    public int bossProjectileBounce;
    public int bossExp;

    public float power = 5.0f;

    [Header("넉백 관련")]
    private bool isMoving = false;
    private float moveDuration = 1.0f;
    private float moveTimer = 0.0f;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    [Header("레이저 관련")]
    public Transform lazerPort;
    public GameObject lazer;
    public GameObject lazerFire;

    [Header("원거리 공격 소형 투사체")]
    public float bulletCount = default;
    public GameObject smallBulletPrefab;
    public float delayTime = default;
    public float destoryTime = default;
    public float speed = default;

    [Header("애드벌룬 투사체 생성")]
    public Transform bouncePort;
    public Transform bouncePortLeft;
    public Transform bouncePortRight;
    public GameObject bounce;
    public int bounceCount = 3;
    public float destoryTimeBounce;

    [Header("타겟")]
    public Transform target;
    public GameObject player;
    public PlayerBackDash knockBack;


    [Header("테이블")]
    public float hp = default;
    public float maxHp = default;

    [Header("조건")]
    public bool isLazer = false;
    public bool isPattern = false;
    public bool isDie = false;
    public bool isPatternExecuting = false;
    public bool isShoot = false;
    public bool isPushPlayer = false;
    public bool isKnockBack = false;
    public bool isKnockBackSecond = false;
    public bool isKnockBackThird = false;

    public bool isStart = false;

    [Header("패턴 간격")]
    public float patternInterval = default;

    //[Header("포물선 터지는 오브젝트")]
    //public GameObject explosionPrefab;
    //public Transform explosionPort;
    //public Transform explosionPortLeft;
    //public Transform explosionPortRight;

    [Header("포물선 안 터지는 오브젝트")]
    public Transform bigBrickPort;
    public Transform bigBrickPortLeft;
    public Transform bigBrickPortRight;
    public GameObject bigBrick;
    new private ParticleSystem particleSystem;
    public float destroy = default;
    public float damageRadius = 1.0f;


    [Header("Conversation")]
    public BossNPC npc;
    public UnityEvent bossMeet;
    public int questID;

    //[Header("유도 미사일 테스트")]
    //public Transform testPort;
    //public GameObject testBullet;


    void Awake()
    {
        GetData(bossId, bossProjectileId, bossProjectileID, bossProjectileBounce);
    }

    void Start()
    {
        InitializeBoss();
    }
    void InitializeBoss()
    {
        GFunc.Log($"게임시작");
        bossState = GameObject.FindWithTag("Boss");

        rigid = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        target = player.GetComponent<PlayerPosition>().playerPos;
        knockBack = player.GetComponent<PlayerBackDash>();

        particleSystem = GetComponent<ParticleSystem>();

        damageable.Health = maxHp;

        SetMaxHealth(damageable.Health);  //maxhp

        if (bossState)
        {
            bossState.GetComponent<BossState>().CastSpell();
        }

        npc = GetComponent<BossNPC>();
    }

    void FixedUpdate()
    {
        if (!target)
        {
            return;
        }

        // Look At Y 각도로만 기울어지게 하기
        Vector3 targetPostition =
            new Vector3(target.position.x, this.transform.position.y, target.position.z);
        this.transform.LookAt(targetPostition);

    }

    public void GetData(int bossId, int bossProjectileId, int bossProjectileID, int bossProjectileBounce)
    {
        //보스
        maxHp = (float)DataManager.Instance.GetData(bossId, "BossHP", typeof(float));
        patternInterval = (float)DataManager.Instance.GetData(bossId, "PatternChange", typeof(float)); //이건 하나만
        bossExp = Data.GetInt(bossId, "GiveEXP");

        //소형 투사체 6910
        bulletCount = (float)DataManager.Instance.GetData(bossProjectileId, "Duration", typeof(float));
        delayTime = (float)DataManager.Instance.GetData(bossProjectileId, "Delay", typeof(float));
        destoryTime = (float)DataManager.Instance.GetData(bossProjectileId, "DesTime", typeof(float));
        speed = (float)DataManager.Instance.GetData(bossProjectileId, "Speed", typeof(float));

        //6914
        destroy = (float)DataManager.Instance.GetData(bossProjectileID, "DesTime", typeof(float));

        //6912
        destoryTimeBounce = (float)DataManager.Instance.GetData(bossProjectileBounce, "DesTime", typeof(float));
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetMaxHealth(float newHealth)
    {
        bossHPSlider.maxValue = newHealth;
        bossHPSlider.value = newHealth;
    }
    public void SetHealth(float newHealth)
    {
        bossHPSlider.value = newHealth;
    }

    IEnumerator ExecutePattern()
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

                    RandomPattern();
                    //GFunc.Log("랜덤 패턴3 발동");

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

                    RandomPattern();
                    //GFunc.Log("랜덤 패턴4 발동");

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

    void RandomPattern()
    {
        int pattern = UnityEngine.Random.Range(0, 4);

        switch (pattern)
        {
            case 0:
                StartCoroutine(PlayShoot());
                Debug.Log("패턴 1 선택");
                break;
            case 1:
                StartCoroutine(LazerCoroutine());
                Debug.Log("패턴 2 선택");
                break;
            case 2:
                StartCoroutine(BigBrickShoot());
                Debug.Log("패턴 3 선택");
                break;
            case 3:
                StartCoroutine(BounceShoot());
                Debug.Log("패턴 4 선택");
                break;

        }


        if (bossState)
        {
            bossState.GetComponent<BossState>().Attack();
            Debug.Log("보스 attack 애니메이션");
        }
    }

    void PushPlayerBackward()
    {
        if (knockBack)
        {
            GFunc.Log("넉백작동");

            knockBack.OnBackDash(20);
        }
    }

    //IEnumerator PushPlayerBackwardCoroutine()
    //{
    //    if (knockBack)
    //    {
    //        GFunc.Log("넉백작동");

    //        knockBack.OnBackDash(20);
    //    }

    //    yield return new WaitForSeconds(moveDuration);

    //    isPushPlayer = false;  // 넉백이 끝난 후 초기화
    //}

    //void PushPlayerBackward()
    //{
    //    StartCoroutine(PushPlayerBackwardCoroutine());
    //}

    IEnumerator PlayShoot()
    {

        if (!isShoot)
        {
            isShoot = true;

            List<GameObject> bullets = new List<GameObject>();

            // 총알 미리 생성
            for (int i = 0; i < bulletCount; i++)
            {
                Vector3 offset = Vector3.zero;

                offset = new Vector3(UnityEngine.Random.insideUnitCircle.x * 2.0f, 2.0f, UnityEngine.Random.insideUnitCircle.y * 2.0f);
                //y값 2.0f

                //Vector3 bossForward = transform.forward;

                //// 전방 방향으로만 랜덤 오프셋을 생성합니다.
                //offset = UnityEngine.Random.insideUnitCircle * 2.0f;
                //offset = new Vector3(offset.x, offset.y, Mathf.Abs(offset.x)); // 전방 방향으로 조정


                GameObject instantBullet = Instantiate(smallBulletPrefab, transform.position + offset, Quaternion.identity);
                bullets.Add(instantBullet);
                instantBullet.transform.LookAt(target.position);

            }

            yield return new WaitForSeconds(2.0f);


            GFunc.Log($"리스트 크기 : {bullets.Count}");

            foreach (var bullet in bullets)
            {
                if (bullet != null)
                {
                    Rigidbody rigidBullet = bullet.GetComponent<Rigidbody>();
                    rigidBullet.transform.LookAt(target.position);

                    yield return new WaitForSeconds(0.4f);

                    rigidBullet.velocity = (target.position - bullet.transform.position).normalized * speed;
                }
            }

            bullets.Clear();

            isShoot = false;
            GFunc.Log($"불값 초기화가 언제 호출 되는지 : {isShoot}");
        }
    }

    //void LazerShot()
    //{
    //    StartCoroutine(LazerCoroutine());
    //}


    IEnumerator LazerCoroutine()
    {
        if (targetImage != null)
        {
            targetImage.transform.position = target.position;
            targetImage.gameObject.SetActive(true);

            yield return new WaitForSeconds(3.0f);

            // 3초 대기 후, targetImage의 위치에서 레이저 발사
            ShootLazer(targetImage.transform.position);

            // 추가: 레이저가 발사된 후 n초 대기 후 레이저 파이어 생성
            yield return new WaitForSeconds(0.1f);
            LazerFire(targetImage.transform.position);
        }
    }

    void LazerFire(Vector3 firePosition)
    {
        instantLazerFire = Instantiate(lazerFire, firePosition, Quaternion.identity);
        Invoke("LazerFireDestroy", 1.0f);
    }

    void LazerFireDestroy()
    {
        Destroy(instantLazerFire);
    }

    void ShootLazer(Vector3 shootPosition)
    {
        // 레이저를 생성하고 shootPosition을 기준으로 방향을 설정합니다.
        instantLazer = Instantiate(lazer, lazerPort.position, lazerPort.rotation);
        instantLazer.transform.LookAt(shootPosition); // target.position 대신에 shootPosition을 사용합니다.
        lazerPort.transform.LookAt(shootPosition);

        // 1초 후에 레이저를 파괴하도록 예약합니다.
        Invoke("LazerDestroy", 1.0f);
    }

    void LazerDestroy()
    {
        // 레이저를 파괴하고 targetImage를 숨깁니다.
        Destroy(instantLazer);
        targetImage.gameObject.SetActive(false);
    }

    //public Vector3 BigBrick(Vector3 portPosition)
    //{
    //    int randomBrick = UnityEngine.Random.Range(0, 10);
    //    // 해당 위치에 따라 초기 속도 설정
    //    Vector3 initialVelocity = transform.forward * 10.0f;  //10.0f;

    //    Vector3 target = transform.forward * 10.0f;   //5.0f;

    //    //int randomBrick = UnityEngine.Random.Range(0, 3);
    //    // 각 포트 위치에 따라 Y축 오프셋 값 조절
    //    if (portPosition == bigBrickPort.position)
    //    {
    //        target.y += randomBrick;
    //    }
    //    else if (portPosition == bigBrickPortLeft.position)
    //    {   
    //        target.y += randomBrick;
    //    }
    //    else if (portPosition == bigBrickPortRight.position)
    //    {
    //        target.y += randomBrick;
    //    }

    //    // 초기 속도와 목표 위치를 결합
    //    Vector3 combinedVector = initialVelocity + target;

    //    return combinedVector;
    //}

    //IEnumerator BrickWait(float waitTime)
    //{
    //    //transform.position = Vector3.zero;
    //    Debug.Log("정지");
    //    yield return new WaitForSeconds(waitTime);

    //}


    IEnumerator BigBrickShoot()
    {
        // 대기 시간 설정 (여기서는 2초로 설정)
        float waitTime = 2.0f;

        List<GameObject> bigbrick = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = new Vector3(UnityEngine.Random.insideUnitCircle.x * 4.0f, 2.0f, UnityEngine.Random.insideUnitCircle.y * 3.0f);

            GameObject instantBrick = Instantiate(bigBrick, transform.position + offset, Quaternion.identity);
            bigbrick.Add(instantBrick);

            instantBrick.SetActive(true);
            SphereCollider brickCollider = instantBrick.GetComponent<SphereCollider>();
            brickCollider.isTrigger = true;

            Rigidbody brickRigidbody = instantBrick.GetComponent<Rigidbody>();
            brickRigidbody.useGravity = false;
        }

        yield return new WaitForSeconds(waitTime);

        foreach (GameObject instantBrick in bigbrick)
        {
            SphereCollider brickCollider = instantBrick.GetComponent<SphereCollider>();
            brickCollider.isTrigger = false;

            Rigidbody brickRigidbody = instantBrick.GetComponent<Rigidbody>();
            brickRigidbody.useGravity = true;
            brickRigidbody.AddForce(parabola(), ForceMode.Impulse);

            Destroy(instantBrick, destroy);
            GFunc.Log($"시간경과 파괴:{this.gameObject}");
        }
    }

    //public void Shoot()
    //{
    //    AudioManager.instance.PlaySFX("FireBomb");

    //    GameObject ball = Instantiate(FireBombPrefab, firePosition.position, Quaternion.identity);
    //    ball.GetComponent<Rigidbody>().AddForce(calculateForce(), ForceMode.Impulse);
    //}
    //// 힘 계산하기
    //public Vector3 calculateForce()
    //{
    //    Vector3 targetPos = GameManager.instance.Golem.transform.position;
    //    targetPos.y += 25f;
    //    //firePosition.LookAt();  // 발사방향은 골렘을 향하게 변경
    //    firePosition.LookAt(targetPos);  // 발사방향은 골렘을 향하게 변경
    //    return firePosition.forward * power;
    //}

    public Vector3 parabola()
    {
        int randomPower = UnityEngine.Random.Range(3, 8);

        Vector3 pos = transform.forward;
        pos.y += 3.0f;

        return pos * randomPower;
    }

    //IEnumerator BigBrickShoot()
    //{
    //    // 대기 시간 설정 (여기서는 2초로 설정)
    //    float waitTime = 2.0f;

    //    Vector3 offset = Vector3.zero;

    //    offset = new Vector3(UnityEngine.Random.insideUnitCircle.x * 3.0f, 2.0f, UnityEngine.Random.insideUnitCircle.y * 2.0f);


    //    // 가운데, 왼쪽, 오른쪽 위치에 대해 동시에 오브젝트를 생성하고 힘을 적용
    //    GameObject instantBrick = Instantiate(bigBrick, transform.position + offset, Quaternion.identity);

    //    instantBrick.SetActive(true);
    //    Rigidbody brickRigidbody = instantBrick.GetComponent<Rigidbody>();
    //    brickRigidbody.useGravity = false;
    //    GFunc.Log($"활성화:{instantBrick}");

    //    GameObject instantBrickLeft = Instantiate(bigBrick, transform.position + offset, Quaternion.identity);

    //    instantBrickLeft.SetActive(true);
    //    Rigidbody brickRigidbodyLeft = instantBrickLeft.GetComponent<Rigidbody>();
    //    brickRigidbodyLeft.useGravity = false;
    //    //GFunc.Log($"활성화:{instantBrickLeft}");

    //    GameObject instantBrickRight = Instantiate(bigBrick, transform.position + offset, Quaternion.identity);

    //    instantBrickRight.SetActive(true);
    //    Rigidbody brickRigidbodyRight = instantBrickRight.GetComponent<Rigidbody>();
    //    brickRigidbodyRight.useGravity = false;
    //    GFunc.Log($"활성화:{instantBrickRight}");

    //    //n초 동안 대기하면서 gravity 비활성화
    //   yield return StartCoroutine(BrickWait(waitTime));
    //    GFunc.Log("대기");


    //    // gravity 활성화 및 힘을 적용
    //    brickRigidbody.useGravity = true;
    //    brickRigidbody.AddForce(BigBrick(bigBrickPort.position), ForceMode.Impulse);
    //    //GFunc.Log("가운데 발사");

    //    brickRigidbodyLeft.useGravity = true;
    //    brickRigidbodyLeft.AddForce(BigBrick(bigBrickPortLeft.position), ForceMode.Impulse);
    //    //GFunc.Log("왼쪽 발사");

    //    brickRigidbodyRight.useGravity = true;
    //    brickRigidbodyRight.AddForce(BigBrick(bigBrickPortRight.position), ForceMode.Impulse);
    //    //GFunc.Log("오른쪽 발사");

    //    // 나머지 대기 시간 후에 오브젝트 파괴
    //    yield return StartCoroutine(BrickWait(waitTime));
    //    Destroy(instantBrick, destroy);
    //    Destroy(instantBrickLeft, destroy);
    //    Destroy(instantBrickRight, destroy);

    //}

    //public Vector3 ExplosionBox(Vector3 portPosition)
    //{
    //    int randomBox = UnityEngine.Random.Range(0, 10);
    //    // 해당 위치에 따라 초기 속도 설정
    //    Vector3 initialVelocity = transform.forward * randomBox;  //10.0f;

    //    Vector3 target = transform.forward * randomBox;   //5.0f;

    //    // 각 포트 위치에 따라 Y축 오프셋 값 조절
    //    if (portPosition == explosionPort.position)
    //    {
    //        target.y += randomBox;
    //    }
    //    else if (portPosition == explosionPortLeft.position)
    //    {
    //        target.y += randomBox;
    //    }
    //    else if (portPosition == explosionPortRight.position)
    //    {
    //        target.y += randomBox;
    //    }

    //    // 초기 속도와 목표 위치를 결합
    //    Vector3 combinedVector = initialVelocity + target;

    //    return combinedVector;
    //}


    //void ExplosionShoot()
    //{
    //    GameObject instantExplosion = Instantiate(explosionPrefab, explosionPort.position, Quaternion.identity);
    //    instantExplosion.GetComponent<Rigidbody>().AddForce(ExplosionBox(explosionPort.position), ForceMode.Impulse);

    //    GameObject instantExplosionLeft = Instantiate(explosionPrefab, explosionPortLeft.position, Quaternion.identity);
    //    instantExplosionLeft.GetComponent<Rigidbody>().AddForce(ExplosionBox(explosionPortLeft.position), ForceMode.Impulse);

    //    GameObject instantExplosionRight = Instantiate(explosionPrefab, explosionPortRight.position, Quaternion.identity);
    //    instantExplosionRight.GetComponent<Rigidbody>().AddForce(ExplosionBox(explosionPortRight.position), ForceMode.Impulse);
    //}


    IEnumerator BounceShoot()
    {
        List<GameObject> bounceBall = new List<GameObject>();

        for (int i = 0; i < bounceCount; i++)
        {
            //Vector3 offset = UnityEngine.Random.insideUnitSphere * 2.0f;
            Vector3 offset = new Vector3(UnityEngine.Random.insideUnitCircle.x * 2.0f, 2.0f, UnityEngine.Random.insideUnitCircle.y * 2.0f);

            GameObject instantBounce = Instantiate(bounce, transform.position + offset, Quaternion.identity);
            bounceBall.Add(instantBounce);

            SphereCollider bounceCollider = instantBounce.GetComponent<SphereCollider>();
            bounceCollider.isTrigger = true;
        }

        yield return new WaitForSeconds(2.0f);

        foreach(GameObject instantBounce in bounceBall)
        {
            SphereCollider bounceCollider = instantBounce.GetComponent<SphereCollider>();
            bounceCollider.isTrigger = false;

            Rigidbody bounceRigidbody = instantBounce.GetComponent<Rigidbody>();
            bounceRigidbody.velocity = transform.forward * speed;

            Destroy(instantBounce, destoryTimeBounce);
            GFunc.Log($"시간 경과 파괴:{instantBounce}");
        }

    }

    ////유도미사일 테스트
    //IEnumerator GuidedShoot()
    //{
    //    if (!isShoot)
    //    {
    //        isShoot = true;

    //        List<GameObject> missiles = new List<GameObject>();

    //        // 미사일 미리 생성
    //        for (int i = 0; i < missileCount; i++)
    //        {
    //            Vector3 offset = UnityEngine.Random.insideUnitSphere * 2.0f;

    //            GameObject instantMissile = Instantiate(testBullet, transform.position + offset, Quaternion.identity);
    //            missiles.Add(instantMissile);
    //        }

    //        yield return new WaitForSeconds(2.0f);

    //        GFunc.Log($"리스트 크기 : {missiles.Count}");

    //        foreach (var missile in missiles)
    //        {
    //            // 미사일이 이미 파괴되었는지 확인
    //            if (missile != null)
    //            {
    //                Rigidbody rigidMissile = missile.GetComponent<Rigidbody>();
    //                HomingMissile homingMissile = missile.GetComponent<HomingMissile>(); // Add HomingMissile script to your missile prefab

    //                if (homingMissile != null)
    //                {
    //                    // 플레이어를 타겟으로 설정
    //                    homingMissile.SetTarget(GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos);
    //                }

    //                yield return new WaitForSeconds(0.4f);

    //                // 발사 방향 설정 및 속도 적용
    //                rigidMissile.velocity = rigidMissile.transform.forward * speed;

    //                Destroy(rigidMissile, 6.0f);
    //            }
    //        }

    //        missiles.Clear();

    //        isShoot = false;
    //        GFunc.Log($"불값 초기화가 언제 호출 되는지 : {isShoot}");
    //    }
    //}


    public void OnDeal(float damage)
    {
        if (!isDie)
        {
            if (damageable.Health >= 0)
            {
                SetHealth(damageable.Health);
                GFunc.Log($"현재 HP: {damageable.Health}");
            }

            if (damageable.Health <= 0)
            {

                isDie = true;
                SetHealth(0);
                GFunc.Log($"isDie:{isDie}");
                // 이벤트 호출
                //unityEvent?.Invoke();

                ClearBossKillQuest();
                UserData.KillBoss(bossExp);

                if (bossState)
                {
                    bossState.GetComponent<BossState>().Die();
                }
                StopAllCoroutines();

                Vector3 newSize = new Vector3(0.00001f, 0.00001f, 0.00001f);
                this.gameObject.transform.localScale = newSize;

                GetComponent<BossMonsterDeadCheck>().BossDie();

                //GFunc.Log("코루틴 멈춤");
            }

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
                //GFunc.Log("스택 별 데미지 진입");

                //GFunc.Log("중첩 숫자 증가");
            }

        }

    }


    public void ApplyStackDamage(float damage)
    {
        Debug.Log($"countNum = {countNum}");

        if (countNum == 2)
        {
            damageable.Health -= SmashDamageCalculate(damage, 1);  //1단계
            // 갱신된 체력 값을 적용
            SetHealth(damageable.Health);

            // 남은 체력을 로그로 출력
            Debug.Log($"추가 분쇄 데미지 1 : {SmashDamageCalculate(damage, 1)}, 남은체력:{damageable.Health}");

        }
        else if (countNum == 3)
        {
            damageable.Health -= SmashDamageCalculate(damage, 2);
            SetHealth(damageable.Health);

            Debug.Log($"추가 분쇄 데미지 2 : {SmashDamageCalculate(damage, 2)}, 남은체력:{damageable.Health}");

        }
        else if (countNum == 4)
        {
            damageable.Health -= SmashDamageCalculate(damage, 3);
            SetHealth(damageable.Health);

            Debug.Log($"남은체력:{damageable.Health}");

            Debug.Log($"추가 분쇄 데미지 3 : {SmashDamageCalculate(damage, 3)}, 남은체력:{damageable.Health}");

        }



    }
    /// <summary> 분쇄 데미지를 계산하는 메서드 </summary>
    /// <param name="damage">플레이어의 최종 데미지</param>
    /// <param name="index">분쇄 단계</param>
    /// <returns></returns>
    public float SmashDamageCalculate(float damage, int index)
    {
        float _debuff = UserData.GetSmashDamage(index); ;
        return (damage * (1 + _debuff)) - damage; ;

        //return damage * (1 + _debuff);
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isStart)
        {
            isStart = true;

            npc.BossMeet();                             //  보스 조우 이벤트 발생

            //Unit.InProgressQuestByID(3122001);          // 다음 퀘스트 진행중 으로 변경
            //isStart = true;
            //GFunc.Log("인식되냐");
            //StartCoroutine(ExecutePattern());

        }
    }

    // 전투 시작
    public void StartAttack()
    {
        StartCoroutine(ExecutePattern());
    }


    public void ClearBossKillQuest()
    {
        QuestCallback.OnBossKillCallback(bossId);

        Quest curQuest = Unit.GetInProgressMainQuest();
        questID = curQuest.QuestData.ID;
        GFunc.Log($"현재 진행중인 메인 퀘스트 ID : {questID}");

        // 보스 죽음 퀘스트
        Unit.ClearQuestByID(3122001);               // 완료 상태로 변경 & 보상 지급 & 선행퀘스트 조건이 있는 퀘스트들 조건 확인후 시작가능으로 업데이트
        //Unit.InProgressQuestByID(3122001);        // 다음 퀘스트 진행중 으로 변경
    }
}