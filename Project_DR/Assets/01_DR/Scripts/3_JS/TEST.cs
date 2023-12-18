using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Unit.AddInventoryItem(5001);
        GFunc.Log($"ClearRewardKeyID: {Data.GetInt(1_100_000_1, "ClearRewardKeyID")}");
        int asd = Data.GetInt(2_000_000_1, "ClearRewardKeyID");
        GFunc.Log($"asd: {asd}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
