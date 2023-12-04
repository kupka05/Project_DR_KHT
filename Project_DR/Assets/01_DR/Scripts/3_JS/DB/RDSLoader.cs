using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

// TODO: 로그인 성공시 플레이어의 기본 정보를 가져온다.
// 기본 정보를 모두 가져올 경우 다음 씬으로 넘어간다.
public class RDSLoader : MonoBehaviour
{
    /*************************************************
     *                 Private Fields
     *************************************************/
    #region [+]
    private bool _isDone = false;
    [SerializeField] string url;
  

    private void Start()
    {
        //StartCoroutine(SetDatabase());
        //GameManager.instance.SetPlayerID("123");
        PlayerData.UpdateID();
        Debug.Log(PlayerData.ID);
        //SetDatabase2();
        //StartCoroutine(SetDatabase());
        //StartCoroutine(PlayerData.GetDatabase());
    }

    IEnumerator GetDatabase()
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "search_all");
        form.AddField("id", "test");
        form.AddField("column", "password");
        //form.AddField("value", "asd");


        // using문을 사용하여 메모리 누수 해결
        // 사용이 끝난 후 할당 해제
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            // 플레이어 정보 프린트
            print(www.downloadHandler.text);

            // using문을 사용해도 메모리 누수가 발생하여
            // 추가로 Dipose()함수를 호출해서 할당 해제
            www.Dispose();
        }
    }
    IEnumerator SetDatabase()
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "search_all");
        form.AddField("id", "123");
        form.AddField("column", "password");
        //form.AddField("value", "304400");


        // using문을 사용하여 메모리 누수 해결
        // 사용이 끝난 후 할당 해제
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            // 플레이어 정보 프린트
            print(www.downloadHandler.text);

            // using문을 사용해도 메모리 누수가 발생하여
            // 추가로 Dipose()함수를 호출해서 할당 해제
            www.Dispose();
        }
    }

    public void SetDatabase2()
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "search_all");
        form.AddField("id", "123");
        form.AddField("column", "password");
        //form.AddField("value", "304400");


        // using문을 사용하여 메모리 누수 해결
        // 사용이 끝난 후 할당 해제
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.SendWebRequest();

            // 플레이어 정보 프린트
            print(www.downloadHandler.text);

            // using문을 사용해도 메모리 누수가 발생하여
            // 추가로 Dipose()함수를 호출해서 할당 해제
            www.Dispose();
        }
    }
    #endregion
}