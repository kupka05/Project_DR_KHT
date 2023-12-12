using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSelectControl : MonoBehaviour
{
    public string googleSheet;
    public string csv;
    // Google Sheets 로드
    public void LoadGoogleSheet()
    {
        SceneManager.LoadScene(googleSheet);
    }

    // CSV 로드
    public void LoadLocalCSV()
    {
        SceneManager.LoadScene(csv);
    }
}

