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

        PlayerDataManager.Update();

        PlayerDataManager.Save("weapon_cri_rate", "53321.8");
    }
    #endregion
}