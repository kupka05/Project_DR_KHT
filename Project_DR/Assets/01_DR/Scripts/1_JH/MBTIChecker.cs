using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MBTIChecker : MonoBehaviour
{
    public enum CheckType { Grab, Collision, Sight, Choice}
    private MBTI mbti;
    [Header("MBTI Checker")]

    public int ID;
    public CheckType type;
    public int checkCount;  // 체크 카운트
    public float checkTime; // 체크 시간
    public bool isDestroy; // 파괴 여부

    [Header("MBTI Value")]
    MBTI checkerMBTI;
    public float I;
    public float N;
    public float F;
    public float P;

    //[Header("Event")]



    // Start is called before the first frame update
    void Start()
    {
        GetData(ID);

      // 값들을 구조체로 저장
      checkerMBTI.SetMBTI(I, N, F, P);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // MBTI 이벤트를 호출하는 메서드
    public void ActiveMBTI()
    {
        // MBTI 계산
        ResultMBTI(mbti, checkerMBTI);

        // 오브젝트 삭제 상태이면 삭제
        if (isDestroy)
        {
            Destroy(gameObject);
        }
    }
    // MBTI를 계산하고 반환하는 메서드
    MBTI ResultMBTI(MBTI playerMBTI, MBTI value)
    {
        return
        playerMBTI.SetMBTI(
        ResultValue(playerMBTI.I, value.I),
        ResultValue(playerMBTI.N, value.N),
        ResultValue(playerMBTI.F, value.F),
        ResultValue(playerMBTI.P, value.P)
        );
    }

    // 각 값을 연산해주는 메서드
    float ResultValue(float preValue, float value)
    {
        if(value == 0)
        { return 0; }

        else if (0 < value)
        {
            return preValue + value <= 100 ? preValue + value : 100;
        }
        else
            return preValue - value >= 0 ? preValue - value : 100;
    }


    // 각 이벤트들
    private void GrabEvent()
    {
        if (checkCount-- < 0)
        {
            return;
        }
        else if (checkCount <= 0)
        {
            ActiveMBTI();
        }
    }

    private void CollisionEvent()
    {

    }

    private void SightEvent()
    {

    }

    private void ChoiceEvent() 
    { 

    }


    // 데이터를 가져오는 함수
    private void GetData(int ID)
    {
       checkCount = (int)DataManager.instance.GetData(ID, "CheckCount", typeof(int));
       checkTime = (float)DataManager.instance.GetData(ID, "CheckTime", typeof(float));
       isDestroy = (bool)DataManager.instance.GetData(ID, "Destroy", typeof(bool));

        switch ((string)DataManager.instance.GetData(ID, "Type", typeof(string)))
        {
            case "Grab":
                type = CheckType.Grab; break;
            case "Collision":
                type = CheckType.Collision; break;
            case "Sight":
                type = CheckType.Sight; break;
            case "Choice":
                type = CheckType.Choice; break;
        }

        I = (float)DataManager.instance.GetData(ID, "I", typeof(float));
        N = (float)DataManager.instance.GetData(ID, "N", typeof(float));
        F = (float)DataManager.instance.GetData(ID, "F", typeof(float));
        P = (float)DataManager.instance.GetData(ID, "P", typeof(float));

    }
}
