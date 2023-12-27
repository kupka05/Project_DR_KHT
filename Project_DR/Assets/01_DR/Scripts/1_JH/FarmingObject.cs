using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingObject : MonoBehaviour
{

    public int itemID;              // 드롭할 아이템 ID
    public int dropValue;           // 몇개 드롭 개수

    private Damageable damage;

    // Start is called before the first frame update
    void Start()
    {
        damage = GetComponent<Damageable>();
    }

    // 게임 오브젝트 디스트로이 되면서 드롭

    public void DropItems()
    {
        Unit.AddFieldItem(this.transform.position, itemID, dropValue);
    }
}
