
public static class Data
{
    /*************************************************
     *                Public Methods
     *************************************************/
    // DataManager에서 Int형 GetData
    public static int GetInt(int id, string category)
    {
        return (int)DataManager.instance.GetData(id, category, typeof(int));
    }

    // DataManager에서 Float형 GetData
    public static float GetFloat(int id, string category)
    {
        return (float)DataManager.instance.GetData(id, category, typeof(float));
    }

    // DataManager에서 String형 GetData
    public static string GetString(int id, string category)
    {
        return (string)DataManager.instance.GetData(id, category, typeof(string));
    }

    // DataManager에서 id가 위치하는 시트의 열 갯수를 가져옴
    public static int GetCount(int id)
    {
        return DataManager.instance.GetCount(id);
    }
}
