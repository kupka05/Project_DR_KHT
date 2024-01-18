using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDuplicateObject : MonoBehaviour
{
    /*************************************************
     *                Private Fields
     *************************************************/
    [SerializeField] private bool isRun = false;


    /*************************************************
     *                 Unity Events
     *************************************************/
    private void Start()
    {
        // 5초 후 isRun 변경
        Invoke("SetIsRun", 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 실행이 안된 경우
        if (! isRun)
        {
            Collider collider = collision.collider;
            // 플레이어와 바닥 태그가 아닐 경우
            if (!(collider.CompareTag("player") && collider.CompareTag("Floor")))
            {
                GFunc.Log($"삭제될 오브젝트: {collider.name}");
                // 오브젝트 삭제
                Destroy(collider.gameObject);
            }
        }
    }

    /*************************************************
     *               Public Methods
     *************************************************/
    public void SetIsRun()
    {
        isRun = true;
    }
}
