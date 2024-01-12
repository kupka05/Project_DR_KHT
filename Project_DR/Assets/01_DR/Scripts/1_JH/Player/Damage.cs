using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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



    // 스킬 액티브 여부
    [Header ("Drill Skill Active")]
    public bool isTeradrill = false;    // 테라 드릴
    public bool isGrinder = false;      // 드릴 연마 
    public bool isSmash = false;        // 분쇄 디버프

    [Header("Critical")]
    public float critProbability;           // 치명타 확률
    public float critIncrease;         // 치명타 배율
    public float grinderCritChance;     // 드릴 연마 추가 치명타 확률

    // 효과
    [Header("Drill Skill Damage Increase")]
    public float teraIncrease;
    public float grinderIncrease;      // 드릴 연마 추가 데미지
    public float landingIncrease;
    public float smashIncrease;

    [Header("Skill Effect")]
    public float effectDmgIncrease;     // 효과에 따른 데미지 증감
    public float effectCritIncrease;    // 효과에 따른 치명타 데미지 증감





    // Start is called before the first frame update
    void Start()
    {
        UserData.GetData(GetData);
    }

    /// <summary>
    /// 데미지 계산기 튜플로 2가지 값 반환.
    /// </summary>
    /// <param name="_damage"></param>
    /// <param name="isLanding"></param>
    /// <returns>float : 데미지, bool 크리티컬 여부</returns>
    //
    public (float, bool) DamageCalculate(float _damage, bool isLanding = false)
    {
        float _teraIncrease = isTeradrill ? teraIncrease : 0;
        float _grinderCritChance = isGrinder ? grinderCritChance : 0;
        float _grinderIncrease = isGrinder ? grinderIncrease : 0;
        float _landingIncrease = isLanding ? landingIncrease : 0;
        bool isCrit = default;

        float val = isLanding ? -1 : Random.Range(0f, 100f);    // 랜딩스킬일 경우 값 -1, 아닐 경우 0 ~ 100확률

        //GFunc.Log("치명타 확률" + critChance +"+"+ grinderCritChance + "이번 확률 :" + val);
        float _critIncrease = critProbability + _grinderCritChance <= val ? 0 : critIncrease;   // 랜딩스킬일 경우 -1이기 때문에 무조건 치명타

        //공격 계산식 = {기본 공격력*(1+테라드릴 증가)}*{1+(치명타 배율+드릴 연마 배율+랜딩 스킬 배율)}

        _damage *= (1 + _teraIncrease + effectDmgIncrease) * (1 + (_critIncrease + _grinderIncrease + _landingIncrease + effectCritIncrease));

        // 크리티컬이면 트루
        if(_critIncrease != 0)
        {
            isCrit = true;
        }

        return (_damage, isCrit);
    }

    void GetData()
    {
        critProbability = UserData.GetCritChance();

        critIncrease = UserData.GetCritIncrease();
        teraIncrease = UserData.GetTeraIncrease();

        grinderIncrease = UserData.GetGinderIncrease();
        grinderCritChance = UserData.GetGrinderCritChance();

        landingIncrease = UserData.GetLandingCritIncrease();

        effectCritIncrease = UserData.GetEffectCritDamage();
        effectDmgIncrease = UserData.GetEffectDamage();

        //critChance = (float)DataManager.Instance.GetData(1100, "CritChance", typeof(float));      // 치명타 확률
        //critIncrease = (float)DataManager.Instance.GetData(1100, "CritIncrease", typeof(float));  // 치명타 증가율

        //teraIncrease = (float)DataManager.Instance.GetData(721100, "Value1", typeof(float));      // 테라드릴 증가율
        //grinderCritChance = (float)DataManager.Instance.GetData(721114, "Value1", typeof(float)); // 드릴연마 치확
        //grinderIncrease = (float)DataManager.Instance.GetData(721115, "Value1", typeof(float));    // 드릴연마 증가
        //landingIncrease = (float)DataManager.Instance.GetData(720217, "Value2", typeof(float));
    }

    // 데미지 추가 버프
    public void AddEffectDamage(float value)
    {
        effectDmgIncrease += value;
        UserDataManager.Instance.effectDamage = effectDmgIncrease;
    }

    // 치명타 데미지 추가 버프
    public void AddEffectCritDamage(float value)
    {
        effectCritIncrease += value;
        UserDataManager.Instance.effectCritDamage = effectCritIncrease;
    }
    public void AddEffectCritProbability(float value)
    {
        critProbability += value;
        UserDataManager.Instance.effectCritProbability = critProbability;
    }
}
