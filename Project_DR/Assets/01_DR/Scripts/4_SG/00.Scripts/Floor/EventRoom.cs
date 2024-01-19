using Js.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventRoom : RandomRoom
{       // EventRoomClass는 스프레드시트의 값을 받아와서 랜덤한 이벤트 발생을 시켜줄 Class

    //  meshPos : 해당 변수로 자신의 방 꼭지점을 가져올수 있음
    

    private bool isFirst;       // 해당 플레이어가 0클리어인지 확인 -> NPC 소환의 영향을 줄것임

    private List<GameObject> spawnNPCList;  // 해당 방이 스폰한NPC들의 리스트  
    private GameObject npcParent;           // NPC를 관리해줄 ParentObj

    private void Awake()
    {
        spawnNPCList = new List<GameObject>();
        npcParent = new GameObject("NPC_Parent");
        npcParent.transform.parent = this.transform;
    }

    void Start()
    {
        StartCoroutine(EventRoomStart());       // 5초뒤에 NPC를 생성함 던전 재 생성 이슈 방지
        GetFloorPos();      // 꼭지점 가져와주는 Class

    }       // Start()

    private void OnDestroy()
    {
        GameManager.isClearRoomList.Remove(this);
        StopAllCoroutines();        // 예의치 못한 코루틴 으로 인한 이슈 방지
    }       // OnDestroy()

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

    /// <summary>
    /// NPC를 스폰하는 함수
    /// </summary>
    private void TryNpcSpawn()
    {
        //Test();       // 2회차 소환이 잘되는지 확인하기 위한 함수

        if (isFirst == true)
        {   // 유령 OBj 스폰
            GhostNpcSpawn();
        }
        else if (isFirst == false)
        {   // 해당층에 스폰이 가능한 NPC 랜덤 생성
            // 연계퀘스트 고려해서 제작해야함
            List<int> spawnNPCIds = CheckHavingSpawnNpc();

            if (spawnNPCIds.Count != 0)
            {
                HavingNpcSpawn(spawnNPCIds);
            }
            else
            {
                RandomNPCSpawn();
            }

        }

    }       // TryNpcSpawn()
    private void Test()
    {
        List<int> spawnNPCIds = CheckHavingSpawnNpc();

        if (spawnNPCIds.Count != 0)
        {
            HavingNpcSpawn(spawnNPCIds);
        }
        else
        {
            RandomNPCSpawn();
        }
    }

    /// <summary>
    /// 소환해야 하는 NPC가 존재하는지 체크하는 함수
    /// </summary>
    /// <returns>소환해야할 NPC의 ID가 담겨있는 List</returns>
    private List<int> CheckHavingSpawnNpc()
    {
        int[] npcIds = (int[])Enum.GetValues(typeof(NPCID));  // Enum의 모든 ID값들을 넣어줌

        List<int> findQuestIdList = new List<int>();    // 완료가능 퀘스트중에 NPC 대화 퀘스트 들만 넣어둔 List
        List<int> findNPCIds = new List<int>();         // 스폰해야하는 ID들을 넣어줄거임
        // npc의 대사마다 선행퀘스트를 뒤져보기 -> 완료 가능한 퀘스트 리스트를 뒤져보기 ->
        // 같은 ID값이 존재하면 List에 Add해주기
        List<Quest> compleateSubQestList = Unit.GetCanCompleteSubQuestForList();

        // NPC와 대화하는 퀘스트가 완료가능인지 확인
        foreach(Quest tempQuest in compleateSubQestList)
        {
            if(tempQuest.QuestData.Condition == QuestData.ConditionType.NPC_TALK)
            {
                GFunc.Log($"NPC소환 로직중에서 NPC 대화 퀘스트 완료를 감지하고 해당 퀘스트 ID 넣음\n퀘스트 ID : {tempQuest.QuestData.ID}");
                findQuestIdList.Add(tempQuest.QuestData.ID);
            }
        }

        for (int i = 0; i < npcIds.Length; i++)
        {
            string dialogueStr = Data.GetString(npcIds[i], "ConversationTableID");
            int[] dialogueIds = GFunc.SplitIds(dialogueStr);

            for(int j = 0; j < dialogueIds.Length; j++)
            {
                string antecedentQuestID = Data.GetString(dialogueIds[j], "AntecedentQuest");
                antecedentQuestID = GFunc.RemoveUnderbar(antecedentQuestID);
                //GFunc.Log($"언더바가 잘 제거 되었는가? : {antecedentQuestID}");
                
                for(int k = 0; k < findQuestIdList.Count; k++)
                {
                    if(antecedentQuestID == findQuestIdList[k].ToString())
                    {
                        GFunc.Log($"소환해야할 NPC를 찾음");
                        findNPCIds.Add(npcIds[i]);
                    }
                }

            }
        }       // 지옥 -> 나중에 리펙토링 해야할 필요성이 느껴짐



        return findNPCIds;
    }

    /// <summary>
    /// 소환해야하는 NPC를 소환하는 함수
    /// </summary>
    /// <param name="_spawnNPCId">소환해야하는 NPC의 ID가 담겨있는 List</param>
    private void HavingNpcSpawn(List<int> _spawnNPCId)
    {
        Vector3 spawnPos;
        float xPos = (meshPos.bottomLeftCorner.x + meshPos.bottomRightCorner.x) * 0.5f;
        float yPos = 1.5f;
        float zPos = (meshPos.bottomLeftCorner.z + meshPos.topLeftCorner.z) * 0.5f;
        spawnPos = new Vector3(xPos, yPos, zPos);

        GameObject npcOriginal = Resources.Load<GameObject>(Data.GetString(_spawnNPCId[0], "PrefabName"));

        GameObject npcClone = Instantiate(npcOriginal, spawnPos, Quaternion.identity, npcParent.transform);
        

        spawnNPCList.Add(npcClone);
        SubscribeToNpcClearEvent(npcClone);
    }       // HavingNpcSpawn()

    /// <summary>
    /// 랜덤한 NPC 소환해주는 함수
    /// </summary>
    private void RandomNPCSpawn()
    {
        int[] npcIds = (int[])Enum.GetValues(typeof(NPCID));

        int randIdx = UnityEngine.Random.Range(0, npcIds.Length -1);

        GFunc.Log($"가져온 Prefab 이름 : {Data.GetString(npcIds[randIdx], "PrefabName")}");
        string prefabName = Data.GetString(npcIds[randIdx], "PrefabName");
        GameObject originalPrefab = Resources.Load<GameObject>(prefabName);


        Vector3 spawnPos;
        float xPos = (meshPos.bottomLeftCorner.x + meshPos.bottomRightCorner.x) * 0.5f;
        float yPos = 1.5f;
        float zPos = (meshPos.bottomLeftCorner.z + meshPos.topLeftCorner.z) * 0.5f;
        spawnPos = new Vector3(xPos, yPos, zPos);

        GameObject npcClone = Instantiate(originalPrefab, spawnPos, Quaternion.identity, npcParent.transform);

        spawnNPCList.Add(npcClone);
        SubscribeToNpcClearEvent(npcClone);

    }       // RandomNPCSpawn()

    /// <summary>
    /// 유령 NPC를 스폰하는 함수
    /// </summary>
    private void GhostNpcSpawn()
    {
        int randIdx = UnityEngine.Random.Range(0, GameManager.instance.ghostObjList.Count);

        Vector3 spawnPos;
        float xPos = (meshPos.bottomLeftCorner.x + meshPos.bottomRightCorner.x) * 0.5f;
        float yPos = 1.5f;
        float zPos = (meshPos.bottomLeftCorner.z + meshPos.topLeftCorner.z) * 0.5f;
        spawnPos = new Vector3(xPos, yPos, zPos);
        
        GameObject pickNpc = GameManager.instance.ghostObjList[randIdx];

        GameObject npcClone = Instantiate(pickNpc, spawnPos, Quaternion.identity,npcParent.transform);

        GameManager.instance.ghostObjList.RemoveAt(randIdx);
        
        spawnNPCList.Add(npcClone);
        SubscribeToNpcClearEvent(npcClone);
    }       // GhostNpcPick()





    /// <summary>
    /// 스폰한 NPC의 대화끝날때 불러주는 이벤트 구독하는 함수
    /// </summary>
    /// <param name="_spawnNpc">스폰된 NPC</param>
    private void SubscribeToNpcClearEvent(GameObject _spawnNpc)
    {
        NPC npc = _spawnNpc.GetComponent<NPC>();

        npc.isCommunityCompleateAction += IsCompleateCommunication;

    }       // SubscribeToNpcClearEvent()

    /// <summary>
    /// 해당 방에서 NPC와 대화가 끝났을때 호출할 함수
    /// </summary>
    public void IsCompleateCommunication()
    {
        ClearRoomBoolSetTrue();     // 문열기        
    }       // IsCompleateCommunication()


    /// <summary>
    /// 해당 플레이어가 처음 클리어하는지체크하고 유령을 List에 넣은적이 있는지 체크하는함수
    /// </summary>
    private void CheckFirstInIt()
    {
        if (GameManager.instance.isInItGhost == false)
        {
            isFirst = true;
            GameManager.instance.AllocatedGhostObj();
        }

        else if (GameManager.instance.isInItGhost == true && UserDataManager.Instance.ClearCount == 0)
        {
            isFirst = true;
        }
        else { isFirst = false; }        
    }       // CheckFirstInIt()


    IEnumerator EventRoomStart()
    {
        yield return new WaitForSeconds(5f);

        CheckFirstInIt();
        GameManager.isClearRoomList.Add(this);
        TryNpcSpawn();
    }       // EventRoomStart()






}       // ClassEnd
