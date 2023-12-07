using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        
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
                Destroy(this.gameObject);
                break;
        }

    }

}
