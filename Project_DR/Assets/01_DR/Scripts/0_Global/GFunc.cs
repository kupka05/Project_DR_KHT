using System;
using System.Text;
using System.Collections;

public static class GFunc
{
    /*************************************************
     *               Private Fields
     *************************************************/
    private static StringBuilder stringBuilder = new StringBuilder();


    /*************************************************
     *               Public Methods
     *************************************************/
    public static string SumString(string inputA, string inputB, string inputC = "")
    {
        stringBuilder.Clear();
        stringBuilder.Append(inputA);
        stringBuilder.Append(inputB);
        stringBuilder.Append(inputC);

        return stringBuilder.ToString();
    }

    // 언더바를 없애주는 String 확장 메서드
    public static string RemoveUnderbar(this string input)
    {
        return input.Replace("_", "");
    }

    /// <summary>
    /// 늘어져있는 대사의 Replace와 Split이후 반환해주는 함수
    /// </summary>    
    /// <param name="_placeString">GetData해온 string값</param>
    /// <returns></returns>
    public static string[] SplitConversation(string _placeString)
    {
        _placeString = _placeString.Replace("#", ",");
        _placeString = _placeString.Replace("\\", "");
        _placeString = _placeString.Replace("_", "");         

        string[] replaceStrings = _placeString.Split("\n");

        //tableIDs = tableIDs.Replace("\\n", "\n");
        //tableIDs = tableIDs.Replace("\\", "");        


        return replaceStrings;
    }       // SplitConveration()

    /// <summary>
    /// GetData해온 string값을 Replace와 Split한후 int[]로 변환시켜줘서 반환해주는 함수
    /// </summary>
    /// <param name="_parsString">Id값이 들어있는 string</param>
    /// <returns>int[] 반환</returns>
    public static int[] SplitIds(string _parsString)
    {
        _parsString.Replace("\\n", "\n");
        _parsString.Replace("\\", "");
        _parsString.Replace("_", "");         
        string[] splitParams = _parsString.Split("\n");

        int[] splitIds = new int[splitParams.Length];
        for (int i = 0; i < splitParams.Length; i++)
        {
            splitIds[i] = int.Parse(splitParams[i]);
        }

        return splitIds;
    }       // SplitIds()

}       // ClassEnd
