using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogBox : MonoBehaviour
{

    private Transform lookAt;

    // Start is called before the first frame update
    void Start()
    {
        lookAt = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTooltipPosition();
    }

    public virtual void UpdateTooltipPosition()
    {
        if (lookAt)
        {
            transform.LookAt(Camera.main.transform);
        }
        else if (Camera.main != null)
        {
            lookAt = Camera.main.transform;
        }
        else if (Camera.main == null)
        {
            return;
        }       
    }
}
