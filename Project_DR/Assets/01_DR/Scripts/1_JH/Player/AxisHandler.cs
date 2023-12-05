using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BNG;



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

    public State state = State.Default;

    public Vector2 axis;        // 방향
    public bool isDeadZone;     // 데드존 여부

    [Tooltip("Used to determine whether to turn left / right. This can be an X Axis on the thumbstick, for example. -1 to snap left, 1 to snap right.")]
    public List<InputAxis> inputAxis = new List<InputAxis>() { InputAxis.RightThumbStickAxis };

    public UnityEvent BackDashEvent;
    public UnityEvent LeftEvent;
    public UnityEvent RightEvent;

    // Update is called once per frame
    void LateUpdate()
    {
        GetAxis();
        CheckDeadZone();


        if (state != State.Default)
        {
            ActiveEvent();
            return; 
        }

        StateHandler();
    }

    public void StateHandler()
    {
        if(Math.Abs(axis.x) <= 0.2 && 0 < axis.y)
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

        if (Math.Pow(axis.x, 2f) + Math.Pow(axis.y, 2f) <= 0.25f)
        {
            isDeadZone = true;
        }
        else
            isDeadZone = false;

    }

    // 이벤트

    public void ActiveEvent()
    {
        if (isDeadZone)
        {

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
    }
}
