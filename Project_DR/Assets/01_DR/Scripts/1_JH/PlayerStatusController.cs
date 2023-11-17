using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusController : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text healthText;
    public Image healthColor;
    private float maxHealth;
    private float curHealth;
    private GameObject player;


    public void SetMaxHealth(float newHealth)
    {
        healthSlider.maxValue = newHealth;
        healthSlider.value = newHealth;

        maxHealth = newHealth;
        curHealth = newHealth;

        healthText.text = string.Format(curHealth + " / " + maxHealth);
        SetHealthColor(curHealth);
    }
    public void SetHealth(float newHealth)
    {
        healthSlider.value = newHealth;
        healthText.text = string.Format(newHealth + " / " + maxHealth);
        SetHealthColor(newHealth);
    }
    private void SetHealthColor(float _curHealth)
    {
        if (_curHealth > maxHealth * 0.5f)
        {
            Debug.Log("초록");

            healthColor.color = new Color(23 / 255, 255 / 255, 100 / 255, 1); // 초록

        }
        else if (_curHealth > maxHealth * 0.25f)
        {
            Debug.Log("주황");
            healthColor.color = new Color(255 / 255, 125 / 255, 23 / 255, 1); // 주황

        }
        else
        {
            Debug.Log("빨강");

            healthColor.color = new Color(255 / 255, 0, 103 / 255, 1); // 빨강
        }
    }

}
