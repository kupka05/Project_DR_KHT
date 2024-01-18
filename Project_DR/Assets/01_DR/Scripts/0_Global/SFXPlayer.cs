using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [Header ("SFX Files")]
    public string[] sfxNames;

    [Header ("SFX Setting")]
    public bool random;
    public bool playAwake;
    public bool loop;
    
    void Start()
    {
        GetSFX();
        SetSFX();       
    }
    private void GetSFX()
    {
        if (sfxNames == null)
        { return; }

        for (int i = 0; i < sfxNames.Length; i++)
        {
            AudioManager.Instance.AddSFX(sfxNames[i]);
        }
    }
    private void SetSFX()
    {
        if (!playAwake)
        {
            return;
        }

        if (loop)
        {
            AudioManager.Instance.PlaySFXLoop(sfxNames[0], this.transform.position);
        }
        else
            AudioManager.Instance.PlaySFX(sfxNames[0]);

    }

    public void PlaySFX()
    {
        if (random) 
        {
            int rand = Random.Range(0, sfxNames.Length);
            AudioManager.Instance.PlaySFX(sfxNames[rand]);
        }

    }
    public void PlaySFX(string name)
    {
        AudioManager.Instance.PlaySFX(name);
    }

}
