using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
public struct MBTI
{
    public float I;
    public float N;
    public float F;
    public float P;
    public MBTI SetMBTI(float i, float n, float f, float p)
    {
        I = i;
        N = n;
        F = f;
        P = p;
        return this;
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
    public float I;
    public float N;
    public float F;
    public float P;
    public TMP_Text debugTxt;


    // Start is called before the first frame update
    void Start()
    {
        GetData();
    }

    // Update is called once per frame
    void Update()
    {
        Debug();
    }

    public void Debug()
    {
        I = playerMBTI.I;
        N = playerMBTI.N;
        F = playerMBTI.F;
        P = playerMBTI.P;

        debugTxt.text = string.Format("I : " + I + ", N : " + N + ", F : " + F + ", P : " + P);
    }

    public void GetData()
    {
        
    }
    // 데이터를 서버에 업로드하는 부분
    public void SetData()
    {

    }


    // 각 MBTI의 값들을 더하거나 빼는 부분
    public void IncreaseI(float value)
    {
        Increase(playerMBTI.I += value);
    }
    public void DecreaseI(float value)
    {
        Decrease(playerMBTI.I -= value);
    }
    public void IncreaseN(float value)
    {
        Increase(playerMBTI.N += value);
    }
    public void DecreaseN(float value)
    {
        Decrease(playerMBTI.N -= value);
    }
    public void IncreaseF(float value)
    {
        Increase(playerMBTI.F += value);
    }
    public void DecreaseF(float value)
    {
        Decrease(playerMBTI.F -= value);
    }
    public void IncreaseP(float value)
    {
        Increase(playerMBTI.P += value);
    }
    public void DecreaseP(float value)
    {
        Decrease(playerMBTI.P -= value);
    }


    // 값 연산하는 부분
    private float Increase(float value)
    {
        return value < 100 ? value : 100;
    }
    private float Decrease(float value)
    {
        return value > 0 ? value : 0;
    }
}
