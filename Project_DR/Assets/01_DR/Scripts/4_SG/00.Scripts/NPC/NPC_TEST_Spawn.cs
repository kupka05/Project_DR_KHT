using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_TEST_Spawn : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(TEST());    
    }

    
    IEnumerator TEST()
    {
        yield return new WaitForSeconds(5);

        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        Vector3 pPos = new Vector3(player.transform.position.x,0.5f, player.transform.position.z + 5.0f);

        this.transform.position = pPos;
    }

}
