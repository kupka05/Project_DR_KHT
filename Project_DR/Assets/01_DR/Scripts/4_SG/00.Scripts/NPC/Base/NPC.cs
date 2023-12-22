using Rito.InventorySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public enum NPCID
{
    Olive = 1111201,
    Ghost_I_E = 1111202,
    Ghost_N_S = 1111203,
    Ghost_P_J = 1111204,
    Ghost_F_T = 1111205,
    Ghost_IE_FT = 1111206


}

public enum RewardID
{
    ItemStartId = 322001,
    ItemEndId = 322005,
    MBTIStartId = 324001,
    MBTIEndId = 324013
}

public enum NpcTriggerType
{
    Auto = 10,
    Trigger = 20
}

public enum NPCAnimationType
{
    Idle,
    Hi_1,
    Hi_2,
    Talk_1,
    Talk_2,
    Walk,
    Sleeping_1,
    Sleeping_2,
    Crying,
    Clap,
    Sneezing,
    Dance_2,
    Kill,
    HappyWalk,
    Bored,
    Loudly,
    Secretly,
    LALA
}

/// <summary>
/// ID값 비교할때 사용할 값
/// </summary>
public enum IdValue
{
    Compensation = 320000,
    Quest = 3100000,
    Dialogue = 30000000
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
    public Queue<string> converationText;  // NPC의 대사를 담아둘 Queue

    private Coroutine delayCoroutine;       // 코루틴 캐싱
    private WaitForSeconds delayTime;       // 딜레이 캐싱

    protected int npcID;                // NPC 스프레드시트 ID    -> 파생된 자식 클래스에서 결정
    protected int nowDialogueId;        // 현재 대화의 Id

    // 자식 클래스가 ParamsInIt()를 호출하면서 결정
    protected NpcType npcType;                    // NPC 타입을 시트값에 따라 지정해줄것임
    public NpcTriggerType npcTriggerType;      // NPC의 트리거 타입을 지정할 Enum 시트값에 따라서 할당될것임

    protected string npcName;               // NPC 이름
    protected StringBuilder npcTitle;              // NPC 칭호 (대화에 따라서 변화 할수 있기 떄문에 StringBuidler사용)   
    protected string npcWaitMotion;         // NPC 대기모션 (애니메이션에 사용)
    protected string npcConversationMotion; // NPC 대화모션 (애니메이션에 사용)
    protected string npcMoveMotion;         // NPC 이동모션 (애니메이션에 사용)     
    protected int[] conversationRefIDs;     // NPC 대화 참조할 테이블 ID -> inIt할때 초기화(선택지도 있기에 ID 필요)

    protected float npcHP;                  // NPC 체력
    protected float npcMoveSpeed;           // NPC 이동속도
    protected float npcConversationScope;   // NPC 대화범위
    protected float npcRecognitionRange;    // NPC 인식범위

    protected bool isTryCommunity;       // 대화를 한적 있는지?
    public bool isComunity;                 // 대화중인지 체크할 변수
    protected bool isCommunityDelray;       // 대화의 딜레이를 줄 함수 대화창 클릭이벤트에 관련있음
    public bool isReadyToAutoComunication;  // 자동으로 다가가서 일정거리 안에 있다면 true가 될것임




    #region 선언 이벤트
    protected delegate void StartConversationDelegate();
    protected event StartConversationDelegate StartConverationEvent; // StartConversationDelegate의 이벤트

    protected delegate void NextConverationDelegate(int _nextConverationId);
    protected event NextConverationDelegate NextConverationEvent;       // 다음대화가 존재한다면 NPC의 다음 대사가 나오도록하는 이벤트

    protected delegate void EndConverationDelegate();             // NPC와 대화가 끝나면 호출될 델리게이트
    protected event EndConverationDelegate EndConverationEvent;   // StopConversationDelegate의 이벤트

    // EventRoom의 SubscribeToNpcClearEvent함수 호출함
    public event System.Action isCommunityCompleateAction;     // NPC와의 대화가 끝나면 이 이벤트를 호출해서 문을 열도록 할것임



    #endregion 선언 이벤트

    #region MonoBehaviour함수
    protected virtual void Awake()
    {
        AwakeInIt();
    }
    protected virtual void OnDestroy()
    {
        // Debug.Log($"NPC 파괴됨 : {npcID}");

    }

    #endregion MonoBehaviour함수

    #region 변수,데이터 기입 관련

