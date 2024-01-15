using BNG;
using Js.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bomb : MonoBehaviour
{
    public int itemID;
    public bool bombTrigger;

    private float damage;
    private float radius;
    private float duration;
    private SphereCollider sphereCollider;
    private DamageCollider damageCollider;
    private Rigidbody rigid;
    private ItemColliderHandler itemHandler;
    private MeshRenderer _renderer;
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
        

        itemHandler = gameObject.GetComponent<ItemColliderHandler>();
        _renderer = gameObject.GetComponent<MeshRenderer>();

        rigid = gameObject.GetOrAddRigidbody();

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
        BombTriggerCheck();
    }

    public void BombTriggerCheck()
    {
        if(itemHandler.state == ItemColliderHandler.State.GRABBED)
        {
            if(checkRoutine == null)
            {
                checkRoutine = CheckBombTriggerRoutine();
                StartCoroutine(checkRoutine);
            }
        }

    }

    IEnumerator CheckBombTriggerRoutine()
    {
        yield return new WaitForSeconds(duration);
        Bomb();
    }

    // 폭발
    public void Bomb()
    {
        GFunc.Log("폭탄터지나?");
        Destroy(GetComponent<Grabbable>());
        Destroy(GetComponent<UseItem>());
        Destroy(GetComponent<ItemBombHandler>());
        Destroy(GetComponent<ItemColliderHandler>());
        Destroy(GetComponent<ItemDataComponent>());

        _renderer.enabled = false;
        BombExplosion();
        sphereCollider.enabled = true;
        damageCollider.enabled = true;

        // 폭탄 사용 콜백 호출
        QuestCallback.OnUseItemCallback(itemID);

        Destroy(gameObject, 1f);
    }

    public void BombExplosion()
    {
        // 파괴 효과 출력
        ParticleSystem particle = transform.Find("Firefly_Circle_End_3_Orange")
            .GetComponent<ParticleSystem>();
        particle.Play();
    }

}
