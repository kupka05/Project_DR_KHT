using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStagePotal : MonoBehaviour
{       // TODO : 여기서 DB에서 클리어여부 확인후 bool값 지정 -> 던전으로 이동할때에 Count체크 ->
        // 처음이라면 Count3일때에 포탈 들어가면 로비 || 아니라면 다음던전으로

    // 던전 3스테이지 까지 갈지 5스테이지 까지 갈지 클리어여부를 확인할 변수
    private bool isFirst;
    // 현재 던전 몇층까지 내려갔는지 확인해줄 변수 -> GameManager에서 땡겨올생각
    private int dungeonCount;
    private void Awake()
    {
        // TODO : 클리어 여부 가져오기
    }   
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            CheckNextStage();
        }
    }
    /// <summary>
    /// 어느 지점으로 이동할지 체크해주는 함수
    /// </summary>
    private void CheckNextStage()
    {
        if(isFirst == true)
        {
            if (dungeonCount == 2)
            {
                // TODO : 로비씬 || 결과씬으로 이동
            }
        }

        else if(dungeonCount == 5)
        {
            // TODO : 정산창 나오고 로비로 이동
        }
    }       // CheckNextStage()

}       // ClassEnd
