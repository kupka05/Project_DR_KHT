using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject smallBulletPrefab;

    public bool isShoot = false;
    public bool isThink = false;

    public int bulletCount = 12;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject playerObject = GameObject.FindWithTag("Player");
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
        //if (playerObject != null)
        //{
        //    target = playerObject.GetComponent<PlayerPosition>().playerPos;
        //}
        //else
        //{
        //    Debug.LogError("씬에서 'Player' 태그가 지정된 플레이어를 찾을 수 없습니다.");
        //}
        StartCoroutine(Think());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Think()
    {
        if(!isThink)
        {
            isThink = true;
            yield return new WaitForSeconds(2.0f);
            isThink = false;
            StartCoroutine(PlayShoot());
            yield return new WaitForSeconds(2.0f);

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

                if (i % 2 == 0)
                {
                    offset = new Vector3(2.0f, 0, 0); // 짝수 번째 총알은 오른쪽으로
                }
                else
                {
                    if (i % 4 == 1)
                    {
                        offset = new Vector3(0, 2.0f, 0); // 홀수 번째 중 1, 5, 9번째 총알은 위로
                    }
                    else
                    {
                        offset = new Vector3(-2.0f, 0, 0); // 홀수 번째 중 3, 7, 11번째 총알은 왼쪽으로
                    }
                }


                GameObject instantBullet = Instantiate(smallBulletPrefab, transform.position + offset, Quaternion.identity);
                

                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();

                // 총알 속도 설정
                rigidBullet.velocity = offset.normalized * 10.0f;

                instantBullet.transform.LookAt(target);

                yield return new WaitForSeconds(0.2f);

            }

            yield return new WaitForSeconds(2.0f);
            Destroy(smallBulletPrefab, 3.0f);

            isShoot = false;
        }
    }
}
