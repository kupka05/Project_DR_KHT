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
        playerMBTI.I = 50f;
        playerMBTI.N = 50f;
        playerMBTI.F = 50f;
        playerMBTI.P = 50f;
    }
    // 데이터를 서버에 업로드하는 부분
    public void SetData()
    {

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
    }

    // 각 값을 연산해주는 메서드
    float ResultValue(float preValue, float value)
    {
        UnityEngine.Debug.Log(preValue + ", " + value);

        if (value == 0)
        { return 0; }
        else if (0 < value)
        {
            return preValue + value <= 100 ? preValue + value : 100;
        }
        else
            return preValue + value >= 0 ? preValue + value : 0;
    }

}
