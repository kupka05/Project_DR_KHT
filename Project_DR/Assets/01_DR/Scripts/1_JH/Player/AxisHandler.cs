using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BNG;
using Unity.VisualScripting;

public class AxisHandler : MonoBehaviour
{
    public enum State
    {
        Default,
        Teleport,
        Left,
        Right,
        Backdash,
    }
    [Header("Axis")]
    public State state = State.Default;
    public Vector2 axis;        // 방향

    [Header ("Dead Zone")]
    public bool isDeadZone;     // 데드존 여부
    public float deadZoneVal;

    [Header("Active Zone")]
    public bool isActiveZone;     // 데드존 여부
    public float activeZoneVal;
    public float activeTime;

    public bool isActiveTeleport;

    [Header("Inputs")]
    [Tooltip("Used to determine whether to turn left / right. This can be an X Axis on the thumbstick, for example. -1 to snap left, 1 to snap right.")]
    public List<InputAxis> inputAxis = new List<InputAxis>() { InputAxis.RightThumbStickAxis };

    [Header("Event")]
    public UnityEvent BackDashEvent;
    public UnityEvent LeftEvent;
    public UnityEvent RightEvent;

    IEnumerator ActiveCheckRoutine;
    WaitForSeconds waitForSeconds;

    private void Start()
    {
        GetData();
        waitForSeconds = new WaitForSeconds(activeTime);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GetAxis();
        CheckDeadZone();


        if (state != State.Default)
        {
            //ActiveEvent();
            ActiveCheck();
            return; 
        }

        StateHandler();
    }

    public void StateHandler()
    {
        if (isDeadZone)
        {
            return;
        }

        if (Math.Abs(axis.x) <= 0.2 && 0 < axis.y)
        {
            state = State.Teleport;
        }
        else if(Math.Abs(axis.x) <= 0.2 && axis.y < 0)
        {
            state = State.Backdash;
        }
        else if (Math.Abs(axis.y) <= 0.2 && axis.x < 0)
        {
            state = State.Left;
        }
        else if(Math.Abs(axis.y) <= 0.2 && 0 < axis.x)
        {
            state = State.Right;
        }
    }

   
    // 입력
    public void GetAxis()
    {
        float AxisX = 0;           
        float AxisY = 0;

        if (inputAxis != null)
        {
            for (int i = 0; i < inputAxis.Count; i++)
            {
                AxisX = InputBridge.Instance.GetInputAxisValue(inputAxis[i]).x;
                AxisY = InputBridge.Instance.GetInputAxisValue(inputAxis[i]).y;
            }
        }
        axis.x = AxisX;
        axis.y = AxisY;
    }

    // 데드존 체크
    public void CheckDeadZone()
    {

        if (Math.Pow(axis.x, 2f) + Math.Pow(axis.y, 2f) <= Math.Pow(deadZoneVal, 2f))
        {
            isDeadZone = true;                        
        }
        else
            isDeadZone = false;


        if (Math.Pow(axis.x, 2f) + Math.Pow(axis.y, 2f) <= Math.Pow(activeZoneVal, 2f))
        {
            isActiveZone = true;
        }
        else
            isActiveZone = false;

    }

    // 실행 전 시간 체크
    public void ActiveCheck()
    {
        if (isActiveZone)
        {
            ActiveEvent();
            if (state == State.Teleport && ActiveCheckRoutine == null)
            {
                ActiveCheckRoutine = ActiveChecking();
                StartCoroutine(ActiveCheckRoutine);
            }
        }
        else if (!isActiveZone)
        {
            if (state == State.Teleport && ActiveCheckRoutine != null)
            {
                StopCoroutine(ActiveCheckRoutine);
                ActiveCheckRoutine = null;
            }
        }
    }
    // 체크 코루틴
    IEnumerator ActiveChecking()
    {
        GFunc.Log("체크시작하나?");
        yield return waitForSeconds;
        //ActiveEvent();
        state = State.Default;

        yield break;
    }


    // 이벤트

    public void ActiveEvent()
    {
        if (state == State.Teleport)
            return;

        if (state == State.Backdash)
        {
            BackDashEvent.Invoke();
        }
        else if (state == State.Left)
        {
            LeftEvent.Invoke();

        }
        else if (state == State.Right)
        {
            RightEvent.Invoke();
        }

        state = State.Default;
    }

    public void GetData()
    {
        deadZoneVal = (float)DataManager.Instance.GetData(1001, "DeadZone", typeof(float));
        activeZoneVal = (float)DataManager.Instance.GetData(1001, "ActiveZone", typeof(float));
        activeTime = (float)DataManager.Instance.GetData(1001, "ActiveTime", typeof(float));
    }
}
