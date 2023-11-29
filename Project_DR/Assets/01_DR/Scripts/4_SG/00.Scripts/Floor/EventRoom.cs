using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRoom : RandomRoom
{       // EventRoomClass는 스프레드시트의 값을 받아와서 랜덤한 이벤트 발생을 시켜줄 Class

        //  meshPos : 해당 변수로 자신의 방 꼭지점을 가져올수 있음



    void Start()
    {
        DungeonManager.clearList.Add(isClearRoom);
        GetFloorPos();      // 꼭지점 가져와주는 Class
        
    }       // Start()

    private void OnDestroy()
    {
        DungeonManager.clearList.Remove(isClearRoom);
    }       // OnDestroy()




}       // ClassEnd
