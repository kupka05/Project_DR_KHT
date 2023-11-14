using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPannelChanger : MonoBehaviour
{

    public GameObject loginPannel, createPannel;


    public void CreateAccountButton()
    {
        loginPannel.SetActive(false);
        createPannel.SetActive(true);
    }
    public void CancelButton()
    {
        loginPannel.SetActive(true);
        createPannel.SetActive(false);
    }
}
