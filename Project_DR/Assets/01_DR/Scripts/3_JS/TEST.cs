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
            //Unit.AddFieldItem(Vector3.zero, 5101);
            //Unit.PrintRewardText(32_1_001, 32_1_002, 32_1_003, 32_1_004);
            Unit.ClearQuestByID(3133001);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //Unit.AddFieldItem(Vector3.zero, 5201);
        }
    }


}
