using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MBTISight : MonoBehaviour
{

    private IEnumerator checkRoutine;
    private MBTIChecker lastChecker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SightCheck(float timer)
    {
        lastChecker.DebugOn();
        yield return new WaitForSeconds(timer);
        lastChecker.SightEvent();
        yield break;
    }


    void OnTriggerEnter(Collider other)
    {
        // 레이캐스트는 무시
        if (other.gameObject.layer == 2)
        {
            return;
        }
        // 체커가 없으면 반환
        MBTIChecker checker = other.gameObject.GetComponent<MBTIChecker>();
        if (checker == null)
        {
            return;
        }
        // 시야 체크 타입이면 실행
        if(checker.type == MBTIChecker.CheckType.Sight)
        {
            // 마지막 체커 업데이트 해주고
            lastChecker = checker;

            // 코루틴이 있으면, 기존 코루틴 정지
            if(checkRoutine != null)
            {
                StopCoroutine(checkRoutine);
                checkRoutine = null;
            }

            // 체크 코루틴 시작
            checkRoutine = SightCheck(checker.checkTime);
            StartCoroutine(checkRoutine);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(!lastChecker)
        { return; }

        if(other.gameObject == lastChecker.gameObject)
        {
            if (lastChecker.type == MBTIChecker.CheckType.Sight)
            {
                lastChecker.DebugOff();
            }
            lastChecker = null;
            StopCoroutine(checkRoutine);
            checkRoutine = null;
        }
    }
}
