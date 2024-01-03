using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class ItemNameTag : MonoBehaviour
{
    public TMP_Text itemName;
    private Transform itemPosition;
    private Transform targetPosition;



    public void SetName(string str) 
    {
        itemName = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        itemName.text = str;
    }
    public void SetPosition(Transform position)
    {
        itemPosition = transform.GetChild(1);
        RingHelper target = position.GetComponentInChildren<RingHelper>();
        if(!target)
        {
            GFunc.Log("타겟을 찾을 수 없음.");
            return;
        }
        targetPosition = transform;
        itemPosition.localPosition = targetPosition.localPosition;
    }

}
