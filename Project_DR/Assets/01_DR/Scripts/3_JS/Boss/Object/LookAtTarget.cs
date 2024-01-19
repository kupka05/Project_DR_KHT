using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    /*************************************************
     *                 Unity Events
     *************************************************/
    [SerializeField] private Transform _target;


    /*************************************************
     *                 Unity Events
     *************************************************/
    private void Start()
    {
        _target = FindTarget("Player");
    }

    void FixedUpdate()
    {
        if (Camera.main.transform != null)
        {
            transform.LookAt(Camera.main.transform);
            //Vector3 rotation = transform.eulerAngles;
            //transform.rotation = 
            //    Quaternion.Euler(-(rotation.x * 0.5f), rotation.y, 0f);
            //ReserseLookAt(_target);
        }
    }

    /*************************************************
     *               Private Methods
     *************************************************/
    private void ReserseLookAt(Transform target)
    {
        if (target != null)
        {
            transform.LookAt(target);
            Quaternion rotation = transform.rotation;
            rotation.x = -1 * rotation.x;
            rotation.z = 0f;
            transform.eulerAngles = rotation.eulerAngles;
        }
    }

    // 공격 대상 검색
    private Transform FindTarget(string targetName)
    {
        GameObject target = GameObject.FindWithTag(targetName);
        if (target != null)
        {
            // 타겟을 찾았을 경우
            if (target.GetComponent<PlayerPosition>() != null)
            {
                Transform targetTransform = target.GetComponent<PlayerPosition>().playerPos;
                return targetTransform;
            }
        }

        // 타겟을 못 찾았을 경우
        return default;
    }
}


