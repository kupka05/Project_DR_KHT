using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BNG;
using System.Threading.Tasks;

public class DungeonExit : MonoBehaviour
{

    public bool isLobby;        // 로비 여부
    public string sceneName;    // 전환할 씬 이름
    public float sceneDelay;    // 씬 딜레이

    private ScreenFader fader;
    private bool playerPass = false;

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
            if(playerPass)
            {
                return;
            }

            playerPass = true;
            if (isLobby)
            {
                SceneLoad(sceneName);
            }

            else if(GameManager.instance.nowFloor <= GameManager.instance.isPlayerMaxFloor )
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

    private async Task LoadSceneAsync(string _sceneName)
    {
        if (string.IsNullOrEmpty(_sceneName))
        {
            GFunc.Log("전환할 씬을 찾지 못했습니다.");
            return;
        }

        Debug.Log("Loading scene asynchronously...");
        var asyncOperation = SceneManager.LoadSceneAsync(_sceneName);

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");
            await Task.Delay(100);
        }

        Debug.Log("Scene loaded.");
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
