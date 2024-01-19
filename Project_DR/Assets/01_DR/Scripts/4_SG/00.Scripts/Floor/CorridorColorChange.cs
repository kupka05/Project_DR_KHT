using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorColorChange : MonoBehaviour
{       // 플레이어가 복도의 바닥을 밟으면 1회 색이 바뀌는 스크립트

    private Material passCorridorMaterial;
    private MeshRenderer meshRenderer;

    private bool isPass;        // 통과 했는지

    private GameObject gameObj; // 새로운 게임오브젝트

    private void Awake()
    {
        isPass = false;
        passCorridorMaterial = Resources.Load<Material>("PassCorridorColor");
        meshRenderer = this.GetComponent<MeshRenderer>();

    }

    private void Start()
    {
        gameObj = new GameObject("DoorColorController", typeof(BoxCollider),typeof(CorridorPassCheck));
        gameObj.transform.parent = this.transform;
        gameObj.layer = 2;
        FloorMeshPos floorMeshPos = this.GetComponent<FloorMeshPos>();

        if(!floorMeshPos)
        {
            return;
        }
        Vector3 size = new Vector3(2.5f, 3f, 2.5f);
        Vector3 centerPos = new Vector3((floorMeshPos.topLeftCorner.x + floorMeshPos.topRightCorner.x) * 0.5f,
            size.y * 0.5f,
            (floorMeshPos.bottomLeftCorner.z + floorMeshPos.topLeftCorner.z) * 0.5f);
        gameObj.GetComponent<CorridorPassCheck>().SetCheckerPos(centerPos, size);
    }

    // CorridorPassCheck 스크립트가 판별
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.CompareTag("Player") && isPass == false)
    //    {
    //        ChangeCorridorMaterial();
    //    }
    //    else { /*PASS*/ }
    //}

    public void ChangeCorridorMaterial()
    {
        isPass = true;
        meshRenderer.material = passCorridorMaterial;
        Destroy(this);

    }       // ChangeCorridorMaterial()
}       // ClassEnd
