using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorColorChange : MonoBehaviour
{       // 플레이어가 복도의 바닥을 밟으면 1회 색이 바뀌는 스크립트

    private Material passCorridorMaterial;
    private MeshRenderer meshRenderer;

    private bool isPass;        // 통과 했는지

    private void Awake()
    {
        isPass = false;
        passCorridorMaterial = Resources.Load<Material>("PassCorridorColor");
        meshRenderer = this.GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && isPass == false)
        {
            ChangeCorridorMaterial();
        }
        else { /*PASS*/ }
    }

    private void ChangeCorridorMaterial()
    {
        isPass = true;
        meshRenderer.material = passCorridorMaterial;
        Destroy(this);

    }       // ChangeCorridorMaterial()
}       // ClassEnd
