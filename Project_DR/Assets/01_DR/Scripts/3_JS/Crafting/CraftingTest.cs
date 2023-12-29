using System.Collections.Generic;
using UnityEngine;
using Js.Crafting;

public class CraftingTest : MonoBehaviour
{
    public bool isDone = false;
    void Start()
    {
        //// 단일 아이템
        //ResultItem sword = gameObject.AddComponent<ResultItem>();
        //sword.itemName = "Iron Sword";

        //// 복합 아이템
        //CompositeItem compositeItem = gameObject.AddComponent<CompositeItem>();
        //SingleItem item_1 = gameObject.AddComponent<SingleItem>();
        //item_1.itemName = "조합1";
        //SingleItem item_2 = gameObject.AddComponent<SingleItem>();
        //item_2.itemName = "조합2";
        //compositeItem.AddComponent(item_1);
        //compositeItem.AddComponent(item_2);
        //compositeItem.AddComponent(sword);

        //// 조합 실행
        //GFunc.Log($"제작 성공 여부: {compositeItem.Craft()}");

        //Invoke("CreateAnvil", 1f);
    }

    private void Update()
    {
        if (! isDone)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                isDone = true;
                CreateAnvil();
            }
        }
    }

    public void CreateAnvil()
    {
        Vector3 pos = Vector3.zero;
        pos.x = 22.883f;
        pos.y = 0.987f;
        pos.z = -40.57f;
        CraftingManager.Instance.CreateAnvil(pos);
    }
}
