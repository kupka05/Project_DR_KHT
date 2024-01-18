using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetScroll : MonoBehaviour
{
    void Start()
    {
        //ResetScrollRect();
    }

    // 스크롤 위치 초기화
    public void ResetScrollRect()
    {
        ScrollRect scrollRect = transform.parent.parent.GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 1.0f;
    }
    public void ResetScrollPos()
    {
        ScrollRect scrollRect = transform.parent.parent.GetComponent<ScrollRect>();
        scrollRect.content.anchoredPosition = Vector3.zero;
    }
}
