using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static SkillManager;

public class BattleRoomObjCreate : RandomRoomObjCreate
{
    private StringBuilder sb;        // 스프레드시트에서 어떤것을 스폰한지 정할 StringBuilder
    private int lightSpawnConditionId;


    private void Start()
    {
        StartInIt();
    }

    /// <summary>
    /// Start함수에서 실행될 함수 = 초기화 함수
    /// </summary>
    private void StartInIt()
    {
        sb = new StringBuilder();
        lightSpawnConditionId = 16000;

    }       // StartInIt()


    /// <summary>
    /// 스폰을 위한 준비를 하는 함수
    /// </summary>
    private void ReadyToSpoawn()
    {
        CreateRightObj();
    }       // ReadyToSpoawn()

    /// <summary>
    /// Right오브젝트 생성하는 함수
    /// </summary>
    private void CreateRightObj()
    {
        // 참조할 오브젝트의 ID
        int refObjId = (int)DataManager.instance.GetData(lightSpawnConditionId, "CategoryID", typeof(int));
        int randMinValue = (int)DataManager.instance.GetData(lightSpawnConditionId, "MinValue", typeof(int));
        int randMaxValue = (int)DataManager.instance.GetData(lightSpawnConditionId, "MaxValue", typeof(int));

        int randNum = UnityEngine.Random.Range(randMinValue, randMaxValue + 1);

        // 어떤것을 소환할지 랜덤 돌리기위해 refId의를 이용해서 열을 담아둘 변수
        int csvColumn = (int)DataManager.instance.GetCount(refObjId);
        // 여기서 spawnObjPick + refObjId 하면 소환해야되는 Obj의 Id 값이 나옴
        int spawnObjPick = UnityEngine.Random.Range(0, csvColumn);





        

    }       // CreateRightObj()

    
}       // ClassEnd
