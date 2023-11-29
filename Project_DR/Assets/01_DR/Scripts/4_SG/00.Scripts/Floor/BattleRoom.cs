using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : RandomRoom
{       // BattleRoomClass는 Monster소환 Monster가 전부 죽었는지 체크할것
        
    private List<GameObject> sponMonsters = new List<GameObject>(); // 소환한 몬스터를 관리해줄 List



    void Start()
    {
        DungeonManager.clearList.Add(isClearRoom);
        GetFloorPos();      // 꼭지점 가져와주는 Class
    }       // Start()

    private void OnDestroy()
    {
        DungeonManager.clearList.Remove(isClearRoom);
    }       // OnDestroy()




    /// <summary>
    /// 몬스터를 스폰하는 함수
    /// </summary>
    private void SponMonster(int _monsterCount)
    {

    }       // SponMonster()

}       // ClassEnd
