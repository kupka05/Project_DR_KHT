using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// 스폰될 오브젝트의 포지션이 고정값인지 아닌지 구별할 Enum
public enum SpawnPlace
{
    Floor = 0,
    Roop = 1,
    Wall = 2

}

public enum SpawnCompass
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
    RandomMax = 4
}

public class RandomRoomObjCreate : MonoBehaviour
{       // 방 오브젝트를 깔아주는 Class
        // 1. 여기서 Create해줌 -> 공통된 기능이 필요하기 때문
        // 2. 이 Class를 상속받는 곳에서 어떤것을 만들지 지정 -> 방마다 제작해야 하는리스트가 다르기때문


    [SerializeField]
    public StringBuilder stringBuilder; // 생성할때에 어떤걸 생성할지 PrefabName을 끌어올 StringBuilder        
    [SerializeField]
    public GameObject parentObj;         // 생성한 오브젝트를 담아둘 ParentObj
    [SerializeField]
    public List<Vector3> spawnPosList;  // 아이템 스폰 위치를 담아둘 List
    [SerializeField]
    public FloorMeshPos cornerPos;      // 해당방에 꼭지점을 담아둔 Class
    private int reCallCount;            // 재귀 Count
    private bool createPass;            // 이번생성 Pass할지 체크할 bool값


    protected virtual void Awake()
    {
        AwakeInIt();
    }

    private void AwakeInIt()
    {
        stringBuilder = new StringBuilder();
        spawnPosList = new List<Vector3>();
        parentObj = new GameObject("SpawnObjs");
        cornerPos = GetComponent<FloorMeshPos>();

        parentObj.transform.parent = this.transform;

        reCallCount = 0;
        createPass = false;
    }




    /// <summary>
    /// Light를 스폰해주는 함수 
    /// </summary>
    /// <param name="_lightCreateTableId">각 방의 아이템 스폰 테이블 ID</param>
    protected void SpawnLightObj(int _lightCreateTableId)
    {
        List<int> lightSpawnIdList = new List<int>();       // 중복처리에 사용될 List        

        int lightCategoryID = (int)DataManager.Instance.GetData(_lightCreateTableId, "CategoryID", typeof(int));

        int lightSpawnCount = UnityEngine.Random.Range
           ((int)DataManager.Instance.GetData(_lightCreateTableId, "MinValue", typeof(int)),
           (int)DataManager.Instance.GetData(_lightCreateTableId, "MaxValue", typeof(int)) + 1);


        for (int i = 0; i < lightSpawnCount; i++)
        {

            int pickObjID = UnityEngine.Random.Range(0, DataManager.Instance.GetCount(lightCategoryID)) + lightCategoryID;
            if (lightSpawnIdList.Contains(pickObjID))
            {
                while (!lightSpawnIdList.Contains(pickObjID))
                {       // 랜덤하게 나온 값이 중복으로나오지 않을때까지 랜덤을 돌림
                    pickObjID = UnityEngine.Random.Range(0, DataManager.Instance.GetCount(lightCategoryID)) + lightCategoryID;
                }
            }
            else { /*PASS*/ }

            lightSpawnIdList.Add(pickObjID);

            CreateObj(pickObjID);
        }

    }       // SpawnLight()

    /// <summary>
    /// EnvObj생성해주는 함수
    /// </summary>
    /// <param name="_envCreateTableID">각방의 ObjectCreate_Table ID</param>
    protected void SpawnEnvObj(int _envCreateTableID)
    {
        int envCategoryID = (int)DataManager.Instance.GetData(_envCreateTableID, "CategoryID", typeof(int));

        int envSpawnCount = UnityEngine.Random.Range
            ((int)DataManager.Instance.GetData(_envCreateTableID, "MinValue", typeof(int)),
            (int)DataManager.Instance.GetData(_envCreateTableID, "MaxValue", typeof(int)) + 1);

        for (int i = 0; i < envSpawnCount; i++)
        {

            int pickObjID = UnityEngine.Random.Range(0, DataManager.Instance.GetCount(envCategoryID)) + envCategoryID;

            CreateObj(pickObjID);
        }
    }       // SpawnEnvObj()

    /// <summary>
    /// MatObj생성해주는 함수
    /// </summary>
    /// <param name="_matCreateID">각방의 ObjectCreate_Table ID</param>
    protected void SpawnMatObj(int _matCreateID)
    {
        int matCateforyID = (int)DataManager.Instance.GetData(_matCreateID, "CategoryID", typeof(int));

        int envSpawnCount = UnityEngine.Random.Range
          ((int)DataManager.Instance.GetData(_matCreateID, "MinValue", typeof(int)),
          (int)DataManager.Instance.GetData(_matCreateID, "MaxValue", typeof(int)) + 1);

        for (int i = 0; i < envSpawnCount; i++)
        {

            int pickObjID = UnityEngine.Random.Range(0, DataManager.Instance.GetCount(matCateforyID)) + matCateforyID;

            CreateObj(pickObjID);
        }
    }       // SpawnMatObj





