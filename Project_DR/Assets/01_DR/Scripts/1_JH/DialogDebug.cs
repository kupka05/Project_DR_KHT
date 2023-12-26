using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogDebug : MonoBehaviour
{
    public UnityEvent enter, input1, input2, input3;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            enter.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            input1.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            input2.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            input3.Invoke();
        }
    }
}
