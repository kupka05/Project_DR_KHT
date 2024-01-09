using BNG;
using UnityEngine;
using System.Collections;

public class GuidedBullet : MonoBehaviour
{
    public DamageCollider damageCollider;
    public int GuidedTableId;
    public Transform target;

    [Header("테이블 관련")]
    public float speed = default;
    public float turnSpeed = 15.0f;
    public float damage = default;
    public float destoryTime = default;

    private void Awake()
    {
        GetData(GuidedTableId);
    }

    private void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;

        if (target == null)
            return;

        Vector3 initialDirection = target.position - transform.position;
        damageCollider.SetDamage(damage);
    }

    private void Update()
    {
        
    }

    public void GetData(int GuidedTableId)
    {
        damage = (float)DataManager.Instance.GetData(GuidedTableId, "Damage", typeof(float));
        destoryTime = (float)DataManager.Instance.GetData(GuidedTableId, "DesTime", typeof(float));
        speed = (float)DataManager.Instance.GetData(GuidedTableId, "Speed", typeof(float));
    }

    //public void Shoot()
    //{
    //    StartCoroutine(MoveTowardsTarget());
    //}

    //IEnumerator MoveTowardsTarget()
    //{
    //    while (target != null)
    //    {
    //        Vector3 direction = (target.position - transform.position).normalized;
    //        float step = speed * Time.deltaTime;
    //        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

    //        Quaternion qua = Quaternion.LookRotation(direction);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, qua, Time.deltaTime * turnSpeed);

    //        // 일정 시간 동안 대기 (예: 0.2초)
    //        yield return new WaitForSeconds(0.2f);
    //    }

    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
            GFunc.Log($"파괴:{gameObject}");
        }
    }
}