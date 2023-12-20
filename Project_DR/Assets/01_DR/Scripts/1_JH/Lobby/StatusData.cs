using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 스탯 데이터를 담을 클래스
[System.Serializable]
public class StatData
{
    public List<UpgradePC> upgradeHp;
    public List<UpgradePC> upgradeGainExp;
    public List<UpgradePC> upgradeGainGold;

    public List<UpgradeWeapon> upgradeAtk;
    public List<UpgradeWeapon> upgradeCrit;
    public List<UpgradeWeapon> upgradeCritDmg;
    public List<UpgradeWeapon> upgradeAtkSpd;

    public List<UpgradeSkill1> upgradeSkill1;
    public List<UpgradeSkill2> upgradeSkill2;
    public List<UpgradeSkill3> upgradeSkill3;
    public List<UpgradeSkill2> upgradeSkill4;
    

}

[System.Serializable]
public class UpgradePC
{
    public int level;       // 현재 레벨

    public float value;     // 증가값
    public float sum;       // 누적 

    public int exp;         // 소모 경험치
    public int totalExp;
}
[System.Serializable]
public class UpgradeWeapon
{
    public int level;       // 현재 레벨

    public float value1;
    public float sum1;

    public float value2;
    public float sum2;

    public float value3;
    public float sum3;

    public float value4;
    public float sum4;

    public int exp;         // 소모 경험치
    public int totalExp;
}
[System.Serializable]
public class UpgradeSkill
{
    public int level;

    public float value1;
    public float sum1;

    public float value2;
    public float sum2;

    public float value3;
    public float sum3;

    public int exp1;
    public int exp2;
    public int exp3;

    public int totalExp1;
    public int totalExp2;
    public int totalExp3;
}
[System.Serializable]
public class UpgradeSkill1
{
    public int level;

    public float value1;    // 기본 공격력
    public float sum1;

    public float value2;    // 드릴 크기 증가값
    public float sum2;

    public int exp1;
    public int totalExp1;

    public float value3;    // 지속 시간 증가값
    public float sum3;

    public int exp2;
    public int totalExp2;
}

[System.Serializable]
public class UpgradeSkill2
{
    public int level;

    public float value1;    // 치명타 확률 증가    // 사용 가능 횟수 추가
    public float sum1;

    public int exp1;
    public int totalExp1;

    public float value2;     // 치명타 데미지 증가  // 치명타 데미지 증가
    public float sum2;

    public int exp2;
    public int totalExp2;

    public float value3;    // 최대 지속 시간 증가값 // 넉백 거리 증가
    public float sum3;

    public int exp3;
    public int totalExp3;
}
[System.Serializable]
public class UpgradeSkill3
{
    public int level;

    public float value1;    // 받는 피해량 증가 1단계
    public float sum1;

    public float value2;     // 받는 피해량 증가 2단계
    public float sum2;

    public float value3;    // 받는 피해량 증가 3단계
    public float sum3;

    public int exp1;
    public int totalExp1;
}


public class StatusData
{
    // 스프레드 시트의 데이터 ID
    [Header("Data ID")]
    public int hpID = 901101;
    public int gainExpID = 901201;
    public int gainGoldID = 901301;

    public int atkID = 911101;
    public int critID = 911201;
    public int critDmgID = 911301;
    public int atkspd = 911401;

    public int skill1ID = 92001;
    public int skill2ID = 92101;
    public int skill3ID = 92201;
    public int skill4ID = 92301;

    [Header("Data")]
    public StatData data;           // 데이터를 모은 변수

    // 데이터 가져오기
    public StatData GetData(StatData _data)
    {
        _data.upgradeHp = GetPcData(hpID);
        _data.upgradeGainExp = GetPcData(gainExpID);
        _data.upgradeGainGold = GetPcData(gainGoldID);

        _data.upgradeAtk = GetWeaponData(atkID);
        _data.upgradeCrit = GetWeaponData(critID);
        _data.upgradeCritDmg = GetWeaponData(critDmgID);
        _data.upgradeAtkSpd = GetWeaponData(atkspd);

        _data.upgradeSkill1 = GetSkill1Data(skill1ID);
        _data.upgradeSkill2 = GetSkill2Data(skill2ID);
        _data.upgradeSkill3 = GetSkill3Data(skill3ID);
        _data.upgradeSkill4 = GetSkill4Data(skill4ID);

        return _data;
    }


