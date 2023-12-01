using BNG;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody rigid;
    public Damageable damageable;
    GameObject instantLazer;
    public int bossId;
   

    [Header("레이저 관련")]
    public Transform lazerPort;
    public GameObject lazer;

    [Header("원거리 공격")]
    public Transform bulletPort;
    public Transform bulletPortLeft;
    public Transform bulletPortRight;
    public GameObject smallBullet;

    [Header("원거리 공격 2번째")]
    public Transform bulletTopLeft;
    public Transform bulletTopRight;
    public GameObject bigBullet;

    [Header("원거리 공격 3번째")]
    public Transform wallPort;
    public Transform wallPortSecond;
    public Transform wallPortThird;
    public GameObject wall;

    [Header("타겟")]
    public Transform target;

    [Header("테이블")]
    public float hp = default;
    public float maxHp = default;

    [Header("조건")]
    public bool isLazer = false;
    public bool isPattern = false;
    public bool isDie = false;
    public bool isPatternExecuting = false;

    [Header("패턴 간격")]
    public float patternInterval = 5.0f;

    

    void Awake()
    {
        //hp = maxHp;
        GetData(bossId);
    }

    void Start()
    {
        damageable.Health = hp;

        rigid = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
        
        //StartCoroutine(PatternController());
        StartCoroutine(ExecutePattern());
    }

    public void GetData(int id)
    {
        maxHp = (float)DataManager.instance.GetData(id, "BossHP", typeof(float));
        hp = maxHp;
    }
    

    void PushPlayerBackward()
    {
        //target.gameObject.GetComponent<Damageable>().OnKnockBack(target.transform.forward * 0.3f);
    }



    //IEnumerator PatternController()
    //{
    //    while (true)
    //    {
    //        if (!isPatternExecuting && !isDie)
    //        {
    //            StartCoroutine(ExecutePattern());
    //        }

    //        yield return null;
    //    }
    //}

    IEnumerator ExecutePattern()
    {
        

        while (!isDie && !isPatternExecuting)
        {
            //Debug.Log($"hp{hp}");
            yield return new WaitForSeconds(0.1f);

            float healthRatio = hp / maxHp;

            // 체력에 따라 랜덤으로 패턴 선택
            //if (hp <= maxHp * 1.0f && hp >= maxHp * 0.76f)
            //{
            //    Debug.Log("작동");
            //    RandomPattern();
            //    isPatternExecuting = true;
            //    yield return new WaitForSeconds(patternInterval);

            //}
            //else if (hp <= maxHp * 0.75f && hp >= maxHp * 0.51f)
            //{
            //    Debug.Log("작동2");
            //    PushPlayerBackward();
            //    Debug.Log("push");
            //    isPatternExecuting = true;
            //    yield return new WaitForSeconds(patternInterval);

            //}
            //else if (hp <= maxHp * 0.5f && hp >= maxHp * 0.26f)
            //{
            //    Debug.Log("작동3");
            //    PushPlayerBackward();
            //    isPatternExecuting = true;
            //    yield return new WaitForSeconds(patternInterval);

            //}
            //else if (hp <= maxHp * 0.25f)
            //{
            //    Debug.Log("작동4");
            //    PushPlayerBackward();
            //    isPatternExecuting = true;
            //    yield return new WaitForSeconds(patternInterval);

            //}
            if (healthRatio <= 1.0f && healthRatio >= 0.76f)
            {
                Debug.Log("작동");
                RandomPattern();
                isPatternExecuting = true;
                yield return new WaitForSeconds(patternInterval);
            }
            else if (healthRatio <= 0.75f && healthRatio >= 0.51f)
            {
                Debug.Log("작동2");
                PushPlayerBackward();
                Debug.Log("push");
                isPatternExecuting = true;
                yield return new WaitForSeconds(patternInterval);
            }
            else if (healthRatio <= 0.5f && healthRatio >= 0.26f)
            {
                Debug.Log("작동3");
                PushPlayerBackward();
                isPatternExecuting = true;
                yield return new WaitForSeconds(patternInterval);
            }
            else if (healthRatio <= 0.25f)
            {
                Debug.Log("작동4");
                PushPlayerBackward();
                isPatternExecuting = true;
                yield return new WaitForSeconds(patternInterval);
            }
            isPatternExecuting = false;

        }
        

    }

    void RandomPattern()
    {
        int random = Random.Range(0, 4);

        switch (random)
        {
            case 0:
                LazerShoot();
                break;
            case 1:
                SmallShoot();
                break;
            case 2:
                BigShoot();
                break;
                case 3:
                WallShoot();
                break;
        }
    }

    void SmallShoot()
    {
        GameObject instantBullet = Instantiate(smallBullet, bulletPort.position, bulletPort.rotation);
        MonsterBullet bullet = instantBullet.GetComponent<MonsterBullet>();
        Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
        rigidBullet.velocity = transform.forward * 10.0f;

        GameObject instantBulletRight =
            Instantiate(smallBullet, bulletPortRight.position, Quaternion.Euler(bulletPortRight.rotation.eulerAngles + new Vector3(0, 10, 0)));
        MonsterBullet bulletRight = instantBulletRight.GetComponent<MonsterBullet>();
        Rigidbody rigidBulletRight = instantBulletRight.GetComponent<Rigidbody>();
        rigidBulletRight.velocity = transform.forward * 10.0f;

        GameObject instantBulletLeft =
            Instantiate(smallBullet, bulletPortLeft.position, Quaternion.Euler(bulletPortLeft.rotation.eulerAngles + new Vector3(0, -10, 0)));
        MonsterBullet bulletLeft = instantBulletLeft.GetComponent<MonsterBullet>();
        Rigidbody rigidBulletLeft = instantBulletLeft.GetComponent<Rigidbody>();
        rigidBulletLeft.velocity = transform.forward * 10.0f;
    }

    void WallShoot()
    {
        GameObject instantWall = Instantiate(wall, wallPort.position, wallPort.rotation);
        WallBullet bulletWall = instantWall.GetComponent<WallBullet>();
        Rigidbody rigidWall = instantWall.GetComponent<Rigidbody>();
        rigidWall.velocity = transform.forward * 10.0f;
        //wallPort.transform.LookAt(target.position);

        GameObject instantWallSecond = Instantiate(wall, wallPortSecond.position, wallPortSecond.rotation);
        Rigidbody rigidWallSecond = instantWallSecond.GetComponent<Rigidbody>();
        rigidWallSecond.velocity = transform.forward * 10.0f;
        //wallPortSecond.transform.LookAt(target.position);

        GameObject instantWallThird = Instantiate(wall, wallPortThird.position, wallPortThird.rotation);
        Rigidbody rigidWallThird = instantWallThird.GetComponent<Rigidbody>();
        rigidWallThird.velocity = transform.forward * 10.0f;
        //wallPortThird.transform.LookAt(target.position);

    }

    void BigShoot()
    {
        GameObject instantBulletLeft = Instantiate(bigBullet, bulletTopLeft.position, bulletTopLeft.rotation);
        MonsterBullet bulletLeft = instantBulletLeft.GetComponent<MonsterBullet>();
        instantBulletLeft.transform.LookAt(target.position);
        bulletPortLeft.transform.LookAt(target.position);

        GameObject instantBulletRight = Instantiate(bigBullet, bulletTopRight.position, bulletTopRight.rotation);
        MonsterBullet bulletRight = instantBulletRight.GetComponent<MonsterBullet>();
        instantBulletRight.transform.LookAt(target.position);
        bulletPortRight.transform.LookAt(target.position);
    }

    void LazerShoot()
    {
        instantLazer = Instantiate(lazer, lazerPort.position, lazerPort.rotation);
        instantLazer.transform.LookAt(target.position);
        lazerPort.transform.LookAt(target.position);

        Invoke("LazerDestroy", 0.5f);
    }

    void LazerDestroy()
    {
        Destroy(instantLazer);
    }


    

    // Other shoot methods remain unchanged

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 20.0f);
    }



    public void OnDamage()
    {
        if(damageable.Health <= 0)
        {
            isDie = true;
            Debug.Log($"isDie:{isDie}");
            StopAllCoroutines();
        }
    }
}