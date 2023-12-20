using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : MonoBehaviour
{
    public Rigidbody rigid;
    public DamageCollider damageCollider;

    public int BounceTableId;

    [Header("테이블 관련")]
    public float speed = default;
    public float damage = default;
    public float destoryTime = default;

    [Header("조건")]
    public bool isShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        GetData(BounceTableId);

        rigid = GetComponent<Rigidbody>();

        damageCollider.Damage = damage;

        StartCoroutine(Activate());
    }

    IEnumerator Activate()
    {
        // 오브젝트 활성화
        gameObject.SetActive(true);
        //GFunc.Log($"활성화:{gameObject}");
        
        yield return new WaitForSeconds(4.0f);
        //GFunc.Log("대기중");
        
        Play();
    }

    void Play()
    {
        rigid.velocity = transform.forward * speed;
    }

    

    public virtual void GetData(int BounceTableId)
    {
        //6912
        speed = (float)DataManager.Instance.GetData(BounceTableId, "Speed", typeof(float));
        damage = (float)DataManager.Instance.GetData(BounceTableId, "Damage", typeof(float));
        destoryTime = (float)DataManager.Instance.GetData(BounceTableId, "DesTime", typeof(float));
    }

}
