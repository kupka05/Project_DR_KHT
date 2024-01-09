using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerFire : MonoBehaviour
{
    public DamageCollider damageCollider;

    public int lazerFireId;

    public float returnTime = default;
    public float damage = default;

    void Awake()
    {
        GetData(lazerFireId);
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

    void Return()
    {
        Destroy(this.gameObject);
    }

    public void GetData(int lazerFireId)
    {
        returnTime = (float)DataManager.Instance.GetData(lazerFireId, "DesTime", typeof(float));
        damage = (float)DataManager.Instance.GetData(lazerFireId, "Damage", typeof(float));
    }


    public void OnParticleCollision(GameObject other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<Damageable>().DealDamage(damage);
            //GFunc.Log("불 장판 데미지 들어온다");
        }
    }
}
