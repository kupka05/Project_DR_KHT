#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Rito.InventorySystem;
// 스크립터블 오브젝트의 데이터를 담을 클래스
[CreateAssetMenu(fileName = "NewScriptableObject", menuName = "MyScriptableObjects/NewScriptableObject", order = 1)]
public class MyScriptableObject : ScriptableObject
{
    public string myString;
    public int myInt;
    public float myFloat;
}

public class ScriptableObjectCreator : MonoBehaviour
{
    void Start()
    {
        // ScriptableObject를 동적으로 생성
        MyScriptableObject myScriptableObject = CreateScriptableObject();

        Debug.Log(myScriptableObject);

        // 생성된 ScriptableObject 사용
        if (myScriptableObject != null)
        {
            Debug.Log("Created String: " + myScriptableObject.myString);
            Debug.Log("Created Int: " + myScriptableObject.myInt);
            Debug.Log("Created Float: " + myScriptableObject.myFloat);
#if UNITY_EDITOR
            SaveScriptableObject(myScriptableObject);
#endif
        }
        else
        {
            Debug.LogError("Failed to create ScriptableObject!");
        }
    }

    // ScriptableObject를 생성하는 함수
    MyScriptableObject CreateScriptableObject()
    {
        MyScriptableObject newScriptableObject = ScriptableObject.CreateInstance<MyScriptableObject>();

        // ScriptableObject에 데이터 할당 (예시 데이터)
        newScriptableObject.myString = "Hello, World!";
        newScriptableObject.myInt = 4522;
        newScriptableObject.myFloat = 3.14f;

        // 생성된 ScriptableObject 리턴
        return newScriptableObject;
    }

    private void SaveScriptableObject(MyScriptableObject data)
    {
#if UNITY_EDITOR
        // 생성된 스크립터블 오브젝트를 에셋으로 저장
        string path = "Assets/Resources/NewScriptableObject.asset";
        UnityEditor.AssetDatabase.CreateAsset(data, path);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

    public static void SaveScriptableObject(int id, PortionItemData data)
    {
#if UNITY_EDITOR
        // 생성된 스크립터블 오브젝트를 에셋으로 저장
        string path = "Assets/Resources/" + id + ".asset";
        UnityEditor.AssetDatabase.CreateAsset(data, path);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }
}
