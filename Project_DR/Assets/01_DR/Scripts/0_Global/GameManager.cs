using BNG;
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
    // 3,
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
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
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
    public int nowFloor = 1;        // 현재 몇층인지 알려줄 변수

    // string변수는 Resources폴더속 필요한 경로를 담고 있음 뒤에 몬스터의 이름을 붙여서 인스턴스할 계획
    public string nomalMonsterSpawnPath = "";
    public string eliteMonsterSpawnPath = "";

    // ----------------------------------------------- SG ------------------------------------------------

    private void Awake()
    {
        // DB 테스트를 위해 DontDestroy로 할당
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
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
            Debug.Log("플레이어를 찾지 못했습니다.");
        }


    }       // Start()



    // 게임오버
    public void GameOver()
    {
        // 스크린 페이더 가져오기
        if (Camera.main)
        {
            fader = Camera.main.transform.GetComponent<ScreenFader>();
        }

        fader.DoFadeIn();
        screenText = player.GetComponent<ScreenText>();
        screenText.OnScreenText(gameoverText);
        input.enabled = false;


        Invoke(nameof(GameOverScene),5f);
    }

    // 현재 씬 리셋
    public void ResetScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // 게임오버시 씬 이동
    public void GameOverScene()
    {
        SceneManager.LoadScene(gameoverScene);
    }
        

    // 데이터 가져오기
    public void GetData()
    {
        gameoverText = (string)DataManager.instance.GetData(1001, "GameOverText", typeof(string));
    }

    // 아이디 가져오기
    public void SetPlayerID(string id)
    {
        _playerID = id;
    }
}       // ClassEnd
