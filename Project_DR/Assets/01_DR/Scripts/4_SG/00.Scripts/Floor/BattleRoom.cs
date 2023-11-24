using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : MonoBehaviour
{       // BattleRoomClass는 Monster소환 Monster가 전부 죽었는지 체크할것
   
    
    void Start()
    {
        Debug.LogFormat("{0}는 BattleRoom을 가지게 됨", this.gameObject.name);
    }


}       // ClassEnd
