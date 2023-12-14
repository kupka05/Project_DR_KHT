using System.Text;

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
}
