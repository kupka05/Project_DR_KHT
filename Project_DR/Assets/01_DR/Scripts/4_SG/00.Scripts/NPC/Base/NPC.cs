using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public enum NPCID
{
    Olive = 11_1_12_01
}
public class NPC : MonoBehaviour
{    
    
    protected enum NpcType  // -> NPC의 타입을 나눌 enum 
    {
        Announcement = 0,
        Human = 1,
        Object = 2,
        Monster = 3
    }       // enumType

    protected Animator animator;        // NPC 애니메이터

    protected StringBuilder npcStringBuilder;   // NPC 대사 출력해줄 Sb
    protected GameObject canvasObj;     // NPC가 가지고 있는 Canvas를 키고 끌때 사용될 GameObject
    protected NPC_CanvasController NpcCanvas;       // NPC의 텍스트들을 관리해주는 스크립트
    private Queue<string> converationText;  // NPC의 대사를 담아둘 Queue

    protected int npcID;                // NPC 스프레드시트 ID       -> 파생된 자식 클래스에서 결정

    // 자식 클래스가 ParamsInIt()를 호출하면서 결정
    protected NpcType npcType;                    // NPC 타입을 시트값에 따라 지정해줄것임

    protected string npcName;               // NPC 이름
    protected string npcTitle;              // NPC 칭호    
    protected string npcWaitMotion;         // NPC 대기모션 (애니메이션에 사용)
    protected string npcConversationMotion; // NPC 대화모션 (애니메이션에 사용)
    protected string npcMoveMotion;         // NPC 이동모션 (애니메이션에 사용)        
    protected int[] conversationRefIDs;     // NPC 대화 참조할 테이블 ID -> inIt할때 초기화(선택지도 있기에 ID 필요)


    protected float npcHP;                  // NPC 체력
    protected float npcMoveSpeed;           // NPC 이동속도
    protected float npcConversationScope;   // NPC 대화범위
    protected float npcRecognitionRange;    // NPC 인식범위

    #region 선언 이벤트
    protected delegate void StartConversationDelegate();            
    protected event StartConversationDelegate StartConverationEvent; // StartConversationDelegate의 이벤트

    protected delegate void NextConverationDelegate(int _nextConverationId);              
    protected event NextConverationDelegate NextConverationEvent;       // 다음대화가 존재한다면 NPC의 다음 대사가 나오도록하는 이벤트

    protected delegate void EndConverationDelegate();             // NPC와 대화가 끝나면 호출될 델리게이트
    protected event EndConverationDelegate EndConverationEvent;   // StopConversationDelegate의 이벤트

    protected delegate void ChoiceImageGetDelegate();           // 선택지 어떤거 선택했는지 확인할 델리게이트
    protected event ChoiceImageGetDelegate ChoiceImageGetEvent;     // 선택한것이 어떤것인지 확인하는 이벤트

    #endregion 선언 이벤트

    #region MonoBehaviour함수
    protected virtual void Awake()
    {
        AwakeInIt();
    }

    #endregion MonoBehaviour함수

    #region 변수,데이터 기입 관련

    private void AwakeInIt()
    {
        animator = GetComponent<Animator>();
        npcStringBuilder = new StringBuilder();        
    }       // AwakeInIt()


    /// <summary>
    /// NPC들이 공통으로 가지고 있는 변수들 기입해주는 함수
    /// </summary>
    /// <param name="_npcID">해당 NPC의 ID</param>
    protected virtual void ParamsInIt(int _npcID)
    {
        npcName = (string)DataManager.instance.GetData(_npcID, "Name", typeof(string));
        npcTitle = (string)DataManager.instance.GetData(_npcID, "Title", typeof(string));
        npcWaitMotion = (string)DataManager.instance.GetData(_npcID, "WaitMotion", typeof(string));
        npcConversationMotion = (string)DataManager.instance.GetData(_npcID, "ConversationMotion", typeof(string));
        npcMoveMotion = (string)DataManager.instance.GetData(_npcID, "MoveMotion", typeof(string));

        npcHP = (float)DataManager.instance.GetData(_npcID, "HP", typeof(float));
        npcMoveSpeed = (float)DataManager.instance.GetData(_npcID, "MoveSpeed", typeof(float));
        npcConversationScope = (float)DataManager.instance.GetData(_npcID, "ConversationScope", typeof(float));
        npcRecognitionRange = (float)DataManager.instance.GetData(_npcID, "RecognitionRange", typeof(float));

        ConvertionRefIdInIt(_npcID);        

    }       // ParamsInIt()

    /// <summary>
    /// 참조할 대화 ID들 기입하는 함수
    /// </summary>
    /// <param name="_npcID">해당 NPC의 ID값</param>
    private void ConvertionRefIdInIt(int _npcID)
    {

        string tableIDs = (string)DataManager.instance.GetData(_npcID, "ConversationTableID", typeof(string));

        conversationRefIDs = GFunc.SplitIds(tableIDs);

    }       // ConvertionRefIDInIt()

