using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BigBrickBullet : MonoBehaviour
{
    public DamageCollider damageCollider;

    public float damage = default;

    public float attack = 0.2f;

    public Transform target;

    public int brickTableId;

    public bool isCheck = false;
    public float damageRadius = 1.0f;

    void Awake()
    {
        GetData(brickTableId); 
    }

    // Start is called before the first frame update
    void Start()
    {
        damageCollider = GetComponent<DamageCollider>();
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
        damageCollider.Damage = damage;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    // 주기적으로 주변의 오브젝트를 확인하여 데미지를 적용
    //    DealDamageToNearbyObjects();
    //}

    //void DealDamageToNearbyObjects()
    //{
    //    float distance = Vector3.Distance(target.position, transform.position);

    //    Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

    //    if (distance <= attack)
    //    {
    //        foreach (Collider collider in colliders)
    //        {
    //            if (collider.CompareTag("Player"))
    //            {
    //                // 데미지를 처리하거나 플레이어 스크립트에 데미지를 전달
    //                collider.GetComponent<Damageable>().DealDamage(damage);
    //                GFunc.Log($"데미지:{damage}");

    //                isCheck = true;
    //                Destroy(this.gameObject);
    //                break;
    //            }

    //        }
    //    }

    //}

    public void GetData(int brickTableId)
    {
        //6914
        damage = (float)DataManager.Instance.GetData(brickTableId, "Damage", typeof(float));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            ObjectPoolManager.ReturnObjectToQueue(this.gameObject);
        }

        if(collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<Damageable>().DealDamage(damage);
            ObjectPoolManager.ReturnObjectToQueue(this.gameObject);
        }

        if(collision.collider.CompareTag("Weapon"))
        {
            ObjectPoolManager.ReturnObjectToQueue(this.gameObject);
        }
    }

}
