using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public Damageable damageable;

    public float damage = 0.0001f;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<PlayerPosition>().playerPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnParticleCollision(GameObject other)
    {
        if(other.tag.Equals("Player"))
        {
            other.GetComponent<Damageable>().DealDamage(damage);
        }
    }
}
