using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEbugQuest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Unit.GetQuestByID(00_00_001).ChangeState(2);
            Unit.ChangeQuestStateToInProgress(00_00_001);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Unit.ClearQuestByID(00_00_001);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Unit.SaveQuestDataToDB();
        }

    }
}
