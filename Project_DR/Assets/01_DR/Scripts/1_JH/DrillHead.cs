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
    public float critChance = 0.1f;
    public float critIncrease = 1.5f;

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
        DamageCalculator();
        damageCollider.Damage = damage;
        col = damageCollider.GetComponent<CapsuleCollider>();
    }
    // Update is called once per frame
    void Update()
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
        damage = (float)DataManager.GetData(1100, "ProjectileDamage", typeof(float));
        speed = (float)DataManager.GetData(1100, "ProjectileSpeed", typeof(float));
        critIncrease = (float)DataManager.GetData(1100, "CritIncrease", typeof(float));
        critChance = (float)DataManager.GetData(1100, "CritChance", typeof(float));

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Damageable>())
        {
            grappling.StopGrapple();
               
        }
    }
    // 데미지 계산
    private void DamageCalculator()
    {
        float val = Random.Range(0f, 100f);
        if (critChance <= val)
        {
            critIncrease = 0;
        }
        damage = damage * (1 + critIncrease);

    }

}
