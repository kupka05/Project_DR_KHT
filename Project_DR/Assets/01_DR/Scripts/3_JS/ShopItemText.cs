using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemText : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private TMP_Text _text;
    
    // ID를 설정
    public void SetID(int id)
    {
        _id = id;
    }

    // 데이터 호출 및 텍스트 갱신
    public void GetDataAndSetText()
    {
        string name = (string)DataManager.instance.GetData(_id, "Name", typeof(string));
        int price = (int)DataManager.instance.GetData(_id, "Price", typeof(int));
        GetTMPText().text = name + "\n" + "<color=#ffd400>" + price.ToString() + " 골드</color>";
    }

    private TMP_Text GetTMPText()
    {
        return _text ?? (_text = GetComponent<TMP_Text>());
    }
}
