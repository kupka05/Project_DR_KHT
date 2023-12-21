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

    public Transform target;

    public float damageRadius = 1.0f;

    public bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        GetData(BounceSmallTableID);
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * 10.0f;

        damageCollider.Damage = damage;
    }

    void Update()
    {
        DealDamageToNearbyObjects();
    }

    public virtual void GetData(int BounceSmallTableID)
    {
        //6913
        damage = (float)DataManager.Instance.GetData(BounceSmallTableID, "Damage", typeof(float));
    }

    void DealDamageToNearbyObjects()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        if (distance <= attack)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    // 데미지를 처리하거나 플레이어 스크립트에 데미지를 전달
                    collider.GetComponent<Damageable>().DealDamage(damage);
                    GFunc.Log($"데미지:{damage}");

                    isHit = true;
                    Destroy(this.gameObject);
                    break;
                }
            }
        }


    }

    //public virtual void OnCollisionEnter(Collision collision)
    //{

    //    if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Wall"))
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}
}

