using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceImage : MonoBehaviour
{
    private Image image;    // 자신의 이미지 컴포넌트
    private bool isHit = false;      // 자신이 레이맞고 클릭이 되었는지
    public bool IsHit
    {
        get { return isHit; }
        set
        {
            if (isHit != value)
            {
                isHit = value;
                CheckIsHit();
            }
        }
    }


    private void Awake()
    {
        image = GetComponent<Image>(); 
    }


    void Start()
    {
        
    }

    private void CheckIsHit()
    {
        if (IsHit == true)
        {
           // 필요한 정보 위쪽으로 보내줘야함
        }

        else
        {

        }
    }
    
}       // ClassEnd
