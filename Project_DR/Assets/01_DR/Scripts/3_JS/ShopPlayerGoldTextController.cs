using TMPro;
using UnityEngine;

public class ShopPlayerGoldTextController : MonoBehaviour
{
    private TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();

        // 데이터 호출 및 텍스트 갱신
        GetDataAndSetText();
    }

    private void GetDataAndSetText()
    {
        int haveGold = 0;
        _text.text = "<color=#ffd400>[참고!]\n당신의 소지 골드\n" + haveGold + " 골드</color>";
    }
}
