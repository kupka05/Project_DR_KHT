using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[System.Serializable]
public struct MBTI
{
    public float I;
    public float N;
    public float F;
    public float P;
    public string mbti;
    public MBTI SetMBTI(float i, float n, float f, float p)
    {
        I = i;
        N = n;
        F = f;
        P = p;
        return this;
    }
    public string GetIE()
    {
        string i;
        if (I <= 50)
            i = "I";
        else
            i = "E";
        return i;
    }
    public string GetNS()
    {
        string n;
        if (N <= 50)
            n = "N";
        else
            n = "S";
        return n;
    }
    public string GetFT()
    {
        string f;
        if (F <= 50)
            f = "F";
        else
            f = "T";
        return f;
    }
    public string GetPJ()
    {
        string p;
        if (P <= 50)
            p = "P";
        else
            p = "J";
        return p;
    }

    public string GetMBTI()
    {         
        return GFunc.SumString(GetIE(), GetNS(), GetFT(), GetPJ()); ;
    }
}

public class MBTIManager : MonoBehaviour
{
    public static MBTIManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<MBTIManager>();
            }

            return m_Instance;
        }
    }


    private static MBTIManager m_Instance; // 싱글톤이 할당될 static 변수    


  
    private MBTI playerMBTI;

    [Header("PlayerMBTI")]
    public string mbti;

    public float I;
    public float N;
    public float F;
    public float P;
    public TMP_Text debugTxt;


    // Start is called before the first frame update
    void Start()
    {
        UserDataManager.Instance.DBRequst(GetData);
    }

    public void MbtiDebug()
    {
        if (debugTxt)
        {
            I = playerMBTI.I;
            N = playerMBTI.N;
            F = playerMBTI.F;
            P = playerMBTI.P;
            mbti = playerMBTI.GetMBTI();
            debugTxt.text = string.Format("I : " + I + ", N : " + N + ", F : " + F + ", P : " + P);
        }
    }

    public void GetData()
    {
        playerMBTI = UserDataManager.Instance.GetMBTI();
        I = playerMBTI.I;
        N = playerMBTI.N;
        F = playerMBTI.F;
        P = playerMBTI.P;        
    }
    // 데이터 전송
    public void SetData()
    {
        UserDataManager.Instance.SetMBTI(playerMBTI);
    }
    // MBTI를 계산하고 반환하는 메서드
    public void ResultMBTI(MBTI value)
    {
        playerMBTI.SetMBTI(
        ResultValue(I, value.I),
        ResultValue(N, value.N),
        ResultValue(F, value.F),
        ResultValue(P, value.P)
        );

        // 계산 이후 데이터 업데이트
        MbtiDebug();
        SetData();
    }

    // 각 값을 연산해주는 메서드
    float ResultValue(float preValue, float value)
    {
        //UnityEngine.GFunc.Log(preValue + ", " + value);

        if (value == 0)
        { return preValue; }
        else if (0 < value)
        {
            return preValue + value <= 100 ? preValue + value : 100;
        }
        else
            return preValue + value >= 0 ? preValue + value : 0;
    }

}
