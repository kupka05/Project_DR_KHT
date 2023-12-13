using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemolisherWall : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(DestroyTime());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("EventWall"))
        {
            Destroy(collision.gameObject);
            //Debug.Log($"파괴 된건 있나? -> {collision.gameObject.name}");
        }
    }

    IEnumerator DestroyTime()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return null;
        }
        //yield return null;
        Destroy(this.gameObject);
    }

}   //ClassEnd
