using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public Transform target;

    public float maxChaseDistance = 10.0f;

    public float rotationSpeed = 15.0f;


    private void Start()
    {
        //target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

        // 플레이어를 지나친 경우 target을 null로 설정하여 추적을 중지합니다.
        if (Vector3.Distance(transform.position, target.position) > maxChaseDistance)
        {
            target = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player") || collision.collider.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
