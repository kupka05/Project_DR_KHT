using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : MonsterBullet
{
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //Destroy(this.gameObject);
        }
    }
}
