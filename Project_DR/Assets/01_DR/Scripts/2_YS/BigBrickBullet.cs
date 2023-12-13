using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBrickBullet : MonoBehaviour
{
    public DamageCollider damageCollider;

    public float damage = default;

    public int brickTableId;

    void Awake()
    {
        GetData(brickTableId); 
    }

    // Start is called before the first frame update
    void Start()
    {
        damageCollider = GetComponent<DamageCollider>();

        damageCollider.Damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetData(int brickTableId)
    {
        //6914
        damage = (float)DataManager.instance.GetData(brickTableId, "Damage", typeof(float));
      

    }

}
