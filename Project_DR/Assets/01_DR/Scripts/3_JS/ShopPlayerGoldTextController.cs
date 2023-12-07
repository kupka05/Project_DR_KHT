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

    // 골드 텍스트를 수정
    // 구매 됐을 경우 해당 함수를 호출하여 갱신
    public void GetDataAndSetText()
    {
        int haveGold = UserDataManager.Instance.Gold;
        _text.text = "<color=#ffd400>[참고!]\n당신의 소지 골드\n" + haveGold + " 골드</color>";
    }
}
