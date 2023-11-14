using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;


public class Monster : MonoBehaviour
{

    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;


    public enum Type
    {
        A, B, C, D
    }
    public Type monsterType = Type.A;

    [Header("몬스터 원거리 관련")]
    public Transform bulletport;
    public GameObject monsterBullet;

    [Header("몬스터 테이블")]
    public float hp = default;
    public float attack = default;
    public float attDelay = default;   //공격간격
    public float speed = default;
    public float recRange = 50.0f;   //pc 인신범위
    public float attRange = 1.0f;   //pc 공격범위

    [Header("트랜스폼")]
    public Transform monsterTr;
    public Transform playerTr;

    [Header("몬스터 컴포넌트")]
    public Animator anim;
    public Rigidbody rigid;
    public NavMeshAgent nav;

    public readonly int hashRun = Animator.StringToHash("isRun");
    public readonly int hashAttack = Animator.StringToHash("isAttack");

    public bool isDie = false;

    // Start is called before the first frame update
    void Start()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();

        StartCoroutine(MonsterBehaviour());
    }

    void Update()
    {
        if (state == State.TRACE || state == State.ATTACK)
        {
            transform.LookAt(playerTr.position);
        }

    }


    IEnumerator MonsterBehaviour()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.1f);

            if (state == State.DIE) yield break;

            float distance = Vector3.Distance(monsterTr.position, playerTr.position);

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

           
            switch (state)
            {
                case State.IDLE:
                    nav.isStopped = true;
                    anim.SetBool(hashRun, false);
                    break;

                case State.TRACE:
                    nav.isStopped = false;
                    nav.SetDestination(playerTr.position);
                    anim.SetBool(hashRun, true);
                    anim.SetBool(hashAttack, false);
                    
                    break;

                case State.ATTACK:

                    switch (monsterType)
                    {
                        case Type.A:
                            anim.SetBool(hashAttack, true);
                            
                            yield return new WaitForSeconds(1.0f);

                            break;
                        case Type.B:
                            anim.SetBool(hashAttack, true);
                           
                            GameObject instantBullet = Instantiate(monsterBullet, bulletport.position, bulletport.rotation);
                            bulletport.LookAt(playerTr.position);

                            yield return new WaitForSeconds(2.0f);
                            break;
                    }
                    break;

                case State.DIE:
                    isDie = true;
                    nav.isStopped = true;
                    //anim
                    StopAllCoroutines();
                    Destroy(this.gameObject, 2.0f);
                    break;

            }
        }
        yield return null;
    }

    //void DistanceCheck()
    //{
    //    if(state == State.TRACE)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireSphere(transform.position, recRange);
    //    }

    //    if(state == State.ATTACK)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(transform.position, attRange);
    //    }

    //}

    private void OnTriggerEnter(Collider other)
    {
        //onDamage();
    }

    public void onDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            state = State.DIE;
        }

    }
}



