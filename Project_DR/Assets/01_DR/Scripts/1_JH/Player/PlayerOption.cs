using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOption : MonoBehaviour
{
    public GameObject optionUI;

    [Header("Input")]
    //[Tooltip("The key(s) to use to toggle locomotion type")]
    public List<ControllerBinding> optionInput = new List<ControllerBinding>() { ControllerBinding.None };
    public InputActionReference InputAction = default;


    public virtual void CheckOptionToggleInput()
    {
        // Check for bound controller button
        for (int x = 0; x < optionInput.Count; x++)
        {
            if (InputBridge.Instance.GetControllerBindingValue(optionInput[x]))
            {
                if (optionUI)
                {
                    optionUI.SetActive(!optionUI.activeSelf);
                }
            }
        }
    }


    private void OnEnable()
    {
        InputAction.action.performed += ToggleActive;
    }

    private void OnDisable()
    {
        InputAction.action.performed -= ToggleActive;
    }

    public void ToggleActive(InputAction.CallbackContext context)
    {
        if (optionUI)
        {
            optionUI.SetActive(!optionUI.activeSelf);
        }
    }
}
