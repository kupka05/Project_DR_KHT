using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
  
    public Rigidbody rigid;
    public Damageable damageable;
    GameObject instantLazer;
    public int bossId;

    public float power = 15.0f;

    [Header("레이저 관련")]
    public Transform lazerPort;
    public GameObject lazer;

    [Header("원거리 공격")]
    public int bulletCount = 6;
    public GameObject smallBulletPrefab;

    [Header("애드벌룬 투사체 생성")]
    public Transform bouncePort;
    public Transform bouncePortLeft;
    public Transform bouncePortRight;
    public GameObject bounce;

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
    public bool isShoot = false;

    [Header("패턴 간격")]
    public float patternInterval = 2.0f;

    [Header("포물선 터지는 오브젝트")]
    public GameObject testPrefab;
    public Transform testPosition;

    [Header("포물선 안 터지는 오브젝트")]
    public Transform bigBrickPort;
    public Transform bigBrickPortLeft;
    public Transform bigBrickPortRight;
    public GameObject bigBrick;

    void Awake()
    {
        GetData(bossId);
    }

    void InitializeBoss()
    {
        Debug.Log($"게임시작");
        
        rigid = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
        damageable.Health = maxHp;
        hp = maxHp;
        StartCoroutine(ExecutePattern());
    }


    void Start()
    {

        rigid = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;

        damageable.Health = maxHp;
        hp = maxHp;

        StartCoroutine(ExecutePattern());
    }

    public void GetData(int id)
    {
        maxHp = (float)DataManager.instance.GetData(id, "BossHP", typeof(float));

    }

    void PushPlayerBackward()
    {
        target.gameObject.GetComponent<Damageable>().OnKnockBack(target.transform.forward * 0.3f);
    }

    IEnumerator ExecutePattern()
    {
        //Debug.Log("코루틴이 한번만 실행이 되는지");

        while (!isDie && !isPatternExecuting)
        {
            //Debug.Log($"hp{hp}");
            yield return new WaitForSeconds(0.1f);

            //float healthRatio = hp / maxHp;

            //체력에 따라 랜덤으로 패턴 선택
            if (hp <= maxHp * 1.0f && hp > maxHp * 0.75f)
            {
                Debug.Log("작동");
                RandomPattern();
                isPatternExecuting = true;
                yield return new WaitForSeconds(patternInterval);

            }
            else if (hp <= maxHp * 0.75f && hp > maxHp * 0.5f)
            {
                Debug.Log("작동2");
                PushPlayerBackward();
                Debug.Log("push");
                isPatternExecuting = true;
                yield return new WaitForSeconds(patternInterval);

            }
            else if (hp <= maxHp * 0.5f && hp > maxHp * 0.25f)
            {
                Debug.Log("작동3");
                PushPlayerBackward();
                isPatternExecuting = true;
                yield return new WaitForSeconds(patternInterval);

            }
            else if (hp < maxHp * 0.25f)
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
                StartCoroutine(PlayShoot());
                break;
            case 1:
               
                break;
            case 2:
                
                break;
            case 3:
               
                break;
        }
    }

    
      IEnumerator PlayShoot()
      {
            if (!isShoot)
            {
                isShoot = true;

                for (int i = 0; i < bulletCount; i++)
                {
                    // 위치 조절
                    Vector3 offset = Vector3.zero;

                    if (i % 2 == 0)
                    {
                        offset = new Vector3(2.0f, 0, 0); // 짝수 번째 총알은 오른쪽으로
                    }
                    else
                    {
                        if (i % 4 == 1)
                        {
                            offset = new Vector3(0, 2.0f, 0); // 홀수 번째 중 1, 5, 9번째 총알은 위로
                        }
                        else
                        {
                            offset = new Vector3(-2.0f, 0, 0); // 홀수 번째 중 3, 7, 11번째 총알은 왼쪽으로
                        }
                    }


                    GameObject instantBullet = Instantiate(smallBulletPrefab, transform.position + offset, Quaternion.identity);


                    Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();

                    // 총알 속도 설정
                    rigidBullet.velocity = offset.normalized * 10.0f;

                    instantBullet.transform.LookAt(target);

                    yield return new WaitForSeconds(0.4f);
                    

                }

                

                isShoot = false;
            }
      }
    

    void LazerShoot()
    {
        instantLazer = Instantiate(lazer, lazerPort.position, lazerPort.rotation);
        instantLazer.transform.LookAt(target.position);
        lazerPort.transform.LookAt(target.position);

        Invoke("LazerDestroy", 1.5f);
        //Debug.Log($"작동:{Invoke}");
    }

    void LazerDestroy()
    {
        Destroy(instantLazer);
    }

    public Vector3 BigBrick()
    {
        Vector3 forwardPos = transform.forward;
        forwardPos.y += 45f;

        Vector3 combinedVector = bigBrickPort.forward * power +
                              bigBrickPortRight.forward * power * 2.0f +
                              bigBrickPortLeft.forward * power * 5.0f;


        return combinedVector;
    }

    void BigBrickShoot()
    {

        GameObject instantBrick = Instantiate(bigBrick, bigBrickPort.position, Quaternion.identity);
        instantBrick.GetComponent<Rigidbody>().AddForce(BigBrick(), ForceMode.Impulse);

        GameObject instantBrickLeft = Instantiate(bigBrick, bigBrickPortLeft.position, Quaternion.identity);
        instantBrickLeft.GetComponent<Rigidbody>().AddForce(BigBrick(), ForceMode.Impulse);

        GameObject instantBrickRight = Instantiate(bigBrick, bigBrickPortRight.position, Quaternion.identity);
        instantBrickRight.GetComponent<Rigidbody>().AddForce(BigBrick(), ForceMode.Impulse);
    }

    public Vector3 CalculateForce()
    {
        Vector3 targetPos = target.transform.position;
        targetPos.y += 45f;
        testPosition.LookAt(targetPos);  

        return testPosition.forward * power;
    }

    void ExplosionShoot()
    {
        GameObject test = Instantiate(testPrefab, testPosition.position, Quaternion.identity);
        test.GetComponent<Rigidbody>().AddForce(CalculateForce(), ForceMode.Impulse);
    }

    void BounceShoot()
    {
        GameObject instantBounce = Instantiate(bounce, bouncePort.position, bouncePort.rotation);

        GameObject bounceLeft = Instantiate(bounce, bouncePortLeft.position, bouncePortLeft.rotation);

        GameObject bounceRight = Instantiate(bounce, bouncePortRight.position, bouncePortRight.rotation);
    }

    public void OnDeal()
    {
        if (hp >= 0)
        {
            Debug.Log($"Current HP: {hp}");
        }

        if (hp <= 0)
        {
            isDie = true;
            Debug.Log($"isDie:{isDie}");
            StopAllCoroutines();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("인식되냐");
            InitializeBoss();
        }
    }

}