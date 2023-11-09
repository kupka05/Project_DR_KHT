using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    // 확인용 변수
    private int InItNum = 1;

    [Header("DungeonSetting")]
    // 던전 크기 설정
    public int dungeonWidth;
    public int dungeonHeight;     // 던전의 넓이와 높이
    // 방 크기 및 기준 설정
    public int roomWidthMin, roomLengthMin;     // 방의 최소 넓이와 길이
    public int maxIterations;       // 던전 생성 최대 반복 횟수
    public int corridorWidth;       // 복도 넓이

    // 그래픽 설정
    public Material material;  // 던전 바닥의 재질

    // 방 모양 수정자 설정
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;  // 방의 하단 모서리 수정자
    [Range(0.7f, 1.0f)]
    public float roomTopCornerModifier;     // 방의 상단 모서리 수정자
    [Range(0, 2)]
    public int roomOffset;                // 방 오프셋

    [Header("WallObj")]
    // 벽 오브젝트 설정
    public GameObject wallVertical, wallHorizontal, wallBreakdown;

    // 부숴지는벽이 나올 확률 // 임시 : 추후 스프레드시트로 바뀔수 있음 11.09
    private float wallBreakDownPercentage = 5;

    [Header("FloorObj")]
    // 바닥에 깔아둘 ObjPrefabs
    public GameObject[] floorPrefabs;
    // 가능한 문 및 벽 위치 목록
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;

    void Start()
    {
        // 던전 생성 시작
        CreateDungeon();
    }

    // 던전 생성 함수
    public void CreateDungeon()
    {
        // 기존 자식 객체 삭제
        DestroyAllChildren();

        // 던전 생성을 위한 제너레이터 초기화
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonHeight);

        // 방 목록 계산
        var listOfRooms = generator.CalculateRooms(maxIterations, roomWidthMin, roomLengthMin,
            roomBottomCornerModifier, roomTopCornerModifier, roomOffset, corridorWidth);

        // 벽 부모 오브젝트 생성
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();

        // 바닥의 부모 오브젝트 생성
        GameObject floorParent = new GameObject("FloorMeshParent");
        floorParent.transform.parent = transform;
        // 복도의 부모 오브젝트 생성
        GameObject corridorParnet = new GameObject("CorridorMeshParent");
        corridorParnet.transform.parent = transform;


        // 각 방에 대한 메시 생성
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner,
                listOfRooms[i].TopRightAreaCorner, listOfRooms[i].isFloor, floorParent, corridorParnet);
        }

        // 각 방에 대한 땅바닥Obj 생성
        //for (int i = 0; i < listOfRooms.Count; i++)
        //{
        //    CreateMeshInFloor(listOfRooms[i].BottomLeftAreaCorner,
        //        listOfRooms[i].TopRightAreaCorner, listOfRooms[i].isFloor, floorParent, corridorParnet);
        //}

        // 벽 생성
        CreateWalls(wallParent);
    }

    private void SG_CreateFloor(Vector2 bottomLeftCorner, Vector2 topRightCorner, GameObject floorParent, GameObject[] floorPrefab)
    {
        // parent로 묶을때,인스턴스 할때 사용할 GameObject
        GameObject floorObjClone;
        GameObject floorCloneTras = new GameObject();

        // 바닥을 랜덤하게 뽑아줄 변수
        int randomFloorNum;

        // 해당 방에 대한 꼭지점 들생성
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0f, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0f, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0f, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0f, topRightCorner.y);
        // 인스턴스될 Position
        Vector3Int instancePos;
        for (float vertical = bottomLeftV.z; vertical < topLeftV.z; vertical++)
        {       // 아래 꼭지점이 윗꼭지점보다 작을때 ++
            for (float horizontal = bottomLeftV.x; horizontal < bottomRightV.z; horizontal++)
            {
                //instancePos = new Vector3Int((int)horizontal, 0, (int)vertical);                
                Debug.LogFormat("Length -> {0}", floorPrefab.Length);

                randomFloorNum = UnityEngine.Random.Range(0, floorPrefab.Length);
                floorObjClone = Instantiate(floorPrefab[randomFloorNum]);
                floorObjClone.transform.position = new Vector3Int((int)horizontal, 0, (int)vertical);
            }

        }
    }

    // 수평 벽 및 수직 벽 생성 함수
    private void CreateWalls(GameObject wallParent)
    {
        // 수평 벽 생성
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal);
        }

        // 수직 벽 생성
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }
    }       // CreateWalls()

    // 벽 오브젝트 생성 함수
    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        #region 벽생성만
        //Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
        #endregion 벽생성만
        #region 1층구조 벽 생성(벽 생성후 YPos조절까지)
        // 벽 오브젝트 생성후 Y축 조절을 위한 GameObject
        //GameObject wallObjClone;
        //wallObjClone = Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
        //Vector3 wallPos = wallObjClone.transform.position;
        //wallPos.y = wallObjClone.transform.localScale.y / 2;
        //wallObjClone.transform.position = wallPos;
        #endregion 1층구조 벽 생성(벽 생성후 YPos조절까지)
        #region 2층 구조 벽 생성 (한 칸에 벽 2개 생성 2번째 생성되는벽은 Y축 추가 조절)
        float randomCreatWall = UnityEngine.Random.Range(0, 1001);     // 0 ~ 1000 임시        
        // 벽 오브젝트 생성후 Y축 조절을 위한 GameObject
        GameObject wallObjClone;
        if (randomCreatWall < wallBreakDownPercentage)
        {
            wallObjClone = Instantiate(wallBreakdown, wallPosition, Quaternion.identity, wallParent.transform);
        }
        else
        {
            wallObjClone = Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
        }

        Vector3 wallPos = wallObjClone.transform.position;
        wallPos.y = wallObjClone.transform.localScale.y / 2;
        wallObjClone.transform.position = wallPos;

        // 2번째 벽 생성 
        wallPos.y = wallPos.y * 3;
        wallObjClone = Instantiate(wallPrefab, wallPos, Quaternion.identity, wallParent.transform);

        #endregion 2층 구조 벽 생성 (한 칸에 벽 2개 생성 2번째 생성되는벽은 Y축 추가 조절)



    }       // CreateWall()

    // 방 크기에 따른 메시 생성 함수
    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner, bool isFloor,
        GameObject floorParent, GameObject corridorParnet)
    {
        // 바닥 메시 생성을 위한 꼭지점 좌표 설정
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0f, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0f, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0f, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0f, topRightCorner.y);

        // 바닥 메시를 위한 꼭지점 배열 생성
        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };



        #region temp //
        // UV 매핑을 위한 배열 생성
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        // 삼각형을 정의하는 배열 생성
        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };

        // 메시 생성 및 설정
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;


        GameObject dungeonFloor = new GameObject("Mesh" + InItNum + bottomLeftCorner,
            typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));

        InItNum++;

        #region 메시의 콜라이더 Center,Size 수정 23.11.07_LEGACY

        // 메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        // Center
        //Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) / 2, 0f, (topLeftV.z + bottomLeftV.z) / 2);
        //BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        //floorCol.center = colCenter;
        //// Size
        //float colSizeX, colSizeY, colSizeZ;
        //colSizeX = bottomLeftV.x - bottomRightV.x;
        //colSizeY = 0.03f;
        //colSizeZ = bottomLeftV.z - topLeftV.z;
        //// 음수값이 나오면 양수로 치환
        //if(colSizeX < 0) {  colSizeX = -colSizeX; }
        //if (colSizeZ < 0) { colSizeZ = -colSizeZ; }
        //Vector3 colSize = new Vector3(colSizeX,colSizeY,colSizeZ);
        //floorCol.size = colSize;

        #endregion 메시의 콜라이더 Center,Size 수정 23.11.07_LEGACY

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        if (isFloor == true)
        {
            dungeonFloor.transform.parent = floorParent.transform;
        }
        else
        {
            dungeonFloor.transform.parent = corridorParnet.transform;
        }

        //dungeonFloor.transform.parent = meshParent.transform;       // TODO : Node에 있는 bool값에 따라서 따라갈 parent를 정해줘야함
        //dungeonFloor.transform.parent = transform; : LEGACY

        #endregion temp //

        // 벽 및 문 위치 추가
        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
    }
    private void CreateFloor(Vector3 bottomLeft, Vector3 topRight, GameObject floorParent)
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(bottomLeft.x, 0, topRight.z),
            new Vector3(topRight.x, 0, topRight.z),
            new Vector3(bottomLeft.x, 0, bottomLeft.z),
            new Vector3(topRight.x, 0, bottomLeft.z)
        };
        GameObject dungeonFloor = Instantiate(floorPrefabs[0]);
        dungeonFloor.transform.position = new Vector3((bottomLeft.x + topRight.x) / 2, 0, (bottomLeft.z + topRight.z) / 2);
        dungeonFloor.transform.localScale = new Vector3(topRight.x - bottomLeft.x, 1, topRight.z - bottomLeft.z);
        dungeonFloor.transform.parent = floorParent.transform;
    }


    // 방 크기에 따른 메시 와 바닥 생성
    private void CreateMeshInFloor(Vector2 bottomLeftCorner, Vector2 topRightCorner, bool isFloor,
        GameObject floorParent, GameObject corridorParnet)
    {
        // 바닥 메시 생성을 위한 꼭지점 좌표 설정
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0f, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0f, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0f, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0f, topRightCorner.y);

        // 바닥 메시를 위한 꼭지점 배열 생성
        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        // 프리팹으로 바닥 생성
        int numTilesX = Mathf.FloorToInt((topRightV.x - bottomLeftV.x) / 1);
        int numTilesZ = Mathf.FloorToInt((topRightV.z - bottomLeftV.z) / 1);

        for (int x = 0; x < numTilesX; x++)
        {
            for (int z = 0; z < numTilesZ; z++)
            {
                Vector3 tileBottomLeft = new Vector3(bottomLeftV.x + x * 1, 0, bottomLeftV.z + z * 1);
                Vector3 tileTopRight = new Vector3(tileBottomLeft.x + 1, 0, tileBottomLeft.z + 1);
                CreateFloor(tileBottomLeft, tileTopRight, floorParent);

            }
        }


        #region temp //
        //// UV 매핑을 위한 배열 생성
        //Vector2[] uvs = new Vector2[vertices.Length];
        //for (int i = 0; i < uvs.Length; i++)
        //{
        //    uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        //}

        //// 삼각형을 정의하는 배열 생성
        //int[] triangles = new int[]
        //{
        //    0,
        //    1,
        //    2,
        //    2,
        //    1,
        //    3
        //};

        //// 메시 생성 및 설정
        //Mesh mesh = new Mesh();
        //mesh.vertices = vertices;
        //mesh.uv = uvs;
        //mesh.triangles = triangles;


        //GameObject dungeonFloor = new GameObject("Mesh" + InItNum + bottomLeftCorner,
        //    typeof(MeshFilter), typeof(MeshRenderer),typeof(BoxCollider));

        //InItNum++;

        #region 메시의 콜라이더 Center,Size 수정 23.11.07_LEGACY

        // 메시의 중간지점을 구하고 콜라이더를 중앙 지점에 놔주기
        // Center
        //Vector3 colCenter = new Vector3((bottomLeftV.x + bottomRightV.x) / 2, 0f, (topLeftV.z + bottomLeftV.z) / 2);
        //BoxCollider floorCol = dungeonFloor.GetComponent<BoxCollider>();
        //floorCol.center = colCenter;
        //// Size
        //float colSizeX, colSizeY, colSizeZ;
        //colSizeX = bottomLeftV.x - bottomRightV.x;
        //colSizeY = 0.03f;
        //colSizeZ = bottomLeftV.z - topLeftV.z;
        //// 음수값이 나오면 양수로 치환
        //if(colSizeX < 0) {  colSizeX = -colSizeX; }
        //if (colSizeZ < 0) { colSizeZ = -colSizeZ; }
        //Vector3 colSize = new Vector3(colSizeX,colSizeY,colSizeZ);
        //floorCol.size = colSize;

        #endregion 메시의 콜라이더 Center,Size 수정 23.11.07_LEGACY

        //dungeonFloor.transform.position = Vector3.zero;
        //dungeonFloor.transform.localScale = Vector3.one;
        //dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        //dungeonFloor.GetComponent<MeshRenderer>().material = material;
        //if (isFloor == true)
        //{
        //    dungeonFloor.transform.parent = floorParent.transform;
        //}
        //else
        //{
        //    dungeonFloor.transform.parent = corridorParnet.transform;
        //}

        //dungeonFloor.transform.parent = meshParent.transform;       // TODO : Node에 있는 bool값에 따라서 따라갈 parent를 정해줘야함
        //dungeonFloor.transform.parent = transform; : LEGACY

        #endregion temp //

        // 벽 및 문 위치 추가
        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
    }
   

    // 벽 위치 목록에 벽 또는 문 위치 추가
    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }       // AddWallPositionToList()

    // 모든 자식 오브젝트 삭제 함수
    private void DestroyAllChildren()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }       // DestroyAllChildren()
}   // ClassEnd


