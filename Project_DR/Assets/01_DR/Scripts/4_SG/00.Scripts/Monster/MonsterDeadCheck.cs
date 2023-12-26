using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MonsterDeadCheck : MonoBehaviour
{       // BattleRoom과 스파게티가 되어있는 클래스
    private BattleRoom battleRoom;              // 몬스터가 있는 BattleRoom    

    private void OnDestroy()
    {
        ReMoveList();
        battleRoom.CheckClearRoom();
    }


    public void AddList()
    {
        battleRoom.monsterList.Add(this.gameObject);
    }       // AddList()

    /// <summary>
    /// List에서 자신을 제거하는 함수(OnDestroy될떄에 호출할함수)
    /// </summary>
    private void ReMoveList()
    {
        battleRoom.monsterList.Remove(this.gameObject);
    }   //  ReMoveList()


    public void BattleRoomInIt
        (BattleRoom _battleRoom)
    {
        battleRoom = _battleRoom;
    }       // BattleRoomInIt()


}       // ClassEnd
