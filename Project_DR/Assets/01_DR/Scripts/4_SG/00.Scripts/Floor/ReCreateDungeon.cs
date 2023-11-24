using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReCreateDungeon : MonoBehaviour
{       

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("DungeonInspection"))
        {
            DungeonInspectionManager.dungeonManagerInstance.CheckDungeonReCreating();
        }
    }

}       // ClassEnd
