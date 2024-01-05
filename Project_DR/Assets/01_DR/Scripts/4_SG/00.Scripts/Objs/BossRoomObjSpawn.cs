using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomObjSpawn : MonoBehaviour
{       // BossRoom스크립트가 보스 스폰할때에 해당 스크립트를 AddComponent해줄것임
    enum ColumnID
    {
        defaultColumn = 16031,
        hookShootColumn = 16032
    }
    [SerializeField]
    GameObject defaultColumn;
    [SerializeField]
    GameObject hookShootColumn;

    //private int defaultColumnMinSpawnValue;             // 기본형 기둥 스폰 최소치              // 스코프 변수로 변경
    //private int defaultColumnMaxSpawnValue;             // 기본형 기둥 스폰 최대치              // 스코프 변수로 변경

    //private float defaultColumnYSizeMinValue;           // 기본형 기둥의 랜덤한 Y사이즈 최소치   // 스코프 변수로 변경
    //private float defaultColumnYSizeMaxValue;           // 기본형 기둥의 랜덤한 Y사이즈 최대치   // 스코프 변수로 변경


    //private int hookShootColumnSpawnValue;              // 훅샷기둥 스폰될 수 // 스코프 변수로 변경
    //private float hookShhotColumnDis;                     // 훅샷기둥간의 거리  // 스코프 변수로 변경


    private List<Vector3> defaultColumnSpawnPosList;    // 스폰된 기본형 기둥의 좌표
    private Vector3 bossPos;                            // 보스의 좌표
    private int hookShootSpawnCount;                    // 훅샷기둥 스폰한 횟수
    private FloorMeshPos roomPos;
    private float hookShootColumnWallDis;               // 벽과 훅샷기둥의 거리  ex) 0.1 = 벽의 10%    
    


    private void Awake()
    {
        DataInIt();
    }

    private void Start()
    {
        Invoke("StartSpawn", 10f);
    }

    private void DataInIt()
    {
        roomPos = this.gameObject.GetComponent<FloorMeshPos>();
        hookShootColumnWallDis = Data.GetFloat(16033, "HookShootColumnWallDis");        
        defaultColumnSpawnPosList = new List<Vector3>();
        hookShootSpawnCount = 1;
        defaultColumn = (GameObject)Resources.Load("tempDefaultColumn");
        hookShootColumn = (GameObject)Resources.Load("HookShotColumn");

    }       // DataInIt()

    public void InItBossSpawnPos(Vector3 _bossSpawnPos)
    {       // 해당 매서드는 BossRoom이 Boss를 스폰하면서 실행해줄것임
        bossPos = _bossSpawnPos;
    }       // InItBossPawnPos


    public void StartSpawn()
    {       // Start지점에서 Invoke함수로 Call함
        GameObject objParent = new GameObject("SpawnObjs");
        objParent.transform.parent = this.transform;

        // {----------------------------------------- 기본 기둥 소환 관련 ---------------------------------------------
        int defaultColumnMinSpawnValue = Data.GetInt((int)ColumnID.defaultColumn, "DefaultColumnMinSpawnValue");
        int defaultColumnMaxSpawnValue = Data.GetInt((int)ColumnID.defaultColumn, "DefaultColumnMaxSpawnValue");
        float defaultColumnYSizeMinValue = Data.GetFloat((int)ColumnID.defaultColumn, "DefaultColumnYSizeMinValue");
        float defaultColumnYSizeMaxValue = Data.GetFloat((int)ColumnID.defaultColumn, "DefaultColumnYSizeMaxValue");
        float defalutColumnXSizeMinValue = Data.GetFloat((int)ColumnID.defaultColumn, "DefalutColumnXSizeMinValue");
        float defalutColumnXSizeMaxValue = Data.GetFloat((int)ColumnID.defaultColumn, "DefalutColumnXSizeMaxValue");
        float bossRoomWidth = Data.GetInt(9105, "BossRoomWidth");

        int defualtColumnSpawnCount = UnityEngine.Random.Range(defaultColumnMinSpawnValue, defaultColumnMaxSpawnValue + 1);
        float spawnStartDefaultColumnWidth = bossRoomWidth * hookShootColumnWallDis;
        float spawnEndDefaultColumnWidth = bossRoomWidth - spawnStartDefaultColumnWidth;

        for (int i = 0; i < defualtColumnSpawnCount; i++)
        {
            SpawnDefaultColumn(objParent, spawnStartDefaultColumnWidth, spawnEndDefaultColumnWidth,
                defaultColumnYSizeMinValue, defaultColumnYSizeMaxValue,defalutColumnXSizeMinValue,
                defalutColumnXSizeMaxValue);
        }
        // }----------------------------------------- 기본 기둥 소환 관련 ---------------------------------------------

        // {----------------------------------------- 훅샷 기둥 소환 관련 ---------------------------------------------
        int hookShootColumnSpawnValue = Data.GetInt((int)ColumnID.hookShootColumn, "HookShootColumnSpawnValue");
        float hookShotColumnDis = Data.GetFloat((int)ColumnID.hookShootColumn, "HookShootColumnDis");
        //GFunc.Log($"시트에서 가져온 Dis값 : {hookShootSpawnCount}\n소환 카운트 : {hookShootSpawnCount}");

        float leftColumnXPos = roomPos.bottomLeftCorner.x + (bossRoomWidth * hookShootColumnWallDis);
        float rightColumnXPos = roomPos.bottomRightCorner.x - (bossRoomWidth * hookShootColumnWallDis);
        for (int i = 0; i < hookShootColumnSpawnValue; i++)
        {
            SpawnHookShootColumn(objParent, leftColumnXPos, rightColumnXPos, hookShotColumnDis);
        }
    }       // StartSpawn()




    private void SpawnDefaultColumn(GameObject _objParent, float _spawnStartDefaultColumnWidth,
        float _spawnEndDefaultColumnWidth, float _minYSclae, float _maxYSclae,
        float _minXScale,float _maxXSclae)
    {       // 기본형 기둥을 인스턴스해주는 함수
            //float bossRoomWidth = Data.GetInt(9105, "BossRoomWidth");
            //float spawnStartWidth = bossRoomWidth * 0.3f;
            //float spawnEndWidth = bossRoomWidth - spawnStartWidth;

        float spawnPosX = UnityEngine.Random.Range(roomPos.bottomLeftCorner.x + _spawnStartDefaultColumnWidth,
            roomPos.bottomRightCorner.x - _spawnStartDefaultColumnWidth);
        float spawnPosY = 0f;

        float sapwnPosZ = UnityEngine.Random.Range(roomPos.bottomLeftCorner.z + 2f,
            roomPos.topLeftCorner.z - 3f);
        Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, sapwnPosZ);

        foreach (Vector3 pos in defaultColumnSpawnPosList)
        {
            if (spawnPos == pos)
            {
                continue;
            }
        }
        defaultColumnSpawnPosList.Add(spawnPos);

        GameObject spawnClone = Instantiate(defaultColumn, spawnPos, Quaternion.identity, _objParent.transform);


        float randYSclae = UnityEngine.Random.Range(_minYSclae, _maxYSclae + 1);
        float randXScale = UnityEngine.Random.Range(_minXScale, _maxXSclae + 1);
        spawnClone.transform.localScale = new Vector3(randXScale, randYSclae, 1f);

        spawnClone.transform.position = new Vector3(spawnClone.transform.position.x, spawnClone.transform.localScale.y * 0.5f,
            spawnClone.transform.position.z);

        spawnClone.transform.tag = "Wall";

    }       // SpawnDefaultColumn()

    private void SpawnHookShootColumn(GameObject _objParent, float _leftColumnXPos, float _rightColumnXPos,
        float _hookShotColumnDis)
    {       // 훅샷기둥을 인스턴스해주는 함수
        Vector3 leftColumnPos = new Vector3(_leftColumnXPos, 6f
            , roomPos.bottomLeftCorner.z + 2f + (_hookShotColumnDis * hookShootSpawnCount));

        GameObject leftColumnClone = Instantiate(hookShootColumn, leftColumnPos, Quaternion.identity, _objParent.transform);
        leftColumnClone.transform.localPosition = leftColumnPos;

        leftColumnClone.transform.tag = "Wall";

        Vector3 rightColumnPos = new Vector3(_rightColumnXPos, 6f,
            leftColumnPos.z);

        GameObject rightColumnClone = Instantiate(hookShootColumn, rightColumnPos, Quaternion.identity, _objParent.transform);
        rightColumnClone.transform.localPosition = rightColumnPos;

        rightColumnClone.transform.tag = "Wall";
        hookShootSpawnCount++;
    }       // SpawnHookShootColumn()

}       // ClassEnd
