using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReCreateDungeon : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OncollisionEnter들어옴");
        if(collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("땅바닥태그를 가진 바닥이랑 만남");
            DungeonCreator dungeonCreator = GameObject.Find("DungeonCreator").GetComponent<DungeonCreator>();
            dungeonCreator.CreateDungeon();
        }
    }

}
