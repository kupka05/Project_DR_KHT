using BNG;
using UnityEngine;

public class GuidedBullet : MonoBehaviour
{
    public DamageCollider damageCollider;

    public int GuidedTableId;

    public Transform target;

    private Vector3 initialDirection;
    
    [Header("테이블 관련")]
    public float speed = default;
    public float turnSpeed = 15.0f;
    public float damage = default;
    public float destoryTime = default;

    void Awake()
    {
        GetData(GuidedTableId);
    }

    void Start()
    {
        // 플레이어를 찾아서 타겟으로 설정
        target = GameObject.FindWithTag("Player")?.GetComponent<PlayerPosition>()?.playerPos;

        // 타겟이 없을 경우 리턴
        if (target == null)
        {
            return;
        }

        // 초기 방향 벡터 계산
        initialDirection = target.position - transform.position;

        damageCollider.Damage = damage;


    }

    void Update()
    {
        Shoot();
    }

    public virtual void GetData(int GuidedTableId)
    {
        //6911
        damage = (float)DataManager.instance.GetData(GuidedTableId, "Damage", typeof(float));
        destoryTime = (float)DataManager.instance.GetData(GuidedTableId, "DesTime", typeof(float));
        speed = (float)DataManager.instance.GetData(GuidedTableId, "Speed", typeof(float));

    }

    void Shoot()
    {
        // 초기 방향 벡터의 크기가 0인 경우 리턴
        if (initialDirection.magnitude == 0f)
            return;

        float t = speed; // 원하는 시간 비율로 조정할 것
        transform.position = Vector3.LerpUnclamped(transform.position, target.position, t);

        // Lerp를 이용하여 위치 이동
        transform.position = Vector3.LerpUnclamped(transform.position, target.position, t);

        // 타겟 방향으로 회전
        Vector3 directionVec = target.position - transform.position;
        Quaternion qua = Quaternion.LookRotation(directionVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, Time.deltaTime * turnSpeed);

        Destroy(this.gameObject, destoryTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player") || collision.collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
            Debug.Log($"파괴:{this.gameObject}");
        }
    }
}
