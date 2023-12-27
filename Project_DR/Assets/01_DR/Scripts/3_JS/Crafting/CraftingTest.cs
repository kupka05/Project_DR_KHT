using System.Collections.Generic;
using UnityEngine;
using Js.Crafting;

public class CraftingTest : MonoBehaviour
{
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

        Invoke("Craft", 1f);
    }

    public void Craft()
    {
        GFunc.Log(CraftingManager.Instance.CraftingDictionary[3_0000_1].CheckCraft());
        GFunc.Log(CraftingManager.Instance.CraftingDictionary[3_0000_2].CheckCraft());
        GFunc.Log(CraftingManager.Instance.CraftingDictionary[3_0000_3].CheckCraft());
    }
}
