using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [Header("테이블 관련")]
    public float damage = default;
    public float speed = default;

    private Rigidbody rigid;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        rigid.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
