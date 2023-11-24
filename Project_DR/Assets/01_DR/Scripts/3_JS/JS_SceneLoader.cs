using UnityEngine;
using UnityEngine.SceneManagement;

public class JS_SceneLoader : MonoBehaviour
{
    public float delay = default; // 로딩 딜레이
    public string sceneName = default; // 로딩할 씬의 이름

    void Start()
    {
        // delay 후에 LoadScene 함수를 호출하여 씬을 로딩
        Invoke("LoadSceneAfterDelay", delay);
    }

    void LoadSceneAfterDelay()
    {
        // 지정된 이름의 씬을 로딩
        SceneManager.LoadScene(sceneName);
    }
}
