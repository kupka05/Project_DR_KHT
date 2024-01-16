using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    /*************************************************
     *                 Unity Events
     *************************************************/
    void FixedUpdate()
    {
        if (Camera.main.transform != null)
        {
            transform.LookAt(Camera.main.transform);
            //Vector3 rotation = transform.eulerAngles;
            //rotation.x = -rotation.x;
            //transform.rotation = Quaternion.Euler(rotation);
        }
    }

    /*************************************************
     *               Private Methods
     *************************************************/
    private void ReserseLookAt(Transform target)
    {
        transform.LookAt(target);
        Quaternion rotation = transform.rotation;
        rotation.x = -1 * rotation.x;
        rotation.z = 0f;
        transform.rotation = rotation;
    }
}