#region LEGACY
/*
 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonHeight;     // 던전의 넓이와 높이
    public int roomWidthMin, roomLengthMin;     // 방의 최소 넓이와 길이
    public int maxIterations;       // 최대 기준
    public int corridorWidth;       // 코리 넓이
    public Material material;
    [Range(0.0f,0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float roomTopCornermidifier;
    [Range(0,2)]
    public int roomOffset;

    public GameObject wallVertical, wallHorizontal;
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;



    void Start()
    {
        CreateDungeon();
    }

    public void CreateDungeon()
    {
        DestroyAllChildren();

        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonHeight);
        var listOfRooms = generator.CalculateRooms(maxIterations, roomWidthMin, roomLengthMin,
            roomBottomCornerModifier,roomTopCornermidifier,roomOffset, corridorWidth);

        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>(); 
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();


        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
        CreateWalls(wallParent);
    }

    private void CreateWalls(GameObject wallParent)
    {
        foreach(var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition,wallHorizontal);
        }
        foreach(var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0f, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0f, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0f, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0f, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh" +bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;

        for(int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for(int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if(wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }       // AddWallPositionToList()


    private void DestroyAllChildren()
    {
        while(transform.childCount != 0)
        {
            foreach(Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }       // DestroyAllChildren()

}   //  ClassEnd
*/
#endregion LEGACY
