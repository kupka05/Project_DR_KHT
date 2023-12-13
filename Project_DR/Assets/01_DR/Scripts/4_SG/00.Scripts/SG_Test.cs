using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using Newtonsoft.Json.Bson;
using TMPro;

public class SG_Test : MonoBehaviour
{

    public string data = "30_1_0_999_0_1\n30_1_0_999_0_2\n30_1_0_999_0_3\n30_1_0_999_0_4\n30_1_0_999_0_5\n30_1_0_999_0_6";

    void Start()
    {
        //ParseData();
        int[] tet = new int[10];
        Debug.Log($"Length : {tet.Length}");
        for( int i = 0; i < tet.Length; i++)
        {
            Debug.Log(i);
        }
    }

    void ParseData()
    {
        // 데이터를 줄 바꿈 문자('\n')로 나누어 배열에 저장
        string[] lines = data.Split('\n');

        // 결과를 저장할 컬렉션(List) 생성
        List<int> parsedValues = new List<int>();

        for (int i = 0; i < lines.Length ; i++)
        {
            lines[i] = lines[i].Replace("_", "");

            Debug.Log(lines[i]);
        }

        int tempID = int.Parse(lines[1]);

        Debug.Log($"변환 잘 됬나? : {tempID}");

        
    }
}       // SG_Test
