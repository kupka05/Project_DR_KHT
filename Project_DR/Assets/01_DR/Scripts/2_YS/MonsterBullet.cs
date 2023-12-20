using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MonsterBullet : MonoBehaviour
{
    public Rigidbody rigid;
    public DamageCollider damageCollider;
    public Transform target;

    public float attack = 0.2f;

    public int ProjectileID;

    [Header("테이블 관련")]
    public float speed = default;
    public float damage = default;

    void Awake()
    {
        GetData(ProjectileID);
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
        damageCollider = GetComponent<DamageCollider>();
        rigid = GetComponent<Rigidbody>();

        transform.LookAt(target.position);

        rigid.velocity = transform.forward * speed;

        damageCollider.Damage = damage;
    }

    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))

        if (distance <= attack)
        {
                if(hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<Damageable>().DealDamage(damage);
                    GFunc.Log($"데미지:{damage}");

                    Destroy(this.gameObject);
                }


        }

    }

    public virtual void GetData(int ProjectileID)
    {
        speed = (float)DataManager.Instance.GetData(ProjectileID, "MonSpd", typeof(float));
        damage = (float)DataManager.Instance.GetData(ProjectileID, "MonAtt", typeof(float));
    }


}
