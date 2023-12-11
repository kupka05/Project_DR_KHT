using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AudioManager : MonoBehaviour
{
    #region 싱글톤
    public static AudioManager Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_Instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_Instance = FindObjectOfType<AudioManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_Instance;
        }
    }
    private static AudioManager m_Instance; // 싱글톤이 할당될 static 변수    
    #endregion

    public Sound backGroundMusic;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        if (backGroundMusic != null)
        {
            PlayMusic(backGroundMusic.name);
        }
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }

    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("SFX Not Found");
        }

        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }

    }


    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }


}