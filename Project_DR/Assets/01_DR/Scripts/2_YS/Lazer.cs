using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public Damageable damageable;

    public float damage = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Return", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Return()
    {
        Destroy(this.gameObject);
    }

    public void OnParticleCollision(GameObject other)
    {
        if(other.tag.Equals("Player"))
        {
            other.GetComponent<Damageable>().DealDamage(damage);
        }
    }
}
