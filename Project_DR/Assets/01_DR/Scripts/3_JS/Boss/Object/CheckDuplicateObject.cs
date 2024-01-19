using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDuplicateObject : MonoBehaviour
{
    private void Start()
    {
        // 3초 후 검출 및 삭제
        Invoke("DestroyOverlapObject", 3.0f);
    }

    // 현재 오브젝트 주위로 오버랩 후 검출된 오브젝트 삭제
    private void DestroyOverlapObject()
    {
        // 오버랩 상자 생성
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);

        // 오버랩된 모든 오브젝트 제거
        foreach (Collider collider in colliders)
        {
            // 태그 검사 후 삭제
            if (IsObject(collider))
            {
                GFunc.Log($"삭제할 오브젝트: {collider.gameObject}");
                Destroy(collider.gameObject);
            }
        }
    }

    private bool IsObject(Collider collider)
    {
        string[] tags =
        {
            "Player", "Boss", "Floor"
        };
        for (int i = 0; i < tags.Length; i++)
        {
            if (collider.CompareTag(tags[i]))
            {
                return false;
            }

        }

        return true;
    }
}
