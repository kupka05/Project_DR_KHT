using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonInspectionManager : MonoBehaviour
{
    Coroutine coroutine;
    DungeonCreator creator = null;
    // 현재 재생성 하고 있는중인지 체크하는 변수(코루틴 내부에서만 값이 바뀔거임)
    private bool isTryDungeonCreate = false;

    public static DungeonInspectionManager dungeonManagerInstance;
    private void Awake()
    {
        if(dungeonManagerInstance == null)
        {
            dungeonManagerInstance = this;
        }
        else { }
    }


    public void CheckDungeonReCreating()
    {
        if(isTryDungeonCreate == false)
        {
            coroutine = StartCoroutine(TryDungeonReCreate());
        }
        else
        {
            return;
        }
        
    }       // CheckDungeonReCreating()

    IEnumerator TryDungeonReCreate()
    {
        
        if(creator == null) { creator = GameObject.Find("DungeonCreator").GetComponent<DungeonCreator>(); }
        isTryDungeonCreate = true;

        yield return null;

        creator.CreateDungeon();
        isTryDungeonCreate = false;
        StopAllCoroutines();
    }

}       // Class
