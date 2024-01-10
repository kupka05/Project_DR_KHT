using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Rendering.Universal;




public class SG_Test : MonoBehaviour
{

    public Material tempMaterial;
    public Vector3[] vertices;
    public int[] triangles;
    Mesh mesh;
    MeshFilter filter;

    GameObject tempObj;
    Mesh tempMesh;

    private void Awake()
    {    
        filter = this.GetComponent<MeshFilter>();
        mesh = filter.mesh;


        tempObj = new GameObject("TempObj", typeof(MeshFilter));
        tempMesh = new Mesh();

    }       // Awake()

    private void Start()
    {
        vertices = new Vector3[] {new Vector3(-1f,1f,0f), new Vector3(1f,1f,0f),
                                  new Vector3 (1f,-1f,0f), new Vector3(-1f,-1f,0f)};

        triangles = new int[] {0, 1, 2,
                               0, 2, 3};
        //vertices = mesh.vertices;

        tempMesh.vertices = vertices;

        tempMesh.triangles = triangles;

        //temp



    }       // Start()


}       // SG_Test
