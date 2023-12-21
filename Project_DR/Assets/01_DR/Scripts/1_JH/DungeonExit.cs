using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BNG;

public class DungeonExit : MonoBehaviour
{
    public bool isLobby;        // 로비 여부
    public string sceneName;    // 전환할 씬 이름
    public float sceneDelay;    // 씬 딜레이

    private ScreenFader fader;

    // 12.12 SG 추가


    public void Start()
    {
        // 플레이어의 페이더 찾아오기
        fader = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenFader>();
        if (!fader)
        {
            GFunc.Log("페이더를 찾지 못했습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (isLobby)
            {
                SceneLoad(sceneName);
            }

            else if(GameManager.instance.isPlayerMaxFloor <= GameManager.instance.nowFloor )
            {
                GameManager.instance.ClearDungeon();
            }
        }
    }

    public void SceneLoad(string _sceneName)
    {
        if (string.IsNullOrEmpty(_sceneName))
        {
            GFunc.Log("전환할 씬을 찾지 못했습니다.");
            return;
        }
        StartCoroutine(SceneChange(_sceneName));
    }


    // 씬 전환
    IEnumerator SceneChange(string _sceneName)
    {
        if (fader)
        {
            fader.DoFadeIn();
        }
        yield return new WaitForSeconds(sceneDelay);
        SceneManager.LoadScene(_sceneName);
    }
}
