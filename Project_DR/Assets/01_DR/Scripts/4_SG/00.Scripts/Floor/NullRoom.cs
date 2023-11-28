using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullRoom : RandomRoom
{       // NullRoomClass는 빈방에 들어갈 Class입니다.

    void Start()
    {
        DungeonManager.clearList.Add(isClearRoom);
    }       // Start()




    private void OnDestroy()
    {
        DungeonManager.clearList.Remove(isClearRoom);
    }       // OnDestroy()

}       // ClassEnd
