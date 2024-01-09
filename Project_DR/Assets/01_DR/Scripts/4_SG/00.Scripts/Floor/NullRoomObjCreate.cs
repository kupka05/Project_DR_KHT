using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullRoomObjCreate : RandomRoomObjCreate
{

    int lightCreateTableID = 16020;
    int envCreateTableId = 16021;
    int matCreateTableId = 16022;

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
