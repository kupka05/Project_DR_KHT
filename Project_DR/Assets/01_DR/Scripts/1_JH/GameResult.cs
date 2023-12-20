using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 결과 정보를 가지고 있는 클래스
/// </summary>
[System.Serializable]
public class GameResult
{
    // 결과를 위한 리스트
    public MonsterResult monster;
    public List<Score> item;
    public List<Score> quest;

    // 최종 스코어
    public Score result;

    // 버프
    public float gainGold;
    public float gainExp;

    /// <summary>
    /// 스코어를 초기화 하는 메서드
    /// </summary>
    public void Initialize()
    {
        monster = new MonsterResult();
        item = new List<Score>();
        quest = new List<Score>();
        result = new Score();

        gainGold = 0;
        gainExp = 0;    
    }
    // 퀘스트 점수 추가
    public void AddQuestScore(int _gold, int _exp)
    {
        AddScore(quest, _gold, _exp);
    }
    // 아이템 점수 추가
    public void AddItemScore(int _gold, int _exp)
    {
        AddScore(item, _gold, _exp);
    }

    /// <summary>
    /// 아이템 및 퀘스트의 스코어를 추가하는 메서드
    /// </summary>
    /// <param name="list">추가할 아이템 및 퀘스트의 리스트</param>
    /// <param name="_gold">추가할 골드 값</param>
    /// <param name="_ext">추가할 경험치 값</param>
    /// 
    public void AddScore(List<Score> list, int _gold, int _exp)
    {
        Score score = new Score();
        score.gold = _gold;
        score.exp = _exp;

        list.Add(score);    // 리스트에 추가
    }

    // 몬스터의 결과를 추가하는 메서드들
    public void AddMonsterNormal(int _gold, int _exp)
    {
        monster.AddMonsterScore(monster.normal, _gold, _exp);
    }
    public void AddMonsterElite(int _gold, int _exp)
    {
        monster.AddMonsterScore(monster.elite, _gold, _exp);
    }
    public void AddMonsterBoss(int _gold, int _exp)
    {
        monster.AddMonsterScore(monster.boss, _gold, _exp);
    }

    /// <summary>
    /// 최종 골드를 계산하여 반환하는
    /// </summary>
    public int ResultGold()
    {
        int resultGold, monsterGold = 0, itemGold = 0 , questGold = 0;
        
        // 몬스터 골드 계산
        monsterGold = 
            monster.normal.gold + monster.elite.gold + monster.boss.gold;

        // 아이템 골드 계산
        for(int i = 0; i < item.Count; i++)
        {
            itemGold += item[i].gold;
        }

        // 퀘스트 골드 계산
        for(int i = 0; i < quest.Count; i++)
        {
            questGold += quest[i].gold;
        }
        resultGold = Mathf.RoundToInt((monsterGold + itemGold + questGold) * (1 * gainGold));

        return resultGold;
    }
    /// <summary>
    /// 최종 경험치를 계산하여 반환하는
    /// </summary>
    public int ResultExp()
    {
        int resultExp, monsterExp = 0, itemExp = 0, questExp = 0;

        // 몬스터 경험치 계산
        monsterExp =
            monster.normal.exp + monster.elite.exp + monster.boss.exp;

        // 아이템 경험치 계산
        for (int i = 0; i < item.Count; i++)
        {
            itemExp += item[i].exp;
        }

        // 퀘스트 경험치 계산
        for (int i = 0; i < quest.Count; i++)
        {
            questExp += quest[i].exp;
        }
        resultExp = Mathf.RoundToInt((monsterExp + itemExp + questExp) * (1 * gainExp));

        return resultExp;
    }


}

/// <summary>
/// 모든 몬스터 스코어
/// </summary>
[System.Serializable]
public class MonsterResult
{
    public MonsterScore normal = new MonsterScore();
    public MonsterScore elite = new MonsterScore();
    public MonsterScore boss = new MonsterScore();

    public void AddMonsterScore(MonsterScore score, int _gold, int _exp)
    {
        score.count += 1;
        score.gold += _gold;
        score.exp += _exp;
    }
}

/// <summary>
/// 몬스터 하위 스코어
/// </summary>
[System.Serializable]
public class MonsterScore
{
    public int count;
    public int gold;
    public int exp;


}
/// <summary>
/// 각 스코어를 담는 클래스
/// </summary>
[System.Serializable]
public class Score
{
    public string name;
    public int gold;
    public int exp;
}