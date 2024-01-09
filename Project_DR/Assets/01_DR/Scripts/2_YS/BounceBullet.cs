using BNG;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BounceBullet : MonoBehaviour
{
    public Rigidbody rigid;
    public DamageCollider damageCollider;

    public int BounceTableId;

    public GameObject bounceEffect;

    public Transform target;
    public float attack = 3.0f;

    public float damageRadius = 1.9f;

    [Header("테이블 관련")]
    //public float speed = default;
    public float damage = default;
    //public float destoryTime = default;
    public float destoryTimeBounce = default;

    [Header("조건")]
    public bool isShoot = false;
    public bool isDamage = false;

    private void Awake()
    {
        GetData(BounceTableId);
    }

    // Start is called before the first frame update
    void Start()
    {


        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
        rigid = GetComponent<Rigidbody>();

        damageCollider.SetDamage(damage);


        Invoke("Return", destoryTimeBounce);
        //StartCoroutine(Activate());
    }

    void Update()
    {
        DealDamageToNearbyObjects();

    }

    void DealDamageToNearbyObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        // 디버그용: 빨간색 구체로 OverlapSphere 영역을 시각화
        //DebugDrawOverlapSphere();

        foreach (Collider collider in colliders)
        {

            if (collider.CompareTag("Player") && !isDamage)
            {
                //GFunc.Log("만났는가");
                // 데미지를 처리하거나 플레이어 스크립트에 데미지를 전달
                collider.GetComponent<Damageable>().DealDamage(damage);
                //GFunc.Log($"데미지: {damage}");

                isDamage = true;
                //Debug.Log($"isdamage: {isDamage}");

                break;
            }
        }
        isDamage = false;
        //GFunc.Log("false되냐");


    }

    void Return()
    {
        Destroy(this.gameObject);
    }

     //기존로직
    //void DealDamageToNearbyObjects()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

    //    // 디버그용: 빨간색 구체로 OverlapSphere 영역을 시각화
    //    OnDrawGizmos();

    //    foreach (Collider collider in colliders)
    //    {
    //        if (collider.CompareTag("Player") && !isDamage)
    //        {
    //            collider.GetComponent<Damageable>().DealDamage(damage);
    //            GameObject effect = Instantiate(bounceEffect, transform.position, Quaternion.identity);
    //            isDamage = true;
    //            break;
    //        }

    //        return;

    //    }
    //    isDamage = false;

    //}

    //private void OnDrawGizmos()
    //{
    //    float distance = Vector3.Distance(target.position, transform.position);

    //    if (distance <= attack)
    //    {
    //        Gizmos.color = Color.yellow;
    //    }
    //    else if(distance >= attack)
    //    {
    //        Gizmos.color = Color.red;
    //    }
    //}

    //void DebugDrawOverlapSphere()
    //{
    //    Vector3 dir = target.position - transform.position;
    //    Debug.DrawRay(transform.position, dir.normalized * damageRadius, Color.magenta);
    //}

    void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, damageRadius);
    }

    public void GetData(int BounceTableId)
    {
        //6912
        //speed = (float)DataManager.Instance.GetData(BounceTableId, "Speed", typeof(float));
        damage = (float)DataManager.Instance.GetData(BounceTableId, "Damage", typeof(float));
        destoryTimeBounce = (float)DataManager.Instance.GetData(BounceTableId, "DesTime", typeof(float));
    }

}
