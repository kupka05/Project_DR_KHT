using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (instance == null)
            {
                // 생성 후 할당
                GameObject obj = new GameObject("GameManager");
                instance = obj.AddComponent<NPCManager>();
            }

            // 싱글톤 오브젝트를 반환
            return instance;
        }
    }
    private static NPCManager instance; // 싱글톤이 할당될 static 변수    
    private void Awake()
    {
        if (instance == null || instance == default)
        {
            instance = this;
        }
        else { /*PASS*/ }

    }       // Awake()

    /// <summary>
    /// 매개변수로 넣어준 확률을 넣어서 해당 확률에 당첨되면 true 아니면 false를 반환해주는 함수
    /// </summary>
    /// <param name="_getProbability">얻을확률</param>
    /// <param name="_minProbability">(Default : 0%)랜덤의 최소값</param>
    /// <param name="_maxProbability">(Default : 100%)랜덤의 최대값</param>
    /// <returns>확률이 당첨되었는지 당첨되지 않았는지</returns>
    public bool GetProbabilityResult(int _getProbability, int _minProbability = 0, int _maxProbability = 100)
    {
        _maxProbability += 1;       // Range의 마지막은 제외되기 때문에 내부에서 1을 더해주고 돌릴것임        

        int randValue = Random.Range(_minProbability, _maxProbability);

        if (_getProbability < randValue)
        {
            return false;
        }
        else
        {
            return true;
        }

    }       // GetProbabilityResult()

    /// <summary>
    /// 보상아이디를 읽고 해당 키의 보상 키 ID가 존재할시 리스트에 담아서 반환해주는 함수
    /// </summary>
    /// <param name="_rewardId">참조할 보상의 ID</param>
    /// <returns>보상의 ID가 담겨져있는 List</returns>
    public List<int> GetRewardItemRefIdList(int _rewardId)
    {
        StringBuilder stringBuilder = new StringBuilder();
        List<int> rewardItemRefIdList = new List<int>();

        string reward = "Reward_";  // 여러번 사용가능
        string keyId = "_KeyID";

        for (int i = 1; i < 5; i++)
        {
            if (i == 5) { continue; }   // 혹시모를 예외처리
            stringBuilder.Clear();
            stringBuilder.Append(reward);
            stringBuilder.Append(i.ToString());
            stringBuilder.Append(keyId);
             
            int rewardKeyId = Data.GetInt(_rewardId, stringBuilder.ToString());

            if (rewardKeyId != 0)
            {
                rewardItemRefIdList.Add(rewardKeyId);
            }
        }

        return rewardItemRefIdList;
    }       // GetRewardItemRefIdList()

    /// <summary>
    /// 보상아이디를 읽고 해당 키보상의 갯수가 존재할경우 리스트에 담아서 반환해주는 함수
    /// </summary>
    /// <param name="_rewardId">참조할 보상의 ID</param>
    /// <returns>보상의 갯수가 담겨져있는 List</returns>
    public List<int> GetRewardAmountList(int _rewardId)
    {
        StringBuilder stringBuilder = new StringBuilder();
        List<int> rewardAmountList = new List<int>();

        string reward = "Reward_";  // 여러번 사용가능
        string amount = "_Amount";

        for (int i = 1; i < 5; i++)
        {
            if (i == 5) { continue; }   // 혹시모를 예외처리
            stringBuilder.Clear();
            stringBuilder.Append(reward);
            stringBuilder.Append(i.ToString());
            stringBuilder.Append(amount);
            
            int rewardAmount = Data.GetInt(_rewardId, stringBuilder.ToString());

            if (rewardAmount != 0)
            {
                rewardAmountList.Add(rewardAmount);
            }
        }

        return rewardAmountList;
    }       // GetRewardAmountList()

    /// <summary>
    /// 보상아이디를 읽고 해당 보상의 확률이 존재한다면 리스트에 담아서 반환해주는 함수
    /// </summary>
    /// <param name="_rewardId">참조할 보상의 ID</param>
    /// <returns>학률이 담겨져있는 리스트</returns>
    public List<int> GetRewardProbabilityList(int _rewardId)
    {
        StringBuilder stringBuilder = new StringBuilder();
        List<int> rewardProbabilityList = new List<int>();

        string reward = "Reward_"; 
        string probability = "_Probability";

        for (int i = 1; i < 5; i++)
        {
            if (i == 5) { continue; }   // 혹시모를 예외처리
            stringBuilder.Clear();
            stringBuilder.Append(reward);
            stringBuilder.Append(i.ToString());
            stringBuilder.Append(probability);
             
            int rewardProbability = Data.GetInt(_rewardId, stringBuilder.ToString());

            if (rewardProbability != 0)
            {
                rewardProbabilityList.Add(rewardProbability);
            }
        }

        return rewardProbabilityList;

    }       // GetRewardProbabilityList()


}       // ClassEnd
