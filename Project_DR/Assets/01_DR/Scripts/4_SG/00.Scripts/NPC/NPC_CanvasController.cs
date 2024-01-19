using Js.Quest;
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
    private string endTalk;                     // \\n을 의미하며 대화를 끝내는 용도로 사용

    private int nowConversationRefID;           // 현재 대화에서 참조 되고 있는 ID    

    /// <summary>
    /// 선택지 중복 출력을 방지하기 위한 bool값 -> EnQueue됄때 false 될거임
    /// </summary>
    public bool isOutPutChoice;                 

    public event System.Action<int> RewardEvent;        // 보상 지급하라고 NPC에게 알려주는 이벤트

    #region MonoBehaviour함수
    private void Awake()
    {
        AwakeInIt();
        CanvasGetComponents();
    }

    private void Start()
    {
        //StartInIt();
        SubscriptionEvent();
    }

    #endregion MonoBehaviour함수

    #region 변수에 데이터 기입관련
    private void AwakeInIt()
    {       // 종속성이 없는 TextMeshPro의 컴포넌트를 Get해올것임
        stringBuilder = new StringBuilder();
        isNon = "0";
        underBar = "_";
        endTalk = "\\\\n0";
        //GFunc.Log($"endTalk : {endTalk}");
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

    private void CanvasGetComponents()
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
    }       // CanvasGetComponents()



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
        if(isOutPutChoice == true)
        { return; }
        else { /*PASS*/ }

        //GFunc.Log($"선택지 출력해주는 함수 진입 : OutPutChoices(NPC_CanvasController_Class)");
        isOutPutChoice = true;
        nowConversationRefID = _conversationRefID;
        // Choice1Event가 존재한다면 그것의 ID를 풀어내서 참조해야함(플레이어퀘스트 || 이후 영향이 줄것)
        string choice1Event = Data.GetString(_conversationRefID, "Choice1Event");

        string choice1 = Data.GetString(_conversationRefID, "Choice1");
        string choice2 = Data.GetString(_conversationRefID, "Choice2");
        string choice3 = Data.GetString(_conversationRefID, "Choice3");

        choice1 = GFunc.ReplaceString(choice1);
        choice2 = GFunc.ReplaceString(choice2);
        choice3 = GFunc.ReplaceString(choice3);
        //GFunc.Log($"선택지 시트에서 가져오기 시도\nChoice1 : {choice1}\nChoice2 : {choice2}\nChoice3 : {choice3}");
        // 아래 Choice3는 존재하면 띄우는 조건이 만족하는지 한번 체크해야함 (12.13기준 퀘스트가 나와야 클리어여부를 끌어와서 체크할수있음)

        CheckChoiceNull(choice1, 1, choice1Text, choice1Image);
        CheckChoiceNull(choice2, 2, choice2Text, choice2Image);
        CheckChoiceNull(choice3, 3, choice3Text, choice3Image); // 임시사용 함수


        //CheckChoice3();

    }       // CheckExistChoices()

    /// <summary>
    /// 해당 선택지가 비어있는지 확인후 비버있지않으면 출력하는 함수
    /// </summary>    
    private void CheckChoiceNull(string _choice, int _choiceNum,
        TextMeshProUGUI _outputText, NPCUIImage _choiceImageComponent)
    {
        //if(_choiceNum == 2)
        //{
        //    //GFunc.Log($"Choice : {_choice}");
        //}
        //GFunc.Log($"선택지가 비어있는지 확인하는 함수 진입 : CheckChoiceNull(NPC_CanvasController_Class)  \n해당 선택지의 값 : {_choice} 가 들어왔음");
        //GFunc.Log($"여기 들어오나? 들어온다면 어떤 값을 가지고 있지?\n_choice :{_choice}\n_ChoiceNum : {_choiceNum}");
        if (_choice == isNon || _choice == underBar)
        {
            ChoiceTextAColorOff(_outputText);
            _outputText.text = "";
            return;
        }
        else { /*DoNothing*/ }

        bool isChoiceNum3Pass = true;       // 3번 선택지만 검사들어가고 불통이라면 출력이 안됨
        if (_choiceNum == 3)
        {
            isChoiceNum3Pass = CheckChoice3();
        }

        if (isChoiceNum3Pass == true)
        {
            _choiceImageComponent.choiceNum = _choiceNum;
            ChoiceTextAColorOn(_outputText);
            _outputText.text = _choice;
        }

    }       // CheckChoiceNull()


    /// <summary>
    /// 3번 선택지는 무조건 MBTI의 조건을 타기 때문에 조건확인
    /// </summary>
    private bool CheckChoice3()
    {
        //GFunc.Log($"3번 선택지 조건이 맞는지 확인하는 함수 진입 : CheckChoice3 (NPC_CanvasConroller_Class)");

        // TODO : 퀘스트클리어 여부를 확인 가능하다면 그떄에 체크해서 출력시키도록 해야함
        // 아래 받아온 string값을 int[], string [] 2개로 나누어야함                                 
        bool isPass = true;

        string choice3ConditionMBTIs = Data.GetString(nowConversationRefID, "Choice3ConditionMBTI");

        string choice3ConditionValues = Data.GetString(nowConversationRefID, "Choice3ConditionValue");

        string[] arrChoice3ConditionMBTI = GFunc.SplitConversation(choice3ConditionMBTIs);

        //GFunc.Log($"NPCTutis의 문제의 변수값 : {choice3ConditionValues}\n현재 대화 ID : {npc.nowDialogueId} ");
        float[] arrChoice3ConditionValue = GFunc.SplitFloats(choice3ConditionValues);

        //GFunc.Log($"arrLength : {arrChoice3ConditionMBTI.Length}"); // 조건 1개짜리 결과 1
        bool[] checkArray = new bool[arrChoice3ConditionValue.Length];

        for (int i = 0; i < arrChoice3ConditionMBTI.Length; i++)
        {
            string findMBTI = FindConditionMBTI(arrChoice3ConditionMBTI[i]);
            //checkArray[i] = CheckMBTIValue(arrChoice3ConditionMBTI[i], arrChoice3ConditionValue[i]);
            checkArray[i] = CheckMBTIValue(findMBTI, arrChoice3ConditionValue[i]);
        }

        for (int i = 0; i < checkArray.Length; i++)
        {
            if (checkArray[i] == false)
            {
                isPass = false;
            }
        }

        return isPass;

    }       // CheckChoice3()


    /// <summary>
    /// 해당 MBTI가 뭔지 확인하는 함수
    /// </summary>
    /// <param name="_compareMBTI">잘린 배열의 i번쨰의 변수</param>
    /// <returns></returns>
    private string FindConditionMBTI(string _compareMBTI)
    {
        if (_compareMBTI == "I")
        {
            return "I";
        }
        else if (_compareMBTI == "E")
        {
            return "E";
        }
        else if (_compareMBTI == "N")
        {
            return "N";
        }
        else if (_compareMBTI == "S")
        {
            return "S";
        }
        else if (_compareMBTI == "F")
        {
            return "F";
        }
        else if (_compareMBTI == "T")
        {
            return "T";
        }
        else if (_compareMBTI == "P")
        {
            return "P";
        }
        else
        {
            return "J";
        }

    }       // FindConditionMBTI()


    /// <summary>
    /// 매개변수로 받은 것에 따라서 값을 비교후 bool값을 리턴해주는 함수
    /// </summary>
    /// <param name="_mbti">비교될 MBTI</param>
    /// <param name="_value">비교할 값</param>
    /// <returns></returns>
    private bool CheckMBTIValue(string _mbti, float _value)
    {
        GFunc.Log($"검사하기 위해 들어온 매개변수 값 : {_mbti}");
        if (_mbti == "I")
        {
            GFunc.Log($"조건에 들어옴 Value : {_value}\n현재 MBTI I 값 : {UserDataManager.Instance.mbti.I}");
            if (_value <= UserDataManager.Instance.mbti.I)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (_mbti == "E")
        {
            if (_value >= UserDataManager.Instance.mbti.I)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else if (_mbti == "N")
        {
            if (_value <= UserDataManager.Instance.mbti.N)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (_mbti == "S")
        {
            if (_value >= UserDataManager.Instance.mbti.N)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else if (_mbti == "F")
        {
            if (_value <= UserDataManager.Instance.mbti.F)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (_mbti == "T")
        {
            if (_value >= UserDataManager.Instance.mbti.F)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (_mbti == "P")
        {
            if (_value <= UserDataManager.Instance.mbti.P)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {       // "J"
            if (_value >= UserDataManager.Instance.mbti.P)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }        // CheckMBTIValue()


    private void ItCompensation(int _compensationRefId)
    {
        // 보상
        // 플레이어의 인벤토리에 보상 넣어주는 기능
        //GFunc.Log($"보상 ID : {_compensationRefId} 가져왔음");

        RewardEvent?.Invoke(_compensationRefId);
    }


    private void ItQuest(int _questRefId)
    {
        // 퀘스트
        // 플레이어에게 퀘스트를 부여하는 기능
        Unit.InProgressQuestByID(_questRefId);
        GFunc.Log($"Unit.InProgressQuestByID호출 : {_questRefId} 를 진행중에 넣었음");


    }       // ItQuest()
    private void ItConveration(int _conversationRefId)
    {
        // 다음 대사 출력하는 기능        
        //string conversationText = Data.GetString(_conversationRefId, "OutPutText");
        //GFunc.Log($"나 다음대사 추가후 출력함\n참조ID : {_conversationRefId}");
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
        if (npc == null || npc == default)
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
        isOutPutChoice = false;
        string choice = "Choice";
        string choiceNum = _choiceImage.choiceNum.ToString();
        string eventText = "Event";
        stringBuilder.Clear();
        stringBuilder.Append(choice);
        stringBuilder.Append(choiceNum);
        stringBuilder.Append(eventText);

        //Debug.Log($"String Builder가 카테고리 참조할때에 어떤 값을 가지고 있지? : {stringBuilder.ToString()}");
        if (nowConversationRefID == 0)
        {
            ResetChoice();
            return;
        }
        else { /*PASS*/ }


        string eventRefId = (string)DataManager.Instance.GetData(nowConversationRefID, stringBuilder.ToString(), typeof(string));
        //GFunc.Log($"StBuild의 값 : {stringBuilder}\n참조해온 선택지 이벤트 값 : {eventRefId}");
        if (eventRefId == isNon || eventRefId == underBar || eventRefId == endTalk)
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

        // GFunc.Log($"에러부분 ->  eventRefId : {eventRefId}\n nowConversationRefID : {nowConversationRefID}");

        if(eventRefId == null)  // 지환 : 예외처리
        { return; }

        int[] eventRefIds = GFunc.SplitIds(eventRefId);

        int defaultCompensation = 320000;
        int defaultQuest = 3100000;
        int defaultConveration = 30000000;

        #region 작은수 -> 큰수  LEGACY
        // 여기서 나온값이 퀘스트인지 보상인지 대사인지 확인할것임
        //for (int i = 0; i < eventRefIds.Length; i++)
        //{
        //    if (eventRefIds[i] >= defaultCompensation && eventRefId[i] < defaultQuest)
        //    {
        //        ItCompensation(eventRefIds[i]);
        //    }
        //    else if (eventRefIds[i] >= defaultQuest && eventRefIds[i] < defaultConveration)
        //    {
        //        ItQuest(eventRefIds[i]);
        //    }
        //    else if (eventRefIds[i] >= defaultConveration)
        //    {
        //        ItConveration(eventRefIds[i]);
        //    }
        //}
        #endregion 큰수 -> 작은수  LEGACY
        // 큰수 -> 작은수
        for (int i = 0; i < eventRefIds.Length; i++)
        {
            if (eventRefIds[i] >= defaultConveration)
            {
                ItConveration(eventRefIds[i]);
            }
            else if (eventRefIds[i] >= defaultQuest && eventRefIds[i] < defaultConveration)
            {
                ItQuest(eventRefIds[i]);
            }
            else if (eventRefIds[i] >= defaultCompensation)
            {
                ItCompensation(eventRefIds[i]);
            }
        }

        ResetChoice();
        if (eventRefIds[^1] == 0)
        {
            npc.InvokeEndConverationEvent();
        }



    }       // CheckOnClickChoice()

    #endregion 이벤트 관련


}       // ClassEnd
