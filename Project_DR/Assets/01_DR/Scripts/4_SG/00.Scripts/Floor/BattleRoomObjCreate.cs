using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;

public class BattleRoomObjCreate : RandomRoomObjCreate
{
    // 부모오브젝트의 CreateObj함수를 이용해 아이템을 스폰할거임
    // 1. 상속받은 Class에서 소환할거 최소치 최대치만큼 for문돌아서 CreateObj함수를 Call하면 될거같음

    int lightCreateTableID = 16000;
    int envCreateTableId = 16001;
    int matCreateTableId = 16002;


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
