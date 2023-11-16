using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class SG_Test : MonoBehaviour
{
    StringBuilder stBuilder;
    string stText;

    delegate void rambdaD();

    event rambdaD RDEvent;

    private void Start()
    {
        RDEvent += TestMethod;
        RDEvent?.Invoke();
        
    }

    private void TestMethod()
    {
        Debug.Log("이벤트 호출");

    }

}       // SG_Test
