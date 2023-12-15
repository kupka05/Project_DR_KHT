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

    public void Initialize()
    {
        spendExpValue = 0;

        curExp.text = UserData.GetExp().ToString();
        spendExp.text = spendExpValue.ToString();
        remainExp.text = curExp.text;
    }

    public void SetSpend(int _spendExp)
    {
        spendExpValue = _spendExp;

        curExp.text = UserData.GetExp().ToString();
        spendExp.text = spendExpValue.ToString();
        remainExp.text = (UserData.GetExp() - _spendExp).ToString();
    }
}
