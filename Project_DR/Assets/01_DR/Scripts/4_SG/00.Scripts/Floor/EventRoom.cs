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
        GameManager.isClearRoomList.Remove(isClearRoom);
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
        if(isFirst == true)
        {   // 유령 OBj 스폰
            GhostNpcSpawn();
        }
        else if (isFirst == false)
        {   // 해당층에 스폰이 가능한 NPC 랜덤 생성
            // 연계퀘스트 고려해서 제작해야함

        }
    }       // TryNpcSpawn()

    /// <summary>
    /// 유령 NPC를 스폰하는 함수
    /// </summary>
    private void GhostNpcSpawn()
    {
        int randIdx = UnityEngine.Random.Range(0, GameManager.instance.ghostObjList.Count);

        Vector3 spawnPos;
        float xPos = (meshPos.bottomLeftCorner.x + meshPos.bottomRightCorner.x) * 0.5f;
        float yPos = 0.5f;
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
        GameManager.isClearRoomList.Add(isClearRoom);
        TryNpcSpawn();
    }       // EventRoomStart()






}       // ClassEnd
