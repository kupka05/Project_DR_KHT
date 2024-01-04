using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonInspectionManager : MonoBehaviour
{


    public static DungeonInspectionManager dungeonManagerInstance;
    public bool isEndCreateFloor = false;       // 바닥 생성만 완료가 되었을때에 바뀔꺼임
    private bool floorCollision = false;        // 검사기바닥 충돌시 true로 변환


    public bool FloorCollision
    {
        get { return floorCollision; }

        set
        {
            if (floorCollision != value)
            {
                floorCollision = value;
            }
        }
    }
           
    private void Awake()
    {
        if(dungeonManagerInstance == null)
        {
            dungeonManagerInstance = this;
        }
        else { /*GFunc.Log("DungeonInspectionManager : else 들어옴");*/ }
    }


}       // Class
