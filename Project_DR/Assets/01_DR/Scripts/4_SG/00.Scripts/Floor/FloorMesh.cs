using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMesh
{
    public Vector3 buttonLeftCornerV;
    public Vector3 buttomRightCornerV;
    public Vector3 topLeftCornerV;
    public Vector3 topRightCornerV;

    public FloorMesh(Vector3 buttonLeftCornerV,Vector3 buttomRightCornerV,
        Vector3 topLeftCornerV,Vector3 topRightCornerV)
    {
        this.buttonLeftCornerV = buttonLeftCornerV;
        this.buttomRightCornerV = buttomRightCornerV;
        this.topLeftCornerV = topLeftCornerV;
        this.topRightCornerV = topRightCornerV;
    }

}       // ClassEnd
