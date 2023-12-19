using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC_CanvasController : MonoBehaviour
{       // NPC의 Text ,RayHit판정 Image등 관련해서 조정할수 있게 해주는 Class

    private NPC npc;                        // 대화 창을 누르면 그떄에 가져올거임


    private StringBuilder stringBuilder;     // NPC의 선택지를 출력시켜줄 StringBuilder

    private TextMeshProUGUI npcNameText;         // Canvas상에서 출력될 이름의 TextMeshPro
    private TextMeshProUGUI npcTitleText;        // Canvas상에서 출력될 칭호의 TextMeshPro
    private TextMeshProUGUI npcConversationText; // Canvas상에서 출력될 대화의 TextMeshPro
    private TextMeshProUGUI choice1Text;         // 선택지 1의 TextMeshPro
    private TextMeshProUGUI choice2Text;         // 선택지 2의 TextMeshPro
    private TextMeshProUGUI choice3Text;         // 선택지 3의 TextMeshPro    

    private NPCUIImage dialogueImage;                // 대화창의 이미지
    private NPCUIImage choice1Image;                 // 선택지 1의 이미지
    private NPCUIImage choice2Image;                 // 선택지 2의 이미지
    private NPCUIImage choice3Image;                 // 선택지 3의 이미지

    private Color aZeroColor;         // 투명한 색
    private Color aMaxColor;          // 흰색

    private string isNon;                       // 해당 선택지가 존재하지 않는지 -> 존재하지 않으면 "0"으로 표시될것
    private string underBar;                    // Underbar도 선택지가 존재하지 않는다는 의미로 사용될것임

    private int nowConversationRefID;           // 현재 대화에서 참조 되고 있는 ID    
    // TODO : Ray의 판정을 위해 Image컴포넌트를 가져와야할수도 있음

    #region MonoBehaviour함수
    private void Awake()
    {
        AwakeInIt();
    }

    private void Start()
    {
        StartInIt();
        SubscriptionEvent();
    }

    #endregion MonoBehaviour함수

    #region 변수에 데이터 기입관련
    private void AwakeInIt()
    {       // 종속성이 없는 TextMeshPro의 컴포넌트를 Get해올것임
        stringBuilder = new StringBuilder();        
        isNon = "0";
        underBar = "_";

        aZeroColor = new Color(1, 1, 1, 0);
        aMaxColor = new Color(1, 1, 1, 1);

    }       // AwakeInIt()

    private void StartInIt()
    {
        npc = this.transform.parent.parent.GetComponent<NPC>();


        npcNameText = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        npcTitleText = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

        npcConversationText = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();

        choice1Text = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        choice2Text = transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();
        choice3Text = transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();

        dialogueImage = transform.GetChild(0).GetChild(2).GetComponent<NPCUIImage>();
        choice1Image = transform.GetChild(0).GetChild(3).GetComponent<NPCUIImage>();
        choice2Image = transform.GetChild(0).GetChild(4).GetComponent<NPCUIImage>();
        choice3Image = transform.GetChild(0).GetChild(5).GetComponent<NPCUIImage>();


    }       // StartInIt()



    #endregion 변수에 데이터 기입관련

    #region 색상관련
    /// <summary>
    /// 선택지의 컬러 A값 1로 변경
    /// </summary>
    public void ChoiceTextAColorOn(TextMeshProUGUI _outputText)
    {
        _outputText.color = aMaxColor;
    }       // ChoiceTextAColorOn()

    /// <summary>
    /// 선택지의 컬러 A값 0으로 변경
    /// </summary>
    public void ChoiceTextAColorOff(TextMeshProUGUI _outputText)
    {
        _outputText.color = aZeroColor;
    }       // ChoiceTextAColorOff()
    #endregion 색상관련

    #region 텍스트 출력관련

    /// <summary>
    /// NPC의 이름을 업데이트 하는 함수
    /// </summary>
    /// <param name="_name">NPC의 이름</param>
    public void NameUpdate(string _name)
    {
        npcNameText.text = _name;        
    }       // Name_TitleUpdate()


    /// <summary>
    /// NPC의 칭호를 업데이트 하는 함수
    /// </summary>
    /// <param name="_title"></param>
    public void TitleUpdate(string _title)
    {
        npcTitleText.text = _title;
    }       // TitleUpdate()

    /// <summary>
    /// 대사 출력해주는 함수
    /// </summary>
    /// <param name="_outputText"></param>
    public void OutPutConversation(string _outputText)
    {
        npcConversationText.text = _outputText;
    }       // OutPutConversation()

    #endregion 텍스트 출력관련

    #region 선택지관련
    /// <summary>
    /// 선택지 출력해주는 함수
    /// </summary>
    /// <param name="_converationRefID">출력해야하는 선택지가 존재하는 대사의 ID값</param>
    public void OutPutChoices(int _conversationRefID)
    {
        nowConversationRefID = _conversationRefID;

        // Choice1Event가 존재한다면 그것의 ID를 풀어내서 참조해야함(플레이어퀘스트 || 이후 영향이 줄것)
        string choice1Event = (string)DataManager.instance.GetData(_conversationRefID, "Choice1Event", typeof(string));

        string choice1 = (string)DataManager.instance.GetData(_conversationRefID, "Choice1", typeof(string));
        CheckChoiceNull(choice1, 1, choice1Text, choice1Image);

        string choice2 = (string)DataManager.instance.GetData(_conversationRefID, "Choice2", typeof(string));
        CheckChoiceNull(choice2, 2, choice2Text, choice2Image);

        // 아래 Choice3는 존재하면 띄우는 조건이 만족하는지 한번 체크해야함 (12.13기준 퀘스트가 나와야 클리어여부를 끌어와서 체크할수있음)
        string choice3 = (string)DataManager.instance.GetData(_conversationRefID, "Choice3", typeof(string));
        CheckChoiceNull(choice3, 3, choice3Text, choice3Image); // 임시사용 함수
        //CheckChoice3();

    }       // CheckExistChoices()

    /// <summary>
    /// 해당 선택지가 비어있는지 확인후 비버있지않으면 출력하는 함수
    /// </summary>    
    private void CheckChoiceNull(string _choice, int _choiceNum,
        TextMeshProUGUI _outputText, NPCUIImage _choiceImageComponent)
    {
        if (_choice == isNon || _choice == underBar)
        {
            ChoiceTextAColorOff(_outputText);
            _outputText.text = "";
            return;
        }
        else { /*DoNothing*/ }
        _choiceImageComponent.choiceNum = _choiceNum;
        ChoiceTextAColorOn(_outputText);
        _outputText.text = _choice;

    }       // CheckChoiceNull()


    /// <summary>
    /// 3번 선택지는 무조건 MBTI의 조건을 타기 때문에 조건확인
    /// </summary>
    private void CheckChoice3()
    {
        // TODO : 퀘스트클리어 여부를 확인 가능하다면 그떄에 체크해서 출력시키도록 해야함

    }       // CheckChoice3()



   
    private void ItCompensation(int _compensationRefId)
    {
        // 보상
        // 플레이어의 인벤토리에 보상 넣어주는 기능
    }


    private void ItQuest(int _questRefId)
    {
        // 퀘스트
        // 플레이어에게 퀘스트를 부여하는 기능
    }
    private void ItConveration(int _conversationRefId)
    {        
        // 다음 대사 출력하는 기능        
        //string conversationText = Data.GetString(_conversationRefId, "OutPutText");
        npc.EnQueueConversation(_conversationRefId);
        npc.DeQueueConversation();
    }       // ItConveration()


    /// <summary>
    /// 내부에 들어있는 값들을 전부 없애주는 함수
    /// </summary>
    private void ResetChoice()
    {
        choice1Text.text = "";
        choice2Text.text = "";
        choice3Text.text = "";
    }       // ResetChoice()

    #endregion 선택지관련

    #region 이벤트 관련
    private void SubscriptionEvent()
    {
        choice1Image.onHitEvent += CheckOnClickChoice;
        choice2Image.onHitEvent += CheckOnClickChoice;
        choice3Image.onHitEvent += CheckOnClickChoice;

        dialogueImage.onHitEvent += OnClickDialogueImage;
    }       // SubscriptionEvent()

    private void OnClickDialogueImage(NPCUIImage _npcUiImage)
    {
        if(npc == null || npc == default)
        {
            npc = this.transform.parent.parent.GetComponent<NPC>();
        }

        npc.DeQueueConversation();
    }       // OnClickDialogueImage()

    /// <summary>
    /// 플레이어가 누른 선택지를 확인하는 함수
    /// </summary>
    public void CheckOnClickChoice(NPCUIImage _choiceImage)
    {

        string choice = "Choice";
        string choiceNum = _choiceImage.choiceNum.ToString();
        string eventText = "Event";
        stringBuilder.Append(choice);
        stringBuilder.Append(choiceNum);
        stringBuilder.Append(eventText);

        string eventRefId = (string)DataManager.instance.GetData(nowConversationRefID, stringBuilder.ToString(), typeof(string));

        if (eventRefId == isNon || eventRefId == underBar)
        {
            // 이벤트가 없으므로 대화 끝
            NPC npc;
            npc = this.transform.parent.parent.GetComponent<NPC>();
            stringBuilder.Clear();
            ResetChoice();
            npc.InvokeEndConverationEvent();
            return;
        }

        // 이벤트가 존재한다면
        // 해당 이벤트 시트에서 가져오기 -> 참조의 ID 확인
        // -> 그 ID 값이 퀘스트인지 보상인지 구별하기 -> 퀘스트,보상 실행

        int[] eventRefIds = GFunc.SplitIds(eventRefId);

        int defaultCompensation = 320000;
        int defaultQuest = 3100000;
        int defaultConveration = 300000000;

        // 여기서 나온값이 퀘스트인지 보상인지 대사인지 확인할것임
        for (int i = 0; i < eventRefIds.Length; i++)
        {
            if (eventRefIds[i] >= defaultCompensation && eventRefId[i] < defaultQuest)
            {
                ItCompensation(eventRefIds[i]);
            }
            else if (eventRefIds[i] >= defaultQuest && eventRefIds[i] < defaultConveration)
            {
                ItQuest(eventRefIds[i]);
            }
            else if (eventRefIds[i] >= defaultConveration)
            {
                ItConveration(eventRefIds[i]);
            }
        }


    }       // CheckOnClickChoice()

    #endregion 이벤트 관련


}       // ClassEnd
