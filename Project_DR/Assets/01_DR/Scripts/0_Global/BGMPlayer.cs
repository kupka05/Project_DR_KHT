using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public string soundName;

    void Start()
    {
        if (soundName == string.Empty)
        {
            GFunc.Log("BGM을 찾을 수 없음");
            return;
        }
            AudioManager.Instance.AddBGM(soundName);
            AudioManager.Instance.PlayBGM(soundName);

    }

}
