using BNG;
using Oculus.Interaction;
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
    public float destoryTime = default;

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
        damageCollider.SetDamage(damage);

        Invoke("Return", destoryTime);
    }

    void Update()
    {
        // 주기적으로 주변의 오브젝트를 확인하여 데미지를 적용
        DealDamageToNearbyObjects();
    }

    void Return()
    {
        Destroy(this.gameObject);
    }

    void DealDamageToNearbyObjects()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        if (distance <= attack && !isDamage)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Weapon"))
                {
                    Destroy(this.gameObject);
                    GameObject effect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
                    float destoryTime = 2.0f;
                    Destroy(effect, destoryTime);
                    //GFunc.Log("연쇄총알 무기에 닿았는가");
                    //ObjectPoolManager.ReturnObjectToQueue(this.gameObject, ObjectPoolManager.ProjectileType.CHAINBULLET);
                    //GFunc.Log("무기에 닿았을 때 반환한다");
                    return;
                }

                if (collider.CompareTag("Wall"))
                {
                    Destroy(this.gameObject);
                    GameObject effect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
                    float destoryTime = 2.0f;
                    Destroy(effect, destoryTime);
                    //GFunc.Log("연쇄총알 벽에 닿았는가");
                    //ObjectPoolManager.ReturnObjectToQueue(this.gameObject, ObjectPoolManager.ProjectileType.CHAINBULLET);
                    //GFunc.Log("벽에 닿았을 때 반환한다");
                    return;
                }
                //ObjectPoolManager.ReturnObjectToQueue(this.gameObject, ObjectPoolManager.ProjectileType.CHAINBULLET);
                //GFunc.Log("플레이어 데미지 후 반환");


                if (collider.CompareTag("Player"))
                {
                    isDamage = true;
                    // 데미지를 처리하거나 플레이어 스크립트에 데미지를 전달
                    collider.GetComponent<Damageable>().DealDamage(damage);
                    GameObject effect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
                    float destoryTime = 2.0f;
                    Destroy(effect, destoryTime);
                    //GFunc.Log($"데미지:{damage}");
                    break;
                }
                Destroy(this.gameObject);
            }
            isDamage = false;
        }

    }

    public void GetData(int smallTableID)
    {
        //6910
        damage = (float)DataManager.Instance.GetData(smallTableID, "Damage", typeof(float));
        destoryTime = (float)DataManager.Instance.GetData(smallTableID, "DesTime", typeof(float));
        
    }

    

}