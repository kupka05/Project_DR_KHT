using BNG;
using Js.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bomb : MonoBehaviour
{
    public int itemID;
    public ParticleSystem particle;

    private float damage;
    private float radius;
    private float duration;
    private SphereCollider sphereCollider;
    private DamageCollider damageCollider;
    private Rigidbody rigid;
    private IEnumerator checkRoutine;

    // Start is called before the first frame update
    void Start()
    {
        GetData();

        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = radius;
        sphereCollider.isTrigger = true;
        sphereCollider.enabled = false;

        damageCollider = gameObject.AddComponent<DamageCollider>();
        damageCollider.damage = (damage, false);
        damageCollider.isPlayer = true;
        damageCollider.enabled = false;

        SkillEvent skillEvent = gameObject.AddComponent<SkillEvent>();
        skillEvent.skill = SkillEvent.Skill.Landing;
        skillEvent.landingForce = 20;
        skillEvent.knockbackRange = radius;
        

        rigid = gameObject.GetOrAddRigidbody();


        Invoke(nameof(Bomb), duration);
    }

    public void GetData()
    {
        damage = Data.GetFloat(itemID, "EffectAmount");
        radius = Data.GetFloat(itemID, "Radius");
        duration = Data.GetFloat(itemID, "Duration");
    }


    // 폭발
    public void Bomb()
    {  
        BombExplosion();
        sphereCollider.enabled = true;
        damageCollider.enabled = true;

        // 폭탄 사용 콜백 호출
        QuestCallback.OnUseItemCallback(itemID);

        Destroy(gameObject, 5f);
    }

    public void BombExplosion()
    {
        // 파괴 효과 출력
        particle.Play();
    }

}
