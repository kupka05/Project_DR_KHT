//using BNG;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static UnityEngine.GraphicsBuffer;
//using static UnityEngine.Rendering.DebugUI;

//public class BossBullet : MonsterBullet
//{
//    public bool isShoot = false;

//    public Transform target;
//    public Rigidbody rigid;

//    [Header("테이블 관련")]
//    private float hp = default;
//    private float bulletCount = 6;

//    [Header("테이블 아이디")]
//    public int tableId;

//    void Awake()
//    {
//        GetData(tableId);
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        rigid = GetComponent<Rigidbody>();
//        rigid.velocity = transform.forward * speed;

//        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;

//        StartCoroutine(PlayShoot());
//    }

//    public void GetData(int tableId)
//    {
//        hp = (float)DataManager.instance.GetData(tableId, "Hp", typeof(float));
//        speed = (float)DataManager.instance.GetData(tableId, "Speed", typeof(float));
//    }

//    public IEnumerator PlayShoot()
//    {
//        if (!isShoot)
//        {
//            isShoot = true;

//            for (int i = 0; i < bulletCount; i++)
//            {
//                // 위치 조절
//                Vector3 offset = Vector3.zero;

//                if (i % 2 == 0)
//                {
//                    offset = new Vector3(2.0f, 0, 0);
//                }
//                else
//                {
//                    if (i % 4 == 1)
//                    {
//                        offset = new Vector3(0, 2.0f, 0);
//                    }
//                    else
//                    {
//                        offset = new Vector3(-2.0f, 0, 0);
//                    }
//                }


//                //GameObject instantBullet = Instantiate(smallBulletPrefab, transform.position + offset, Quaternion.identity);
//                //Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();

//                GameObject instantBullet = new GameObject(); // 새로운 GameObject 생성
//                Rigidbody rigidBullet = instantBullet.AddComponent<Rigidbody>(); // Rigidbody 추가

//                // 총알 속도 설정
//                rigidBullet.velocity = offset.normalized * 10.0f;

//                instantBullet.transform.LookAt(target);

//                yield return new WaitForSeconds(0.4f);

//                Destroy(instantBullet, 6.0f);

//            }

//            isShoot = false;
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (hp >= 0)
//        {
//            Destroy(this.gameObject);
//        }
//    }
//}
