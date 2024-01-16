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
    private WaitForSeconds waitForSeconds = new WaitForSeconds(2.5f);
    IEnumerator knockbackRoutine;


    [Header("Input")]
    public List<ControllerBinding> healthUpInput = new List<ControllerBinding>() { ControllerBinding.None };
    public PlayerStatusController[] playerHealthUI;
    // Start is called before the first frame update

    IEnumerator dyingRoutine;

    private void Awake()
    {
        playerDamage =  GetComponent<Damageable>();
    }
    
  
    // Update is called once per frame
    void Start()
    {
        UserData.GetData(GetData);

        AudioManager.Instance.AddSFX("SFX_PC_LowHealth_01");

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

        UserData.SetCurHealth(health);
    }

    public void OnDamage(float damage)
    {
        SetHealth();
        fader.OnDamage();
        if (health <= maxHealth * dyingAmount)
        {
            fader.OnDying();
            if(dyingRoutine == null)
            {
                dyingRoutine = DyingRoutine();
                StartCoroutine(dyingRoutine);
            }
           
        }
        if(health <= 0)
        {
            Die();
        }
        UserData.SetCurHealth(health);
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

    IEnumerator DyingRoutine()
    {
        while (true) 
        {
            if (health <= maxHealth * dyingAmount)
            {
                break;
            }
            AudioManager.Instance.PlaySFX("SFX_PC_LowHealth_01");
            yield return waitForSeconds;
        }
        yield break;
    }
}
