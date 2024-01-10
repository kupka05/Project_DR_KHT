using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCUIImage : MonoBehaviour
{
    private Button button;          // 해당 이미지의 버튼
    public int choiceNum;            // 자신이 몇번선택지인지 나타내줄 번호 선택지가 들어갈떄에 CnavasController가 넣어줄거임
    private bool isHit = false;      // 자신이 레이맞고 클릭이 되었는지

    public delegate void OnHitCallback(NPCUIImage _npcUIImage);
    public event OnHitCallback onHitEvent;      // CanvasController가 해당 이벤트를 구독하고 히트시 이벤트 호출을 해서 기능 실행시킬예정
    public bool IsHit
    {
        get { return isHit; }
        set
        {
            if (isHit != value)
            {
                isHit = value;
                if (IsHit == true)
                {
                    //GFunc.Log($"IS Hit 이벤트가 돌며 INVOKE했음");
                    onHitEvent?.Invoke(this);
                    IsHit = false;
                }
                else { /*PASS*/ }
                //ImageHit();
            }
        }
    }

    private void Start()
    {
        button = this.transform.GetComponent<Button>();
        ButtonOnClickEventSetting();
        //GFunc.Log($"buttonIs null? -> {button == null}");
    }


    public void ButtonOnClickEventSetting()
    {
        //Debug.Log($"이벤트 지정 함수 도는중 : {this.gameObject.name}");
        button.onClick.AddListener(CheckIsHit);
    }       // ButtonOnClickEventSetting()

    public void CheckIsHit()
    {
        if (IsHit == true)
        {
            onHitEvent?.Invoke(this);
            IsHit = false;
        }
        else { /*PASS*/ }
    }

}       // ClassEnd
