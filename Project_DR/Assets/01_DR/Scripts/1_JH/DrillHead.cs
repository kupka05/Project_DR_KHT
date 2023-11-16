using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DrillHead : MonoBehaviour
{
    public Vector3 targetPos;
    private bool isStop=false;
    private float currentGrappleDistance;


    // Update is called once per frame
    void Update()
    {
        if (targetPos == null || isStop)
            return;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime*10);
        
        transform.LookAt(targetPos);
        currentGrappleDistance = Vector3.Distance(transform.position, targetPos);
        if(currentGrappleDistance < 0.3f)
            isStop = true;
    }

}
