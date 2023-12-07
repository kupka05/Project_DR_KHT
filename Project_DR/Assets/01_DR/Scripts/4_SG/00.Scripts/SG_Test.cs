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
    public List<GameObject> mon;
    GameObject clone;
    public GameObject testPrefab;

    public int desSeconds = 5;

    private void Awake()
    {
        mon = new List<GameObject>();
    }

    private void Start()
    {
        TestStart();

    }
    public void CheckCount()
    {
        Debug.Log($"listCount : {mon.Count}");

        if (mon.Count == 0)
        {
            Debug.Log($"실험잘됨\nListCount : {mon.Count}");
        }

    }

    private void TestStart()
    {
        StartCoroutine(InstanceMonster());
    }

    IEnumerator InstanceMonster()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("인스턴스");
            clone = Instantiate(testPrefab, Vector3.one, Quaternion.identity, this.gameObject.transform);
            clone.gameObject.AddComponent<SG_Test002>();
            SG_Test002 test002 = clone.GetComponent<SG_Test002>();
            test002.testInIt(this);
            yield return new WaitForSeconds(3);
        }

    }
}       // SG_Test
