using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReCreateDungeon : MonoBehaviour
{
    Rigidbody rigid;

    private bool secondCheck = false;       // 재 생성시 간혹 곂침현상이 일어나서 재생성후 CollisonStay에서 한번더 체크

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(DesRigid());
        StartCoroutine(SecondCheckStart());
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("DungeonInspection"))
        {
            EndCoroutine();
            DungeonInspectionManager.dungeonManagerInstance.CheckDungeonReCreating();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (secondCheck == true)
        {
            if (collision.gameObject.CompareTag("DungeonInspection"))
            {
                Debug.Log($"재생성기 감지");
                EndCoroutine();
                DungeonInspectionManager.dungeonManagerInstance.CheckDungeonReCreating();
                secondCheck = false;
                SecondCheckStart();
            }
        }
    }

    // 커스텀방의 곂침현상을 감지할때 rigidBody가 있으면 점점 내려가는 현상떄문에 제작한 코루틴
    IEnumerator DesRigid()
    {
        yield return new WaitForSeconds(3f);
        Destroy(rigid);
        EndCoroutine();
    }

    IEnumerator SecondCheckStart()
    {
        yield return null;
        secondCheck = true;

    }
    // 코루틴을 끝내는 함수
    private void EndCoroutine()
    {
        //StopAllCoroutines();
    }

}       // ClassEnd
