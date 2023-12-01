using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonExit : MonoBehaviour
{
    public bool isLobby;
    public string dungeonScene;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("다음씬으로?");
            if(isLobby)
            {
                SceneManager.LoadScene(dungeonScene);
            }
            else
            GameManager.instance.ResetScene();
        }
    }


}
