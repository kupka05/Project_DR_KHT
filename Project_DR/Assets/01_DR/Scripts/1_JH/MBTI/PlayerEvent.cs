using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent : MonoBehaviour
{



    public void GrabCheck(Grabbable grabItem)
    {
        if(grabItem.GetComponent<MBTIChecker>() != null)
        {
            grabItem.GetComponent<MBTIChecker>().GrabEvent();
        }

        if (grabItem.GetComponent<ItemColliderHandler>() != null)
        {
            grabItem.GetComponent<ItemColliderHandler>().state = ItemColliderHandler.State.Stop;
        }
    }

    public void ReleaseItem(Grabbable grabItem)
    {
        if (grabItem.GetComponent<ItemColliderHandler>() != null)
        {
            grabItem.GetComponent<ItemColliderHandler>().state = ItemColliderHandler.State.Default;
        }
    }



}
