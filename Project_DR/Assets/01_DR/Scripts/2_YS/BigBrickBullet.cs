using BNG;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    public float brickHp = 3;

    public GameObject brickStoneEffect;
    public GameObject birckSmokeEffect;
    public GameObject brickFloorEffect;

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


    public void GetData(int brickTableId)
    {
        //6914
        damage = (float)DataManager.Instance.GetData(brickTableId, "Damage", typeof(float));
        brickHp = (float)DataManager.Instance.GetData(brickTableId, "Hp", typeof(float));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
            //ObjectPoolManager.ReturnObjectToQueue(this.gameObject, ObjectPoolManager.ProjectileType.BIGBRICK);
        }

        if(collision.collider.CompareTag("Player"))
        {

            collision.collider.GetComponent<Damageable>().DealDamage(damage);
            
            //ObjectPoolManager.ReturnObjectToQueue(this.gameObject, ObjectPoolManager.ProjectileType.BIGBRICK);
        }

        if(collision.collider.CompareTag("Floor"))
        {
            GameObject effectFloor = Instantiate(brickFloorEffect, transform.position, Quaternion.identity);
        }

        if(collision.collider.CompareTag("Weapon"))
        {
            brickHp--;
            GameObject effectStone = Instantiate(brickStoneEffect, transform.position, Quaternion.identity);
            GameObject effectSmoke = Instantiate(birckSmokeEffect, transform.position, Quaternion.identity);
            //ObjectPoolManager.ReturnObjectToQueue(this.gameObject, ObjectPoolManager.ProjectileType.BIGBRICK);
        }

        if(brickHp == 0)
        {
            Destroy(this.gameObject);
        }
    }

}
