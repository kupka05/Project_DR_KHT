using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [Header("테이블 관련")]
    [SerializeField]
    private float speed = 15.0f;

    private Rigidbody rigid;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        rigid.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