    private void AwakeInIt()
    {
        animator = this.transform.GetComponent<Animator>();
        npcStringBuilder = new StringBuilder();
        npcTitle = new StringBuilder();
        converationText = new Queue<string>();
        delayTime = new WaitForSeconds(0.5f);

        isTryCommunity = false;
        isReadyToAutoComunication = false;
        isCommunityDelray = false;

    }       // AwakeInIt()


    /// <summary>
    /// NPC들이 공통으로 가지고 있는 변수들 기입해주는 함수
    /// </summary>
    /// <param name="_npcID">해당 NPC의 ID</param>
    protected virtual void ParamsInIt(int _npcID)
    {
        //GFunc.Log($"시트값 가져오기위한 곳에 받아온 ID : {_npcID}");
        npcName = (string)DataManager.Instance.GetData(_npcID, "Name", typeof(string));
        //npcTitle = (string)DataManager.instance.GetData(_npcID, "Title", typeof(string)); // 대사 출력시 해당 대화의 칭호를 가져와서 출력하는식으로 변경
        npcWaitMotion = (string)DataManager.Instance.GetData(_npcID, "WaitMotion", typeof(string));
        npcConversationMotion = (string)DataManager.Instance.GetData(_npcID, "ConversationMotion", typeof(string));
        npcMoveMotion = (string)DataManager.Instance.GetData(_npcID, "MoveMotion", typeof(string));

        npcHP = (float)DataManager.Instance.GetData(_npcID, "HP", typeof(float));
        npcMoveSpeed = (float)DataManager.Instance.GetData(_npcID, "MoveSpeed", typeof(float));
        npcConversationScope = (float)DataManager.Instance.GetData(_npcID, "ConversationScope", typeof(float));
        npcRecognitionRange = (float)DataManager.Instance.GetData(_npcID, "RecognitionRange", typeof(float));



        NPCTriggerTypeInIt(_npcID);
        ConvertionRefIdInIt(_npcID);

    }       // ParamsInIt()

    private void NPCTriggerTypeInIt(int _npcID)
    {
        int value = Data.GetInt(_npcID, "interaction");

        if (value == (int)NpcTriggerType.Auto)
        {
            npcTriggerType = NpcTriggerType.Auto;

        }
        else if (value == (int)NpcTriggerType.Trigger)
        {
            npcTriggerType = NpcTriggerType.Trigger;

        }

    }       // NPCTriggerTypeInIt()

    protected virtual void TitleInIt(int _conversationID)
    {
        npcTitle.Clear();
        npcTitle.Append(Data.GetString(_conversationID, "Title"));
    }       // TitleInIt()

    /// <summary>
    /// 참조할 대화 ID들 기입하는 함수
    /// </summary>
    /// <param name="_npcID">해당 NPC의 ID값</param>
    private void ConvertionRefIdInIt(int _npcID)
    {

        string tableIDs = (string)DataManager.Instance.GetData(_npcID, "ConversationTableID", typeof(string));

        //GFunc.Log($"오류의 아이디 : {tableIDs}");
        conversationRefIDs = GFunc.SplitIds(tableIDs);

    }       // ConvertionRefIDInIt()

    /// <summary>
    /// Canvas의 스크립트와 GameObject를 GetComponent해오는 함수
    /// </summary>
    protected virtual void GetCanvasScript_Obj()
    {
        canvasObj = this.transform.GetChild(0).GetChild(0).gameObject;
        NpcCanvas = this.transform.GetChild(0).GetChild(0).GetComponent<NPC_CanvasController>();
        NpcCanvas.RewardEvent += RewardTypeCheck;
    }       // GetCanvasScript()



    #endregion 변수,데이터 기입 관련

    #region 이벤트 호출
    /// <summary>
    /// 대화 시작시 플레이어가 이 함수를 호출해서 대화 시작을 알릴거임
    /// </summary>
    public void InvokeStartConverationEvent()
    {
        if (npcTriggerType == NpcTriggerType.Trigger)
        {
            if (isTryCommunity == false)
            {
                isTryCommunity = true;
                //delayCoroutine = StartCoroutine(CommunicationDelay());
                StartConverationEvent?.Invoke();
            }
            else { /*PASS*/ }
        }

        else if (npcTriggerType == NpcTriggerType.Auto)
        {
            if (isReadyToAutoComunication == true && isComunity == false)
            {
                isComunity = true;
                StartConverationEvent?.Invoke();
            }
        }

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
        isCommunityCompleateAction?.Invoke();

    }       // EndConveration()

