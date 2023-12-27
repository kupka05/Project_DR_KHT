using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ResultItem : Component
{
    public TMP_Text itemName;
    public TMP_Text count;
    public TMP_Text gold;
    public TMP_Text exp;


    public ResultItem ItemInit()
    {
        itemName = transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>();
        count = transform.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>();
        gold = transform.GetChild(2).GetChild(0).gameObject.GetComponent<TMP_Text>();
        exp = transform.GetChild(3).GetChild(0).gameObject.GetComponent<TMP_Text>();

        return this;
    }
}

public class ResultUI : MonoBehaviour
{

    public enum State { Monster, Item, Quest }

    public State state;

    private TMP_Text label;             // 라벨
    public GameObject item;            // 아이템 프리팹
    private GameObject underLineObj;    // 언더라인


    // Start is called before the first frame update
    void Start()
    {
        label = GetComponent<TMP_Text>();

        item = transform.GetChild(0).gameObject;
        underLineObj = transform.GetChild(1).gameObject;
        ResultItem resultItem = item.AddComponent<ResultItem>();

        item.gameObject.SetActive(false);
        underLineObj.gameObject.SetActive(false);

    }

  

    void SetUI()
    {
        switch (state)
        {
            case State.Monster:
                break;

        }
    }
}
