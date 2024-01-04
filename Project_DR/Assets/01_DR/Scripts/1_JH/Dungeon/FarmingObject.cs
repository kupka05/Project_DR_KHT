using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingObject : MonoBehaviour
{

    public int[] dropItem;          // 드롭할 아이템 ID
    public int maxDrop;             // 최대 드롭 개수

    //public void Start()
    //{
    //    GFunc.Log(gameObject.name);
    //}

    public void DrpoItems()
    {
        // 하나 확정 드롭
        DropItem();

        if (dropItem.Length <= 1)
        { return; }

        for(int i = 0; i < maxDrop - 2; i++)
        {
            if (0 == Random.Range(0, 2))
            {
                DropItem();
            }
        }
    }

    private void DropItem()
    {
        int randIndex = Random.Range(0, 100) % dropItem.Length;
        GFunc.Log(randIndex);
        Unit.AddFieldItem(this.transform.position, dropItem[randIndex]);
    }
}
