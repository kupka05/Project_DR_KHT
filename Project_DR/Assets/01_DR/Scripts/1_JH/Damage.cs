using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour 
{
    public static Damage instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Damage>();
            }

            return m_instance;
        }
    }
    private static Damage m_instance; // 싱글톤이 할당될 static 변수    

    private float critChance;           // 치명타 확률
    private float critIncrease;         // 치명타 배율

    // 스킬 액티브 여부
    public bool isTeradrill = false;    // 테라 드릴
    public bool isGrinder = false;      // 드릴 연마 
    public bool isLanding = false;      // 랜딩 스킬
    public bool isSmash = false;        // 분쇄 디버프

    // 효과
    private float teraIncrease;
    private float grinderIncrease;      // 드릴 연마 추가 데미지
    private float grinderCritChance;     // 드릴 연마 추가 치명타 확률
    private float landingIncrease;
    private float smashIncrease;



    // Start is called before the first frame update
    void Start()
    {
        GetData();
    }

    // 데미지 계산기
    public float DamageCalculate(float _damage)
    {
        grinderCritChance = isGrinder ? grinderCritChance : 0;
        grinderIncrease = isGrinder ? grinderIncrease : 0;
        landingIncrease = isLanding ? landingIncrease : 0;

        float val = Random.Range(0f, 100f);
        critIncrease = critChance + grinderCritChance <= val ? 0 : critIncrease ;

        //공격 계산식 = {기본 공격력*(1+테라드릴 증가)}*{1+(치명타 배율+드릴 연마 배율+랜딩 스킬 배율)}
        _damage *= (1 + teraIncrease) * (1 + (critIncrease + grinderIncrease + landingIncrease));

        // 최종 데미지
        float finaldamage = _damage * (1 + smashIncrease);

        return finaldamage;
    }

    void GetData()
    {
        critChance = (float)DataManager.instance.GetData(1100, "CritChance", typeof(float));
        critIncrease = (float)DataManager.instance.GetData(1100, "CritIncrease", typeof(float));
        teraIncrease = (float)DataManager.instance.GetData(20001, "Value1", typeof(float));
        grinderCritChance = (float)DataManager.instance.GetData(20015, "Value1", typeof(float));
        grinderIncrease = (float)DataManager.instance.GetData(20016, "Value1", typeof(float));
    }
}
