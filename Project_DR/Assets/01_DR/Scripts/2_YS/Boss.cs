using BNG;
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
    public UnityEvent unityEvent;

    public UnityEngine.UI.Slider bossHPSlider;

    public GameObject bossState;

    public Rigidbody rigid;
    
    public Damageable damageable;

    public UnityEngine.UI.Image targetImage;

    GameObject instantLazer;

    public float turnSpeed = 15.0f;

    public float missileCount = 6.0f;

    [Header("보스 및 투사체 테이블 관련")]
    public int bossId;  //보스 테이블
    public int bossProjectileId;  //투사체 테이블
    public int bossProjectileID;

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

    [Header("포물선 터지는 오브젝트")]
    public GameObject explosionPrefab;
    public Transform explosionPort;
    public Transform explosionPortLeft;
    public Transform explosionPortRight;

    [Header("포물선 안 터지는 오브젝트")]
    public Transform bigBrickPort;
    public Transform bigBrickPortLeft;
    public Transform bigBrickPortRight;
    public GameObject bigBrick;
    public float destroy = default;

    [Header("유도 미사일 테스트")]
    public Transform testPort;
    public GameObject testBullet;

    
    void Awake()
    {
        GetData(bossId, bossProjectileId, bossProjectileID);
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

        damageable.Health = maxHp;

        SetMaxHealth(maxHp);

        if(bossState)
        {
            bossState.GetComponent<BossState>().CastSpell();
        }

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

    public void GetData(int bossId, int bossProjectileId, int bossProjectileID)
    {
        //보스
        maxHp = (float)DataManager.instance.GetData(bossId, "BossHP", typeof(float));

        //소형 투사체 6910
        bulletCount = (float)DataManager.instance.GetData(bossProjectileId, "Duration", typeof(float));
        delayTime = (float)DataManager.instance.GetData(bossProjectileId, "Delay", typeof(float));
        patternInterval = (float)DataManager.instance.GetData(bossProjectileId, "DelTime", typeof(float)); //이건 하나만
        destoryTime = (float)DataManager.instance.GetData(bossProjectileId, "DesTime", typeof(float));
        speed = (float)DataManager.instance.GetData(bossProjectileId, "Speed", typeof(float));

        //6914
        destroy = (float)DataManager.instance.GetData(bossProjectileID, "DesTime", typeof(float));

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
                BounceShoot();
                Debug.Log("패턴 2 선택");
                break;
            case 2:
                StartCoroutine(BigBrickShoot());
                Debug.Log("패턴 3 선택");
                break;
            case 3:
                LazerShoot();
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

                offset = UnityEngine.Random.insideUnitSphere * 2.0f;

                GameObject instantBullet = Instantiate(smallBulletPrefab, transform.position + offset, Quaternion.identity);
                bullets.Add(instantBullet);
                instantBullet.transform.LookAt(target);
            }

            yield return new WaitForSeconds(2.0f);


            GFunc.Log($"리스트 크기 : {bullets.Count}");

            foreach (var bullet in bullets)
            {
                
                if (bullet != null)
                {
                    Rigidbody rigidBullet = bullet.GetComponent<Rigidbody>();
                    rigidBullet.transform.LookAt(target);

                    yield return new WaitForSeconds(0.4f);

                    rigidBullet.velocity = (target.position - bullet.transform.position).normalized * speed;
                }
            }

            bullets.Clear();

            isShoot = false;
            GFunc.Log($"불값 초기화가 언제 호출 되는지 : {isShoot}");
        }
    }

    void LazerShoot()
    {
        StartCoroutine(ShowRangeIndicatorCoroutine());
    }

    IEnumerator ShowRangeIndicatorCoroutine()
    {
        if (targetImage != null)
        {
            targetImage.transform.position = target.position;
            targetImage.gameObject.SetActive(true);

            yield return new WaitForSeconds(3.0f);

            // After waiting for 3 seconds, shoot the laser
            ShootLazer();
        }
    }

    void ShootLazer()
    {
        // Instantiate the laser and set its direction
        instantLazer = Instantiate(lazer, lazerPort.position, lazerPort.rotation);
        instantLazer.transform.LookAt(target.position);
        lazerPort.transform.LookAt(target.position);

        // Schedule the destruction of the laser after 1 second
        Invoke("LazerDestroy", 1.0f);
    }

    void LazerDestroy()
    {
        // Destroy the laser and hide the target image
        Destroy(instantLazer);
        targetImage.gameObject.SetActive(false);
    }
    public Vector3 BigBrick(Vector3 portPosition)
    {
        int randomBrick = UnityEngine.Random.Range(0, 10);
        // 해당 위치에 따라 초기 속도 설정
        Vector3 initialVelocity = transform.forward * randomBrick;  //10.0f;

        Vector3 target = transform.forward * randomBrick;   //5.0f;

        //int randomBrick = UnityEngine.Random.Range(0, 3);
        // 각 포트 위치에 따라 Y축 오프셋 값 조절
        if (portPosition == bigBrickPort.position)
        {
            target.y += randomBrick;
        }
        else if (portPosition == bigBrickPortLeft.position)
        {   
            target.y += randomBrick;
        }
        else if (portPosition == bigBrickPortRight.position)
        {
            target.y += randomBrick;
        }

        // 초기 속도와 목표 위치를 결합
        Vector3 combinedVector = initialVelocity + target;

        return combinedVector;
    }

    IEnumerator BrickWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }


    IEnumerator BigBrickShoot()
    {
        // 대기 시간 설정 (여기서는 2초로 설정)
        float waitTime = 2.0f;

        // 가운데, 왼쪽, 오른쪽 위치에 대해 동시에 오브젝트를 생성하고 힘을 적용
        GameObject instantBrick = Instantiate(bigBrick, bigBrickPort.position, Quaternion.identity);
        instantBrick.SetActive(true);
        Rigidbody brickRigidbody = instantBrick.GetComponent<Rigidbody>();
        brickRigidbody.useGravity = false;
        //GFunc.Log($"활성화:{instantBrick}");

        GameObject instantBrickLeft = Instantiate(bigBrick, bigBrickPortLeft.position, Quaternion.identity);
        instantBrickLeft.SetActive(true);
        Rigidbody brickRigidbodyLeft = instantBrickLeft.GetComponent<Rigidbody>();
        brickRigidbodyLeft.useGravity = false;
        //GFunc.Log($"활성화:{instantBrickLeft}");

        GameObject instantBrickRight = Instantiate(bigBrick, bigBrickPortRight.position, Quaternion.identity);
        instantBrickRight.SetActive(true);
        Rigidbody brickRigidbodyRight = instantBrickRight.GetComponent<Rigidbody>();
        brickRigidbodyRight.useGravity = false;
        //GFunc.Log($"활성화:{instantBrickRight}");

        // 2초 동안 대기하면서 gravity 비활성화
        yield return StartCoroutine(BrickWait(waitTime));

        // gravity 활성화 및 힘을 적용
        brickRigidbody.useGravity = true;
        brickRigidbody.AddForce(BigBrick(bigBrickPort.position), ForceMode.Impulse);
        //GFunc.Log("가운데 발사");

        brickRigidbodyLeft.useGravity = true;
        brickRigidbodyLeft.AddForce(BigBrick(bigBrickPortLeft.position), ForceMode.Impulse);
        //GFunc.Log("왼쪽 발사");

        brickRigidbodyRight.useGravity = true;
        brickRigidbodyRight.AddForce(BigBrick(bigBrickPortRight.position), ForceMode.Impulse);
        //GFunc.Log("오른쪽 발사");

        // 나머지 대기 시간 후에 오브젝트 파괴
        yield return StartCoroutine(BrickWait(waitTime));
        Destroy(instantBrick, destroy);
        Destroy(instantBrickLeft, destroy);
        Destroy(instantBrickRight, destroy);
    }

    public Vector3 ExplosionBox(Vector3 portPosition)
    {
        int randomBox = UnityEngine.Random.Range(0, 10);
        // 해당 위치에 따라 초기 속도 설정
        Vector3 initialVelocity = transform.forward * randomBox;  //10.0f;

        Vector3 target = transform.forward * randomBox;   //5.0f;

        // 각 포트 위치에 따라 Y축 오프셋 값 조절
        if (portPosition == explosionPort.position)
        {
            target.y += randomBox;
        }
        else if (portPosition == explosionPortLeft.position)
        {
            target.y += randomBox;
        }
        else if (portPosition == explosionPortRight.position)
        {
            target.y += randomBox;
        }

        // 초기 속도와 목표 위치를 결합
        Vector3 combinedVector = initialVelocity + target;

        return combinedVector;
    }


    void ExplosionShoot()
    {
        GameObject instantExplosion = Instantiate(explosionPrefab, explosionPort.position, Quaternion.identity);
        instantExplosion.GetComponent<Rigidbody>().AddForce(ExplosionBox(explosionPort.position), ForceMode.Impulse);

        GameObject instantExplosionLeft = Instantiate(explosionPrefab, explosionPortLeft.position, Quaternion.identity);
        instantExplosionLeft.GetComponent<Rigidbody>().AddForce(ExplosionBox(explosionPortLeft.position), ForceMode.Impulse);

        GameObject instantExplosionRight = Instantiate(explosionPrefab, explosionPortRight.position, Quaternion.identity);
        instantExplosionRight.GetComponent<Rigidbody>().AddForce(ExplosionBox(explosionPortRight.position), ForceMode.Impulse);
    }

    
    void BounceShoot()
    {
        GameObject instantBounce = Instantiate(bounce, bouncePort.position, bouncePort.rotation);
        GameObject bounceLeft = Instantiate(bounce, bouncePortLeft.position, bouncePortLeft.rotation);
        GameObject bounceRight = Instantiate(bounce, bouncePortRight.position, bouncePortRight.rotation);

        Destroy(instantBounce, 8.0f);
        Destroy(bounceLeft, 8.0f);
        Destroy(bounceRight, 8.0f);
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


    public void OnDeal()
    {
        if (!isDie)
        {
            if (damageable.Health >= 0)
            {
                SetHealth(damageable.Health);
                GFunc.Log($"Current HP: {damageable.Health}");
            }

            if (damageable.Health <= 0)
            {
                SetHealth(0);
                isDie = true;
                GFunc.Log($"isDie:{isDie}");
                // 이벤트 호출
                unityEvent?.Invoke();

                if (bossState)
                {
                    bossState.GetComponent<BossState>().Die();
                }
                StopAllCoroutines();
                //GFunc.Log("코루틴 멈춤");
            }
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isStart)
        {
            isStart = true;
            GFunc.Log("인식되냐");
            StartCoroutine(ExecutePattern());
            
        }
    }

}