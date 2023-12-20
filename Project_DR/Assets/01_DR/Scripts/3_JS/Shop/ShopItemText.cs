using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemText : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private int _price;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _fontColor = "<color=#ffd400>";

    // Init
    public void Initialize(int id)
    {
        _id = id;
        _name = (string)DataManager.Instance.GetData(_id, "Name", typeof(string));
        _price = (int)DataManager.Instance.GetData(_id, "Price", typeof(int));
    }

    // 데이터 호출 및 텍스트 갱신
    public void GetDataAndSetText()
    {
        gameObject.GetTMPText(ref _text).text = _name + "\n" + _fontColor + _price.ToString() + " 골드</color>"; ;
    }
}
