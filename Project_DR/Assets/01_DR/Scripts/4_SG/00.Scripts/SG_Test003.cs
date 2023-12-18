using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_Test003 : MonoBehaviour
{
    protected delegate void TESTDelegate();
    protected event TESTDelegate testEvent;

    protected virtual void Start()
    {
        isTest = false;
        GFunc.Log("SG_Test003 Start함수 실행");
    }

    private bool isTest;
    public bool IsTest
    {
        get { return isTest; }
        set
        {
            if(isTest != value) 
            {
                isTest = value;
                LetsInvoke();
            }
        }
    }

    private void LetsInvoke()
    {
        testEvent?.Invoke();
    }

    

    
}
