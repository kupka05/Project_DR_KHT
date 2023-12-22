using Js.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************************
 *                Public Classes
 *************************************************/
public class QuestDebug : MonoBehaviour
{
    public Quest quest = default;
}

public class QuestListDebug : MonoBehaviour
{
    /*************************************************
     *               Private Fields
     *************************************************/
    private string[] _QuestTypeName =
    {
        "NONE", "[메인] ", "[서브] ", "[특수] "
    };
    private List<Quest> _questList => UserDataManager.QuestList;


    /*************************************************
     *                Public Methods
     *************************************************/
    public void Initialize()
    {
        // Init
        Transform parent = transform;
        for (int i = 0; i < _questList.Count; i++)
        {
            Quest quest = _questList[i];
            int type = (int)quest.QuestData.Type;
            string objName = GFunc.SumString(_QuestTypeName[type], quest.QuestData.ID.ToString());
            GameObject obj = new GameObject(objName);
            obj.AddComponent<QuestDebug>().quest = quest;
            obj.transform.SetParent(parent);
        }

        _questList[_questList.Count - 1].ChangeState(2);
    }
}
