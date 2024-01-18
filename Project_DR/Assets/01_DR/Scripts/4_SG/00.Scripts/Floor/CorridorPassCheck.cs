using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CorridorPassCheck : MonoBehaviour
{
    CorridorColorChange colorChange = default;    // 충돌시 컬러를 바꾸주는 함수 부를 변수


    /// <summary>
    /// 체크할 범위 설정하는 함수
    /// </summary>
    /// <param name="_colliderCenter">콜라이더의 위치</param>
    /// <param name="_size">콜라이더의 사이즈</param>
    public void SetCheckerPos(Vector3 _colliderCenter, Vector3 _size)
    {
        BoxCollider boxCollider = this.gameObject.GetComponent<BoxCollider>();
        boxCollider.center = _colliderCenter;
        boxCollider.size = _size;
        boxCollider.isTrigger = true;
    }       // SetCheckerPos()

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            colorChange = this.transform.parent.GetComponent<CorridorColorChange>();

            if (colorChange != null || colorChange != default)
            {
                colorChange.ChangeCorridorMaterial();
            }

        }
        else { /*PASS*/ }

    }

}
