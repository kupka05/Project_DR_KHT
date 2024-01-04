using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MBTIChecker : MonoBehaviour
{
    public enum CheckType { Default, Grab, Collision, Sight, Choice, Touch}
    private MBTI mbti;
    private BoxCollider boxCollider;

    [Header("MBTI Checker")]

    public int ID;
    public CheckType type;
    public int checkCount;  // 체크 카운트
    public float checkTime; // 체크 시간
    public bool isDestroy; // 파괴 여부

    MBTI checkerMBTI;
    [Header("MBTI Value")]
    public float I;
    public float N;
    public float F;
    public float P;

    [Header("Debug")]
    public bool DEBUG;



    // Start is called before the first frame update
    void Start()
    {
        GetData(ID);
        boxCollider = GetComponent<BoxCollider>();


      // 값들을 구조체로 저장
      checkerMBTI.SetMBTI(I, N, F, P);
    }

    // MBTI 이벤트를 호출하는 메서드
    public void ActiveMBTI()
    {
        // MBTI 계산
        MBTIManager.Instance.ResultMBTI(checkerMBTI);        
    }


    // 잡기 체크
    public void GrabEvent()
    {
        if (checkCount < 0 )
        { return; }


        if (--checkCount == 0)
        {
            ActiveMBTI();
            if (isDestroy)
            {
                Destroy(gameObject);
            }
        }
    }

    // 충돌 체크
    public void CollisionEvent(Collider other)
    {
        if (!other.CompareTag("Player"))
        { return; }

        ActiveMBTI();

        if (isDestroy)
        {
            boxCollider.enabled = false;
        }
    }
    // 시야 체크
    public void SightEvent()
    {
        ActiveMBTI();
        if(isDestroy)
        {
            type = CheckType.Default;
        }
        if(DEBUG)
        {
            GetComponent<MeshRenderer>().materials[0].color = Color.yellow;
        }
    }

    // 터치 체크
    public void TouchEvent(Collider other)
    {
        if (!other.GetComponent<Grabber>())
        { return; }

        ActiveMBTI();

        if (isDestroy)
        {
            boxCollider.enabled = false;
        }
    }
    public void DebugOn()
    {
        if (DEBUG)
        {
            GetComponent<MeshRenderer>().materials[0].color = Color.red;
        }
    }
    public void DebugOff()
    {
        if (DEBUG)
        {
            GetComponent<MeshRenderer>().materials[0].color = Color.grey;
        }
    }
    public void ChoiceEvent() 
    { 

    }

    public void OnTriggerEnter(Collider other)
    {
        // 충돌 체크일 경우
        if (type == CheckType.Collision)
        {
            CollisionEvent(other);
        }
        else if (type == CheckType.Touch)
        {
            TouchEvent(other);
        }
    }



    // 데이터를 가져오는 함수
    private void GetData(int ID)
    {
       checkCount = (int)DataManager.Instance.GetData(ID, "CheckCount", typeof(int));
       checkTime = (float)DataManager.Instance.GetData(ID, "CheckTime", typeof(float));
       isDestroy = (bool)DataManager.Instance.GetData(ID, "Destroy", typeof(bool));

        switch ((string)DataManager.Instance.GetData(ID, "Type", typeof(string)))
        {
            case "Grab":
                type = CheckType.Grab; break;
            case "Collision":
                type = CheckType.Collision; break;
            case "Sight":
                type = CheckType.Sight; break;
            case "Choice":
                type = CheckType.Choice; break;
            case "Touch":
                type = CheckType.Touch; break;
        }

        I = (float)DataManager.Instance.GetData(ID, "I", typeof(float));
        N = (float)DataManager.Instance.GetData(ID, "N", typeof(float));
        F = (float)DataManager.Instance.GetData(ID, "F", typeof(float));
        P = (float)DataManager.Instance.GetData(ID, "P", typeof(float));

    }
}
