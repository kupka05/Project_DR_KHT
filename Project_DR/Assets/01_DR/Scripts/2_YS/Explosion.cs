using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag.Equals("Player"))
        {
            //Debug.Log("부딪힘");
            other.GetComponent<Damageable>().DealDamage(damage);
            //Debug.Log($"damage:{damage}");
        }
    }
}
