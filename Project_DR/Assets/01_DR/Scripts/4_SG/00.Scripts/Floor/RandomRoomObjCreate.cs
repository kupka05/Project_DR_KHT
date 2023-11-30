using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// 스폰될 오브젝트의 포지션이 고정값인지 아닌지 구별할 Enum
public enum IsFixPosition
{
    False = 0,
    True = 1
}

public class RandomRoomObjCreate : MonoBehaviour
{       // 방 오브젝트를 깔아주는 Class
        // 상속받은 Class에서 스폰시킬 Resource폴더속 Obj의 이름을 stringBuilder로 넘겨주면 소환해줌


    [SerializeField]
    protected List<Vector3> objSpawnPositionList;       // 소환할 오브젝트의 곂침현상을 체크해줄 V3 List
    [SerializeField]
    protected List<GameObject> objList;     // 인스턴스 할 Obj를 관리해줄 List (추후 필요없다면 삭제 예정)
    [SerializeField]
    protected StringBuilder stringBuilder;  // 인스턴스 할떄에 아이템 이름을 받아소환할 StringBuilser
    [SerializeField]
    protected FloorMeshPos floorPos;        // 현재 방의 꼭지점을 알수 있게하는 Class
    [SerializeField]
    protected int isFixPosition;           // 0 = false , 1 : true  스프레드 시트에서 위에 Enum과 비교할 int변수
    // 위 isFixPosition은 자식 Class에서 사용될것임

    private GameObject spawnObjParent;


    private void Start()
    {
        StartInIt();
        
    }

    private void StartInIt()
    {
        spawnObjParent = new GameObject("SpawnObjectParent");

        spawnObjParent.transform.parent = this.transform;

        floorPos = this.GetComponent<FloorMeshPos>();
    }       // StartInIt()





    /// <summary>
    /// 방에 아이템을 스폰해주는 함수 
    /// </summary>    
    /// <param name="_spawnItemName">소환할 아이템의 이름(Resource폴더속에 있는 Obj이름)</param>
    /// <param name="_isFixPosition"> 고정된 위치에 생성되어야하는지 int값 </param>
    protected void SpawnRoomObj(StringBuilder _spawnItemName,int _isFixPosition)
    {
        PositionSetting(_spawnItemName, _isFixPosition);

    }       // SpawnRoomObj()

    /// <summary>
    /// 랜덤한 포지션을 셋팅해주는 메서드(검수통과하지 못한다면 재귀해서 새로운 랜덤 포지션 생성)
    /// </summary>
    /// <param name="_spawnItemName">스폰할 아이템의 이름(Resource폴더속에 있는 Obj이름)</param>
    /// /// <param name="_isFixPosition"> 고정된 위치에 생성되어야하는지 int값 </param>
    private void PositionSetting(StringBuilder _spawnItemName,int _isFixPosition)
    {
        float inspectionDis;        // 기존의 obj거리와 랜덤 생성된 obj의 Distance를 가질 변수
        Vector3 randPosition;       // 랜덤하게 생성될 V3
        if(_isFixPosition == (int)IsFixPosition.False)
        {
            randPosition.x = UnityEngine.Random.Range
            (floorPos.bottomLeftCorner.x + 1, floorPos.bottomRightCorner.x - 2);
            randPosition.y = 1f;
            randPosition.z = UnityEngine.Random.Range
                (floorPos.bottomLeftCorner.z + 1, floorPos.topLeftCorner.z - 2);

            // 곂치는 포지션이 있는지 검수할 foreach
            foreach (Vector3 existingPos in objSpawnPositionList)
            {
                inspectionDis = Vector3.Distance(existingPos, randPosition);

                if (inspectionDis > 2)
                {   // 곂칠 우려가 있으면 재귀해서 새로운 랜덤 포지션 구하기
                    PositionSetting(_spawnItemName, _isFixPosition);
                }
            }
        }       // if(_isFixPosition == false)
        else { randPosition = Vector3.one; }  // TODO : randPosition이 FixPosition일떄에 어떤 Position인지 V3값으로 넣어주어야함

        // 리스트에 추가
        objSpawnPositionList.Add(randPosition);

        InstanceRoomObj(_spawnItemName, randPosition);      // 인스턴스 함수 호출

    }       // PositionSetting()

    /// <summary>
    /// 인스턴스 해서 오브젝트를 생성하는 함수
    /// </summary>
    /// <param name="_spawnItemName">스폰할 아이템 이름</param>
    /// <param name="_spawnPosition">아이템을 스폰할 포지션</param>
    private void InstanceRoomObj(StringBuilder _spawnItemName, Vector3 _spawnPosition)
    {
        GameObject getSpawnObj;     // 스폰할 아이템 지정해줄 Obj
        GameObject objClone;        // 생성될 obj의 클론
        stringBuilder.Clear();      // Append전 Clear
        stringBuilder.Append(_spawnItemName);       // 생성할 아이템 이름 추가

        getSpawnObj = Resources.Load<GameObject>($"{stringBuilder}");

        objClone = Instantiate(getSpawnObj, _spawnPosition, Quaternion.identity, spawnObjParent.transform);

        objList.Add(objClone);
    }       // InstanceRoomObj()







}       // ClassEnd
