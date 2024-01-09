using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogDebug : MonoBehaviour
{
    public UnityEvent enter, input1, input2, input3;

    [Header("Input")]
    //[Tooltip("The key(s) to use to toggle locomotion type")]
    public List<ControllerBinding> palyerInput = new List<ControllerBinding>() { ControllerBinding.None };
    public InputActionReference InputAction = default;

    

    public virtual void CheckOptionToggleInput()
    {
        // Check for bound controller button
        for (int x = 0; x < palyerInput.Count; x++)
        {
            if (InputBridge.Instance.GetControllerBindingValue(palyerInput[x]))
            {
                enter?.Invoke();
                input1?.Invoke();
                input2?.Invoke();
                input3?.Invoke();

            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        CheckOptionToggleInput();
        if (Input.GetKeyDown(KeyCode.F1))
        {
            enter.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            input1.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            input2.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            input3.Invoke();
        }
    }

    // 환경설정 토글
    private void OnEnable()
    {
        InputAction.action.performed += Dialog;
    }

    private void OnDisable()
    {
        InputAction.action.performed -= Dialog;
    }

    public void Dialog(InputAction.CallbackContext context)
    {
        enter?.Invoke();
        input1?.Invoke();
        input2?.Invoke();
        input3?.Invoke();
    }

}
