using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOption : MonoBehaviour
{
    private PlayerRotation rotation;

    public GameObject optionUI;
    public TMP_Text rotationText;


    [Header("Input")]
    //[Tooltip("The key(s) to use to toggle locomotion type")]
    public List<ControllerBinding> optionInput = new List<ControllerBinding>() { ControllerBinding.None };
    public InputActionReference InputAction = default;

    private void Start()
    {
        rotation = GetComponent<PlayerRotation>();
    }


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

    public void SetRotation()
    {
        rotation.SetRotation();
        rotationText.text = string.Format(rotation.SnapRotationAmount + "˚");
    }
}
