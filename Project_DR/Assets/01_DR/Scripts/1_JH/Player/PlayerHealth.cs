using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Reference")]
    public PlayerController playerController;
    public Rigidbody playerRigid;
    private Damageable playerDamage;
    private DamageScreenFader fader;
    private SmoothLocomotion locomo;

    [Header("Health")]
    public float health;
    public float maxHealth;
    public float dyingAmount = 0.25f; // 빈사상태 수치

    [Header("Damage")]
    public float knockbackForce = 1.5f; // 넉백



    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    IEnumerator knockbackRoutine;


    [Header("Input")]
    public List<ControllerBinding> healthUpInput = new List<ControllerBinding>() { ControllerBinding.None };
    public PlayerStatusController[] playerHealthUI;
    // Start is called before the first frame update

    private void Awake()
    {
        playerDamage =  GetComponent<Damageable>();
    }
    
  
    // Update is called once per frame
    void Start()
    {
        UserData.GetData(GetData);


        playerController = GetComponent<PlayerController>();
        playerRigid = gameObject.GetOrAddRigidbody();
        locomo = GetComponent<SmoothLocomotion>();

        if (Camera.main)
        {
            fader = Camera.main.transform.GetComponent<DamageScreenFader>();
        }
    }

     public void RestoreHealth(float newHealth)
    {
        health += newHealth;
        if (maxHealth < health)
        {
            health = maxHealth;
        }
        playerDamage.Health = health;
        SetHealthUIUpdate();
        if (health > maxHealth * dyingAmount)
        { fader.OnRestore(); }

        GFunc.Log($"플레이어 현재 체력:{health} / 증가량:{newHealth}");
    }

    public void OnDamage(float damage)
    {
        SetHealth();
        fader.OnDamage();
        if (health <= maxHealth * dyingAmount)
        {
            fader.OnDying();
        }
        if(health <= 0)
        {
            Die();
        }
        UserData.OnDamage(damage);
    }

    public void GetData()
    {
        maxHealth = UserData.GetMaxHP();
        health = UserData.GetHP();

        playerDamage.Health = health; // 체력 세팅해주기
        SetMaxHealthUIUpdate(maxHealth);
        SetHealthUIUpdate() ;
    }
    // 데미지를 입을 때 체력 업데이트
    public void SetHealth()
    {
        health = playerDamage.Health;
        SetHealthUIUpdate();
    }

    private void SetMaxHealthUIUpdate(float value)
    {
        for (int i = 0; i < playerHealthUI.Length; i++)
        {
            playerHealthUI[i].SetMaxHealth(value);
        }
    }

    private void SetHealthUIUpdate()
    {
        for(int i = 0; i < playerHealthUI.Length; i++ )
        {
            playerHealthUI[i].SetHealth(health);
        }
    }

    public void Die()
    {
        GameManager.instance.GameOver();
    }

    public void OnKnockback(Vector3 targetPos)
    {
        if (locomo.state == PlayerState.grounded || locomo.state == PlayerState.walking)
        {
            playerRigid.AddForce(transform.localPosition - targetPos * knockbackForce, ForceMode.Impulse);          
        }

    }

    // 최대체력 업데이트
    public void EffectMaxHPUpdate()
    {
        float newMaxHealth = UserData.GetMaxHP();
        health += UserData.GetEffectMaxHP();

        if (health > newMaxHealth)
        {
            health = newMaxHealth;
            playerDamage.Health = health;
        }
        //GFunc.Log($"체력 업그레이드 | 최대 체력 : {UserData.GetMaxHP()}, 더할 체력 : {UserData.GetEffectMaxHP()}, 더해진 체력 : {health}");

        SetMaxHealthUIUpdate(newMaxHealth);
        SetHealthUIUpdate();
    }

    // Regacy
    //public void OnKnockback(Vector3 targetPos)
    //{
    //    StopKnockBack();


    //    //Vector3 knockbackTarget = transform.localPosition + (-transform.forward * knockbackDistance);
    //    Vector3 knockbackTarget = transform.localPosition - targetPos*knockbackDistance;
    //    knockbackRoutine = KnockBackRoutine(knockbackTarget);
    //    StartCoroutine(knockbackRoutine);
    //    Invoke("StopKnockBack", 0.5f);
    //}
//    IEnumerator KnockBackRoutine(Vector3 target)
//    {
//        while (true)
//        {
//            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.fixedDeltaTime * knockbackSpeed);
//            float distance = Vector3.Distance(transform.localPosition, target);
//            if (distance <= 0.05f)
//            {
//                break;
//            }
//            yield return waitForFixedUpdate;
////            yield return null;
//        }
//    }
    //public void StopKnockBack()
    //{
    //    if (knockbackRoutine != null)
    //    {
    //        StopCoroutine(knockbackRoutine);
    //        knockbackRoutine = null;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
