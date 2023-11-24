using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRoom : MonoBehaviour
{       // EventRoomClass는 스프레드시트의 값을 받아와서 랜덤한 이벤트 발생을 시켜줄 Class
    


    void Start()
    {
        Debug.LogFormat("{0}는 EventRoom을 가지게 됨", this.gameObject.name);
    }

}       // ClassEnd
