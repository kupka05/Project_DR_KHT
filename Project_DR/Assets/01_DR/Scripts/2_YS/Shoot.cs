using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject smallBulletPrefab;
    //public DamageCollider damageCollider;

    public bool isShoot = false;
    public bool isThink = false;

    public int tableID;

    public float attack = 0.2f;

    [Header("테이블 관련")]
    public float bulletCount = default;
    public float speed = default;
    public float destoryTime = default;
    public float delayTime = default;
    public float delay = default;
    
    
    public Transform target;

    void Awake()
    {
        GetData(tableID);
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
       
        StartCoroutine(Think());
    }

    

    public void GetData(int tableID)
    {
        //6913
        speed = (float)DataManager.Instance.GetData(tableID, "Speed", typeof(float));
        bulletCount = (float)DataManager.Instance.GetData(tableID, "Duration", typeof(float));
        destoryTime = (float)DataManager.Instance.GetData(tableID, "DesTime", typeof(float));
        delayTime = (float)DataManager.Instance.GetData(tableID, "DelTime", typeof(float));
        delay = (float)DataManager.Instance.GetData(tableID, "Delay", typeof(float));
        

    }

    IEnumerator Think()
    {
        if(!isThink)
        {
            isThink = true;
            yield return new WaitForSeconds(delayTime);
            isThink = false;
            StartCoroutine(PlayShoot());
            yield return new WaitForSeconds(delayTime);

        }
      

    }

    IEnumerator PlayShoot()
    {
        if (!isShoot)
        {
            isShoot = true;

            for (int i = 0; i < bulletCount; i++)
            {
                // 위치 조절
                Vector3 offset = Vector3.zero;

                offset = new Vector3(UnityEngine.Random.insideUnitCircle.x * 2.0f, 2.0f, UnityEngine.Random.insideUnitCircle.y * 2.0f);

                //기존 로직
                //if (i % 2 == 0)
                //{
                //    offset = new Vector3(2.0f, 0, 0); // 짝수 번째 총알은 오른쪽으로
                //}
                //else
                //{
                //    if (i % 4 == 1)
                //    {
                //        offset = new Vector3(0, 2.0f, 0); // 홀수 번째 중 1, 5, 9번째 총알은 위로
                //    }
                //    else
                //    {
                //        offset = new Vector3(-2.0f, 0, 0); // 홀수 번째 중 3, 7, 11번째 총알은 왼쪽으로
                //    }
                //}

                //GameObject instantBullet =
                //    Instantiate(smallBulletPrefab, transform.position + offset, Quaternion.identity);

                GameObject instantBullet = ObjectPoolManager.GetObject();
                instantBullet.transform.position = transform.position + offset;
                instantBullet.transform.rotation = Quaternion.identity;
                instantBullet.transform.LookAt(target);

                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = offset.normalized * speed;   //10.0f;

                yield return new WaitForSeconds(delay);   //0.4f

                //Destroy(instantBullet, destoryTime);
                yield return new WaitForSeconds(6.0f);

                ObjectPoolManager.ReturnObjectToQueue(this.gameObject);
            }
            isShoot = false;
        }
    }
}
