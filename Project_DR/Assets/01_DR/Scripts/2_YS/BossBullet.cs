using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public int smallTableID;

    public DamageCollider damageCollider;

    public Rigidbody rigid;

    [Header("테이블")]
    public float damage = default;


    void Awake()
    {
        GetData(smallTableID);
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        damageCollider = GetComponent<DamageCollider>();
        rigid.velocity = transform.forward;

        damageCollider.Damage = damage;
    }

    void Update()
    {
        
    }

    public void GetData(int smallTableID)
    {
        //6910
        damage = (float)DataManager.instance.GetData(smallTableID, "Damage", typeof(float));
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}