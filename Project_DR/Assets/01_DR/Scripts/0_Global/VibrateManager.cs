using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateManager : MonoBehaviour
{
    public static VibrateManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<VibrateManager>();
            }

            return m_instance;
        }
    }
    private static VibrateManager m_instance;

    private InputBridge input;
    private void Start()
    {
        input = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<InputBridge>();
        if(!input)
        {
            Debug.LogError("인풋을 찾을 수 없음");
        }
    }
    public void Vibrate(float frequency, float amplitude, float duration, ControllerHand hand)
    {
        input.VibrateController(frequency, amplitude, duration, hand);
    }

}
