using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class NPC_CanvasController : MonoBehaviour
{       // NPC의 Text ,RayHit판정 Image등 관련해서 조정할수 있게 해주는 Class

    private string npcName;                      // NPC의 이름을 담아둘 string
    private string npcTitle;                     // NPC의 칭호를 담아둘 string

    private StringBuilder npcConversations;     // NPC의 대화를 출력시켜줄 StringBuilder

    private TextMeshProUGUI npcNameText;         // Canvas상에서 출력될 이름의 TextMeshPro
    private TextMeshProUGUI npcTitleText;        // Canvas상에서 출력될 칭호의 TextMeshPro
    private TextMeshProUGUI npcConversationText; // Canvas상에서 출력될 대화의 TextMeshPro
    private TextMeshProUGUI choice1Text;         // 선택지 1의 TextMeshPro
    private TextMeshProUGUI choice2Text;         // 선택지 2의 TextMeshPro
    private TextMeshProUGUI choice3Text;         // 선택지 3의 TextMeshPro

    // TODO : Ray의 판정을 위해 Image컴포넌트를 가져와야할수도 있음


    private void Awake()
    {
        AwakeInIt();
        //Test();
    }


    void Start()
    {
        
    }

    private void AwakeInIt()
    {       // 종속성이 없는 TextMeshPro의 컴포넌트를 Get해올것임
        npcConversations = new StringBuilder();

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
        npcName = _name;
        npcTitle = _title;

        npcNameText.text = npcName;
        npcTitleText.text = npcTitle;
    }       // Name_TitleUpdate()


    /// <summary>
    /// NPC의 대사를 출력시켜주는 함수
    /// </summary>
    /// <param name="_conversationText"></param>
    public void OutPutConversations(StringBuilder _conversationText)
    {       // 이건 매개변수 바뀔수도 있음
        npcConversationText.text = _conversationText.ToString();
    }       // OutPutConversations()

    private void Test()
    {
        npcNameText.text = "이름 잘불러왔냐";
        npcTitleText.text = "칭호 잘불러왔냐";
        npcConversationText.text = "대사 잘 불러오냐";
        choice1Text.text = "선택지1 잘 가져왔냐";
        choice2Text.text = "선택지2 잘 가져왔냐";
        choice3Text.text = "선택지3 잘 가져왔냐";
    }


}       // ClassEnd
