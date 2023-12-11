using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    private SphereCollider grabberCollider;
    public GameObject inventory;
    private Vector3 smallScale;
    private Vector3 defaultScale;
    [Header("Input")]
    public InputActionReference InputAction = default;


    private void Start()
    {
        grabberCollider = GetComponent<SphereCollider>();
        smallScale = new Vector3(0.00001f, 0.00001f, 0.00001f);
        defaultScale = new Vector3(0.25f, 0.25f, 0.25f);
        inventory.transform.localScale = smallScale;
    }

    // 인벤토리를 여는 이벤트
    public void OpenInventory()
    {

        if(inventory.transform.localScale == smallScale)
        { 
            inventory.transform.localScale = defaultScale; 
        }

        else
            inventory.transform.localScale = smallScale;
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
        OpenInventory();
    }

}
