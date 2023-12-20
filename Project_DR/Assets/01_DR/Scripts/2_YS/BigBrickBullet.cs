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
                }

            }

    }

    public void GetData(int brickTableId)
    {
        //6914
        damage = (float)DataManager.Instance.GetData(brickTableId, "Damage", typeof(float));
      

    }

}
