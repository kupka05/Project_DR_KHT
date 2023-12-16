using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UpgradeUI : MonoBehaviour
{
    public int spendExpValue;
    public TMP_Text curExp;
    public TMP_Text spendExp;
    public TMP_Text remainExp;

    public void OnEnable()
    {
        Initialize();
    }
    /// <summary>
    /// 업그레이드 UI를 초기화 해주는 메서드
    /// 경험치는 UserData에서 불러옴
    /// </summary>
    public void Initialize()
    {
        spendExpValue = 0;

        curExp.text = UserData.GetExp().ToString();
        spendExp.text = spendExpValue.ToString();
        remainExp.text = curExp.text;
    }

    /// <summary>
    /// 소비할 EXP를 입력하면 해당 EXP와 UserData의 EXP를 계산해 UI를 업데이트 해주는 메서드
    /// </summary>
    /// <param name="_spendExp">소비할 EXP</param>
    public void SetSpend(int _spendExp)
    {
        spendExpValue = _spendExp;

        curExp.text = UserData.GetExp().ToString();
        spendExp.text = spendExpValue.ToString();
        remainExp.text = (UserData.GetExp() - _spendExp).ToString();
    }
}
