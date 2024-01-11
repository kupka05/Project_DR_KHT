using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using System.Text;




public class SG_Test : MonoBehaviour
{

    public Material tempMaterial;
    public Vector3[] vertices;
    public int[] triangles;
    Mesh mesh;
    MeshFilter filter;

    GameObject tempObj;
    Mesh tempMesh;

    StringBuilder sb;

    private void Awake()
    {
        sb = new StringBuilder();
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

        tempObj.transform.position = new Vector3(0f, 0f, 10f);

        MeshFilter tempObjMesh = tempObj.GetComponent<MeshFilter>();
        MeshRenderer tempObjRenderer = tempObj.AddComponent<MeshRenderer>();

        tempObjMesh.mesh = tempMesh;

        tempObjRenderer.material = tempMaterial;


        //temp

        sb.Append("Triangles :");
        foreach (int triangle in filter.mesh.triangles)
        {
            sb.Append(" ");
            sb.Append(triangle);
        }

        Debug.Log(sb.ToString());
        Debug.Log($"Triangle.Length : {filter.mesh.triangles.Length}");

        List<int> testList = new List<int>();

        testList.Add(0);
        testList.Add(1);
        testList.Add(2);

        int[] testArr = testList.ToArray();

        Debug.Log($"arrLength : {testArr.Length}\nOriginListCount : {testList.Count}");


    }       // Start()


}       // SG_Test
