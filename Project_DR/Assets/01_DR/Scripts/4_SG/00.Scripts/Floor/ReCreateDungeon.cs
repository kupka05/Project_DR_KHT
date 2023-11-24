using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReCreateDungeon : MonoBehaviour
{
    Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(DesRigid());
    }


    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("DungeonInspection"))
        {
            EndCoroutine();
            DungeonInspectionManager.dungeonManagerInstance.CheckDungeonReCreating();
        }
    }

    // 커스텀방의 곂침현상을 감지할때 rigidBody가 있으면 점점 내려가는 현상떄문에 제작한 코루틴
    IEnumerator DesRigid()
    {
        yield return new WaitForSeconds(2f);
        Destroy(rigid);
        EndCoroutine();
    }
    // 코루틴을 끝내는 함수
    private void EndCoroutine()
    {
        StopAllCoroutines();
    }

}       // ClassEnd
