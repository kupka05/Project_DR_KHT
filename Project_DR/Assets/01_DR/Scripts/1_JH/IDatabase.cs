using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface IDatabase
{

    float GetData(int id, string key, float value);

    int GetData(int id, string key, int value);

    string GetData(int id, string key, string value);
  
}
