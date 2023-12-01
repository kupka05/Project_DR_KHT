using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBullet : MonsterBullet
{
    public int hp = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Weapon")) 
        {
            hp--;
            Debug.Log($"hp:{hp}");

            if (hp == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
