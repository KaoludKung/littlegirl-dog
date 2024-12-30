using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip clickClip;

    private DogControl dogControl;
    private GirlControl girlControl;
    private PerformAction[] performActions;
    //private float originalSoundFXVolume;

    private void Start()
    {
        dogControl = FindObjectOfType<DogControl>();
        girlControl = FindObjectOfType<GirlControl>();
        performActions = FindObjectsOfType<PerformAction>();
        //LoadSoundFXVolume();
    }

    private void LoadSoundFXVolume()
    {
        /*
        if (PlayerPrefs.HasKey("soundFXVolume"))
        {
            originalSoundFXVolume = PlayerPrefs.GetFloat("soundFXVolume");
        }
        else
        {
            originalSoundFXVolume = 1.0f;
        }*/
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(!pausePanel.activeSelf);
        Debug.Log("Pause Status:" + isPaused);

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        SoundFXManager.instance.PlaySoundFXClip(clickClip, transform, false, 1.0f);
        dogControl.WalkSource.Pause();
        girlControl.WalkSource.Pause();

        foreach (PerformAction performAction in performActions)
        {
            if (performAction.ActionSource != null)
            {
                performAction.ActionSource.Pause();
            }
        }

        //audioMixer.SetFloat("soundFXVolume", Mathf.Log10(0.001f) * 20);
        //Debug.Log("SoundFXVolume:" + originalSoundFXVolume);

        Debug.Log("Pause game");
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        SoundFXManager.instance.PlaySoundFXClip(clickClip, transform, false, 1.0f);
        dogControl.WalkSource.UnPause();
        girlControl.WalkSource.UnPause();

        foreach (PerformAction performAction in performActions)
        {
            if (performAction.ActionSource != null)
            {
                performAction.ActionSource.UnPause();
            }
        }


        //LoadSoundFXVolume();
        //Debug.Log("SoundFXVolume:" + originalSoundFXVolume);
        //audioMixer.SetFloat("soundFXVolume", Mathf.Log10(originalSoundFXVolume) * 20);
        Debug.Log("Resume Game");
    }

    public void OpenSetting()
    {
        SoundFXManager.instance.PlaySoundFXClip(clickClip, transform, false, 1.0f);
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    public void ExitMenu()
    {
        //SceneManager.ChangeScene("Forest", SceneManager.SceneOption.WithSound);
    }

   

}
