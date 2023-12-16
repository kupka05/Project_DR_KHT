using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserData
{
    public static int GetExp()
    {
        return UserDataManager.Instance.Exp;
    }
    public static StatData GetStat()
    {
        return UserDataManager.Instance.statData;
    }

    // 골드 관리
    public static void AddGold(int value)
    {
        UserDataManager.Instance.Gold += value;
    }
    public static void SpendGold(int value)
    {
        UserDataManager.Instance.Gold -= value;
    }

    // 경험치 관리
    public static void AddExp(int value)
    {
        UserDataManager.Instance.Exp += value;
    }
    public static void SpendExp(int value)
    {
        UserDataManager.Instance.Exp -= value;
    }

    // 몬스터 처치시

    public static void KillMonster(int gold, int exp)
    {
        UserDataManager.Instance.result.AddMonsterNormal(gold, exp);
    }
    public static void KillElite(int gold, int exp)
    {
        UserDataManager.Instance.result.AddMonsterElite(gold, exp);
    }
    public static void KillBoss(int gold, int exp)
    {
        UserDataManager.Instance.result.AddMonsterBoss(gold, exp);
    }

    // 퀘스트 보상
    public static void AddQuestScore(int gold, int exp)
    {
        UserDataManager.Instance.result.AddQuestScore(gold, exp);
    }
    public static void AddItemScore(int gold, int exp)
    {
        UserDataManager.Instance.result.AddItemScore(gold, exp);
    }
}
