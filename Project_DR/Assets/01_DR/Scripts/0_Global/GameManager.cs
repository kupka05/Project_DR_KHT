using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Layer
{
    Default = 0,
    TransparentFX = 1,
    IgnoreRaycast = 2,
    // 3 == null,
    Water = 4,
    UI = 5,
    Grabbable = 9,
    Drill = 11,
    Monster = 12,
    MonsterWall = 13,
    Player = 14,
    MapObject = 19,
    BattleRoomFloor = 20
}       // Layer

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 생성 후 할당
                GameObject obj = new GameObject("GameManager");
                m_instance = obj.AddComponent<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static GameManager m_instance; // 싱글톤이 할당될 static 변수    


    [Header("Player Object")]
    public GameObject player;
    private ScreenFader fader;
    private InputBridge input;
    private ScreenText screenText;

    [Header("Game Over")]
    public string gameoverText;
    public string gameoverScene;

    private string _playerID; // SetPlayerID(string id) 메서드로 설정함
    public string PlayerID => _playerID;


    [Header("Dungeon")]
    // ----------------------------------------------- SG ------------------------------------------------

    public bool IsProtoType = true;

    public int nowFloor;        // 현재 몇층인지 알려줄 변수
    public int isPlayerMaxFloor;    // 현재 플레이어의 회차의따라서 아래 const변수값이 대입될것임
    // 던전 진행 Max층을 알려줄 const int 변수
    public const int PROTOTYPE = 1;
    public const int FIRSTTIME = 3;
    public const int FIRSTAFTER = 5;

    public static List<RandomRoom> isClearRoomList;       // 모든 방의 클리어 여부를 관리해줄 List

    private bool allRoomClear = false;              // 모든 방이 클리어 됬다면 true가 될 변수

    private bool isClear = false;                   // 방의 클리어 여부에 따라 변수값이 변하고 문을 관리해줄것임
    
    public bool isGameOver;


    public bool IsClear
    {
        get { return isClear; }
        set
        {
            if (isClear != value)
            {
                isClear = value;
                DoorControll(IsClear);
                if (isClear == true)
                {
                    allRoomClear = CheckAllRoomClear();          
                }
                if(allRoomClear == true)
                {
                    BossRoomDoorOnEvent?.Invoke();
                }
            }

        }
    }

    private bool isBossRoomClear = false;
    public bool IsBossRoomClear
    {
        get { return isBossRoomClear; }
        set
        {
            if (isBossRoomClear != value)
            {
                isBossRoomClear = value;
            }
            if (isBossRoomClear == true)
            {
                NextRoomDoorOnEvent?.Invoke();
            }
        }
    }

    public List<GameObject> ghostObjList;       // 유령을 담아둘 리스트
    public List<NullRoom> nullRoomList;         // 빈방을 담아두는 리스트 (모루생성에 필요)
    public bool isInItGhost = false;    

    public event System.Action DoorOnEvent;
    public event System.Action DoorOffEvent;
    public event System.Action BossRoomDoorOnEvent;
    public event System.Action NextRoomDoorOnEvent;



    // ----------------------------------------------- SG ------------------------------------------------

    private void Awake()
    {
        // 싱글톤 인스턴스 초기화
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AwakeInIt();
    }

    void Start()
    {
        // 데이터 가져오기
        GetData();
        StartInIt();

        // 플레이어 찾아오기
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            input = player.transform.parent.GetComponent<InputBridge>();
        }
        else
        {
            GFunc.Log("플레이어를 찾지 못했습니다.");
        }
        // 스크린 페이더 가져오기
        if (Camera.main)
        {
            fader = Camera.main.transform.GetComponent<ScreenFader>();
        }
    }       // Start()    


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            GameOver();
        }
    }

    private void OnLevelWasLoaded()
    {
        // GFunc.Log("객체의 첫 생성일때에도 이게 호출이 되나?");
        // 데이터 가져오기
        GetData();

        // 플레이어 찾아오기
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            input = player.transform.parent.GetComponent<InputBridge>();
        }
        else
        {
            GFunc.Log("플레이어를 찾지 못했습니다.");
        }

    }


    /// <summary>
    /// Awake사이클에서 초기화해야하는것 초기화하는 함수
    /// </summary>
    private void AwakeInIt()
    {
        if (isClearRoomList == null || isClearRoomList == default)
        {
            isClearRoomList = new List<RandomRoom>();
        }

    }       // AwakeInIt()

    private void StartInIt()
    {
        // TODO : 프로토타입 이후 수정 예정
        if (IsProtoType == true)
        {
            isPlayerMaxFloor = PROTOTYPE;
            return;
        }
        if (UserDataManager.Instance.ClearCount <= 1)
        {
            isPlayerMaxFloor = FIRSTAFTER;
        }
        else
        {
            isPlayerMaxFloor = FIRSTTIME;
        }
    }       // StartInIt()



    /*************************************************
     *            Game Over & Game Clear
    *************************************************/
    #region GameManager
    // 게임오버
    public void GameOver()
    {
        fader.DoFadeIn();
        screenText = player.GetComponent<ScreenText>();
        screenText.OnScreenText(gameoverText);
        input.enabled = false;

        isGameOver = true;
        UserData.GameOver();
        //UserData.ResetPlayer();

        SceneLoad(gameoverScene); // 게임오버 씬 전환
    }

    // 현재 씬 리셋
    public void ResetScene()
    {
        isGameOver = true;
        UserData.GameOver();
        //UserData.ResetPlayer();

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneLoad(currentSceneName);
    }

    /// <summary>  던전 클리어후 로비로 보내줄 함수 </summary>
    public void ClearDungeon()
    {
        // 출구 층이면 로비로 보내주기
        if (nowFloor == isPlayerMaxFloor)
        {
            isGameOver = true;

            UserData.ClearDungeon();

            string lobbySceneName = "3_LobbyScene";
            SceneLoad(lobbySceneName);
        }
        // 출구 층이 아니라면, 다시 던전씬 돌리기
        else if (nowFloor < isPlayerMaxFloor)
        {
            // 층 높이고
            nowFloor++;
            string dungeonSceneName = "SG_TestScene";
            SceneLoad(dungeonSceneName);
        }
        else
            GFunc.Log("클리어 실패, 현재 층 : " + nowFloor);
     
    }       // ClearDungeon()

    #endregion

    public void DestroyGameManager()
    {
        Destroy(gameObject);
    }


    /*************************************************
     *                Scene Manager
    *************************************************/
    #region Scene Manager
    // 씬 전환 함수
    public void SceneLoad(string _sceneName)
    {
        if (string.IsNullOrEmpty(_sceneName))
        {
            GFunc.Log("전환할 씬을 찾지 못했습니다.");
            return;
        }
        StartCoroutine(SceneChangeDelay(_sceneName));
    }

    // 플레이어의 페이드를 포함한 씬 전환 딜레이
    IEnumerator SceneChangeDelay(string _sceneName)
    {
        if (fader)
        {
            fader.DoFadeIn();
        }

        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(_sceneName);

        if (isGameOver)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    /*************************************************
    *                 Dungeon Object
    *************************************************/
    #region Dungeon
    /// <summary>
    /// 문을 열고 닫는 함수를 하나로 묶은것
    /// </summary>
    /// <param name="_isDoorOn">문을 열지 닫을지 bool값</param>
    private void DoorControll(bool _isDoorOn)
    {
        if (_isDoorOn == true)
        {
            DoorOn();
        }
        else if (_isDoorOn == false)
        {
            DoorOff();
        }
    }       // DoorControll()

    private void DoorOn()
    {
        DoorOnEvent?.Invoke();
    }       // DoorOn()

    private void DoorOff()
    {
        DoorOffEvent?.Invoke();
    }       // DoorOff()

    /// <summary>
    /// 모든 방이 클리어 됬는지 체크해줄 함수
    /// </summary>
    public bool CheckAllRoomClear()
    {
        foreach(RandomRoom temp in isClearRoomList)
        {
            if(temp.isClearRoom == false)
            {
                return false;
            }
        }
        return true;
    }       // CheckAllRoomClear()

    /// <summary>
    /// 리스트에 NullRoom이라는 객체를 Add해주는 함수
    /// </summary>
    /// <param name="_nullRoom"></param>
    public void AddNullRoom(NullRoom _nullRoom)
    {
        if (nullRoomList == null || nullRoomList == default)
        {       // 참조되는순간에 New할당
            nullRoomList = new List<NullRoom>();
        }
        else { /*PASS*/ }

        nullRoomList.Add(_nullRoom);
        
        //GFunc.Log($"NullRomListCount : {nullRoomList.Count}\nNowFloor : {nowFloor}");
        
        if(nullRoomList.Count == 2 && nowFloor % 2 == 0)
        {   // 빈방이 2개일때와 현재층이 짝수 일때에 들어오는 조건문
            int randListIdx = UnityEngine.Random.Range(0, nullRoomList.Count);                        

            nullRoomList[randListIdx].CreateAnvilObj();
            GFunc.Log($"모루 생성 -> 위치 {nullRoomList[randListIdx].gameObject.name}");
        }
        else { GFunc.Log($"조건에 맞지않아서 모루가 생성되지 않았음");/*PASS*/ }
    }       // AddNullRoom()


    public void FadeIn()
    {
        if (fader)
        {
            fader.DoFadeIn();
        }
    }
    public void FadeOut()
    {
        if (fader)
        {
            fader.DoFadeOut();
        }
    }



  

    /// <summary>
    /// 유령 오브젝트를 List에 Add해주는 함수
    /// </summary>
    public void AllocatedGhostObj()
    {        
        if (UserDataManager.Instance.ClearCount < 100)
        {
            if (isInItGhost == false)
            {       // 문제 X
                isInItGhost = true;
                ghostObjList = new List<GameObject>();
                GameObject ghostObj;
                ghostObj = Resources.Load<GameObject>("NPC_12_Ghost_FT");
                ghostObjList.Add(ghostObj);
                ghostObj = Resources.Load<GameObject>("NPC_12_Ghost_IE");
                ghostObjList.Add(ghostObj);
                ghostObj = Resources.Load<GameObject>("NPC_12_Ghost_IEFT");
                ghostObjList.Add(ghostObj);
                ghostObj = Resources.Load<GameObject>("NPC_12_Ghost_NS");
                ghostObjList.Add(ghostObj);
                ghostObj = Resources.Load<GameObject>("NPC_12_Ghost_PJ");
                ghostObjList.Add(ghostObj);

            }
            else { /*PASS*/ }
        }
        else { /*PASS*/ }



    }       // AllocatedGhostObj()
    #endregion

    /*************************************************
    *                        Data
    *************************************************/
    #region Data
    // 데이터 가져오기
    public void GetData()
    {
        gameoverText = (string)DataManager.Instance.GetData(1001, "GameOverText", typeof(string));
    }

    // 아이디 가져오기
    public void SetPlayerID(string id)
    {
        _playerID = id;
    }
    #endregion
}       // ClassEnd
