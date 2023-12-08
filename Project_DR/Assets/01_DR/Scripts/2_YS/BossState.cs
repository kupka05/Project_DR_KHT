using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    public Animator anim;

    public Transform target;

    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
    }

    void FixedUpdate()
    {
        if (!target)
        {
            return;
        }
        // Look At Y 각도로만 기울어지게 하기
        Vector3 targetPostition =
            new Vector3(target.position.x, this.transform.position.y, target.position.z);
        this.transform.LookAt(targetPostition);
        //transform.LookAt(playerTr.position);

    }

    public void Die()
    {
        Debug.Log("애니메이션 작동");
        anim.SetTrigger("isDie");

    }

    public void Explosion(int index)
    {
        switch (index)
        {
            case 0:
                Destroy(this.gameObject,1f);
                break;
        }

    }

}
