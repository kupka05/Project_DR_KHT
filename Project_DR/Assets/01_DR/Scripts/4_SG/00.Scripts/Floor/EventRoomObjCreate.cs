using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRoomObjCreate : RandomRoomObjCreate
{

    int lightCreateTableID = 16010;
    int envCreateTableId = 16011;
    int matCreateTableId = 16012;

    protected override void Awake()
    {
        base.Awake();
        //SpawnLightObj(lightCreateTableID);
        //SpawnEnvObj(envCreateTableId);
        //SpawnMatObj(matCreateTableId);
        StartSpawn(lightCreateTableID, envCreateTableId, matCreateTableId);

    }

    //protected override void Start()
    //{
    //    base.Start();
    //    SpawnLightObj(lightCreateTableID);
    //    SpawnEnvObj(envCreateTableId);
    //    SpawnMatObj(matCreateTableId);
    //}
}       // ClassEnd
