using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Damageable playerDamage;
    public float health;
    public float maxHealth;
    private DamageScreenFader fader;
    public float dyingAmount = 0.25f; // 빈사상태 수치
    public float knockbackDistance = 1.5f; // 넉백
    public float knockbackSpeed = 2f; // 넉백

    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    public PlayerController playerController;

    public List<ControllerBinding> healthUpInput = new List<ControllerBinding>() { ControllerBinding.None };
    IEnumerator knockbackRoutine;
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

        Debug.Log($"플레이어 현재 체력:{health} / 증가량:{newHealth}");
    }

    public void OnDamage()
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
        GameManager.instance.GameOver();
    }

    public void OnKnockback(Vector3 targetPos)
    {
        StopKnockBack();


        //Vector3 knockbackTarget = transform.localPosition + (-transform.forward * knockbackDistance);
        Vector3 knockbackTarget = transform.localPosition - targetPos*knockbackDistance;
        knockbackRoutine = KnockBackRoutine(knockbackTarget);
        StartCoroutine(knockbackRoutine);
        Invoke("StopKnockBack", 0.5f);
    }
    public void StopKnockBack()
    {
        if (knockbackRoutine != null)
        {
            StopCoroutine(knockbackRoutine);
            knockbackRoutine = null;
        }
    }
    IEnumerator KnockBackRoutine(Vector3 target)
    {
        while (true)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.fixedDeltaTime * knockbackSpeed);
            float distance = Vector3.Distance(transform.localPosition, target);
            if (distance <= 0.05f)
            {
                break;
            }
            yield return waitForFixedUpdate;
//            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
