using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    protected StringBuilder dialogue;   // NPC 대사 출력해줄 Sb
    protected GameObject canvasObj;     // NPC가 가지고 있는 Canvas를 키고 끌때 사용될 GameObject

    protected NPC_CanvasController NpcCanvas;       // NPC의 텍스트들을 관리해주는 스크립트

    protected int npcID;                // NPC 스프레드시트 ID       -> 파생된 자식 클래스에서 결정

    // 자식 클래스가 ParamsInIt()를 호출하면서 결정
    protected NpcType npcType;                    // NPC 타입을 시트값에 따라 지정해줄것임

    protected string npcName;               // NPC 이름
    protected string npcTitle;              // NPC 칭호    
    protected string npcWaitMotion;         // NPC 대기모션 (애니메이션에 사용)
    protected string npcConversationMotion; // NPC 대화모션 (애니메이션에 사용)
    protected string npcMoveMotion;         // NPC 이동모션 (애니메이션에 사용)    
    protected string[] conversationTexts;   // NPC 대화 텍스트들
    protected int[] conversationRefIDs;     // NPC 대화 참조할 테이블 ID -> inIt할때 초기화(선택지도 있기에 ID 필요)


    protected float npcHP;                  // NPC 체력
    protected float npcMoveSpeed;           // NPC 이동속도
    protected float npcConversationScope;   // NPC 대화범위
    protected float npcRecognitionRange;    // NPC 인식범위


    protected delegate void StartConversationDelegate();            // NPC와 대화를 시작하는지 체크할 델리게이트
    protected event StartConversationDelegate StartConverationEvent; // StartConversationDelegate의 이벤트

    protected delegate void NextConverationDelegate();              // 다음대화가 존재한다면 해당 이벤트를 불러서 NPC의 다음 대사가 나오도록하는 델리게이트
    protected event NextConverationDelegate NextConverationEvent;

    protected delegate void EndConverationDelegate();             // NPC와 대화가 끝나면 호출될 델리게이트
    protected event EndConverationDelegate EndConverationEvent;   // StopConversationDelegate의 이벤트

    protected delegate void ChoiceImageGetDelegate();           // 선택지 어떤거 선택했는지 확인할 델리게이트
    protected event ChoiceImageGetDelegate ChoiceImageGetEvent;     // 선택한것이 어떤것인지 확인하는 이벤트
    



    #region 사용할지 안할지 지켜봐야함
    private bool startConversation = false;
    protected bool StartComversation
    {
        get
        {
            return startConversation;
        }

        set
        {
            if(startConversation != value)
            {
                startConversation = value;
            }
        }
    }
    #endregion 사용할지 안할지 지켜봐야함





    protected virtual void Awake()
    {
        AwakeInIt();
    }

    private void AwakeInIt()
    {
        animator = GetComponent<Animator>();
        dialogue = new StringBuilder();        
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

        ConvertionTextInIt(_npcID);
        
    }       // ParamsInIt()




    /// <summary>
    /// string으로 연결되어있는 ID값들을 int 형 배열로 옮겨주는 함수
    /// </summary>
    /// <param name="_npcID">해당 NPC의 ID값</param>
    private void ConvertionTextInIt(int _npcID)
    {
        
        string tableIDs = (string)DataManager.instance.GetData(_npcID, "ConversationTableID", typeof(string));
        int[] conversationRefIDs;


        tableIDs = tableIDs.Replace("\\n","\n");
        string[] splits = new string[10];
        splits = tableIDs.Split('\n');

        conversationRefIDs = new int[splits.Length];

        for (int i = 0; i < splits.Length; i++) 
        {
            splits[i] = splits[i].Replace("_", "");
            conversationRefIDs[i] = int.Parse(splits[i]);
        }

        conversationTexts = new string[conversationRefIDs.Length];

        for (int i = 0; i < conversationTexts.Length; i++)
        {
            conversationTexts[i] = (string)DataManager.instance.GetData(conversationRefIDs[i], "OutPutText", typeof(string));
            conversationTexts[i] = conversationTexts[i].Replace("#", ",");
            conversationTexts[i] = conversationTexts[i].Replace("\\", "");
        }

    }       // ConvertionRefIDInIt()

    /// <summary>
    /// 해당 gameobjcet변수에 대립해주는 함수 
    /// </summary>
    protected virtual void GetCanvasObj()
    {
        canvasObj = this.transform.GetChild(0).GetChild(0).gameObject;
    }       // GetCanvasObj()


    /// <summary>
    /// Canvas의 스크립트를 GetComponent해오는 함수
    /// </summary>
    protected virtual void GetCanvasScript()
    {
        NpcCanvas = this.transform.GetChild(0).GetChild(0).GetComponent<NPC_CanvasController>();
    }       // GetCanvasScript()


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

    public void InvokeNextConverationEvent()
    {
        NextConverationEvent?.Invoke();
    }       // InvokeNextConverationEvent()

    /// <summary>
    /// 대화 시작시 돌아가야하는 로직 (빈함수)
    /// </summary>
    protected virtual void StartConvertion()
    {
        // 대화시작시 실행은 각각 다를예정
    }       // StartConvertion()

    protected virtual void NextConveration()
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

}           // ClassEnd
