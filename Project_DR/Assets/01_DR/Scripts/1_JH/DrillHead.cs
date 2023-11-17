using BNG;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DrillHead : MonoBehaviour
{
    public Vector3 targetPos;
    private bool isStop=false;
    private float currentGrappleDistance;
    public float damage;
    public float speed;
    public DamageCollider damageCollider;

    private void Awake()
    {
        GetData();
    }
    private void Start()
    {
        damageCollider.Damage = damage;
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
        damage = (float)DataManager.GetData(1100, "ProjectileDamage");
        speed = (float)DataManager.GetData(1100, "ProjectileSpeed");

    }

}
