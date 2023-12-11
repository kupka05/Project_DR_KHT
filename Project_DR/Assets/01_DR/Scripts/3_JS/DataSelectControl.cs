using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSelectControl : MonoBehaviour
{
    // Google Sheets 로드
    public void LoadGoogleSheet()
    {
        SceneManager.LoadScene("1_DataLodingScene");
    }

    // CSV 로드
    public void LoadLocalCSV()
    {
        SceneManager.LoadScene("2_LoginScene");
    }
}