    #endregion 가상함수

    #region 대사관련 함수

    /// <summary>
    /// 대사들을 EnQueue해주는 함수
    /// </summary>
    /// <param name="_ComunicationTableId">대사테이블의 ID</param>
    public virtual void EnQueueConversation(int _comunicationTableId)
    {
        nowDialogueId = _comunicationTableId;
        string converationText = (string)DataManager.Instance.GetData(_comunicationTableId, "OutPutText", typeof(string));

        string[] splitTexts = GFunc.SplitConversation(converationText);

        this.npcTitle.Clear();
        this.npcTitle.Append(Data.GetString(_comunicationTableId, "Title"));
        NpcCanvas.TitleUpdate(npcTitle.ToString());

        for (int i = 0; i < splitTexts.Length; i++)
        {
            this.converationText.Enqueue(splitTexts[i]);
        }
    }       // EnQueueConveration()

    /// <summary>
    /// Queue의 텍스트를 TextMeshPro에 출력해주는 함수
    /// </summary>
    /// <param name="_outputText">출력할 대사</param>
    public virtual void DeQueueConversation()
    {
        if (converationText.Count > 0)
        {
            NpcCanvas.OutPutConversation(converationText.Dequeue());
        }
        else
        {
            // 선택지 출력해야함
            NpcCanvas.OutPutChoices(nowDialogueId);
        }
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

        //{---------------------------- 정해진 대사 이벤트 ------------------------------
        bool isPass = Inspection(conversationIds);

        if (isPass == false) { /*PASS*/ }

        //---------------------------- 정해진 대사 이벤트 ------------------------------ }


        //{---------------------------- 랜덤한 대사 이벤트 -------------------------------
        NpcRandomEventPick(_npcId);    // (선행조건이 있는것 제외)

        //---------------------------- 랜덤한 대사 이벤트 -------------------------------}

    }       // PickConverationEvent()


    /// <summary>
    /// 출력해야할 대사가 존재하는지 검사를 해주는 함수
    /// </summary>
    private bool Inspection(int[] _conversationIds) // return값이 존재해야할수도 있음
    {
        int havingDialogueID = InspectionClearQuest(_conversationIds);    // 해당 NPC에서 파생된 퀘스트중 클리어완료가능한 퀘스트가 존재하는지

        if (havingDialogueID != 0)
        {   // 0이 아니라면 위함수에서 완료한 퀘스트에 따른 출력해야하는 대사의 ID값
            CompleateQuest(havingDialogueID);
            return false;     // 완료한것이 있어서 위에서 대사넣고 다할것이니까 return
        }

        return true;
    }       // Inspection

    #region Inspection이 실행하는 함수
    /// <summary>
    /// 클리어 가능한 퀘스트가 해당 NPC의 전조퀘스트랑 같은것이 존재하는지 체크하는함수
    /// </summary>
    /// <param name="_conversationIds">해당NPC의 대사ID값들이 들어있는 int배열</param>
    /// <returns>완료한 퀘스트에 따른 출력해야하는 대사의 ID값</returns>
    private int InspectionClearQuest(int[] _conversationIds)
    {
        // 클리어조건 달성된 퀘스트가 존재하는지 확인
        // TODO

        //List<int> choiceQuestIdList = new List<int>();
        List<int> antecedentQuestIdList = new List<int>();      // NPC의 대사들의 선행퀘스트들을 담아둘 List
        List<int> conversationIdList = new List<int>();         // NPC의 선행 퀘스트 들이 존재하는 대사의 Id들을 모아둘 List

        StringBuilder stringBuilder = new StringBuilder();      // 비교할때 사용할 StringBuilder

        // 모든 선택지 수집해서 체크 -> 선행퀘스트가 완료가능한 퀘스트중에 중복된것이 있는지 체크

        for (int i = 0; i < _conversationIds.Length; i++)
        {
            stringBuilder.Clear();
            stringBuilder.Append(Data.GetString(_conversationIds[i], "AntecedentQuest"));
            stringBuilder.Replace("_", "");
            if (stringBuilder.ToString() != "0")
            {
                conversationIdList.Add(_conversationIds[i]);
                antecedentQuestIdList.Add(int.Parse(stringBuilder.ToString()));
            }
        }



        //foreach(/*완료가능한 퀘스트 List들*/)
        //{
        //    foreach(int questId in choiceQuestIdList)
        //    {
        //        if (/*완료 가능한 퀘스트 List의 현재 완료가능한 퀘스트값과 NPC 전조 퀘스트 값이 같다면*/)
        //        {
        //            foreach(int outId in conversationIdList)
        //             {
        //                  stringBuilder.Clear();
        //                  stringBuilder.Append(Data.GetString(outId,"AntecedentQuest"))
        //                  stringBuilder.Replace("_", "");
        //                 if(questId == int.Parse(stringBuidler.ToString()));
        //                  {
        //                      return int.Parse(stringBuidler.ToString())
        //                  }
        //             }        
        //        }
        //    }
        //}

        return 0;

    }       // InspectionClearQuest()

