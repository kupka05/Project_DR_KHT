using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_Test : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Floor¶û ´êÀ½");
        }
    }
}       // SG_Test
