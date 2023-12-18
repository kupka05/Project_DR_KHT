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
        Debug.Log($"게임시작");
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
        Debug.Log("코루틴이 한번만 실행이 되는지");

        while (!isDie && !isPatternExecuting)
        {
           
            yield return new WaitForSeconds(0.1f);

            //체력에 따라 랜덤으로 패턴 선택
            if (damageable.Health <= maxHp * 1.0f && damageable.Health > maxHp * 0.75f)
            {
                RandomPattern();
                //if (bossState)
                //{
                //    bossState.GetComponent<BossState>().Attack();
                //}

                isPatternExecuting = true;
                yield return new WaitForSeconds(patternInterval);
                isPatternExecuting = false;

            }
            else if (damageable.Health <= maxHp * 0.75f && damageable.Health > maxHp * 0.5f)
            {

                RandomPattern();
                //if (bossState)
                //{
                //    bossState.GetComponent<BossState>().Attack();
                //}

                if (bossState && !isPushPlayer)
                {
                    PushPlayerBackward();
                    Debug.Log("넉백");

                    bossState.GetComponent<BossState>().CastSpell();
                    Debug.Log("애니메이션 작동");

                    isPushPlayer = true;
                }

                //PushPlayerBackward();
                ////T0DO:넉백시
                //if (bossState)
                //{
                //    bossState.GetComponent<BossState>().CastSpell();
                //}



                isPatternExecuting = true;
                yield return new WaitForSeconds(patternInterval);
                isPatternExecuting = false;

            }
            else if (damageable.Health <= maxHp * 0.5f && damageable.Health > maxHp * 0.25f)
            {
                RandomPattern();
                //if (bossState)
                //{
                //    bossState.GetComponent<BossState>().Attack();
                //}

                if (bossState && !isPushPlayer)
                {
                    PushPlayerBackward();
                    Debug.Log("넉백");

                    bossState.GetComponent<BossState>().CastSpell();
                    Debug.Log("애니메이션 작동");

                    isPushPlayer = true;
                }


                isPatternExecuting = true;
                yield return new WaitForSeconds(patternInterval);
                isPatternExecuting = false;


            }
            else if (damageable.Health <= maxHp * 0.25f)
            {

                RandomPattern();
                //if (bossState)
                //{
                //    bossState.GetComponent<BossState>().Attack();
                //}

                if (bossState && !isPushPlayer)
                {
                    PushPlayerBackward();
                    Debug.Log("넉백");
                   
                    bossState.GetComponent<BossState>().CastSpell();
                    Debug.Log("애니메이션 작동");
                   
                    isPushPlayer = true;
                }

                isPatternExecuting = true;
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
                break;
            case 1:
                BounceShoot();
                break;
            case 2:
                BigBrickShoot();
                break;
            case 3:
                LazerShoot();
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
            Debug.Log("넉백작동");

            knockBack.OnBackDash(20);
        }
        //if (playerHealth != null)
        //{
        //    playerHealth.OnKnockback(this.transform.position);
        //    //target.gameObject.GetComponent<PlayerHealth>().OnKnockback(target.forward * 5.0f);
        //}
    }
    
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

            Debug.Log($"리스트 : {bullets.Count}");

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
            Debug.Log($"불값 초기화가 언제 호출 되는지 : {isShoot}");
        }
    }

    void LazerShoot()
    {   
        instantLazer = Instantiate(lazer, lazerPort.position, lazerPort.rotation);
        instantLazer.transform.LookAt(target.position);
        lazerPort.transform.LookAt(target.position);


        ShowRangeIndicator();

        Invoke("LazerDestroy", 1.0f);
        //Debug.Log($"작동:{Invoke}");
    }

    void ShowRangeIndicator()
    {
        if (targetImage != null)
        {
            targetImage.transform.position = target.position;
            targetImage.gameObject.SetActive(true);
        }
    }

    void LazerDestroy()
    {
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
        Debug.Log($"활성화:{instantBrick}");

        GameObject instantBrickLeft = Instantiate(bigBrick, bigBrickPortLeft.position, Quaternion.identity);
        instantBrickLeft.SetActive(true);
        Rigidbody brickRigidbodyLeft = instantBrickLeft.GetComponent<Rigidbody>();
        brickRigidbodyLeft.useGravity = false;
        Debug.Log($"활성화:{instantBrickLeft}");

        GameObject instantBrickRight = Instantiate(bigBrick, bigBrickPortRight.position, Quaternion.identity);
        instantBrickRight.SetActive(true);
        Rigidbody brickRigidbodyRight = instantBrickRight.GetComponent<Rigidbody>();
        brickRigidbodyRight.useGravity = false;
        Debug.Log($"활성화:{instantBrickRight}");

        // 2초 동안 대기하면서 gravity 비활성화
        yield return StartCoroutine(BrickWait(waitTime));

        // gravity 활성화 및 힘을 적용
        brickRigidbody.useGravity = true;
        brickRigidbody.AddForce(BigBrick(bigBrickPort.position), ForceMode.Impulse);
        Debug.Log("가운데 발사");

        brickRigidbodyLeft.useGravity = true;
        brickRigidbodyLeft.AddForce(BigBrick(bigBrickPortLeft.position), ForceMode.Impulse);
        Debug.Log("왼쪽 발사");

        brickRigidbodyRight.useGravity = true;
        brickRigidbodyRight.AddForce(BigBrick(bigBrickPortRight.position), ForceMode.Impulse);
        Debug.Log("오른쪽 발사");

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

    //유도미사일 테스트
    IEnumerator GuidedShoot()
    {
        if (!isShoot)
        {
            isShoot = true;

            List<GameObject> missiles = new List<GameObject>();

            // 미사일 미리 생성
            for (int i = 0; i < missileCount; i++)
            {
                Vector3 offset = UnityEngine.Random.insideUnitSphere * 2.0f;

                GameObject instantMissile = Instantiate(testBullet, transform.position + offset, Quaternion.identity);
                missiles.Add(instantMissile);
            }

            yield return new WaitForSeconds(2.0f);

            Debug.Log($"리스트 크기 : {missiles.Count}");

            foreach (var missile in missiles)
            {
                // 미사일이 이미 파괴되었는지 확인
                if (missile != null)
                {
                    Rigidbody rigidMissile = missile.GetComponent<Rigidbody>();
                    HomingMissile homingMissile = missile.GetComponent<HomingMissile>(); // Add HomingMissile script to your missile prefab
                    
                    if (homingMissile != null)
                    {
                        // 플레이어를 타겟으로 설정
                        homingMissile.SetTarget(GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos);
                    }

                    yield return new WaitForSeconds(0.4f);

                    // 발사 방향 설정 및 속도 적용
                    rigidMissile.velocity = rigidMissile.transform.forward * speed;

                    Destroy(rigidMissile, 6.0f);
                }
            }

            missiles.Clear();

            isShoot = false;
            Debug.Log($"불값 초기화가 언제 호출 되는지 : {isShoot}");
        }
    }


    public void OnDeal()
    {
        if (!isDie)
        {
            if (damageable.Health >= 0)
            {
                SetHealth(damageable.Health);
                Debug.Log($"Current HP: {damageable.Health}");
            }

            if (damageable.Health <= 0)
            {
                SetHealth(0);
                isDie = true;
                Debug.Log($"isDie:{isDie}");
                // 이벤트 호출
                unityEvent?.Invoke();

                if (bossState)
                {
                    bossState.GetComponent<BossState>().Die();
                }
                StopAllCoroutines();
                //Debug.Log("코루틴 멈춤");
            }
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("인식되냐");
            StartCoroutine(ExecutePattern());
        }
    }

}