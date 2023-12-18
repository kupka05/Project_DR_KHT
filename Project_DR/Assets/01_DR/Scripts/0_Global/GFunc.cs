using System.Text;
using UnityEngine;

public static class GFunc
{
    /*************************************************
     *               Private Fields
     *************************************************/
    private static StringBuilder stringBuilder = new StringBuilder();


    /*************************************************
     *               Public Methods
     *************************************************/
    public static string SumString(string inputA, string inputB, string inputC = "", string inputD = "")
    {
        stringBuilder.Clear();
        stringBuilder.Append(inputA);
        stringBuilder.Append(inputB);
        stringBuilder.Append(inputC);
        stringBuilder.Append(inputD);

        return stringBuilder.ToString();
    }


    // 언더바를 없애주는 String 확장 메서드
    public static string RemoveUnderbar(this string input)
    {
        return input.Replace("_", "");
    }

    // 디버그 로그를 찍어주는 메서드
    public static void Log(string _input)
    {
        Debug.Log(_input);
    }

    // 디버그 로그를 찍어주는 메서드
    public static void LogWarning(string _input)
    {
        Debug.LogWarning(_input);
    }

    // 디버그 로그를 찍어주는 메서드
    public static void LogError(string _input)
    {
        Debug.LogError(_input);
    }
    // 디버그 로그를 찍어주는 메서드
    public static void LogErrorFormat(string _input)
    {
        Debug.LogErrorFormat(_input);
    }

    // 디버그 로그를 찍어주는 메서드
    public static void LogException(System.Exception _input)
    {
        Debug.LogException(_input);
    }
}