    /// <summary>
    /// Canvas의 스크립트와 GameObject를 GetComponent해오는 함수
    /// </summary>
    protected virtual void GetCanvasScript_Obj()
    {
        canvasObj = this.transform.GetChild(0).GetChild(0).gameObject;
        NpcCanvas = this.transform.GetChild(0).GetChild(0).GetComponent<NPC_CanvasController>();
    }       // GetCanvasScript()

   

    #endregion 변수,데이터 기입 관련

    #region 이벤트 호출
    /// <summary>
    /// 대화 시작시 플레이어가 이 함수를 호출해서 대화 시작을 알릴거임
    /// </summary>
    public void InvokeStartConverationEvent()
    {
        StartConverationEvent?.Invoke();
    }       // InvokeConvertionEvent()

    public void InvokeEndConverationEvent()
    {
        EndConverationEvent?.Invoke();
    }       // InvokeEndConverationEvent()

    public void InvokeNextConverationEvent(int _nextConverationId)
    {
        NextConverationEvent?.Invoke(_nextConverationId);
    }       // InvokeNextConverationEvent()
    #endregion 이벤트 호출

    #region 가상함수
    
    /// <summary>
    /// 대화 시작시 돌아가야하는 로직 (빈함수)
    /// </summary>
    protected virtual void StartConvertion()
    {
        // 대화시작시 실행은 각각 다를예정
    }       // StartConvertion()

    protected virtual void NextConveration(int _nextConverationId)
    {
        // 이건 Start이후 진행중에 필요하면 호출될 함수
    }       // NextConveration()

    /// <summary>
    /// 대화가 끝날때에 돌아가야할 로직 (빈함수)
    /// </summary>
    protected virtual void EndConveration()
    {
        // NPC들의 Canvas를 SetActive = false 해야할듯

    }       // EndConveration()

    #endregion 가상함수

    #region 대사관련 함수

    /// <summary>
    /// 대사들을 EnQueue해주는 함수
    /// </summary>
    /// <param name="_ComunicationTableId"></param>
    protected virtual void EnQueueConversation(int _comunicationTableId)
    {
        string converationText = (string)DataManager.instance.GetData(_comunicationTableId, "OutPutText", typeof(string));

        string[] splitTexts = GFunc.SplitConversation(converationText);

        for (int i = 0; i < splitTexts.Length; i++)
        {
            this.converationText.Enqueue(splitTexts[i]);
        }
    }       // EnQueueConveration()

    /// <summary>
    /// Queue의 텍스트를 TextMeshPro에 출력해주는 함수
    /// </summary>
    /// <param name="_outputText">출력할 대사</param>
    protected virtual void DeQueueConversation(string _outputText)
    {
        NpcCanvas.OutPutConversation(_outputText);
    }       // DeQueueConveration()
    //Conversation

    /// <summary>
    /// 어떤대사이벤트가 발생할지 정하는 함수
    /// </summary>
    /// <param name="_npcId">해당 NPC의 ID값</param>
    protected virtual void PickConversationEvent(int _npcId)
    {
        // 참조할 대사 정함 -> 그거의 선조 퀘스트가 있는지 확인 -> 있다면 현재 클리어상태인지 확인 ->
        // 클리어됬으면 출력 -> 안됐으면 다시돌리기
        //  위 조건에서 중복으로 나올 가능성이 있기에 재귀 하더라도 Queue나 Array에 넣어서 한정된 값으로 돌도록해야할거같음
        //      GFunc의 활용을 위해 Array로 채택

        string conversation = Data.GetString(_npcId, "ConversationTableID");

        int[] conversationIds = GFunc.SplitIds(conversation);

        InspectionConversationEvent(conversationIds);


    }       // PickConverationEvent()

    /// <summary>
    /// 해당 대화가 선행퀘스트가 존재하는지 확인하는 함수
    /// </summary>
    private void InspectionConversationEvent(int[] _conversationIds)
    {
       // 선행퀘스트 우선 확인 해야함 
       
    }       // InspectionConversationEvent()

    #endregion 대사관련 함수

    #region Canvas.SetActive 관련
    /// <summary>
    /// CanvasOBj의 SetActive = true
    /// </summary>
    protected void OnCanvasObj()
    {
        canvasObj.SetActive(true);
    }       // OnCanvasObj()


    /// <summary>
    /// CanvasOBj의 SetActive = false
    /// </summary>
    protected void OffCanvasObj()
    {
        canvasObj.SetActive(false);
    }       // OffCanvasObj()

    #endregion Canvas.SetActive 관련

    

}           // ClassEnd
