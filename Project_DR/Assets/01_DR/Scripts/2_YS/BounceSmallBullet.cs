using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class BounceSmallBullet : MonoBehaviour
{
    public Rigidbody rigid;
    public DamageCollider damageCollider;

    public int BounceSmallTableID;

    public float attack = 0.3f;
    
    public float damage = default;
    public float destoryTime = default;

    public Transform target;

    public float damageRadius = 1.0f;

    public bool isDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        GetData(BounceSmallTableID);
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
        rigid = GetComponent<Rigidbody>();
        //rigid.velocity = transform.forward * 10.0f;

        damageCollider.Damage = damage;

        Invoke(nameof(Return), destoryTime);
    }

    void Update()
    {
        DealDamageToNearbyObjects();
    }

    public virtual void GetData(int BounceSmallTableID)
    {
        //6913
        damage = (float)DataManager.Instance.GetData(BounceSmallTableID, "Damage", typeof(float));
        destoryTime = (float)DataManager.Instance.GetData(BounceSmallTableID, "DesTime", typeof(float));
    }

    void Return()
    {
        ObjectPoolManager.ReturnObjectToQueue(this.gameObject, ObjectPoolManager.ProjectileType.BOUNCEBULLET);
    }

    void DealDamageToNearbyObjects()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        if (distance <= attack && !isDamage)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    isDamage = true;
                    // 데미지를 처리하거나 플레이어 스크립트에 데미지를 전달
                    collider.GetComponent<Damageable>().DealDamage(damage);
                    GFunc.Log($"데미지:{damage}");
                }
                else if (collider.CompareTag("Weapon"))
                {
                    ObjectPoolManager.ReturnObjectToQueue(this.gameObject, ObjectPoolManager.ProjectileType.BOUNCEBULLET);
                    GFunc.Log("반환");
                    GFunc.Log("무기에 닿았는가");
                }
                else if (collider.CompareTag("Wall"))
                {
                    ObjectPoolManager.ReturnObjectToQueue(this.gameObject, ObjectPoolManager.ProjectileType.BOUNCEBULLET);
                    GFunc.Log("반환");
                    GFunc.Log("벽에 닿았는가");
                }

                ObjectPoolManager.ReturnObjectToQueue(this.gameObject, ObjectPoolManager.ProjectileType.BOUNCEBULLET);
                GFunc.Log("플레이어 데미지 후 반환");
            }
            isDamage = false;
        }
    }

}

