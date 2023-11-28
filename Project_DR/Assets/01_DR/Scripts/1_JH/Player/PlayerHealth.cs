using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Damageable playerDamage;
    public float health;
    public float maxHealth;
    private DamageScreenFader fader;
    public float dyingAmount = 0.25f; // 빈사상태 수치
    public float knockbackSpeed = 0.5f; // 넉백

    public PlayerController playerController;

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
        GetData();
        playerDamage.Health = health; // 체력 세팅해주기
        SetMaxHealthUIUpdate();

        playerController = GetComponent<PlayerController>();
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
    }

    public void OnDamage()
    {
        SetHealth();
        fader.OnDamage();
        KnockBack();
        if (health <= maxHealth * dyingAmount)
        {
            fader.OnDying();
        }
    }

    private void GetData()
    {
        health = (float)DataManager.instance.GetData(1001, "Health", typeof(float));
        maxHealth = health;
    }
    // 데미지를 입을 때 체력 업데이트
    public void SetHealth()
    {
        health = playerDamage.Health;
        SetHealthUIUpdate();
    }

    private void SetMaxHealthUIUpdate()
    {
        for (int i = 0; i < playerHealthUI.Length; i++)
        {
            playerHealthUI[i].SetMaxHealth(health);
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

    }

    public void KnockBack()
    {
        transform.position += -Vector3.forward * knockbackSpeed;

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HealthItem>())
        {
            float newHealth = other.GetComponent<HealthItem>().health;
            RestoreHealth(newHealth);
        }
    }
}
