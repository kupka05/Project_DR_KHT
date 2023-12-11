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
        player.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 60f, this.transform.position.z);


    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}       // ClassEnd
