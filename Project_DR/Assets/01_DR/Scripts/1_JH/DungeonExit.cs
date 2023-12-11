using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BNG;

public class DungeonExit : MonoBehaviour
{
    private GameObject player;
    public bool isLobby;
    public string dungeonScene;
    public ScreenFader fader;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fader = player.GetComponentInChildren<ScreenFader>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("다음씬으로?");
            if (isLobby)
            {
                fader.DoFadeIn();
                StartCoroutine(SceneChange(dungeonScene));
            }
            else
            {
                //ToDo : 다음 던전 층 어떻게 보낼지 필요
                fader.DoFadeIn();
                Invoke("ResetScene", 3f);
            }
        }
    }

    public void ResetScene()
    {
        GameManager.instance.ResetScene();
    }
    IEnumerator SceneChange(string sceneName)
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(sceneName);
    }
}
