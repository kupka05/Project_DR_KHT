using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MonsterBullet : MonoBehaviour
{
    public Rigidbody rigid;
    public DamageCollider damageCollider;

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
        
        damageCollider = GetComponent<DamageCollider>();
        rigid = GetComponent<Rigidbody>();

        rigid.velocity = transform.forward * speed;

        damageCollider.Damage = damage;
    }
    
    public virtual void GetData(int ProjectileID)
    {
        speed = (float)DataManager.instance.GetData(ProjectileID, "MonSpd", typeof(float));
        damage = (float)DataManager.instance.GetData(ProjectileID, "MonAtt", typeof(float));
    }

    //public virtual void OnCollisionEnter(Collision collision)
    //{

    //    if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Wall"))
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag.Equals("Player"))
        {
            //GFunc.Log("부딪힘");
            other.GetComponent<Damageable>().DealDamage(damage);
            //GFunc.Log($"damage:{damage}");
            Destroy(this.gameObject);
        }

        if(other.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

}
