using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Rendering.Universal;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;
using System.Text;




public class SG_Test : MonoBehaviour
{
    string sharp = "이건 샵이다 # 이자식";
    private void Awake()
    {
        sharp = sharp.Replace("#", ",");
        Debug.Log($"값 : {sharp}");
    }

    private void Start()
    {
        
    }



}       // SG_Test
