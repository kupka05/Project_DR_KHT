using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BattleRoomRunTimeNav : MonoBehaviour
{


    private NavMeshSurface navgation;
    private NavMeshData navMeshData;
    private FloorMeshPos cornerPos;
    private void Awake()
    {
        AwakeInIt();
        Test();
        BuildNavMesh();
    }

    private void AwakeInIt()
    {
        this.gameObject.AddComponent<NavMeshSurface>();
        navgation = GetComponent<NavMeshSurface>();
        cornerPos = GetComponent<FloorMeshPos>();                

    }
    

    /// <summary>
    /// 네비매시 베이크하는 함수
    /// </summary>
    private void BuildNavMesh()
    {
        navgation.BuildNavMesh();
    }

    private void Test()
    {
        int navLayerMask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Water");
        this.gameObject.layer = 4;
        navgation.layerMask = navLayerMask;
        Debug.Log($"Layer : {this.gameObject.layer}\n navLayer : {navgation.layerMask}");




        navMeshData = new NavMeshData();
        navMeshData.position = new Vector3(
            (cornerPos.bottomLeftCorner.x + cornerPos.bottomLeftCorner.x) * 0.5f,
            0.2f,
            (cornerPos.bottomLeftCorner.z + cornerPos.topLeftCorner.z) * 0.5f);
        
        
        navgation.navMeshData = navMeshData;

    }




}       // ClassEnd
