using Oculus.Platform;
using OVR.OpenVR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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


    #region 변수
    /// <summary>
    /// 생성할때에 어떤걸 생성할지 PrefabName을 끌어올 StringBuilder
    /// </summary>
    [SerializeField]
    public StringBuilder stringBuilder;
    /// <summary>
    /// 생성한 오브젝트를 담아둘 ParentObj
    /// </summary>
    [SerializeField]
    public GameObject parentObj;
    /// <summary>
    /// 아이템 스폰 위치를 담아둘 List
    /// </summary>
    [SerializeField]
    public List<Vector3> spawnPosList;
    /// <summary>
    /// 해당방에 꼭지점을 담아둔 Class
    /// </summary>
    [SerializeField]
    public FloorMeshPos cornerPos;
    /// <summary>
    /// 스폰시 스폰이 안돼야하는 포지션이 담긴 리스트
    /// </summary>
    [SerializeField]
    private List<Vector3> exceptionV3List;
    /// <summary>
    /// 재귀한 횟수
    /// </summary>
    private int reCallCount;
    /// <summary>
    /// 이번생성 Pass할지 체크할 bool값
    /// </summary>
    private bool createPass;
    #endregion 변수


    List<Vector3> doorPos;
    Vector3 upDoorPos;
    Vector3 downDoorPos;
    Vector3 leftDoorPos;
    Vector3 rightDoorPos;
    int spawnPlace;

    private SpawnCompass spawnCompass;
    protected virtual void Awake()
    {
        AwakeInIt();
        ExceptionV3Setting();
    }

    private void AwakeInIt()
    {
        stringBuilder = new StringBuilder();
        spawnPosList = new List<Vector3>();
        exceptionV3List = new List<Vector3>();
        parentObj = new GameObject("SpawnObjs");
        cornerPos = this.GetComponent<FloorMeshPos>();

        doorPos = new List<Vector3>();
        upDoorPos = new Vector3((cornerPos.topLeftCorner.x + cornerPos.topRightCorner.x) * 0.5f, 0f, cornerPos.topLeftCorner.z);
        downDoorPos = new Vector3((cornerPos.bottomLeftCorner.x + cornerPos.bottomRightCorner.x) * 0.5f, 0f, cornerPos.bottomLeftCorner.z);
        leftDoorPos = new Vector3(cornerPos.bottomLeftCorner.x, 0f, (cornerPos.bottomLeftCorner.z + cornerPos.topLeftCorner.z) * 0.5f);
        rightDoorPos = new Vector3(cornerPos.bottomRightCorner.x, 0f, (cornerPos.bottomRightCorner.z + cornerPos.topRightCorner.z) * 0.5f);


        doorPos.Add(upDoorPos);
        doorPos.Add(downDoorPos);
        doorPos.Add(leftDoorPos);
        doorPos.Add(rightDoorPos);

        parentObj.transform.parent = this.transform;

        reCallCount = 0;
        createPass = false;

    }

    /// <summary>
    /// 방 중앙기준에서 3 x 3 포지션은 예외의 포지션으로 선택되지않도록 하기위해 넣는 위치
    /// </summary>
    private void ExceptionV3Setting()
    {
        Vector3 centerPos = new Vector3((cornerPos.bottomLeftCorner.x + cornerPos.bottomRightCorner.x) * 0.5f,
            cornerPos.bottomLeftCorner.y, (cornerPos.bottomLeftCorner.z + cornerPos.topLeftCorner.z) * 0.5f);

        float radius = 1.5f;
        Vector3 startExceptionPos = new Vector3(centerPos.x - radius, centerPos.y, centerPos.z + radius);

        float reslutX;
        float reslutZ;
        Vector3 reslutPos = Vector3.zero;
        for (float depth = 0; depth < radius * 2; depth++)
        {
            for (float width = 0; width < radius * 2; width++)
            {
                reslutX = startExceptionPos.z - depth;
                reslutZ = startExceptionPos.x + width;
                reslutPos.x = reslutX;
                reslutPos.z = reslutZ;
                exceptionV3List.Add(reslutPos);
            }
        }


    }       // ExceptionV3Setting()


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

            CreateMatObj(pickObjID);
        }
    }       // SpawnMatObj


    /// <summary>
    ///  Mat오브젝트를 생성해주는 함수
    /// </summary>
    /// <param name="_pickObjId">랜덤으로 선택된 오브젝트의 아이디값</param>
    private void CreateMatObj(int _pickObjId)
    {

        Vector3 spawnPos = PickSpwanPos(_pickObjId);
        if (spawnPos != Vector3.zero)
        {
            int pickObjRefId = Data.GetInt(_pickObjId, "ReferenceObjID");
            spawnPosList.Add(spawnPos);
            bool isWallSpawn = ObjRotationSetting(_pickObjId);
            GameObject spawnObj = Unit.AddFieldItem(spawnPos, pickObjRefId);
            //Debug.Log($"Mat 스폰된 오브젝트 : {spawnObj.name} ID: {pickObjRefId}");
            spawnObj.transform.parent = parentObj.transform;
            spawnObj.transform.localPosition = spawnPos;
            spawnObj.layer = (int)Layer.MapObject;
        }
        else { /*PASS*/ }

    }       // CreateMatObj()



    /// <summary>
    /// 해당방에 오브젝트를 생성해줄 함수
    /// </summary>
    private void CreateObj(int _CreateObjId)
    {
        GameObject spawnObjClone = spawnObjInIt(_CreateObjId);
        Vector3 spawnPos = PickSpwanPos(_CreateObjId);
        if (spawnPos != Vector3.zero)
        {
            spawnPosList.Add(spawnPos);
            bool isWallSpawn = ObjRotationSetting(_CreateObjId);
            InstantiateObj(spawnObjClone, spawnPos, isWallSpawn);
        }
        else { /*PASS*/ }


    }       // CreateObj()

    /// <summary>
    /// 방 내부에서 랜덤한 포지션값을 지정해주는 함수
    /// </summary>
    /// <returns>중복 검사이후 중복이 없는 Vector3값</returns>
    private Vector3 PickSpwanPos(int _CreateObjId)
    {
        spawnPlace = (int)DataManager.Instance.GetData(_CreateObjId, "SpawnPlace", typeof(int));
        Vector3 tempPos;
        if (reCallCount >= 15)
        {
            //GFunc.Log($"초과재귀로 오브젝트 생성 건너뜀");
            reCallCount = 0;
            createPass = true;
            return Vector3.zero;
        }
        else { /*PASS*/ }


        if (spawnPlace == (int)SpawnPlace.Floor)
        {
            tempPos = GetFloorSpawnPos(_CreateObjId);
        }
        else if (spawnPlace == (int)SpawnPlace.Roop)
        {
            tempPos = GetRoopSpawnPos(_CreateObjId);
        }
        else if (spawnPlace == (int)SpawnPlace.Wall)
        {
            //Debug.Log("벽오브젝트 생성으로 진입");
            int isSpawnCompass = UnityEngine.Random.Range((int)SpawnCompass.Up, (int)SpawnCompass.RandomMax);
            spawnCompass = (SpawnCompass)isSpawnCompass;
            tempPos = GetWallSpawnPos(isSpawnCompass);
            bool isCollision = IsNotCollisionDoor(tempPos);
            if (isCollision == true)
            {
                reCallCount++;
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

            if (dis < 1f)
            {
                reCallCount++;
                return PickSpwanPos(_CreateObjId);

            }
        }
        return tempPos;
    }       // PickSpwanPos()

    /// <summary>
    /// 벽에 생성되어야하는 오브젝트 포지션 지정해주는 함수
    /// </summary>
    /// <param name="_CreateObjId">생성할오브젝트의 ID</param>
    /// <param name="_isSpawnCompass">생성할 오브젝트의 위치</param>
    /// <returns>랜덤으로 지정된 위치</returns>
    private Vector3 GetWallSpawnPos(int _isSpawnCompass)
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
                spawnPos = new Vector3(xPos, yPos, zPos);
                return spawnPos;

            case (int)SpawnCompass.Down:
                xPos = UnityEngine.Random.Range(cornerPos.bottomLeftCorner.x + 0.5f, cornerPos.bottomRightCorner.x);
                zPos = cornerPos.bottomLeftCorner.z + 0.5f;
                spawnPos = new Vector3(xPos, yPos, zPos);
                return spawnPos;

            case (int)SpawnCompass.Left:
                xPos = cornerPos.bottomLeftCorner.x + 0.5f;
                zPos = UnityEngine.Random.Range(cornerPos.bottomLeftCorner.z + 0.5f, cornerPos.topLeftCorner.z);
                spawnPos = new Vector3(xPos, yPos, zPos);
                return spawnPos;

            case (int)SpawnCompass.Right:
                xPos = cornerPos.bottomLeftCorner.x - 0.5f;
                zPos = UnityEngine.Random.Range(cornerPos.bottomRightCorner.z + 0.5f, cornerPos.topRightCorner.z);
                spawnPos = new Vector3(xPos, yPos, zPos);
                return spawnPos;
        }

        //GFunc.Log($"해당 리턴에 들어오면 안되는데 들어옴");
        return Vector3.zero;

    }       // GetWallSpawnPos()

    private bool ObjRotationSetting(int _createObjId)
    {

        int objCompass = Data.GetInt(_createObjId, "SpawnPlace");
        if (objCompass == (int)SpawnPlace.Wall)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 문이 존재하는 위치와의 Distance가 일정거리 이상이 되는지 확인해주는 함수
    /// </summary>
    /// <param name="_tempSpawnPos">적용되기 전에 스폰될 위치</param>
    /// <param name="_doorList">문이 존재하는 위치 리스트</param>
    /// <returns></returns>
    private bool IsNotCollisionDoor(Vector3 _tempSpawnPos)
    {
        foreach (Vector3 tempV3 in doorPos)
        {
            float dis = Vector3.Distance(_tempSpawnPos, tempV3);
            if (dis < 3)
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
    private Vector3 GetFloorSpawnPos(int _createObjId)
    {
        Vector3 spawnPos;
        float xPos = UnityEngine.Random.Range(cornerPos.bottomLeftCorner.x + 1f, cornerPos.bottomRightCorner.x - 1f);
        float yPos;
        float zPos = UnityEngine.Random.Range(cornerPos.bottomLeftCorner.z + 1f, cornerPos.topLeftCorner.z - 1f);

        if (Data.GetString(_createObjId, "PrefabName").Contains("Flower") == true)
        {
            yPos = 0.2f;
        }
        else
        {
            yPos = 0f;
        }

        spawnPos = new Vector3(xPos, yPos, zPos);

        foreach (Vector3 exceptionPos in exceptionV3List)
        {
            if (exceptionPos == spawnPos)
            {
                reCallCount++;
                PickSpwanPos(_createObjId);
            }
        }

        return spawnPos;
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

        return spawnPos = new Vector3(xPos, yPos, zPos);
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
    private void InstantiateObj(GameObject _spawnObj, Vector3 _spawnPos, bool _isWallSpawn)
    {
        createPass = false;
        reCallCount = 0;

        GameObject spawnObjClone = Instantiate(_spawnObj, _spawnPos, Quaternion.identity, parentObj.transform);
        ObjectLayerSetting(spawnObjClone);
        //GFunc.Log($"스폰된 오브젝트 : {spawnObjClone.name}");
        if (_isWallSpawn == true)
        {
            SpawnObjRotationSet(spawnObjClone);
        }
        else { /*PASS*/ }

    }       // InstantiateObj(GameObject,Vecotr3)

    /// <summary>
    /// 인스턴스 된 오브젝트에 레이어를 수정해주는 함수
    /// </summary>
    private void ObjectLayerSetting(GameObject _spawnObj)
    {
        _spawnObj.layer = (int)Layer.MapObject;
    }       // ObjectLayerSetting()

    private void SpawnObjRotationSet(GameObject _spawnObjClone)
    {
        if (spawnCompass == SpawnCompass.Up)
        {
            _spawnObjClone.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (spawnCompass == SpawnCompass.Down)
        {
            _spawnObjClone.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (spawnCompass == SpawnCompass.Left)
        {
            _spawnObjClone.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (spawnCompass == SpawnCompass.Right)
        {
            _spawnObjClone.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
        }
        else
        {
            // 추가적인 축 추가를 위해 임시로 비워둠
        }
    }       // SpawnObjRotationSet()

    protected void StartSpawn(int _lightCreateTableID, int _envCreateTableId, int _matCreateTableId)
    {
        StartCoroutine(OderSpawn(_lightCreateTableID, _envCreateTableId, _matCreateTableId));
    }

    IEnumerator OderSpawn(int _lightCreateTableID, int _envCreateTableId, int _matCreateTableId)
    {
        GameManager.instance.spawnDelayFrame++;
        for (int i = 0; i < GameManager.instance.spawnDelayFrame; i++)
        {
            yield return null;
        }

        SpawnLightObj(_lightCreateTableID);
        GameManager.instance.spawnDelayFrame++;
        for (int i = 0; i < GameManager.instance.spawnDelayFrame; i++)
        {
            yield return null;
        }

        SpawnEnvObj(_envCreateTableId);
        GameManager.instance.spawnDelayFrame++;
        for (int i = 0; i < GameManager.instance.spawnDelayFrame; i++)
        {
            yield return null;
        }
        GameManager.instance.spawnDelayFrame++;
        SpawnMatObj(_matCreateTableId);
    }       //OderSpawn()




}       // ClassEnd
