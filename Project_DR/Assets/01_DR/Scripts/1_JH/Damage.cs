using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour 
{
    

    private float critChance;           // 치명타 확률
    private float critIncrease;         // 치명타 배율

    private float teraIncrease;
    private float grinderIncrease;
    private float landingIncrease;
    private float smashIncrease;

    public bool isTeradrill = false;    // 테라 드릴
    public bool isGrinder = false;      // 드릴 연마 
    public bool isLanding = false;      // 랜딩 스킬
    public bool isSmash = false;        // 분쇄 디버프

    // Start is called before the first frame update
    void Start()
    {
        GetData();
    }

    public float DamageCalculate(float _damage)
    {
        float val = Random.Range(0f, 100f);
        critIncrease = critChance <= val ? 0 : critIncrease ;
        grinderIncrease = isGrinder ? 0 : grinderIncrease ;
        landingIncrease = isLanding ? 0 : landingIncrease ;

        //공격 계산식 = {기본 공격력*(1+테라드릴 증가)}*{1+(치명타 배율+드릴 연마 배율+랜딩 스킬 배율)}
        _damage *= (1 + teraIncrease) * (1 + (critIncrease + grinderIncrease + landingIncrease));

        // 최종 데미지

        float finaldamage = _damage * (1 + smashIncrease);

        return finaldamage;
    }

    void GetData()
    {
      
    }
}
