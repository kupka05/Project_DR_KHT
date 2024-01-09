using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public DamageCollider damageCollider;

    public int LazerId;


    public float returnTime = default;
    public float damage = default;

    void Awake()
    {
        GetData(LazerId);
    }
    // Start is called before the first frame update
    void Start()
    {
        damageCollider = GetComponent<DamageCollider>();
        damageCollider.SetDamage(damage);
        Invoke("Return", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetData(int lazerId)
    {
        returnTime = (float)DataManager.Instance.GetData(lazerId, "DesTime", typeof(float));
        damage = (float)DataManager.Instance.GetData(lazerId, "Damage", typeof(float));
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
            //GFunc.Log("레이저 데미지 들어온다");
        }
    }
}
