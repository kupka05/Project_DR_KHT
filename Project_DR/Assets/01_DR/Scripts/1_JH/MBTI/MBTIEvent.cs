using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBTIEvent : MonoBehaviour
{



    public void GrabCheck(Grabbable grabItem)
    {
        if(grabItem.GetComponent<MBTIChecker>() != null)
        {
            grabItem.GetComponent<MBTIChecker>().GrabEvent();
        }
    }



}
