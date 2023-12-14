using System.Text;

public class GFunc
{
    /*************************************************
     *               Private Fields
     *************************************************/
    private static StringBuilder stringBuilder = new StringBuilder();


    /*************************************************
     *               Public Methods
     *************************************************/
    public static string SumString(string strA, string strB)
    {
        stringBuilder.Clear();
        stringBuilder.Append(strA);
        stringBuilder.Append(strB);

        return stringBuilder.ToString();
    }
}
