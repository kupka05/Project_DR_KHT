using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ResultItem : MonoBehaviour
{
    [Header ("Item")]
    public TMP_Text itemName;
    public TMP_Text count;
    public TMP_Text gold;
    public TMP_Text exp;


    // UI 세팅
    public ResultItem ItemInit()
    {
        itemName = transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        count = transform.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>();
        gold = transform.GetChild(2).gameObject.GetComponent<TMP_Text>();
        exp = transform.GetChild(3).gameObject.GetComponent<TMP_Text>();

        return this;
    }

    // 퀘스트 모드 시, 카운트 꺼두기
    public void QuestMode()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void SetInfo(string _name, int _count, int _gold, int _exp)
    {
        itemName.text = _name;
        count.text = _count.ToString();
        gold.text = _gold.ToString();
        exp.text = _exp.ToString();
    }
}

public class ResultUI : MonoBehaviour
{

    public enum State { Monster, Item, Quest }

    public State state;

    private TMP_Text label;             // 라벨
    public GameObject item;            // 아이템 프리팹
    private GameObject lineObj;    // 언더라인


    // Start is called before the first frame update
    //void Start()
    //{
    //    Initialized();
    //    //AddItem("일반", 1, 10, 20);
    //    //AddItem("엘리트", 2, 10, 20);
    //    //AddItem("정예", 3, 10, 20,false);
    //    //AddLine();
    //}

    // 결과 UI 초기화
    public void Initialized()
    {
        label = GetComponent<TMP_Text>();
        label.text = SetLabel();

        item = transform.GetChild(0).gameObject;
        lineObj = transform.GetChild(1).gameObject;

        ResultItem resultItem = item.AddComponent<ResultItem>();
        resultItem.ItemInit();

        if(state == State.Quest)
        {
            resultItem.QuestMode();
        }

        item.gameObject.SetActive(false);
        lineObj.gameObject.SetActive(false);
    }

    // 라벨 세팅
    private string SetLabel()
    {
        string newlabel = "Label";
        switch (state)
        {
            case State.Monster:
                newlabel = "몬스터";
                break;
            case State.Item:
                newlabel = "아이템";
                break;
            case State.Quest:
                newlabel = "퀘스트";
                break;
        }
        return newlabel;        
    }

    // 아이템 추가
    public void AddItem(string _name, int _count, int _gold, int _exp, bool line = true)
    {
        if(_count + _gold + _exp == 0)
        { return; }

        GameObject newItem = Instantiate(item, transform);

        //new GameObject("Result Item");
        newItem.GetComponent<ResultItem>().SetInfo(_name, _count, _gold, _exp);

        newItem.transform.parent = transform;
        newItem.SetActive(true);

        newItem.transform.GetChild(4).gameObject.SetActive(line);   // 라인 여부 (마지막줄일 경우 라인 제거)
    }
    public void AddLine()
    {
        GameObject newLine = Instantiate(lineObj, transform);
        newLine.SetActive(true);
    }
}