    /// <summary>
    /// 완료한 퀘스트에 대한 대사출력과 보상지급해주는 함수
    /// </summary>
    /// <param name="_havingToDialogueID">출력해야하는 대사의 ID값</param>
    private void CompleateQuest(int _havingToDialogueID)
    {       // 퀘스트 완료 대사 출력과 보상 지급해줘야함
        EnQueueConversation(_havingToDialogueID);   // 대사 넣기

        // TODO : 보상지급 해줘야함        
        DeQueueConversation(); // 대사출력
    }       // CompleateQuest()
    #endregion Inspection이 실행하는 함수

    /// <summary>
    /// NPC의 랜덤한 이벤트를 지정하는 함수
    /// </summary>
    private void NpcRandomEventPick(int _npcId)
    {
        string dialogueRefId = Data.GetString(_npcId, "ConversationTableID");   // NPC의 모든 대사ID 긁어오기

        int[] dialogueIds = GFunc.SplitIds(dialogueRefId);      // 긁어온 대사를 int형 배열에 기입

        List<int> passDialogueIdList = InspectionNoAntecedentQuest(dialogueIds);    // 전조퀘스트가 0인 대화들만 List에 반환

        int randIndex = UnityEngine.Random.Range(0, passDialogueIdList.Count);

        EnQueueConversation(passDialogueIdList[randIndex]);
        DeQueueConversation();
    }       // NpcRandomEventPick()

    /// <summary>
    /// 랜덤하게 결정되도 괜찮은(선행퀘스트가 없는) 대사만 반환하게 해주는 함수
    /// </summary>
    private List<int> InspectionNoAntecedentQuest(int[] _dialogueIds)
    {
        // 선행퀘스트 우선 확인 해야함 
        List<int> passDialogueList = new List<int>();

        for (int i = 0; i < _dialogueIds.Length; i++)
        {
            npcStringBuilder.Clear();
            npcStringBuilder.Append(Data.GetString(_dialogueIds[i], "AntecedentQuest"));
            if (npcStringBuilder.ToString() == "0")
            {   // 해당 Id의 전조 퀘스트가 0 == 없음 이라면 대화가능리스트에 추가
                passDialogueList.Add(_dialogueIds[i]);
            }
        }


        return passDialogueList;

    }       // InspectionConversationEvent()

    IEnumerator CommunicationDelay()
    {
        isCommunityDelray = true;
        yield return delayTime;
        isCommunityDelray = false;

    }       // CommunicationDelay()

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

    #region 보상 관련 함수

    /// <summary>
    /// 어떤 보상인지 체크하는 함수
    /// </summary>
    /// /// <param name="_rewardId">보상 ID</param>
    private void RewardTypeCheck(int _rewardId)
    {
        if((int)RewardID.ItemStartId <= _rewardId && (int)RewardID.ItemEndId >= _rewardId)
        {
            RewardItem(_rewardId);
        }
        else if((int)RewardID.MBTIStartId <= _rewardId && (int)RewardID.MBTIEndId >= _rewardId)
        {
            RewardMBTI(_rewardId);
        }
    }       // RewardTypeCheck()

    /// <summary>
    /// MBTI 보상을 지급하는 함수
    /// </summary>
    /// <param name="_rewardId">보상 ID</param>
    private void RewardMBTI(int _rewardId)
    {
        float i = Data.GetFloat(_rewardId, "MBTI_VALUE_I");
        float n = Data.GetFloat(_rewardId, "MBTI_VALUE_N");
        float f = Data.GetFloat(_rewardId, "MBTI_VALUE_F");
        float p = Data.GetFloat(_rewardId, "MBTI_VALUE_P");

        MBTI mbti = new MBTI();
        mbti.SetMBTI(i, n, f, p);
        //GFunc.Log($"새로 만든 MBTI값\nI : {i}\nN : {n}\nF : {f}\nP : {p}");

        MBTIManager.Instance.ResultMBTI(mbti);
    }       // RewardMBTI()

