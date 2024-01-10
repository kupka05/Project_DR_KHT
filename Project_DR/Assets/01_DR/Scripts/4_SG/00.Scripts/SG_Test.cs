using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Rendering.Universal;



public enum testEnum
{
    one = 1,
    two = 2,
    three = 3,
    four = 4,
    five = 5
}


public class SG_Test : MonoBehaviour
{
    Transform box;

    
    OcclusionArea occlusionArea = new OcclusionArea();
    XROcclusionMeshPass testClass;
        
    private void Awake()
    {
        box = this.GetComponent<Transform>();
        
        Camera.main.useOcclusionCulling = true;

        occlusionArea.size = new Vector3(100f, 100f, 100f);
        occlusionArea.transform.position = occlusionArea.size * 0.5f;
    }

    private void Start()
    {
        

        box.transform.localEulerAngles = new Vector3 (0, 90, 0);
        Debug.Log($"LocalEulerAngles 수정후 Rotation : {box.transform.rotation}");

        box.transform.localEulerAngles = new Vector3(0, 0, 0);

        box.transform.rotation = Quaternion.Euler(0, 90, 0);
        Debug.Log($"rotation = Quaternion.Euler 수정후 Rotation : {box.transform.rotation}");



    }


}       // SG_Test
