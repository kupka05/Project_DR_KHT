using System;
using System.IO;
using UnityEngine;

public class SaveDataToFile
{
    /*************************************************
     *                 Private Methods
     *************************************************/
    #region [+]
    private static string _fileName = "";
    #endregion
    /*************************************************
     *                 Public Methods
     *************************************************/
    #region [+]
    public static void WriteJsonToFile(string jsonData, string sheetName)
    {
        string directory = "Resources/JSONs";
        string extension = ".json";
        string filePath = Path.Combine(Application.dataPath, directory, _fileName + sheetName + extension);

        // 파일에 쓸 때 사용할 스트림 생성
        using (StreamWriter streamWriter = File.CreateText(filePath))
        {
            // JSON 데이터를 파일에 쓰기
            streamWriter.Write(jsonData);

            Debug.Log($"{filePath} 경로에 JSON 데이터가 파일로 저장되었습니다.");
        }
    }

    public static void WriteCsvToFile(string jsonData, string sheetName)
    {
#if UNITY_EDITOR
        try
        {
            string directory = "Resources";
            string extension = ".csv";
            string filePath = Path.Combine(Application.dataPath, directory, _fileName + sheetName + extension);

            // 파일에 쓸 때 사용할 스트림 생성
            using (StreamWriter streamWriter = File.CreateText(filePath))
            {
                // JSON 데이터를 파일에 쓰기
                streamWriter.Write(jsonData);

                Debug.Log($"{filePath} 경로에 CSV 데이터가 파일로 저장되었습니다.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"SaveDataToFile.WriteCsvToFile() Error: {sheetName} 시트가 열려있어 수정할 수 없습니다. {ex.Message}");
           
        }
#endif
    }
    #endregion
}
