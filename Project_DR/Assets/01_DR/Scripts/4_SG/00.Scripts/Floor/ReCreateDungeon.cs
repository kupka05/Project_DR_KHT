using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReCreateDungeon : MonoBehaviour
{
    //Rigidbody rigid;
    //BoxCollider boxCollider;

    private bool secondCheck = false;       // 재 생성시 간혹 곂침현상이 일어나서 재생성후 CollisonStay에서 한번더 체크

    private void Start()
    {
        //rigid = GetComponent<Rigidbody>();
        //boxCollider = GetComponent<BoxCollider>();
        StartCoroutine(DesRigid());
        StartCoroutine(SecondCheckStart());
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("DungeonInspection"))
        {            
            DungeonInspectionManager.dungeonManagerInstance.FloorCollision = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (secondCheck == true)
        {
            if (collision.gameObject.CompareTag("DungeonInspection"))
            {
                secondCheck = false;
                //GFunc.Log($"던전 재생성예정임");
                DungeonInspectionManager.dungeonManagerInstance.FloorCollision = true;
            }
        }
    }

    // 커스텀방의 곂침현상을 감지할때 rigidBody가 있으면 점점 내려가는 현상떄문에 제작한 코루틴
    IEnumerator DesRigid()
    {
        yield return new WaitForSeconds(5f);
        //Destroy(rigid);
        //Destroy(boxCollider);
        Destroy(this.gameObject);        
    }

    IEnumerator SecondCheckStart()
    {
        yield return null;
        secondCheck = true;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}       // ClassEnd
