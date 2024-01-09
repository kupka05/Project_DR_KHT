using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Js.Quest;
using System;
using Rito.InventorySystem;

public class TEST : MonoBehaviour
{
    // Start is called before the first frame update


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            //Unit.AddFieldItem(Vector3.zero, 5101);
            //Unit.PrintRewardText(32_1_001, 32_1_002, 32_1_003, 32_1_004);
            //Unit.ClearQuestByID(3133001);
            Unit.AddInventoryItem(5201);
            Unit.AddInventoryItem(5202);

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            List<(int, int)> itemList = Unit.GetInventoryMaterialItems();
            for (int i = 0; i < itemList.Count; i++)
            {
                int itemID = itemList[i].Item1;
                int itemAmount = itemList[i].Item2;
                GFunc.Log($"ID: {itemID} / Amount: {itemAmount}");
                UserData.AddItemScore(itemID);
            }
        }
    }


}