    /// <summary>
    /// 해당방에 오브젝트를 생성해줄 함수
    /// </summary>
    private void CreateObj(int _CreateObjId)
    {
        GameObject spawnObjClone = spawnObjInIt(_CreateObjId);
        Vector3 spawnPos = PickSpwanPos(_CreateObjId);
        InstantiateObj(spawnObjClone, spawnPos);


    }       // CreateObj()

    /// <summary>
    /// 방 내부에서 랜덤한 포지션값을 지정해주는 함수
    /// </summary>
    /// <returns>중복 검사이후 중복이 없는 Vector3값</returns>
    private Vector3 PickSpwanPos(int _CreateObjId)
    {
        List<Vector3> doorPos = new List<Vector3>();
        Vector3 upDoorPos = new Vector3((cornerPos.topLeftCorner.x + cornerPos.topRightCorner.x) * 0.5f, 0f, cornerPos.topLeftCorner.z);
        Vector3 downDoorPos = new Vector3((cornerPos.bottomLeftCorner.x + cornerPos.bottomRightCorner.x) * 0.5f, 0f, cornerPos.topLeftCorner.z);
        Vector3 leftDoorPos = new Vector3(cornerPos.bottomLeftCorner.x, 0f, (cornerPos.bottomLeftCorner.z + cornerPos.topLeftCorner.z) * 0.5f);
        Vector3 rightDoorPos = new Vector3(cornerPos.bottomLeftCorner.x, 0f, (cornerPos.bottomRightCorner.z + cornerPos.topRightCorner.z) * 0.5f);

        doorPos.Add(upDoorPos);
        doorPos.Add(downDoorPos);
        doorPos.Add(leftDoorPos);
        doorPos.Add(rightDoorPos);


        int spawnPlace = (int)DataManager.Instance.GetData(_CreateObjId, "SpawnPlace", typeof(int));
        bool isWallSpawn = false;
        Vector3 tempPos;
        if (reCallCount >= 15)
        {
            createPass = true;
            return Vector3.zero;
        }
        else { /*PASS*/ }


        if (spawnPlace == (int)SpawnPlace.Floor)
        {
            tempPos = GetFloorSpawnPos();
            reCallCount++;
        }
        else if (spawnPlace == (int)SpawnPlace.Roop)
        {
            tempPos = GetRoopSpawnPos(_CreateObjId);
        }
        else if (spawnPlace == (int)SpawnPlace.Wall)
        {
            int isSpawnCompass = UnityEngine.Random.Range((int)SpawnCompass.Up, (int)SpawnCompass.RandomMax);
            isWallSpawn = true;

            tempPos = GetWallSpawnPos(_CreateObjId, isSpawnCompass);
            bool isCollision = IsNotCollisionDoor(tempPos, doorPos);
            if (isCollision == true)
            {
                PickSpwanPos(_CreateObjId);
            }
        }
        else
        {
            GFunc.Log("방 오브젝트 스폰에서 예외적인 상황이 발생했습니다.");
            tempPos = Vector3.zero;
        }


        float dis;

        foreach (Vector3 pos in spawnPosList)
        {
            dis = Vector3.Distance(pos, tempPos);

            if (dis < 2f)
            {
                reCallCount++;
                return PickSpwanPos(_CreateObjId);

            }
        }

        //if(spawnPosList.Contains(tempPos))
        //{
        //    return PickSpwanPos();
        //}

        spawnPosList.Add(tempPos);
        return tempPos;
    }       // PickSpwanPos()

    /// <summary>
    /// 벽에 생성되어야하는 오브젝트 포지션 지정해주는 함수
    /// </summary>
    /// <param name="_CreateObjId">생성할오브젝트의 ID</param>
    /// <param name="_isSpawnCompass">생성할 오브젝트의 위치</param>
    /// <returns>랜덤으로 지정된 위치</returns>
    private Vector3 GetWallSpawnPos(int _CreateObjId, int _isSpawnCompass)
    {
        float xPos;
        float yPos = Data.GetFloat(9009, "RoopYpos");
        yPos = yPos * 0.5f;
        float zPos;        
        Vector3 spawnPos;
        switch (_isSpawnCompass)
        {
            case (int)SpawnCompass.Up:
                xPos = UnityEngine.Random.Range(cornerPos.topLeftCorner.x + 0.5f, cornerPos.topRightCorner.x);
                zPos = cornerPos.topLeftCorner.z - 0.5f;
                return spawnPos = new Vector3(xPos, yPos, zPos);

            case (int)SpawnCompass.Down:
                xPos = UnityEngine.Random.Range(cornerPos.bottomLeftCorner.x + 0.5f, cornerPos.bottomRightCorner.x);
                zPos = cornerPos.bottomLeftCorner.z + 0.5f;
                return spawnPos = new Vector3(xPos, yPos, zPos);

            case (int)SpawnCompass.Left:
                xPos = cornerPos.bottomLeftCorner.x + 0.5f;
                zPos = UnityEngine.Random.Range(cornerPos.bottomLeftCorner.z + 0.5f, cornerPos.topLeftCorner.z);
                return spawnPos = new Vector3(xPos, yPos, zPos);

            case (int)SpawnCompass.Right:
                xPos = cornerPos.bottomLeftCorner.x - 0.5f;
                zPos = UnityEngine.Random.Range(cornerPos.bottomRightCorner.z + 0.5f, cornerPos.topRightCorner.z);
                return spawnPos = new Vector3(xPos, yPos, zPos);
        }

        GFunc.Log($"해당 리턴에 들어오면 안되는데 들어옴");
        return Vector3.zero;

    }       // GetWallSpawnPos()

