using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testdebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach (var item in DataManager.Instance.dataTable)
        {
            GFunc.Log($"데이터 테이블[{i}] 갯수: {DataManager.Instance.dataTable[item.Key].Count}");

            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
