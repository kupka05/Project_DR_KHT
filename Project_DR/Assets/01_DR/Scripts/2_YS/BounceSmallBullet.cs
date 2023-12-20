using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BounceSmallBullet : MonoBehaviour
{
    public Rigidbody rigid;
    public DamageCollider damageCollider;

    public int BounceSmallTableID;

    [Header("테이블 관련")]
    public float damage = default;



    // Start is called before the first frame update
    void Start()
    {
        GetData(BounceSmallTableID);

        rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * 10.0f;

        damageCollider.Damage = damage;
    }

    public virtual void GetData(int BounceSmallTableID)
    {
        //6913
        damage = (float)DataManager.Instance.GetData(BounceSmallTableID, "Damage", typeof(float));
    }

    public virtual void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}