    /// <summary>
    /// 아이템을 지급하는 함수
    /// </summary>
    /// <param name="_rewardId">보상 ID</param>
    private void RewardItem(int _rewardId)
    {
        int rewardItemRefId = Data.GetInt(_rewardId, "Reward_1_KeyID");
        int rewardAmount = Data.GetInt(_rewardId, "Reward_1_Amount");
        // 잘하면 확률도 확인해야할수도 있음
        Unit.AddInventoryItem(rewardItemRefId, rewardAmount);     // 인벤토리에 아이템 넣어주기
    }       // RewardItem()

    #endregion 보상 관련 함수




    #region 애니메이션 관련
    /// <summary>
    /// 매개변수 값에 따라서 애니메이터에 존재하는 애니메이션 실행해주는 함수
    /// </summary>
    /// <param name="_playAnimationName">실행시킬 애니메이션 이름</param>
    protected void ChangeAnimationString(string _playAnimationName)
    {
        animator.Play(_playAnimationName);
        //Debug.Log($"NPC의 애니메이션 변경 함수 진입\n매개변수값 : {_playAnimationName}");
    }       // ChangeAnimationString()

    /// <summary>
    /// 애니메이션이 NPCAnimationType값에 따라 실행되는 함수 (사용 안함)
    /// </summary>
    /// <param name="_aniType">현재 NPC의 NPCAnimationType</param>
    protected void ChangeAnimationEnum(NPCAnimationType _aniType)
    {
        string aniname;     // 애니메이션 이름
        switch (_aniType)
        {
            case NPCAnimationType.Idle:
                aniname = "Ani_Motion_Idle";        // 아직 이름모름
                animator.Play(aniname);
                break;

            case NPCAnimationType.Walk:
                aniname = "Ani_Motion_Walking";
                animator.Play(aniname);
                break;

            case NPCAnimationType.Talk_1:
                aniname = "Ani_Motion_Talk_1";
                animator.Play(aniname);
                break;

            case NPCAnimationType.Talk_2:
                aniname = "Ani_Motion_Talk_2";
                animator.Play(aniname);
                break;

            case NPCAnimationType.Hi_1:
                aniname = "Ani_Motion_HI_1";
                animator.Play(aniname);
                break;
            case NPCAnimationType.Hi_2:
                aniname = "Ani_Motion_HI_2";
                animator.Play(aniname);
                break;

            case NPCAnimationType.Sleeping_1:
                aniname = "Ani_Motion_Sleeping_1";
                animator.Play(aniname);
                break;
            case NPCAnimationType.Sleeping_2:
                aniname = "Ani_Motion_Sleeping_2";
                animator.Play(aniname);
                break;

            case NPCAnimationType.Clap:
                aniname = "Ani_Motion_Clap";
                animator.Play(aniname);
                break;
            case NPCAnimationType.Sneezing:
                aniname = "Ani_Motion_Sneezing";
                animator.Play(aniname);
                break;
            case NPCAnimationType.Loudly:
                aniname = "Ani_Motion_Talk_Loudly";
                animator.Play(aniname);
                break;
            case NPCAnimationType.Dance_2:
                aniname = "Ani_Motion_DANCE_2";
                animator.Play(aniname);
                break;
            case NPCAnimationType.Kill:
                aniname = "Ani_Motion_Kill";
                animator.Play(aniname);
                break;
            case NPCAnimationType.Secretly:
                aniname = "Ani_Motion_Talk_Secretly";
                animator.Play(aniname);
                break;
            case NPCAnimationType.Bored:
                aniname = "Ani_Motion_Bored";
                animator.Play(aniname);
                break;
            case NPCAnimationType.HappyWalk:
                aniname = "Ani_Motion_Happy Walk";
                animator.Play(aniname);
                break;
            case NPCAnimationType.LALA:
                aniname = "Ani_Motion_LALA";
                animator.Play(aniname);
                break;
            case NPCAnimationType.Crying:
                aniname = "Ani_Motion_Crying";
                animator.Play(aniname);
                break;


        }
    }


    #endregion 애니메이션 관련

}           // ClassEnd
