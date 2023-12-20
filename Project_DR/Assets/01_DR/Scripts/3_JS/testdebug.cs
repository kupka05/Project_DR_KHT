using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Js.Quest;

public class testdebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TestCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(3f);
        GFunc.Log($"testdebug.GetQuestByID(): {QuestManager.Instance.GetQuestByID(1_000_000_1).QuestData.ID}");
        GFunc.Log($"testdebug.GetQuestByIndex(): {QuestManager.Instance.GetQuestByIndex(0).QuestData.ID}");
        List<Quest> questList = QuestManager.Instance.GetQuestsOfType(1);
        int i = 0;
        foreach (var item in questList)
        {
            GFunc.Log($"testdebug.GetQuestsOfType(): [{i}] {item.QuestData.ID}");
            i++;
        }
        GFunc.Log($"testdebug.GetQuestCountOfType(): {QuestManager.Instance.GetQuestCountOfType(1)}");
    }
}
