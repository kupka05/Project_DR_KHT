using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopPannel : MonoBehaviour
{
    public GameObject pannel;
    private GameObject textObj;

    // Start is called before the first frame update
    void Start()
    {
        pannel = transform.GetChild(0).gameObject;
        textObj = pannel.transform.GetChild(0).GetChild(0).gameObject;
        if (pannel)
        {
            pannel.GetComponent<Animation>().Play("Status_Off");
            textObj.SetActive(false);
        }
    }

    #region 디스플레이 트리거
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PannelOn();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PannelOff();
        }
    }
    #endregion

    private void PannelOn()
    {
        pannel.GetComponent<Animation>().Play("Status_On");
        textObj.SetActive(true);
    }
    private void PannelOff()
    {
        pannel.GetComponent<Animation>().Play("Status_Off");
        textObj.SetActive(false);
    }
}
