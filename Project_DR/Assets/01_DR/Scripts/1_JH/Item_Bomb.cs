using BNG;
using Js.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Bomb : MonoBehaviour
{
    public int itemID;
    public bool bombTrigger;

    private float damage;
    private float radius;
    private float duration;
    private float force;
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
        skillEvent.knockbackRange = radius;
        skillEvent.bombForce = force;
        skillEvent.damageVal = damage;
        skillEvent.bomb = true;



        itemHandler = gameObject.GetComponent<ItemColliderHandler>();
        _renderer = gameObject.GetComponent<MeshRenderer>();

        rigid = gameObject.GetOrAddRigidbody();

        gameObject.tag = "PlayerSkill"; // 플레이어 스킬에 닿은 몬스터들은 넉백 실행
        AudioManager.Instance.AddSFX("SFX_Item_Bomb_Trigger");
        AudioManager.Instance.AddSFX("SFX_Bomb_Explosion");
    }

    public void GetData()
    {
        damage = Data.GetFloat(itemID, "EffectAmount");
        radius = Data.GetFloat(itemID, "Radius");
        duration = Data.GetFloat(itemID, "Duration");
        force = Data.GetFloat(itemID, "Duration");
    }
 
    public void BombTriggerCheck()
    {
        if (!bombTrigger)
        {
            bombTrigger = true;
            GameObject nameTag = transform.GetComponentInChildren<ItemNameTag>().gameObject;
            nameTag.SetActive(false);
            Destroy(GetComponent<UseItem>());
            Destroy(GetComponent<ItemBombHandler>());
            Destroy(GetComponent<ItemColliderHandler>());
            Destroy(GetComponent<ItemDataComponent>());
            _renderer.material.color = Color.red;
            AudioManager.Instance.PlaySFXPoint("SFX_Item_Bomb_Trigger", this.transform.position);

            //Invoke(nameof(Bomb), duration);
            StartCoroutine(BombRoutine());
        }   
    }
    IEnumerator BombRoutine()
    {
        yield return new WaitForSeconds(duration);
        BombExplosion();
        _renderer.enabled = false;

        sphereCollider.enabled = true;
        damageCollider.enabled = true;

        yield return new WaitForSeconds(0.25f);

        sphereCollider.enabled = false;
        // 폭탄 사용 콜백 호출
        QuestCallback.OnUseItemCallback(itemID);
        AudioManager.Instance.PlaySFXPoint("SFX_Bomb_Explosion", this.transform.position);
        
        yield return new WaitForSeconds(5f);
        Destroy(gameObject, 0);
    }

    // 폭발
    public void Bomb()
    {
        BombExplosion();
        _renderer.enabled = false;

        sphereCollider.enabled = true;
        damageCollider.enabled = true;

        // 폭탄 사용 콜백 호출
        QuestCallback.OnUseItemCallback(itemID);
        AudioManager.Instance.PlaySFXPoint("SFX_Bomb_Explosion", this.transform.position);

        Destroy(gameObject, 5f);
    }

    public void BombExplosion()
    {
        // 파괴 효과 출력
        ParticleSystem particle = transform.Find("Firefly_Circle_End_3_Orange")
            .GetComponent<ParticleSystem>();
        particle.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.DealDamage(damage);
        }
    }
}
