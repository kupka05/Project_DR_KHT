using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossBullet : MonoBehaviour
{
    public int smallTableID;

    public DamageCollider damageCollider;

    public Rigidbody rigid;

    public Transform target;

    public float attack = 0.2f;

    public float damageRadius = 1.0f;

    public bool isDamage = false;

    [Header("이펙트")]
    public GameObject bulletEffect;

    [Header("테이블")]
    public float damage = default;


    void Awake()
    {
        GetData(smallTableID);
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
        damageCollider = GetComponent<DamageCollider>();

        //rigid.velocity = transform.forward * 10.0f;
        damageCollider.Damage = damage;

        StartCoroutine(DestroyGameObject());
    }

    //void Update()
    //{


    //    float distance = Vector3.Distance(target.position, transform.position);

    //    RaycastHit hit;

    //    if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))

    //        if (distance <= attack)
    //        {
    //            if (hit.collider.CompareTag("Player"))
    //            {
    //                hit.collider.GetComponent<Damageable>().DealDamage(damage);
    //                GFunc.Log($"데미지:{damage}");

    //                Destroy(this.gameObject);
    //                GameObject instanceEffect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
    //            }

    //            if (hit.collider.CompareTag("Wall"))
    //            {
    //                Destroy(this.gameObject);
    //                GameObject instanceEffect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
    //            }
    //        }

    //}

    void Update()
    {
        // 주기적으로 주변의 오브젝트를 확인하여 데미지를 적용
        DealDamageToNearbyObjects();
    }

    void DealDamageToNearbyObjects()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        if(distance <= attack)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    // 데미지를 처리하거나 플레이어 스크립트에 데미지를 전달
                    collider.GetComponent<Damageable>().DealDamage(damage);
                    GFunc.Log($"바운스 데미지:{damage}");

                    isDamage = true;
                    Destroy(this.gameObject);
                    break;
                }

            }
        }

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Wall"))
            {
                Destroy(this.gameObject);
                GFunc.Log("벽이나 바닥 만났을 때 파괴되는가");
            }
        }

    }

    public void GetData(int smallTableID)
    {
        //6910
        damage = (float)DataManager.Instance.GetData(smallTableID, "Damage", typeof(float));
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(8.0f);

        Destroy(this.gameObject);
    }

}