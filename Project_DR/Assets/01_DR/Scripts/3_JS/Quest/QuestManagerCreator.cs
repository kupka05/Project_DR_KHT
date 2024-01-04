using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Js.Quest;

public class QuestManagerCreator : MonoBehaviour
{
    void Start()
    {
        // 퀘스트 전체 생성 & 퀘스트 매니저 생성
        QuestManager.Instance.CreateQuestFromDataTable();
    }
}
