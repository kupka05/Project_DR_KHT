using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopTextController : MonoBehaviour
{
    [SerializeField]
    private int id = default;
    private TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();

        // 데이터 호출 및 텍스트 갱신
        GetDataAndSetText();
    }

    private void GetDataAndSetText()
    {
        string name = (string)DataManager.instance.GetData(id, "Name", typeof(string));
        int price = (int)DataManager.instance.GetData(id, "Price", typeof(int));
        _text.text = name + "\n" + "<color=#ffd400>" + price.ToString() + " 골드</color>";
    }
}
