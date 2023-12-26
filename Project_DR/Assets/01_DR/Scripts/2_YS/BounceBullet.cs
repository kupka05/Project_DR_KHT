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
    public float attack = 3.0f;

    [SerializeField]
    private float damageRadius = 1.5f;

    [Header("테이블 관련")]
    public float speed = default;
    public float damage = default;
    public float destoryTime = default;

    [Header("조건")]
    public bool isShoot = false;
    public bool isDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        GetData(BounceTableId);

        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
        rigid = GetComponent<Rigidbody>();

        damageCollider.Damage = damage;

        //StartCoroutine(Activate());
    }

    void Update()
    {
        DealDamageToNearbyObjects();

    }

    void DealDamageToNearbyObjects()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

        // 디버그용: 빨간색 구체로 OverlapSphere 영역을 시각화
        DebugDrawOverlapSphere();

        //Debug.Log("진입"+ distance); 

        foreach (Collider collider in colliders)
        {
            if (!isDamage)
            {
                if (collider.CompareTag("Player"))
                {
                    GFunc.Log("만났는가");
                    // 데미지를 처리하거나 플레이어 스크립트에 데미지를 전달
                    collider.GetComponent<Damageable>().DealDamage(damage);
                    GFunc.Log($"데미지:{damage}");

                    isDamage = true;
                    //Destroy(this.gameObject);
                    break;
                }
                ObjectPoolManager.ReturnObjectToQueue(this.gameObject);
                //isDamage = false;
            }

        }
        isDamage = false;


    }

    void DebugDrawOverlapSphere()
    {
        Vector3 dir = target.position - transform.position;
        Debug.DrawRay(transform.position, dir.normalized * damageRadius, Color.yellow);
    }

    public virtual void GetData(int BounceTableId)
    {
        //6912
        speed = (float)DataManager.Instance.GetData(BounceTableId, "Speed", typeof(float));
        damage = (float)DataManager.Instance.GetData(BounceTableId, "Damage", typeof(float));
        //destoryTime = (float)DataManager.Instance.GetData(BounceTableId, "DesTime", typeof(float));
    }

}
