using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonInspectionManager : MonoBehaviour
{
    Coroutine coroutine;
    DungeonCreator creator = null;
    // 던전 생성이 끝났는지 확인해줄 변수 -> 생성중에 다시생성시 오류생김 방지
    public bool isCreateDungeonEnd = false;
    // 현재 재생성 하고 있는중인지 체크하는 변수(코루틴 내부에서만 값이 바뀔거임)
    private bool isTryDungeonCreate = false;
    public static DungeonInspectionManager dungeonManagerInstance;

    private int returnCount = 0;        // 몇번 재귀했는지 확인할 변수
    private const int MAXRECREATCOUNT = 100;
    private bool isReLoadScene;

    private int reCreacteCount = 0;         // 던전 재생성한 횟수
    private void Awake()
    {
        if(dungeonManagerInstance == null)
        {
            dungeonManagerInstance = this;
        }
        else { /*GFunc.Log("DungeonInspectionManager : else 들어옴");*/ }

        isReLoadScene = false;
    }


    public void CheckDungeonReCreating()
    {
        if(isTryDungeonCreate == false && isCreateDungeonEnd == true)
        {
            coroutine = StartCoroutine(TryDungeonReCreate());
        }
        else
        {
            StartCoroutine(ReCheckCheckDungeonReCreating());
            //GFunc.Log($"재생성 else로 재귀함수 실행");
        }
        
    }       // CheckDungeonReCreating()

    // 위에 곂쳤을때에 들어왔는데 던전 생성 덜되었을떄에 한프레임뒤에 다시 체크하도록 제작
    IEnumerator ReCheckCheckDungeonReCreating()
    {
        
        if(returnCount >= 50)
        {
            StartCoroutine(TryDungeonReCreate());
            returnCount = 0;
        }
        yield return null;
        CheckDungeonReCreating();
        returnCount++;
    }   // ReCheckCheckDungeonReCreating()

    IEnumerator TryDungeonReCreate()
    {
        
        
        if(creator == null) { creator = GameObject.Find("DungeonCreator").GetComponent<DungeonCreator>(); }
        isTryDungeonCreate = true;

        yield return null;
        reCreacteCount++;
        //if(reCreacteCount < MAXRECREATCOUNT && isReLoadScene == false)
        //{
        //    isReLoadScene = true;
        //    ReLoadDungeonScene();
        //}
        creator.CreateDungeon();
        //GFunc.Log($"던전 재생성");
        isTryDungeonCreate = false;
        isCreateDungeonEnd = false;
        StopAllCoroutines();
    }       // TryDungeonReCreate()

    /// <summary>
    /// 던전 씬을 다시 로드하는 함수
    /// </summary>
    private void ReLoadDungeonScene()
    {
        string nowSceneName = SceneManager.GetActiveScene().name;

        GameManager.instance.SceneLoad(nowSceneName);
    }

}       // Class
