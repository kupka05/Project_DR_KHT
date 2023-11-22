using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private GameObject player;
    private ScreenFader fader;
    private InputBridge input;
    private ScreenText screenText;

    public string gameoverText;

    void Start()
    {
        GetData();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            input = player.transform.parent.GetComponent<InputBridge>();
        }
    }       // Start()


    void Update()
    {


    }       // Update()


    public void GameOver()
    {
        // 스크린 페이더 가져오기
        if (Camera.main)
        {
            fader = Camera.main.transform.GetComponent<ScreenFader>();
        }
        screenText = player.GetComponent<ScreenText>();

        fader.DoFadeIn();
        screenText.OnScreenText(gameoverText);
        input.enabled = false;


        Invoke(nameof(ResetScene),5f);
    }
    public void ResetScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public void GetData()
    {
        gameoverText = (string)DataManager.instance.GetData(1001, "GameOverText", typeof(string));
    }


}       // ClassEnd