    /// <summary> PC 업그레이드에 필요한 데이터를 가져오는 메서드 </summary>
    /// <param name="id">PC 업그레이드의 첫 ID</param>
    /// <returns>PC 업그레이드 데이터 리스트 반환</returns>        
    public List<UpgradePC> GetPcData(int id)
    {
        List<UpgradePC> list = new List<UpgradePC>();
        int total = 0;
        for (int i = 0; i < 10; i++)
        {
            UpgradePC newData = new UpgradePC();
            //GFunc.Log(id+i);

            newData.level = i + 1;
            newData.value = (float)DataManager.Instance.GetData(id + i, "Value1", typeof(float));
            newData.sum = (float)DataManager.Instance.GetData(id + i, "Value2", typeof(float));
            newData.exp = (int)DataManager.Instance.GetData(id + i, "EXP", typeof(int));
            total += newData.exp;
            newData.totalExp = total;
            list.Add(newData);
        }

        return list;
    }


    /// <summary> 무기 업그레이드에 필요한 데이터를 가져오는 메서드 </summary>
    /// <param name="id">무기 업그레이드의 첫 ID</param>
    /// <returns>무기 업그레이드 데이터 리스트 반환</returns>    
    public List<UpgradeWeapon> GetWeaponData(int id)
    {
        List<UpgradeWeapon> list = new List<UpgradeWeapon>();
        int total = 0;
        for (int i = 0; i < 10; i++)
        {
            UpgradeWeapon newData = new UpgradeWeapon();
            //GFunc.Log(id + i);

            newData.level = (int)DataManager.Instance.GetData(id + i, "LV", typeof(int));
            newData.value1 = (float)DataManager.Instance.GetData(id + i, "Value1", typeof(float));
            newData.sum1 = (float)DataManager.Instance.GetData(id + i, "Value2", typeof(float));
            newData.value2 = (float)DataManager.Instance.GetData(id + i, "Value3", typeof(float));
            newData.sum2 = (float)DataManager.Instance.GetData(id + i, "Value4", typeof(float));
            newData.value3 = (float)DataManager.Instance.GetData(id + i, "Value5", typeof(float));
            newData.sum3 = (float)DataManager.Instance.GetData(id + i, "Value6", typeof(float));
            newData.exp = (int)DataManager.Instance.GetData(id + i, "EXP", typeof(int));
            total += newData.exp;
            newData.totalExp = total;

            list.Add(newData);
        }
        return list;
    }

    /// <summary> 스킬 업그레이드에 필요한 데이터를 가져오는 메서드 </summary>
    /// <param name="id">스킬 업그레이드의 첫 ID</param>
    /// <returns>스킬 업그레이드 데이터 리스트 반환</returns>
    public List<UpgradeSkill> GetSkillData(int id)
    {
        List<UpgradeSkill> list = new List<UpgradeSkill>();
        int totalExp1 = 0, totalExp2 = 0, totalExp3 = 0;

        for(int i=0; i<10; i++)
        {
            UpgradeSkill newData = new UpgradeSkill();

            newData.level = Data.GetInt(id+i, "LV");

            newData.value1 = Data.GetFloat(id + i, "Value1");
            newData.sum1 = Data.GetFloat(id + i, "Sum1");
            newData.value2 = Data.GetFloat(id + i, "Value2");
            newData.sum2 = Data.GetFloat(id + i, "Sum2");
            newData.value3 = Data.GetFloat(id + i, "Value3");
            newData.sum3 = Data.GetFloat(id + i, "Sum3");

            newData.exp1 = Data.GetInt(id + i, "EXP1");
            newData.exp2 = Data.GetInt(id + i, "EXP2");
            newData.exp3 = Data.GetInt(id + i, "EXP3");

            totalExp1 += newData.exp1;
            totalExp2 += newData.exp2;
            totalExp3 += newData.exp3;

            newData.totalExp1 = totalExp1;
            newData.totalExp2 = totalExp2;
            newData.totalExp3 = totalExp3;

            list.Add(newData);
        }
        return list;
    }

    /// <summary> 스킬 1 업그레이드에 필요한 데이터를 가져오는 메서드 </summary>
    /// <param name="id">스킬 업그레이드의 첫 ID</param>
    /// <returns>스킬 업그레이드 데이터 리스트 반환</returns>
    public List<UpgradeSkill1> GetSkill1Data(int id)
    {
        List<UpgradeSkill1> list = new List<UpgradeSkill1>();
        int totalExp1 = 0, totalExp2 = 0;

        for (int i = 0; i < 10; i++)
        {
            UpgradeSkill1 newData = new UpgradeSkill1();

            newData.level = Data.GetInt(id + i, "LV");

            newData.value1 = Data.GetFloat(id + i, "Value1");
            newData.sum1 = Data.GetFloat(id + i, "Sum1");
            newData.value2 = Data.GetFloat(id + i, "Value2");
            newData.sum2 = Data.GetFloat(id + i, "Sum2");
            newData.value3 = Data.GetFloat(id + i, "Value3");
            newData.sum3 = Data.GetFloat(id + i, "Sum3");

            newData.exp1 = Data.GetInt(id + i, "EXP1");
            newData.exp2 = Data.GetInt(id + i, "EXP2");

            totalExp1 += newData.exp1;
            totalExp2 += newData.exp2;

            newData.totalExp1 = totalExp1;
            newData.totalExp2 = totalExp2;

            list.Add(newData);
        }
        return list;
    }

