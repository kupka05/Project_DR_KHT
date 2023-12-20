using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : IDatabase
{
    public float GetData(int id, string key, float value)
    {
        value = (float)DataManager.Instance.GetData(id, key, typeof(float));
        return value;
    }
    public int GetData(int id, string key, int value)
    {
        value = (int)DataManager.Instance.GetData(id, key, typeof(int));
        return value;
    }
    public string GetData(int id, string key, string value)
    {
        value = (string)DataManager.Instance.GetData(id, key, typeof(string));
        return value;
    }
}
