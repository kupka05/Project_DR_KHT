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
        damage = (float)DataManager.instance.GetData(1100, "ProjectileDamage", typeof(float));
        speed = (float)DataManager.instance.GetData(1100, "ProjectileSpeed", typeof(float));

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Damageable>())
        {
            Debug.Log("뭔가 부딧쳤다");
            grappling.StopGrapple();
                
        }
    }

}
