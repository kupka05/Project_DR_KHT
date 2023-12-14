using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using Newtonsoft.Json.Bson;
using TMPro;
using System.ComponentModel;

public class SG_Test : SG_Test002
{



    protected override void Start()
    {
        base.Start();
        Debug.Log("SG_Test에서 Base호출 이후 Start내부 실행");
        testEvent += ISINVOKE;
        StartCoroutine(TestCoroutine());

    }

    private void ISINVOKE()
    {
        Debug.Log("최상위 부모 클래스에서 INVOKE하면 잘 불러와지나?");
    }

    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(5);
        IsTest = true;
    }


}       // SG_Test
