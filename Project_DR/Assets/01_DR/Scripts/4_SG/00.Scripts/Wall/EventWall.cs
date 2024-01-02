using BNG;
using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventWall : MonoBehaviour
{
    public enum Dir { None, Left, Right, Up, Down}
    public Dir direction = Dir.None;
    [Header("GameObject")]
    public GameObject wall;
    public GameObject floor;
    public GameObject roof;
    public GameObject block;

    [Header("Secret")]
    public bool isSecrecItem;
    public int itemID;
    public int[] itemIDs;
    public int index = 3;

    [Header("Option")]
    public LayerMask layerMask;
    public float range;
    //public bool isLeft, isRight, isUp, isDown = false;
    private FloorMeshPos floorMesh = null;
    private GameObject leftWall, rightWall;

    private void Start()
    {     
        Invoke(nameof(SetEventWall), 1f) ;       
    }
    private void SetEventWall()
    {
        FloorCheck();

        // 방향을 정하지 못하면 막기
        if (direction == Dir.None)
        {
            wall.GetComponent<Damageable>().DestroyOnDeath = false;
            wall.SetActive(false);
            block.SetActive(true);
        }
    }

    // 레이를 활용 바닥 체크
    private void FloorCheck()
    {
        Vector3 pos = transform.position,
                leftPos = pos,
                rightPos = pos, 
                upPos = pos, 
                downPos = pos;

        leftPos.x += 0.25f;
        rightPos.x -= 0.25f;
        upPos.z += 0.25f;
        downPos.z -= 0.25f;

        RaycastHit hit;
        if (Physics.Raycast(leftPos, -transform.up, out hit, range, layerMask))
        {
            direction = Dir.Left;
            //GFunc.Log("왼쪽");
        }
        else if (Physics.Raycast(rightPos, -transform.up, out hit, range, layerMask))
        {
            direction = Dir.Right;
            //GFunc.Log("오른쪽");
        }
        else if (Physics.Raycast(rightPos, -transform.up, out hit, range, layerMask))
        {
            direction = Dir.Up;
            //GFunc.Log("위");
        }
        else if (Physics.Raycast(rightPos, -transform.up, out hit, range, layerMask))
        {
            direction = Dir.Down;
            //GFunc.Log("아래");
        }
        else
        {

            if (Physics.Raycast(pos, -transform.up, out hit, 1, layerMask))
            {
                //GFunc.Log("중앙에서는 발견");
            }
            else
            {
                //GFunc.Log("아무것도 체크 못함");
                direction = Dir.None;
            }
            return;
        }

        if (!hit.collider.gameObject.CompareTag("Floor"))
        {
            //GFunc.Log("바닥이 아님");
            return;
        }
        EventWallPosSet();
    }

    private void EventWallPosSet()
    {
        // 코너 체크 성공하면 이벤트 벽 스케일 셋
        if (!CornerCheck())
        {
            direction = Dir.None;
            return;
        }
        Vector3 setPos, clonePos, secretObjPos;
        setPos = this.transform.position;
        clonePos = this.transform.position;
        secretObjPos = this.transform.position;

        if (direction == Dir.Left)
        {
            //setPos.x = 0.5f;
            this.transform.position = setPos;
            clonePos.x -= (index + 1);
            secretObjPos.x -= index;
        }
        if (direction == Dir.Right)
        {
            //setPos.x -= 0.5f;
            this.transform.position = setPos;
            clonePos.x += (index + 1);
            secretObjPos.x += index;

        }
        if (direction == Dir.Up)
        {
            //setPos.z -= 0.5f;
            this.transform.position = setPos;
            clonePos.z += (index + 1);
            secretObjPos.z += index;

        }
        if (direction == Dir.Down)
        {
            //setPos.z += 0.5f;
            this.transform.position = setPos;
            clonePos.z -= (index + 1);
            secretObjPos.z -= index;
        }

        Instantiate(rightWall, clonePos, this.transform.rotation, this.transform);

        // 비밀 아이템 생성
        if(isSecrecItem)
        {
            //itemIDs = new int[Data.GetCount(itemID)];
            itemIDs = new int[6];


            for (int i = 0; i < 6; i++)
            {
                itemIDs[i] = Data.GetInt(itemID + i, "ID");
            }

            int index = Random.Range(0, itemIDs.Length);
            GFunc.Log(itemIDs[index] + "생성 예정");
            Unit.AddFieldItem(secretObjPos, itemIDs[index]);

            //GameObject Item = Instantiate(scerecObj, secretObjPos, this.transform.rotation, this.transform);
            //Item.transform.localScale = Vector3.one;
        }
        



        SetFloorAndRoof();
        leftWall.transform.parent = this.transform;
        rightWall.transform.parent = this.transform;
        CreateSecretRoom();


    }       // EventWallPosSet()

    // 자신의 위치에 따라서 Sclae을 바꿔주는 함수
    private void EventWallScaleSet()
    {
        Vector3 setSclae;
        if (direction == Dir.Left || direction == Dir.Right)
        {
            setSclae = this.transform.localScale;
            setSclae.z = 3f;
            this.transform.localScale = setSclae;
        }

        if (direction == Dir.Up || direction == Dir.Down)
        {
            setSclae = this.transform.localScale;
            setSclae.x = 3f;
            this.transform.localScale = setSclae;
        }     
    }       // EventWallScaleSet()

    // 양쪽 벽 체크하여 모서리 체크
    private bool CornerCheck()
    {
        RaycastHit hit;
        float maxDis = 1f;
        if (direction == Dir.Left || direction == Dir.Right)
        {       // 이벤트벽이 왼쪽 혹은 오른쪽에 있다면 -> 전방,후방 레이를 쏨
            if (Physics.Raycast(this.transform.position, Vector3.forward, out hit, maxDis))
            {
                leftWall = hit.collider.gameObject;
            }
            if (Physics.Raycast(this.transform.position, Vector3.back, out hit, maxDis))
            {
                rightWall = hit.collider.gameObject;
            }
        }
        else if (direction == Dir.Up || direction == Dir.Down)
        {       // 이벤트벽이 위쪽 혹은 아래쪽에 있다면 -> 좌,우 레이를쏨
            if (Physics.Raycast(this.transform.position, Vector3.left, out hit, maxDis))
            {
                leftWall = hit.collider.gameObject;
            }
            if (Physics.Raycast(this.transform.position, Vector3.right, out hit, maxDis))
            {
                rightWall = hit.collider.gameObject;
             }
        }
        if(leftWall == null || rightWall == null)
        {
            GFunc.Log("좌우 벽을 찾을 수 없습니다.");
            return false;
        }
        return true;

    }       // CornerCheck()



    private void CreateSecretRoom()
    {
        switch (direction)
        {
            case Dir.Left:

                CreateHorizontalWall(-1f);
             break;
            case Dir.Right:
                CreateHorizontalWall(1f);
            break;
            case Dir.Up:
                CreateVerticalWall(1f);
                break;
            case Dir.Down:
                CreateVerticalWall(-1f);
                break;
        }
    }

    private void CreateVerticalWall(float distance)
    {
        Vector3 leftWallPos = leftWall.transform.position,
                rightWallPos = rightWall.transform.position;

        for (int i = 0; i < index; i++)
        {
            leftWallPos.z += distance;
            rightWallPos.z += distance;
            Instantiate(leftWall, leftWallPos, leftWall.transform.rotation, this.transform);
            Instantiate(rightWall, rightWallPos, rightWall.transform.rotation, this.transform);           
        }

        Vector3 _wallPos = this.transform.position;
        _wallPos.z = rightWallPos.z;
        //Instantiate(leftWall, _wallPos, leftWall.transform.rotation, this.transform);
    }
    private void CreateHorizontalWall(float distance)
    {
        Vector3 leftWallPos = leftWall.transform.position,
                rightWallPos = rightWall.transform.position;

        for (int i = 0; i < index; i++)
        {
            leftWallPos.x += distance;
            rightWallPos.x += distance;
            Instantiate(leftWall, leftWallPos, leftWall.transform.rotation, this.transform);
            Instantiate(rightWall, rightWallPos, rightWall.transform.rotation, this.transform);
        }

        Vector3 _wallPos = this.transform.position;
        _wallPos.x = rightWallPos.x;
        //Instantiate(leftWall, _wallPos, leftWall.transform.rotation, this.transform);
    }
    private void SetFloorAndRoof()
    {
        Vector3 scale = floor.transform.localScale;
        Vector3 newFloorPos = floor.transform.position;
        Vector3 newRoofPos = roof.transform.position;
        scale.x = index+1;

        if (direction == Dir.Left || direction == Dir.Right)
        {
            if (direction == Dir.Left)
            {
                newFloorPos.x -= 2;
                newRoofPos.x -= 2;
            }
            else
            {
                newFloorPos.x += 2;
                newRoofPos.x += 2;
            }
        }
        else if (direction == Dir.Up || direction == Dir.Down)
        {
            if (direction == Dir.Up)
            {
                newFloorPos.z += 2;
                newRoofPos.z += 2;
            }
            else
            {
                newFloorPos.z -= 2;
                newRoofPos.z -= 2;
            }
        }

        floor.gameObject.SetActive(true);
        roof.gameObject.SetActive(true);
        floor.transform.localScale = scale;
        roof.transform.localScale = scale;
        floor.transform.position = newFloorPos;
        roof.transform.position = newRoofPos;

    }


    #region [+] REGACY

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Floor"))
    //    {
    //        if (floorMesh == null)
    //        {
    //            floorMesh = collision.gameObject.GetComponent<FloorMeshPos>();
    //            EventWallPosExamine();
    //        }
    //    }


    //}       // OnCollisioinEnter()


    //// 이벤트벽 위치 검수
    //private void EventWallPosExamine()
    //{
    //    // 이벤트 벽이 왼쪽인지 오른쪽인지 검수
    //    if (this.transform.position.x == floorMesh.bottomLeftCorner.x)
    //    {       // 왼쪽 하단 꼭지점과 포지션이 같을경우
    //        isLeft = true;
    //    }
    //    else if (this.transform.position.x == floorMesh.bottomRightCorner.x)
    //    {       // 오른쪽 하단 꼭지점과 포지션이 같을경우
    //        isRight = true;
    //    }
    //    if (this.transform.position.z == floorMesh.topLeftCorner.z)
    //    {       // 상단의 높이와 같을경우
    //        isUp = true;
    //    }
    //    else if (this.transform.position.z == floorMesh.bottomLeftCorner.z)
    //    {       // 하단의 높이와 같을경우
    //        isDown = true;
    //    }
    //    EventWallPosSet();

    //}       // EventWallPosExamine()

    //private void EventWallPosSet()
    //{
    //    Vector3 setPos;
    //    setPos = this.transform.position;
    //    if (isLeft == true)
    //    {
    //        setPos.x += 0.5f;
    //        this.transform.position = setPos;
    //    }
    //    if (isRight == true)
    //    {
    //        setPos.x -= 0.5f;
    //        this.transform.position = setPos;
    //    }
    //    if (isUp == true)
    //    {
    //        setPos.z -= 0.5f;
    //        this.transform.position = setPos;
    //    }
    //    if (isDown == true)
    //    {
    //        setPos.z += 0.5f;
    //        this.transform.position = setPos;
    //    }
    //    EventWallScaleSet();
    //}       // EventWallPosSet()


    //// 자신의 위치에 따라서 Sclae을 바꿔주는 함수
    //private void EventWallScaleSet()
    //{
    //    Vector3 setSclae;
    //    if (isLeft == true)
    //    {
    //        setSclae = this.transform.localScale;
    //        setSclae.z = 3f;
    //        this.transform.localScale = setSclae;
    //    }
    //    if (isRight == true)
    //    {
    //        setSclae = this.transform.localScale;
    //        setSclae.z = 3f;
    //        this.transform.localScale = setSclae;
    //    }
    //    if (isUp == true)
    //    {
    //        setSclae = this.transform.localScale;
    //        setSclae.x = 3f;
    //        this.transform.localScale = setSclae;
    //    }
    //    if (isDown == true)
    //    {
    //        setSclae = this.transform.localScale;
    //        setSclae.x = 3f;
    //        this.transform.localScale = setSclae;
    //    }
    //    MakeChildrenObjects();

    //}       // EventWallScaleSet()

    //// 양쪽 벽 obj를 자신의 자식오브젝트로 만드는 함수
    //private void MakeChildrenObjects()
    //{
    //    RaycastHit hit;
    //    float maxDis = 1f;
    //    if (isLeft == true || isRight == true)
    //    {       // 이벤트벽이 왼쪽 혹은 오른쪽에 있다면 -> 전방,후방 레이를 쏨
    //        if (Physics.Raycast(this.transform.position, Vector3.forward, out hit, maxDis))
    //        {
    //            hit.collider.transform.parent = this.transform;
    //        }
    //        if (Physics.Raycast(this.transform.position, Vector3.back, out hit, maxDis))
    //        {
    //            hit.collider.transform.parent = this.transform;
    //        }
    //    }
    //    else if (isUp == true || isDown == true)
    //    {       // 이벤트벽이 위쪽 혹은 아래쪽에 있다면 -> 좌,우 레이를쏨
    //        if (Physics.Raycast(this.transform.position, Vector3.left, out hit, maxDis))
    //        {
    //            hit.collider.transform.parent = this.transform;
    //        }
    //        if (Physics.Raycast(this.transform.position, Vector3.right, out hit, maxDis))
    //        {
    //            hit.collider.transform.parent = this.transform;
    //        }
    //    }
    #endregion
}       // ClassEnd
