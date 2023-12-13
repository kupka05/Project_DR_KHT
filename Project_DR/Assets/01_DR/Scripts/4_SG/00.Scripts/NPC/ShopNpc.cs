using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNpc : AnnouncementNPC
{       // 상점 NPC의 Class
    
    protected override void Awake()
    {
        base.Awake();
        AwakeInIt();
        ConvertionEventInIt();
    }       // Awake()


    private void Start()
    {
        GetCanvasObj();
        OnCanvasObj();

        GetCanvasScript();
        ParamsInIt(npcID);
        NpcCanvas.Name_TitleUpdate(npcName, npcTitle);

        OffCanvasObj();

    }       // Start()


    private void AwakeInIt()
    {
        npcID = (int)NPCID.Olive;
        
    }       // AwakeInIt()

    protected override void ParamsInIt(int _npcID)
    {
        base.ParamsInIt(_npcID);
    }       // ParamsInIt()

    protected override void GetCanvasScript()
    {
        base.GetCanvasScript();
    }       // GetCanvasScript()

    protected override void GetCanvasObj()
    {
        base.GetCanvasObj();
    }       // GetCanvasObj()


    /// <summary>
    /// NPC 베이스 스크립트에 이벤트 구독
    /// </summary>
    private void ConvertionEventInIt()
    {
        StartConvertionEvent += StartConvertion;
    }       // ConvertionEventInIt()

    /// <summary>
    /// 대화 시작
    /// </summary>
    protected override void StartConvertion()
    {
        OnCanvasObj();
        OutPutShopText();
    }       // StartConvertion()

    private void OutPutShopText()
    {   // 6개의 대사중 랜덤으로 대사가 출력되는 함수
        int randIndex = UnityEngine.Random.Range(0, conversationRefIDs.Length);

        string outputConversation = conversationTexts[randIndex];
        


        // 이아래는 대사 출력 -> 확인이미지 에서 누른것체크 -> 다음행동(다음대사 || 대화창 종료)
        NpcCanvas.OutPutConversations(outputConversation);      // 대사출력

        NpcCanvas.OutPutChoices(conversationRefIDs[randIndex]); // 선택지를 출력해주는 함수

    }       // OutPutShopText()




}       // ClassEnd
