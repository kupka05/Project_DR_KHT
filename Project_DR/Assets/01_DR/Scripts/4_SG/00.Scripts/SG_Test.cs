using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using Newtonsoft.Json.Bson;

public class SG_Test : MonoBehaviour
{
    #region STB_DelegateTEST
    //StringBuilder stBuilder = new StringBuilder();
    //string stText;

    //delegate void ButtonInputD();
    //event ButtonInputD inputEvent;

    //event System.Action<int> actionEvent;

    //int tempNum003 = 10;

    //private void Awake()
    //{
    //    inputEvent += TestMethod;
    //    actionEvent += ActionTest;
    //}
    //private void Start()
    //{
    //    stBuilder.Append("여러변 추가되나?");

    //    Debug.Log($"stB -> {stBuilder}");
    //    stBuilder.Clear();
    //    Debug.Log($"stB -> {stBuilder}");

    //    inputEvent?.Invoke();
    //    actionEvent?.Invoke(tempNum003);


    //}

    //private void TestMethod()
    //{
    //    Debug.Log("ButtonEventCall");
    //}

    //private void ActionTest(int tempNum_)
    //{
    //    Debug.Log("ActionCall");
    //    Debug.Log($"ActionParamiter -> {tempNum_}");
    //}
    #endregion STB_DelegateTEST

    Stack<int> testStack = new Stack<int>();

    List<int> testList = new List<int>();

    private void Start()
    {
        Debug.Log("Start함수 실행");

        for (int i = 0; i < 10; i++)
        {
            int test = Test001();

        }
        Debug.Log("Test001 함수 이후");
        Test002();

        foreach (int test in testList)
        {
            Debug.Log($"List내부값 : {test}");
        }

        Debug.Log("Start함수 실행끝");


    }


    private int Test001()
    {
        int num = UnityEngine.Random.Range(0, 10);
        Debug.Log($"함수 돌기 시작");
        if(testList.Contains(num))
        {
            Debug.Log($"중복된 값이 있으므로 재귀 시작");
            return Test001();
        }

        testList.Add(num);
        Debug.Log($"함수끝");
        return num;
    }

    private void Test002()
    {
        Debug.Log("다음함수 불러오나?");
    }

}       // SG_Test
