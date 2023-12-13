using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class NPC_CanvasController : MonoBehaviour
{       // NPC의 Text ,RayHit판정 Image등 관련해서 조정할수 있게 해주는 Class


    private StringBuilder npcConversations;     // NPC의 대화를 출력시켜줄 StringBuilder

    private TextMeshProUGUI npcNameText;         // Canvas상에서 출력될 이름의 TextMeshPro
    private TextMeshProUGUI npcTitleText;        // Canvas상에서 출력될 칭호의 TextMeshPro
    private TextMeshProUGUI npcConversationText; // Canvas상에서 출력될 대화의 TextMeshPro
    private TextMeshProUGUI choice1Text;         // 선택지 1의 TextMeshPro
    private TextMeshProUGUI choice2Text;         // 선택지 2의 TextMeshPro
    private TextMeshProUGUI choice3Text;         // 선택지 3의 TextMeshPro

    private Color aZeroColor;         // 투명한 색
    private Color aMaxColor;          // 흰색

    private string isNon;                       // 해당 선택지가 존재하지 않는지 -> 존재하지 않으면 "0"으로 표시될것
    private string underBar;                    // Underbar도 선택지가 존재하지 않는다는 의미로 사용될것임
    // TODO : Ray의 판정을 위해 Image컴포넌트를 가져와야할수도 있음


    private void Awake()
    {
        AwakeInIt();
    }

    private void AwakeInIt()
    {       // 종속성이 없는 TextMeshPro의 컴포넌트를 Get해올것임
        npcConversations = new StringBuilder();
        isNon = "0";
        underBar = "_";

        aZeroColor = new Color(1, 1, 1, 0);
        aMaxColor = new Color(1, 1, 1, 1);

        npcNameText = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        npcTitleText = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        npcConversationText = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        choice1Text = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        choice2Text = transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();
        choice3Text = transform.GetChild(0).GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
    }


    /// <summary>
    /// NPC의 이름과 칭호를 변수에 대입해주며 이름과 칭호의 텍스트를 변수값을 출력
    /// </summary>
    /// <param name="_name">NPC의 이름</param>
    /// <param name="_title">NPC의 칭호</param>
    public void Name_TitleUpdate(string _name,string _title)
    {
        npcNameText.text = _name;
        npcTitleText.text = _title;
    }       // Name_TitleUpdate()


    /// <summary>
    /// NPC의 대사를 출력시켜주는 함수
    /// </summary>
    /// <param name="_conversationText"></param>
    public void OutPutConversations(string _conversationText)
    {       // 이건 매개변수 바뀔수도 있음
        npcConversationText.text = _conversationText;
    }       // OutPutConversations()


    /// <summary>
    /// 선택지 출력해주는 함수
    /// </summary>
    /// <param name="_converationRefID">출력해야하는 대사의 ID값</param>
    public void OutPutChoices(int _converationRefID)
    {

        // Choice1Event가 존재한다면 그것의 ID를 풀어내서 참조해야함(플레이어퀘스트 || 이후 영향이 줄것)
        string choice1Event = (string)DataManager.instance.GetData(_converationRefID, "Choice1Event", typeof(string));

        string choice1 = (string)DataManager.instance.GetData(_converationRefID, "Choice1", typeof(string));
        CheckChoiceNull(choice1, choice1Text);

        string choice2 = (string)DataManager.instance.GetData(_converationRefID, "Choice2", typeof(string));
        CheckChoiceNull(choice2, choice2Text);

        // 아래 Choice3는 존재하면 띄우는 조건이 만족하는지 한번 체크해야함 (12.13기준 퀘스트가 나와야 클리어여부를 끌어와서 체크할수있음)
        string choice3 = (string)DataManager.instance.GetData(_converationRefID, "Choice3", typeof(string));
        CheckChoiceNull(choice3, choice3Text); // 임시사용 함수
        //CheckChoice3();

    }       // CheckExistChoices()

    /// <summary>
    /// 해당 선택지가 비어있는지 확인하는 함수
    /// </summary>
    /// <param name="_choice"></param>
    private void CheckChoiceNull(string _choice,TextMeshProUGUI _outputText)
    {
        if (_choice == isNon || _choice == underBar)
        {
            ChoiceTextAColorOff(_outputText);
            _outputText.text = "";
            return;
        }
        else { /*DoNothing*/ }

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

}       // ClassEnd
