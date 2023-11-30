using BNG;
using Oculus.Interaction.Throw;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody rigid;

    public Damageable damageable;

    GameObject instantLazer;

    //public GameObject decal;

    //public Transform tr;
    [Header("레이저 관련")]
    public Transform lazerPort;
    public GameObject lazer;

    [Header("원거리 공격")]
    public Transform bulletPort;
    public Transform bulletPortLeft;
    public Transform bulletPortRight;
    public GameObject smallBullet;

    [Header("원거리 공격 2번째")]
    public Transform bulletTopLeft;
    public Transform bulletTopRight;
    public GameObject bigBullet;

    [Header("타겟")]
    public Transform target;

    [Header("테이블")]
    public float hp = default;

    [Header("조건")]
    public bool isLazer = false;
    public bool isPattern = false;

    // Start is called before the first frame update
    void Start()
    {
        //GetData();

        damageable.Health = hp;

        rigid = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;

        StartCoroutine(PatternPick());
        StartCoroutine(Pattern());
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.position);
    }

    //public void GetData(int id)
    //{
    //    hp = (float)DataManager.instance.GetData(id, "MonHP", typeof(float));
    //    exp = (float)DataManager.instance.GetData(id, "MonExp", typeof(float));
    //    attack = (float)DataManager.instance.GetData(id, "MonAtt", typeof(float));
    //    attDelay = (float)DataManager.instance.GetData(id, "MonDel", typeof(float));
    //    speed = (float)DataManager.instance.GetData(id, "MonSpd", typeof(float));
    //    attRange = (float)DataManager.instance.GetData(id, "MonAtr", typeof(float));
    //    recRange = (float)DataManager.instance.GetData(id, "MonRer", typeof(float));
    //    stunDelay = (float)DataManager.instance.GetData(id, "MonSTFDel", typeof(float));

    //    stopDistance = (float)DataManager.instance.GetData(id, "MonStd", typeof(float));
    //}

  
    IEnumerator Pattern()
    {
        yield return new WaitForSeconds(0.1f);

        int random = Random.Range(0, 1);

        switch(random)
        {
                case 0:
                BigShoot();
                break;
        }
    }

    IEnumerator PatternPick()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);

            if(!isPattern)
            {
                if (damageable.Health <= 1.0f)
                {
                    StartCoroutine(Pattern());
                    isPattern = true;
                }
                else if (damageable.Health <= 0.75f)
                {
                    //Vector3 backForward = transform.position - transform.forward * 5.0f;
                    //transform.position = backForward;
                }
                else if (damageable.Health <= 0.5f)
                {
                    //Vector3 backForward = transform.position - transform.forward * 5.0f;
                    //transform.position = backForward;
                }
                else if (damageable.Health <= 0.25f)
                {
                    //Vector3 backForward = transform.position - transform.forward * 5.0f;
                    //transform.position = backForward;
                }
            }

        }


    }

    void SmallShoot()
    {
        
        GameObject instantBullet = Instantiate(smallBullet, bulletPort.position, bulletPort.rotation);
        GameObject instantBulletRight = Instantiate(smallBullet, bulletPortRight.position, Quaternion.Euler(bulletPortRight.rotation.eulerAngles + new Vector3(0, 10, 0)));
        //GameObject instantBulletRight2 = Instantiate(smallBullet, bulletPort.position, Quaternion.Euler(bulletPort.rotation.eulerAngles + new Vector3(0, 60, 0)));
        //GameObject instantBulletRight3 = Instantiate(smallBullet, bulletPort.position, Quaternion.Euler(bulletPort.rotation.eulerAngles + new Vector3(0, 90, 0)));
        GameObject instantBulletLeft = Instantiate(smallBullet, bulletPortLeft.position, Quaternion.Euler(bulletPortLeft.rotation.eulerAngles + new Vector3(0, -10, 0)));
        //GameObject instantBulletLeft2 = Instantiate(smallBullet, bulletPort.position, Quaternion.Euler(bulletPort.rotation.eulerAngles + new Vector3(0, -60, 0)));
        //GameObject instantBulletLeft3 = Instantiate(smallBullet, bulletPort.position, Quaternion.Euler(bulletPort.rotation.eulerAngles + new Vector3(0, -90, 0)));



    }

    void BigShoot()
    {
        GameObject instantBulletLeft = Instantiate(bigBullet, bulletTopLeft.position, bulletTopLeft.rotation);      
        instantBulletLeft.transform.LookAt(target.position);
        bulletPortLeft.transform.LookAt(target.position);

        GameObject instantBulletRight = Instantiate(bigBullet, bulletTopRight.position, bulletTopRight.rotation);   
        instantBulletRight.transform.LookAt(target.position);
        bulletPortRight.transform.LookAt(target.position);

        //GameObject instantDecal = Instantiate(decal, transform.position, transform.rotation);
        //instantDecal.GetComponent<Renderer>().enabled = true;

    }

    void Lazer()
    {
        instantLazer = Instantiate(lazer, lazerPort.position, lazerPort.rotation);
        instantLazer.transform.LookAt(target.position);
        lazerPort.transform.LookAt(target.position);

        Invoke("LazerDestroy", 2.0f);
    }

    void LazerDestroy()
    {
        Destroy(instantLazer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 20.0f);
        
    }
}
