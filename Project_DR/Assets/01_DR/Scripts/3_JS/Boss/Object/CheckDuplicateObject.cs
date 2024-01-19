using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDuplicateObject : MonoBehaviour
{
    [SerializeField] private bool _isRun = false;

    private void Start()
    {
        //Invoke("SetIsRun", 3.0f);
    }

    private void OnTriggerStay(Collider other)
    {
        GFunc.Log($"닿은 오브젝트 {other.name}");
        if (IsObject(other) && _isRun)
        {
            //_isRun = true;
            Destroy(other);
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

    private void SetIsRun()
    {
        _isRun = true;
    }
}
