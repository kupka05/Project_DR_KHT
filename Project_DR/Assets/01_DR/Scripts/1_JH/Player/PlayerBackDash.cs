using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Windows;

public class PlayerBackDash : MonoBehaviour
{

    private SmoothLocomotion locomo;
    public Rigidbody playerRigid;
    public float force = 1.5f; // 넉백
    public bool input = false;
    public bool onBackDash = false;
    public float coolDown = 1.5f;

    Camera cam;

    [Tooltip("Used to determine whether to turn left / right. This can be an X Axis on the thumbstick, for example. -1 to snap left, 1 to snap right.")]
    public List<InputAxis> inputAxis = new List<InputAxis>() { InputAxis.RightThumbStickAxis };

    IEnumerator dashRoutine;
    private WaitForSeconds waitForSeconds;


    // Start is called before the first frame update
    void Start()
    {
        Getdata();
        locomo = GetComponent<SmoothLocomotion>();
        waitForSeconds = new WaitForSeconds(coolDown); ;
        playerRigid = transform.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        input = GetAxis();

        if(input)
        {
            OnBackDash();
        }
    }


    public bool GetAxis()
    {


        // Check Raw Input
        if (inputAxis != null)
        {
            for (int i = 0; i < inputAxis.Count; i++)
            {

                float yAxisVal = InputBridge.Instance.GetInputAxisValue(inputAxis[i]).y;
                float xAxisVal = InputBridge.Instance.GetInputAxisValue(inputAxis[i]).x;

                // 중앙 데드존
                if (Math.Abs(xAxisVal) < 0.5 && Math.Abs(yAxisVal) < 0.3)
                {
                    return false;
                }

                if (0 > yAxisVal && Math.Abs(xAxisVal) < 0.8f)
                {
                    Debug.Log("대시 입력");

                    return true;
                }              
            }
        }
            return false;

    }

    public void OnBackDash()
    {
        if(onBackDash)
        {
            return;
        }

        if (locomo.state == PlayerState.grounded || locomo.state == PlayerState.walking)
        {


            onBackDash = true;
            playerRigid.AddForce(-transform.forward * force, ForceMode.Impulse);

            if (dashRoutine != null)
            {
                StopCoroutine(dashRoutine);
                dashRoutine = null;
            }
            dashRoutine = BackDashRoutine();
            StartCoroutine(dashRoutine);
        }
    }
    
    IEnumerator BackDashRoutine()
    {
        yield return waitForSeconds;
        onBackDash = false;
        yield break;
    }

    public void Getdata()
    {
        force = (float)DataManager.instance.GetData(1001, "BackDash", typeof(float))*50f;
        coolDown = (float)DataManager.instance.GetData(1001, "BackDashCD", typeof(float));
    }
}
