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
        float distance = Vector3.Distance(target.position, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))

            if (distance <= attack)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<Damageable>().DealDamage(damage);
                    GFunc.Log($"데미지:{damage}");

                    Destroy(this.gameObject);
                }

                if (hit.collider.CompareTag("Wall"))
                {
                    Destroy(this.gameObject);
                }
            }

    }

    public virtual void GetData(int BounceSmallTableID)
    {
        //6913
        damage = (float)DataManager.instance.GetData(BounceSmallTableID, "Damage", typeof(float));
    }

    //public virtual void OnCollisionEnter(Collision collision)
    //{

    //    if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Wall"))
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}
}

