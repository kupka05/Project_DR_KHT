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

        transform.LookAt(target.position);
        //rigid.velocity = transform.forward * 10.0f;
        damageCollider.Damage = damage;

        StartCoroutine(DestroyGameObject());
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
                    GameObject instanceEffect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
                }

                if(hit.collider.CompareTag("Wall"))
                {
                    Destroy(this.gameObject);
                    GameObject instanceEffect = Instantiate(bulletEffect, transform.position, Quaternion.identity);
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