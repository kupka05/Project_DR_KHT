using BNG;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrillHead : MonoBehaviour
{
    public Vector3 targetPos;
    private bool isStop=false;
    private float currentGrappleDistance;
    public float damage;

    public float speed;
    public DamageCollider damageCollider;
    public bool isTrigger;
    public Grappling grappling;
    private CapsuleCollider col;

    private void Awake()
    {
        GetData();
    }
    private void Start()
    {
        damageCollider.damage = FinalDamage();
        col = damageCollider.GetComponent<CapsuleCollider>();      
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetPos == null || isStop)
            return;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime* speed);
        currentGrappleDistance = Vector3.Distance(transform.position, targetPos);


        if (currentGrappleDistance > 0.5f)
        { transform.LookAt(targetPos); }
        if(currentGrappleDistance < 0.3f)
            isStop = true;
    }
    private void GetData()
    {
        damage = (float)DataManager.Instance.GetData(1100, "ProjectileDamage", typeof(float));
        speed = (float)DataManager.Instance.GetData(1100, "ProjectileSpeed", typeof(float));
     
    }

    public void OnTriggerEnter(Collider other)
    {
        //GFunc.Log("닿았나?" + other.gameObject.name);
        //if (other.gameObject.GetComponent<Damageable>())
        //{
        //    grappling.StopGrapple();
        //}
        //else if (other.gameObject.GetComponent<DamageablePart>())
        //{
        //    grappling.StopGrapple();
        //}
    }
    // 데미지 연산하는 함수
    private (float, bool) FinalDamage()
    {
        return Damage.instance.DamageCalculate(damage);
    }
    public void DrillSide(bool isLeft = default)
    { 
        damageCollider.isLeft = isLeft;
    }

}
