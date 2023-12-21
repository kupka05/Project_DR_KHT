using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BounceBullet : MonoBehaviour
{
    public Rigidbody rigid;
    public DamageCollider damageCollider;

    public int BounceTableId;

    public GameObject bounceEffect;

    public Transform target;
    public float attack = 0.2f;

    [Header("테이블 관련")]
    public float speed = default;
    public float damage = default;
    public float destoryTime = default;

    [Header("조건")]
    public bool isShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        GetData(BounceTableId);

        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
        rigid = GetComponent<Rigidbody>();

        damageCollider.Damage = damage;

        StartCoroutine(Activate());
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

                    //GameObject instanceEffect = Instantiate(bounceEffect, transform.position, Quaternion.identity);

                }

            }

        Destroy(this.gameObject, 8.0f);
        //GameObject instanceEffectDestroy = Instantiate(bounceEffect, transform.position, Quaternion.identity);

    }

    IEnumerator Activate()
    {
        // 오브젝트 활성화
        gameObject.SetActive(true);
        //GFunc.Log($"활성화:{gameObject}");

        
        GFunc.Log($"포지션:{gameObject.transform.position}");
        yield return new WaitForSeconds(2.0f);
        //GFunc.Log("대기중");
        
        Play();
    }

    void Play()
    {
        rigid.velocity = transform.forward * speed;
    }

    

    public virtual void GetData(int BounceTableId)
    {
        //6912
        speed = (float)DataManager.Instance.GetData(BounceTableId, "Speed", typeof(float));
        damage = (float)DataManager.Instance.GetData(BounceTableId, "Damage", typeof(float));
        destoryTime = (float)DataManager.Instance.GetData(BounceTableId, "DesTime", typeof(float));
    }

}
