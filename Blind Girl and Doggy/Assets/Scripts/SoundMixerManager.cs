using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    
    private void Start()
    {
        InitializeVolumeSettings();
    }

    public void SetMasterVolume(float level)
    {
        if(level <= 0)
        {
            audioMixer.SetFloat("masterVolume", -80.0f);
        }
        else
        {
            audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20);
        }
        
        PlayerPrefs.SetFloat("masterVolume", level);
        Debug.Log("masterVolume: " + level);
    }

    public void SetMusicVolume(float level)
    {
        if(level <= 0)
        {
            audioMixer.SetFloat("musicVolume", -80.0f);
        }
        else
        {
            audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20);
        }

        PlayerPrefs.SetFloat("musicVolume", level);
        Debug.Log("musicVolume: " + level);
    }

    public void SetSoundFXVolume(float level)
    {
        if(level <= 0)
        {
           audioMixer.SetFloat("soundFXVolume", -80.0f);
        }
        else
        {
           audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20);
        }

        PlayerPrefs.SetFloat("soundFXVolume", level);
        Debug.Log("soundFXVolume: " + level); 
    }

    public void InitializeVolumeSettings()
    {
        if (PlayerPrefs.HasKey("masterVolume") && PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("soundFXVolume"))
        {
            float masterVolume = PlayerPrefs.GetFloat("masterVolume");
            float musicVolume = PlayerPrefs.GetFloat("musicVolume");
            float soundFXVolume = PlayerPrefs.GetFloat("soundFXVolume");

            SetMasterVolume(masterVolume);
            SetMusicVolume(musicVolume);
            SetSoundFXVolume(soundFXVolume);
        }
        else
        {
            SetMasterVolume(1.0f);
            SetMusicVolume(1.0f);
            SetSoundFXVolume(1.0f);
        }

    }
}
