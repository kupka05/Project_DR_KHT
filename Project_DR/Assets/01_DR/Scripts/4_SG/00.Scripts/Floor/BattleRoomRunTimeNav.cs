using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;



public class BattleRoomRunTimeNav : MonoBehaviour
{


    private NavMeshSurface navgation;    
    private FloorMeshPos cornerPos;
    private void Awake()
    {
        AwakeInIt();
        SettingNavLayer();
        BuildNavMesh();
    }

    private void AwakeInIt()
    {
        this.gameObject.AddComponent<NavMeshSurface>();
        navgation = GetComponent<NavMeshSurface>();
        cornerPos = GetComponent<FloorMeshPos>();                

    }

    /// <summary>
    /// 네비게이션레이어설정해주는 함수
    /// </summary>
    private void SettingNavLayer()
    {
        //int floorLayer = (int)Layer.BattleRoomFloor;    // Layer : BattleRoomLayer 가져와야함
        this.gameObject.layer = (int)Layer.BattleRoomFloor;

        LayerMask layerMask = 1 << (int)Layer.MapObject | 1 << (int)Layer.BattleRoomFloor;
        navgation.layerMask = layerMask;

    }       // SettingNavLayer()



    /// <summary>
    /// 네비매시 베이크하는 함수
    /// </summary>
    private void BuildNavMesh()
    {
        navgation.BuildNavMesh();
    }






}       // ClassEnd
