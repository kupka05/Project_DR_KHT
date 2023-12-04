using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class PlayerData
{
    /*************************************************
     *                 Public Fields
     *************************************************/
    #region [+]
    public static string PlayerID => GameManager.instance.PlayerID;
    public static string ID => _id;
    //public static float HP => GetHP();

    #endregion
    /*************************************************
        *                 Private Fields
        *************************************************/
    #region [+]
    private static string _url = 
        "https://80koj3uzn4.execute-api.ap-northeast-2.amazonaws.com/default/UserTableLambda";
    private static string _id;

    #endregion
    /*************************************************
        *                 Public Methods
        *************************************************/
    #region [+]


    #endregion
    /*************************************************
        *                 Private Methods
        *************************************************/
    #region [+]
    private static IEnumerator Update()
    {
        WWWForm form = new WWWForm();
        form.AddField("command", "search_all");
        form.AddField("id", PlayerID);

        using (UnityWebRequest www = UnityWebRequest.Post(_url, form))
        {
            yield return www.SendWebRequest();

            // 플레이어 정보 프린트
            Debug.Log(www.downloadHandler.text);

            // using문을 사용해도 메모리 누수가 발생하여
            // 추가로 Dipose()함수를 호출해서 할당 해제
            www.Dispose();
        }
    }

    #endregion
}
