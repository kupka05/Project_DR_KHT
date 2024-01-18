using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Js.Quest;
using System.Text;
using System;
using Js.Boss;

public class BossNPC : NPC
{
    public enum BossLevel { Floor1, Floor2, Floor3, Floor4, Floor5 };

    public BossLevel boss;
    private int conversationID;

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    private void AwakeInIt()
    {
        switch(boss)
        {
            case BossLevel.Floor1:
                npcID = 6872;
                break;
            case BossLevel.Floor2:
                npcID = 6873;
                break;
            case BossLevel.Floor3:
                npcID = 6874;
                break;
            case BossLevel.Floor4:
                npcID = 6875;
                break;
            case BossLevel.Floor5:
                npcID = 6876;
                break;
        }

    }       // AwakeInIt()

    public void ChangeBossLevel(int level)
    {
        boss = (BossLevel)level;
        AwakeInIt();
    }

    protected override void Awake()
    {
        base.Awake();
        npcID = GetComponent<Old_Boss>().bossId;
        ConvertionEventInIt();
    }       // Awake()


    private void Start()
    {      
        GetCanvasScript_Obj();
        OnCanvasObj();

        ParamsInIt(npcID);
        NpcCanvas.NameUpdate(npcName);
        //ChangeAnimationString(npcWaitMotion);
        OffCanvasObj();

    }       // Start()


    #region Base
    protected override void ParamsInIt(int _npcID)
    {
        base.ParamsInIt(_npcID);
    }       // ParamsInIt()

    protected override void GetCanvasScript_Obj()
    {
        base.GetCanvasScript_Obj();
    }       // GetCanvasScript_Obj()
    #endregion Base

    /// <summary> NPC 베이스 스크립트에 이벤트 구독 </summary>
    private void ConvertionEventInIt()
    {
        //StartConverationEvent += StartConvertion;
        NextConverationEvent += NextConveration;
        EndConverationEvent += EndConveration;

    }       // ConvertionEventInIt()


    /// <summary> 보스와의 조우 </summary>
    public void BossMeet()
    {
        QuestCallback.OnBossMeetCallback(npcID);        // 상태값 갱신 및 자동 완료

        StartConvertion();                              // 완료 가능한 퀘스트 ID의 대화를 불러와 시작
    }


    /// <summary> 대화 시작 </summary>
    protected override void StartConvertion()
    {
        // 플레이어 화면에 UI 캔버스 붙여주기
        //if (Camera.main)
        //{
        //    canvasObj.transform.parent = Camera.main.transform;
        //    canvasObj.transform.localPosition = new Vector3(0,-0.38f,0.8f);
        //    canvasObj.transform.localRotation = Quaternion.identity;
        //}

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) 
        {
            canvasObj.transform.parent = player.transform.parent.GetChild(3);
            canvasObj.transform.localPosition = new Vector3(0f, 0.1f, 0.7f);
            canvasObj.transform.localRotation = Quaternion.identity;
            canvasObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }


        // 보스 컷씬 시작
        GameManager.instance.BossCutScene();
        
        // 캔버스 켜주기
        Invoke(nameof(OnCanvasObj), 1f);

        int questID = GetCanCompleteMainQuestID();                              // 완료 가능한 퀘스트 불러오기 
        int[] conversationIDs = Unit.ClearQuestByID(questID);                   // 완료 상태로 변경 & 보상 지급 & 선행퀘스트 조건이 있는 퀘스트들 조건 확인 후 시작가능으로 업데이트
        conversationID = conversationIDs[0];
        // npc 대화 선택
        PickConversation(conversationID);
    }       // StartConvertion()

    /// <summary> 다음 대사 출력할 때 호출 </summary>
    /// <param name="_nextConverationId">다음 대사의 ID</param>
    protected override void NextConveration(int _nextConverationId)
    {
        if (isCommunityDelray == false)
        {
            base.TitleInIt(_nextConverationId);
            NpcCanvas.TitleUpdate(npcTitle.ToString());
            DeQueueConversation();
        }
        else { /* PASS */ }
    }       // NextConveration()

    /// <summary> 대사끝날 때 호출 </summary>
    protected override void EndConveration()
    {
        base.EndConveration();

        OffCanvasObj();                              // 캔버스 끄기
        GameManager.instance.EndBossCutScene();      // 플레이어 페이드 아웃

        Old_Boss oldBoss = GetComponent<Old_Boss>();
        // 신 보스일 경우
        if (oldBoss.IsUseFunctionalityOnly)
        {
            GameManager.instance.isBossBattle = true;

            // 1초 후 공격 시작
            BossSummoningStone bossStone = GetComponent<BossSummoningStone>();
            bossStone.Boss.BossMonster.InvokeStartAttack(1f);
            transform.GetComponentInChildren<DialogDebug>().enabled = false;
        }

        // 아닐 경우(기존 보스일 경우) 
        else
        {
            // 1초 후 공격 시작
            Invoke(nameof(StartBossBattle), 1f);
        }          
    }       // EndConveration()

    // 전투 시작
    private void StartBossBattle()
    {
        GFunc.ChoiceEvent(conversationID);   // 대화 종료 후 대사 클리어 이벤트 진행중으로 변경

        GetComponent<Old_Boss>().StartAttack();  // 보스 전투 시작
        GameManager.instance.isBossBattle = true;

        transform.GetChild(0).gameObject.SetActive(false);
    }

    /// <summary> 대사 선정 </summary>
    public void PickConversation(int conversationId)
    {
        //// 테이블의 해당 conversationID를 가져와 분리
        //string conversation = Data.GetString(_npcId, "ConversationTableID");
        //int[] conversationIds = GFunc.SplitIds(conversation);

        //int conversationId = FindQuestConversationID(conversationIds);

        //if (conversationId == -1)
        //{
        //    GFunc.LogError(_npcId + "찾으려는 현재 진행중인 퀘스트 ID가 없습니다..");
        //    conversationID = conversationIds[0];
        //    return;
        //}
        //conversationID = conversationId;

        EnQueueConversation(conversationId);
        DeQueueConversation();
    }

    /// <summary> 현재 진행중인 퀘스트의 ID 를 찾는 함수  </summary>
    /// <param name="_conversationIds">찾고자 하는 ID 배열</param>
    /// <returns>퀘스트 ID</returns>
    public int FindQuestConversationID(int[] _conversationIds)
    {
        Quest curQuest = Unit.GetCanCompleteMainQuest();

        StringBuilder stringBuilder = new StringBuilder();      // 비교할때 사용할 StringBuilder

        for (int i = 0; i < _conversationIds.Length; i++)
        {
            stringBuilder.Clear();
            stringBuilder.Append(Data.GetString(_conversationIds[i], "AntecedentQuest"));
            stringBuilder.Replace("_", "");

            if (curQuest.QuestData.ID == int.Parse(stringBuilder.ToString()))
            {
                GFunc.Log("ID 발견 : " + curQuest.QuestData.ID);
                return _conversationIds[i];
            }
        }
        GFunc.Log("못찾은 퀘스트 ID: " + curQuest.QuestData.ID);
        return -1;
    }

    /// <summary>완료 가능 퀘스트 ID 반환</summary>
    public int GetCanCompleteMainQuestID()
    {
        Quest curQuest = Unit.GetCanCompleteMainQuest();
        int questID = curQuest.QuestData.ID;
        return questID;
    }




}       // ClassEnd