    private bool IsNotCollisionDoor(Vector3 _tempSpawnPos,List<Vector3> _doorList)
    {
        foreach(Vector3 tempV3 in _doorList)
        {
            float dis = Vector3.Distance(_tempSpawnPos,tempV3);
            if(dis > 3)
            {
                return true;
            }
        }

        return false;

    }       // isNotCollisionDoor()

    /// <summary>
    /// 바닥에 생성되어야 하는 오브젝트 포지션 지정해주는 함수
    /// </summary>
    /// <returns>랜덤으로 지정된 위치</returns>
    private Vector3 GetFloorSpawnPos()
    {
        Vector3 spawnPos;
        float xPos = UnityEngine.Random.Range(cornerPos.bottomLeftCorner.x + 1f, cornerPos.bottomRightCorner.x - 1f);
        float yPos = 0f;
        float zPos = UnityEngine.Random.Range(cornerPos.bottomLeftCorner.z + 1f, cornerPos.topLeftCorner.z - 1f);

        return spawnPos = new Vector3(xPos, yPos, zPos);
    }       // GetFloorSpawnPos()

    /// <summary>
    /// 지붕에 생성되어야 하는 오브젝트 포지션 지정해주는 함수
    /// </summary>
    /// <param name="_createObjId">생성할 오브젝트의 ID</param>
    /// <returns>랜덤으로 지정된 위치</returns>
    private Vector3 GetRoopSpawnPos(int _createObjId)
    {
        Vector3 spawnPos;
        float xPos = (cornerPos.bottomLeftCorner.x + cornerPos.bottomRightCorner.x) * 0.5f;
        float yPos = (float)DataManager.Instance.GetData(_createObjId, "PosY", typeof(float)); // 대충 지붕 위치
        float zPos = (cornerPos.bottomLeftCorner.z + cornerPos.topLeftCorner.z) * 0.5f;

        return spawnPos = new Vector3(xPos,yPos, zPos);
    }       // GetRoopSpawnPos()


    /// <summary>
    /// Id가 가지고 있는 Prefab의 이름을 가져와서 Resource폴더속있는 GameObject를 대입해서 리턴해주는 함수
    /// </summary>
    /// <param name="_objId">생성할 오브젝트의 ID</param>
    /// <returns>Resource폴더속있는 GameObject를 대입한 GameObject</returns>
    private GameObject spawnObjInIt(int _objId)
    {
        stringBuilder.Clear();
        stringBuilder.Append((string)DataManager.Instance.GetData(_objId, "PrefabName", typeof(string)));
        //GFunc.Log($"들어간 Sb : {stringBuilder}\n sb에 참조된 ID : {_objId}");        
        GameObject prefabObj = Resources.Load<GameObject>($"{stringBuilder}");
        //GFunc.Log($"지정된 Prefab : {prefabObj}");
        return prefabObj;
    }       // spawnObjInIt(int)

    /// <summary>
    /// 정해진 위치와 오브젝트를 인스턴스해주는 함수
    /// </summary>
    /// <param name="_spawnObj">소환할 오브젝트</param>
    /// <param name="_spawnPos">소환할 위치</param>
    private void InstantiateObj(GameObject _spawnObj, Vector3 _spawnPos)
    {
        if (createPass == true)
        {
            //GFunc.Log($"제작 패스");
            createPass = false;
            reCallCount = 0;
            return;
        }
        else { /*PASS*/ }
        GameObject spawnObjClone = Instantiate(_spawnObj, _spawnPos, Quaternion.identity, parentObj.transform);

        reCallCount = 0;
        ObjectLayerSetting(spawnObjClone);

    }       // InstantiateObj(GameObject,Vecotr3)

    /// <summary>
    /// 인스턴스 된 오브젝트에 레이어를 수정해주는 함수
    /// </summary>
    private void ObjectLayerSetting(GameObject _spawnObj)
    {
        _spawnObj.layer = (int)Layer.MapObject;
    }       // ObjectLayerSetting()


}       // ClassEnd
