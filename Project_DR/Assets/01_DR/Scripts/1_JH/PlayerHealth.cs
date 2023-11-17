using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Damageable playerDamage;
    public float health;
    private DamageScreenFader fader;


    public PlayerStatusController[] playerHealthUI;
    // Start is called before the first frame update

    private void Awake()
    {
        playerDamage =  GetComponent<Damageable>();
        GetData();
        playerDamage.Health = health; // 체력 세팅해주기
        SetMaxHealthUIUpdate();
    }
  
    // Update is called once per frame
    void Start()
    {
        if (Camera.main)
        {
            fader = Camera.main.transform.GetComponent<DamageScreenFader>();
        }
    }

    public void OnDamage()
    {
        SetHealth();
        fader.OnDamage();
    }

    private void GetData()
    {
        health = (float)DataManager.GetData(1001, "Health");

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
}
