using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetsReader
{
    #region [예시(Example)]
    // 예시 보여주기용 미사용 함수
    private void Example()
    {
        // 스프레드 시트 문서 url에 있는 ID
        string spreadsheetId = "1zGWet3RC6xmqQTkGS31E8BQjEPTuwhIFT1LkVIJSJYI";
        // API의 접근 KEY
        string apiKey = "AIzaSyCXNUN-D43FJGsu37TjJxRUDWD7wcF98CU";
        // 스프레드 시트 문서의 시트 이름
        string sheetName = "Minion_Table";
        // 함수 호출 방법
        //GetGoogleSheetsData(spreadsheetId, apiKey, sheetName);
    } // 예시 보여주기용 미사용 함수
    #endregion

    public static WaitForSeconds waitForSceonds = new WaitForSeconds(0.5f);
    // 구글 스프레드 시트에 있는 문서를 가져오는 함수
    // 네트워크 요청과 같이 시간이 걸리는 동작을 수행할 때는
    // 코루틴을 사용하는게 좋다.
    // 매개변수 isCSVConvert = true / false 값을 통해
    // 자동으로 csv 형식으로 변환할 수 있다. 
    private static WaitForEndOfFrame waitForEndOfFrame;
    public static IEnumerator GetGoogleSheetsData(string spreadsheetId, 
        string apiKey, string sheetName, bool isCsvConvert, int waitFrame, Action<string> callBack)
    {
        GFunc.Log($"프레임대기: {waitFrame}");
        //// waitFrame만큼 대기
        //for (int i = 0; i < waitFrame; i++)
        //{
        //    yield return null;
        //}
        yield return waitForSceonds;

        // 구글 문서 url 할당
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{sheetName}?key={apiKey}";

        // 메모리 절약을 위해 using 사용
        // using 객체를 사용한 후에 자동으로 메모리 할당이
        // 해제된다.
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            string data = "";
            if (www.result != UnityWebRequest.Result.Success)
            {
                GFunc.LogError("Error: " + www.error);

                // 리퀘스트 오류 발생시 다시 로드 한다.
                GoogleSheetLoader.instance.ReLoadGoogleSheetsData(
                    spreadsheetId, apiKey, sheetName, isCsvConvert, waitFrame, callBack);
            }
            else
            {
                // JSON 형식 데이터를 가져옴
                data = www.downloadHandler.text;

                // JSON 데이터를 파일로 저장
                //SaveDataToFile.WriteJsonToFile(data, sheetName);

                // isCsvConert가 true일 경우
                if (isCsvConvert)
                {
                    // JSON 형식 데이터를 CSV 형식으로 변환
                    data = JsonToCsvConverter.ConvertJsonToCsv(data);

                    // CSV 데이터를 파일로 저장
                    SaveDataToFile.WriteCsvToFile(data, sheetName);
                }
            }

            // 데이터 반환
            callBack(data);
        }
    }
}
