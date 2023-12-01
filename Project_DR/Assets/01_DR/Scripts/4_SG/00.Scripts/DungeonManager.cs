using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{       // 던전의 큰 틀을 잡아줄 매니저

    private static DungeonManager instance = null;
    public static DungeonManager DungeonManagerInstance
    {
        get 
        { 
            if (instance == null || instance == default)
            {
                instance = FindObjectOfType<DungeonManager>();
            }
            return instance; 
        }
    }
    
    public static List<bool> clearList; // 모든방들이 클리어 했는지 여부를 체크해줄 static List

    private void Awake()
    {
        AwakeInIt();
        

    }       // Awake()

    /// <summary>
    /// Awake단계에서 가져올 컴포넌트들
    /// </summary>
    private void AwakeInIt()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지되도록 설정
        }
        else
        {
            Destroy(gameObject);
        }        
        if (clearList == null || clearList == default)
        {
            clearList = new List<bool>();
        }
    }       // AwakeInIt()

    

}       // ClassEnd
