using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bomb : MonoBehaviour
{
    public int itemID;
    private float damage;
    private float radius;
    private float duration;
    private SphereCollider sphereCollider;
    private DamageCollider damageCollider;

    // Start is called before the first frame update
    void Start()
    {
        GetData();

        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = radius;
        sphereCollider.isTrigger = true;

        damageCollider = gameObject.AddComponent<DamageCollider>();
        damageCollider.damage = (damage, false);
        damageCollider.isPlayer = true;
        damageCollider.enabled = false;

        gameObject.tag = "PlayerSkill"; // 플레이어 스킬에 닿은 몬스터들은 넉백 실행
    }

    public void GetData()
    {
        damage = Data.GetFloat(itemID, "EffectAmount");
        radius = Data.GetFloat(itemID, "Radius");
        duration = Data.GetFloat(itemID, "Duration");
    }
    private void OnTriggerEnter(Collider other)
    {
        Invoke(nameof(Bomb), duration);
    }

    // 폭발
    public void Bomb()
    {
        damageCollider.enabled = true;


        Destroy(gameObject);
    }

}
