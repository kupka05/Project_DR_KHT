using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceImage : MonoBehaviour
{
    private Image image;    // 자신의 이미지 컴포넌트

    public int choiceNum;            // 자신이 몇번선택지인지 나타내줄 번호 선택지가 들어갈떄에 CnavasController가 넣어줄거임
    private bool isHit = false;      // 자신이 레이맞고 클릭이 되었는지
    public bool IsHit
    {
        get { return isHit; }
        set
        {
            if (isHit != value)
            {
                isHit = value;
                ImageHit();
            }
        }
    }

    private NPC_CanvasController canvasController;      // 자신이 선택되었다고 알리기 위해 가져오는 컴포넌트 -> 선택될떄만 할당



    void Start()
    {
        image = GetComponent<Image>();
        
    }       // Start()

    private void ImageHit()
    {
        if (IsHit == true) { /*Null*/ }

        else{ return; }

        if (canvasController == null || canvasController == default)
        {
            // 필요한 정보 위쪽으로 보내줘야함
            GetNpcCanvasController();
        }
        else { /*PASS*/ }

        canvasController.CheckOnClickChoice(this);


    }       // ImageHit()


    /// <summary>
    /// CanvasController를 GetComponent해오는 함수
    /// </summary>    
    private void GetNpcCanvasController()
    {
        Transform temptansform = transform;
        temptansform = temptansform.parent.parent;

        canvasController = temptansform.GetComponent<NPC_CanvasController>();
    }       // GetNpcCanvasController()


}       // ClassEnd
