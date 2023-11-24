using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonInspectionManager : MonoBehaviour
{
    Coroutine coroutine;
    DungeonCreator creator = null;
    // 던전 생성이 끝났는지 확인해줄 변수 -> 생성중에 다시생성시 오류생김 방지
    public bool isCreateDungeonEnd = false;
    // 현재 재생성 하고 있는중인지 체크하는 변수(코루틴 내부에서만 값이 바뀔거임)
    private bool isTryDungeonCreate = false;
    public static DungeonInspectionManager dungeonManagerInstance;
    private void Awake()
    {
        if(dungeonManagerInstance == null)
        {
            dungeonManagerInstance = this;
        }
        else { Debug.Log("DungeonInspectionManager : else 들어옴"); }
    }


    public void CheckDungeonReCreating()
    {
        if(isTryDungeonCreate == false && isCreateDungeonEnd == true)
        {
            coroutine = StartCoroutine(TryDungeonReCreate());
        }
        else
        {
            ReCheckCheckDungeonReCreating();
        }
        
    }       // CheckDungeonReCreating()

    // 위에 곂쳤을때에 들어왔는데 던전 생성 덜되었을떄에 한프레임뒤에 다시 체크하도록 제작
    IEnumerator ReCheckCheckDungeonReCreating()
    {
        yield return null;
        CheckDungeonReCreating();
    }   // ReCheckCheckDungeonReCreating()

    IEnumerator TryDungeonReCreate()
    {
        
        if(creator == null) { creator = GameObject.Find("DungeonCreator").GetComponent<DungeonCreator>(); }
        isTryDungeonCreate = true;

        yield return null;

        creator.CreateDungeon();
        isTryDungeonCreate = false;
        isCreateDungeonEnd = false;
        StopAllCoroutines();
    }       // TryDungeonReCreate()

}       // Class
