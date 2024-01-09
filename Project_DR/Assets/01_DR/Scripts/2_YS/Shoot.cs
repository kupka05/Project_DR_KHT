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
    public float destoryTimeBounceSmall = default;
    public float delay = default;
    public float shotDelay = default;
    
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
        destoryTimeBounceSmall = (float)DataManager.Instance.GetData(tableID, "DesTime", typeof(float));
        delay = (float)DataManager.Instance.GetData(tableID, "Delay", typeof(float));
        shotDelay = (float)DataManager.Instance.GetData(tableID, "DelTime", typeof(float));

    }

    IEnumerator Think()
    {
        if (!isThink)
        {
            isThink = true;
            yield return new WaitForSeconds(shotDelay);
            isThink = false;
            StartCoroutine(PlayShoot());
            yield return new WaitForSeconds(shotDelay);

        }


    }
    IEnumerator PlayShoot()
    {

        if (!isShoot)
        {
            isShoot = true;

            List<GameObject> bullets = new List<GameObject>();

            // 총알 미리 생성
            for (int i = 0; i < bulletCount; i++)
            {
                Vector3 offset = new Vector3(UnityEngine.Random.insideUnitCircle.x * 2.0f, UnityEngine.Random.insideUnitCircle.y * 2.0f);

                //기존 로직
                GameObject instantBullet = Instantiate(smallBulletPrefab, transform.position + offset, Quaternion.identity);
                instantBullet.transform.LookAt(target.position);
                
                bullets.Add(instantBullet);
                
                //instantBullet.transform.LookAt(target.position);

                // 오브젝트 풀을 사용하여 총알을 가져옵니다.
                //GameObject instantBullet = ObjectPoolManager.GetObject(ObjectPoolManager.ProjectileType.BOUNCEBULLET);
                //GFunc.Log("오브젝트 풀 생성");
                //instantBullet.transform.position = transform.position + offset;
                //instantBullet.transform.rotation = Quaternion.identity;
                //instantBullet.transform.LookAt(target.position);

               // bullets.Add(instantBullet);

            }

            //GFunc.Log($"리스트 크기 : {bullets.Count}");

            foreach (GameObject bullet in bullets)
            {
                if (bullet != null && bullet.activeSelf)
                {
                    Rigidbody rigidBullet = bullet.GetComponent<Rigidbody>();
                    rigidBullet.transform.LookAt(target.position);

                    yield return new WaitForSeconds(delay);

                    rigidBullet.velocity = (target.position - bullet.transform.position).normalized * speed;
                }
            }

            //yield return new WaitForSeconds(destoryTimeBounceSmall);
            //// 오브젝트 풀을 사용하여 총알을 반환합니다.
            //foreach (GameObject bullet in bullets)
            //{
            //     Destroy(smallBulletPrefab);
            //    //ObjectPoolManager.ReturnObjectToQueue(bullet, ObjectPoolManager.ProjectileType.BOUNCEBULLET);
            //    //GFunc.Log("반환 이상 없이 작동하는가?");
            //}

            bullets.Clear();

            isShoot = false;
            //GFunc.Log($"isShoot:{isShoot}");
        }
    }

    //IEnumerator PlayShoot()
    //{
    //    if (!isShoot)
    //    {
    //        isShoot = true;

    //        for (int i = 0; i < bulletCount; i++)
    //        {
    //            // 위치 조절
    //            //Vector3 offset = Vector3.zero;

    //            Vector3 offset = new Vector3(UnityEngine.Random.insideUnitCircle.x * 2.0f, 2.0f, UnityEngine.Random.insideUnitCircle.y * 2.0f);

    //            GameObject instantBullet = ObjectPoolManager.GetObject(ObjectPoolManager.ProjectileType.BOUNCEBULLET);
    //            instantBullet.transform.position = transform.position + offset;
    //            instantBullet.transform.rotation = Quaternion.identity;
    //            instantBullet.transform.LookAt(target);

    //            Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
    //            rigidBullet.velocity = offset.normalized * speed;   //10.0f;

    //            yield return new WaitForSeconds(delay);   

    //            //Destroy(instantBullet, destoryTime);
    //            yield return new WaitForSeconds(destoryTimeBounceSmall);

    //            ObjectPoolManager.ReturnObjectToQueue(this.gameObject);
    //        }

    //        foreach(GameObject )

    //        isShoot = false;
    //    }
    //}
}
