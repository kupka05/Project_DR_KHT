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
            healthColor.color = new Color(23f / 255f, 255f / 255f, 100f / 255f, 1); // 초록

        }
        else if (_curHealth > maxHealth * 0.25f)
        {
            healthColor.color = new Color(255f / 255f, 125f / 255f, 23f / 255f, 1); // 주황
        }
        else
        {
            healthColor.color = new Color(255f / 255f, 0f, 103f / 255f, 1); // 빨강
        }
    }

}
