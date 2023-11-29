using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;


public class DamageDebug : MonoBehaviour
{
    private Damageable damageable;
    public GameObject UI;

    public float maxHealth;

    public TMP_Text healthTxt;
    public TMP_Text damageTxt;
    public Slider healthSlider;


    // Start is called before the first frame update
    void Start()
    {
        damageable = GetComponent<Damageable>();
        maxHealth = damageable.Health;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UI.transform.LookAt(Camera.main.transform);

        healthTxt.text = string.Format(damageable.Health + " / " + maxHealth);
        healthSlider.value = damageable.Health;
        


    }

    public void DealDamage(float damage)
    {
        damageTxt.text = string.Format("Damage : " + damage);
    }

}
