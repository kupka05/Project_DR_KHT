using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    void Start()
    {
        StartCoroutine(PlayerCreateD());

    }

    IEnumerator PlayerCreateD()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return null;
        }
        player = GameManager.instance.player;
        player.transform.position = this.transform.position;
        
        
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}       // ClassEnd
