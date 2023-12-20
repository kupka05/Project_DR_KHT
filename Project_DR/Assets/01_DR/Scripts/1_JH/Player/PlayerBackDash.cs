using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Input = UnityEngine.Input;

public class PlayerBackDash : MonoBehaviour
{
    private AxisHandler axis;
    private SmoothLocomotion locomo;
    public Rigidbody playerRigid;
    public float force = 1.5f; // 넉백
    public bool input = false;
    public bool onBackDash = false;
    public bool onKnockBack = false;
    public float coolDown = 1.5f;


    [Tooltip("Used to determine whether to turn left / right. This can be an X Axis on the thumbstick, for example. -1 to snap left, 1 to snap right.")]
    public List<InputAxis> inputAxis = new List<InputAxis>() { InputAxis.RightThumbStickAxis };

    IEnumerator dashRoutine;
    IEnumerator knockBackRoutine;
    private WaitForSeconds waitForSeconds;


    // Start is called before the first frame update
    void Start()
    {
        Getdata();
        axis = GetComponent<AxisHandler>();
        locomo = GetComponent<SmoothLocomotion>();
        waitForSeconds = new WaitForSeconds(coolDown); ;
        playerRigid = gameObject.GetOrAddRigidbody();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnBackDash();
        };
    }

    //// Update is called once per frame
    //void LateUpdate()
    //{
    //    input = GetAxis();

    //    if(input)
    //    {
    //        OnBackDash();
    //    }
    //}


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
                    GFunc.Log("대시 입력");

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
    // 힘 조절이 필요할 경우
    public void OnBackDash(float _force)
    {
        if (onBackDash)
        {
            return;
        }

        if (locomo.state == PlayerState.grounded || locomo.state == PlayerState.walking)
        {
            onBackDash = true;
            playerRigid.AddForce(-transform.forward * (_force*1000), ForceMode.Impulse);

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
        onKnockBack = false;
        yield break;
    }

    public void Getdata()
    {
        force = (float)DataManager.Instance.GetData(1001, "BackDash", typeof(float))*1000f;
        coolDown = (float)DataManager.Instance.GetData(1001, "BackDashCD", typeof(float));
    }

}
