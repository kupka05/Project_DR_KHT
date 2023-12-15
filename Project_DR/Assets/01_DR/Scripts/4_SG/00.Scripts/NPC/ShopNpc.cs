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
        GetCanvasScript_Obj();
        OnCanvasObj();
        
        ParamsInIt(npcID);
        NpcCanvas.Name_TitleUpdate(npcName, npcTitle);

        OffCanvasObj();

    }       // Start()


    private void AwakeInIt()
    {
        npcID = (int)NPCID.Olive;
        
    }       // AwakeInIt()

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
        StartConverationEvent += StartConvertion;
        NextConverationEvent += NextConveration;
        EndConverationEvent += EndConveration;

    }       // ConvertionEventInIt()

    /// <summary>
    /// 대화 시작
    /// </summary>
    protected override void StartConvertion()
    {
        OnCanvasObj();
        //OutPutShopText();     : 다시제작
    }       // StartConvertion()

    protected override void NextConveration(int _nextConverationId)
    {                
        OutPutPickText(_nextConverationId);

    }

    protected override void EndConveration()
    {
        OffCanvasObj();
    }       // EndConveration()


    private void OutPutPickText(int _refConverationId)
    {
        string texts = (string)DataManager.instance.GetData(_refConverationId, "OutPutText",typeof(string));
        

    }       // OutPutPickText()




}       // ClassEnd
