using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float masterVolume = PlayerPrefs.GetFloat("masterVolume");
            SetMasterVolume(masterVolume);
        }
        else
        {
            SetMasterVolume(1.0f);
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("musicVolume");
            SetMusicVolume(musicVolume);
        }
        else
        {
            SetMusicVolume(1.0f);
        }

        if (PlayerPrefs.HasKey("soundFXVolume"))
        {
            float soundFXVolume = PlayerPrefs.GetFloat("soundFXVolume");
            SetSoundFXVolume(soundFXVolume);
        }
        else
        {
            SetSoundFXVolume(1.0f);
        }
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
      //Debug.Log("masterVolume: " + level);
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
     //Debug.Log("musicVolume: " + level);
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
      //Debug.Log("soundFXVolume: " + level); 
    }
}
