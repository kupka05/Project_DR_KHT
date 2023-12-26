using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Js.Quest;
using System.Text;


public class BossNPC : NPC
{

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    private void AwakeInIt()
    {
        npcID = 6872;

    }       // AwakeInIt()

    protected override void Awake()
    {
        base.Awake();
        AwakeInIt();
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

    /// <summary>
    /// NPC 베이스 스크립트에 이벤트 구독
    /// </summary>
    private void ConvertionEventInIt()
    {
        //StartConverationEvent += StartConvertion;
        NextConverationEvent += NextConveration;
        EndConverationEvent += EndConveration;

    }       // ConvertionEventInIt()

    /// <summary>
    /// 대화 시작
    /// </summary>
    protected override void StartConvertion()
    {
        if (Camera.main)
        {
            canvasObj.transform.parent = Camera.main.transform;
            canvasObj.transform.localPosition = new Vector3(0.2f,-0.35f,1.2f);
            canvasObj.transform.localRotation = Quaternion.identity;
        }

        OnCanvasObj();
        PickConversation(npcID);
        //base.PickConversationEvent(npcID);
    }       // StartConvertion()

    /// <summary>
    /// 다음 대사 출력할때 호출
    /// </summary>
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
        //OutPutPickText(_nextConverationId);
    }       // NextConveration()

    /// <summary>
    /// 대사끝날때 호출
    /// </summary>
    protected override void EndConveration()
    {
        //ChangeAnimationString(npcWaitMotion);
        base.EndConveration();
        OffCanvasObj();
        GFunc.Log("전투를 시작한다.");

        GetComponent<Boss>().StartAttack();
        
    }       // EndConveration()


    public void BossMeet()
    {
        GFunc.Log("대화를 시작한다.");
        StartConvertion();
    }


    public void PickConversation(int _npcId)
    {
        // 테이블의 해당 conversationID를 가져와 분리
        string conversation = Data.GetString(_npcId, "ConversationTableID");
        int[] conversationIds = GFunc.SplitIds(conversation);

        // 디버그용
        //Unit.ChangeQuestStateToInProgress(31_1_1_001);

        int conversationId = FindQuestConversationID(conversationIds);

        if (conversationId == -1)
        {
            GFunc.Log("찾으려는 현재 진행중인 퀘스트 ID가 없습니다.");
            return;
        }
        GFunc.Log(conversationId);

        EnQueueConversation(conversationId);
        DeQueueConversation();
    }

    /// <summary>
    /// 현재 진행중인 퀘스트의 ID 를 찾는 함수
    /// </summary>
    /// <param name="_conversationIds">찾고자 하는 ID 배열</param>
    /// <returns></returns>
    public int FindQuestConversationID(int[] _conversationIds)
    {
        Quest curQuest = Unit.GetInProgressMainQuest();
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
        return -1;
    }



}       // ClassEnd
