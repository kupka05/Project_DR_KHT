using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Js.Quest;
using System;

public class TEST : MonoBehaviour
{
    // Start is called before the first frame update


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Unit.AddInventoryItem(5401);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Unit.AddInventoryItem(5201);
        }
    }


}
