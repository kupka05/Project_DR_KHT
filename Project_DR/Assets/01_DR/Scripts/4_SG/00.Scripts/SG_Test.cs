using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using Newtonsoft.Json.Bson;
using TMPro;
using System.ComponentModel;
using System.Runtime.InteropServices;

public class SG_Test : MonoBehaviour
{
    SG_Test002 test002;
    public GameObject tempObj;

    private void Start()
    {
        //Debug.Log($"GetComponent전 용량{Marshal.SizeOf(test002.num4)}");
        test002 = tempObj.GetComponent<SG_Test002>();

        
        Debug.Log($"GetComponent후 용량{Marshal.SizeOf(test002.num4)}");
    }


}       // SG_Test