    /// <summary> 스킬 2 업그레이드에 필요한 데이터를 가져오는 메서드 </summary>
    /// <param name="id">스킬 업그레이드의 첫 ID</param>
    /// <returns>스킬 업그레이드 데이터 리스트 반환</returns>
    public List<UpgradeSkill2> GetSkill2Data(int id)
    {
        List<UpgradeSkill2> list = new List<UpgradeSkill2>();
        int totalExp1 = 0, totalExp2 = 0, totalExp3 = 0;

        for (int i = 0; i < 10; i++)
        {
            UpgradeSkill2 newData = new UpgradeSkill2();

            newData.level = Data.GetInt(id + i, "LV");

            newData.value1 = Data.GetFloat(id + i, "Value1");
            newData.sum1 = Data.GetFloat(id + i, "Sum1");
            newData.value2 = Data.GetFloat(id + i, "Value2");
            newData.sum2 = Data.GetFloat(id + i, "Sum2");
            newData.value3 = Data.GetFloat(id + i, "Value3");
            newData.sum3 = Data.GetFloat(id + i, "Sum3");

            newData.exp1 = Data.GetInt(id + i, "EXP1");
            newData.exp2 = Data.GetInt(id + i, "EXP2");
            newData.exp3 = Data.GetInt(id + i, "EXP3");

            totalExp1 += newData.exp1;
            totalExp2 += newData.exp2;
            totalExp3 += newData.exp3;

            newData.totalExp1 = totalExp1;
            newData.totalExp2 = totalExp2;
            newData.totalExp3 = totalExp3;

            list.Add(newData);
        }
        return list;
    }

    /// <summary> 스킬 3 업그레이드에 필요한 데이터를 가져오는 메서드 </summary>
    /// <param name="id">스킬 업그레이드의 첫 ID</param>
    /// <returns>스킬 업그레이드 데이터 리스트 반환</returns>
    public List<UpgradeSkill3> GetSkill3Data(int id)
    {
        List<UpgradeSkill3> list = new List<UpgradeSkill3>();
        int totalExp1 = 0;

        for (int i = 0; i < 10; i++)
        {
            UpgradeSkill3 newData = new UpgradeSkill3();

            newData.level = Data.GetInt(id + i, "LV");

            newData.value1 = Data.GetFloat(id + i, "Value1");
            newData.sum1 = Data.GetFloat(id + i, "Sum1");
            newData.value2 = Data.GetFloat(id + i, "Value2");
            newData.sum2 = Data.GetFloat(id + i, "Sum2");
            newData.value3 = Data.GetFloat(id + i, "Value3");
            newData.sum3 = Data.GetFloat(id + i, "Sum3");

            newData.exp1 = Data.GetInt(id + i, "EXP1");

            totalExp1 += newData.exp1;

            newData.totalExp1 = totalExp1;

            list.Add(newData);
        }
        return list;
    }

    /// <summary> 스킬 4 업그레이드에 필요한 데이터를 가져오는 메서드 </summary>
    /// <param name="id">스킬 업그레이드의 첫 ID</param>
    /// <returns>스킬 업그레이드 데이터 리스트 반환</returns>
    public List<UpgradeSkill2> GetSkill4Data(int id)
    {
        List<UpgradeSkill2> list = new List<UpgradeSkill2>();
        int totalExp1 = 0, totalExp2 = 0, totalExp3 = 0;

        for (int i = 0; i < 10; i++)
        {
            UpgradeSkill2 newData = new UpgradeSkill2();

            newData.level = Data.GetInt(id + i, "LV");

            // 2단계 강화만 존재
            if (i < 3)
            {
                newData.value1 = Data.GetFloat(id + i, "Value1");
                newData.sum1 = Data.GetFloat(id + i, "Sum1");
                newData.exp1 = Data.GetInt(id + i, "EXP1");
                totalExp1 += newData.exp1;
                newData.totalExp1 = totalExp1;
            }

            newData.value2 = Data.GetFloat(id + i, "Value2");
            newData.sum2 = Data.GetFloat(id + i, "Sum2");
            newData.value3 = Data.GetFloat(id + i, "Value3");
            newData.sum3 = Data.GetFloat(id + i, "Sum3");

            newData.exp2 = Data.GetInt(id + i, "EXP2");
            newData.exp3 = Data.GetInt(id + i, "EXP3");

            totalExp2 += newData.exp2;
            totalExp3 += newData.exp3;

            newData.totalExp2 = totalExp2;
            newData.totalExp3 = totalExp3;

            list.Add(newData);
        }
        return list;
    }

}
