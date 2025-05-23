using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingManager : MonoBehaviour
{
    //[SerializeField] private List<tabOption> tabOptions;
    [SerializeField] private List<MusicOption> musicOptions;
    //[SerializeField] private List<DisplayOption> displayOptions;
    [SerializeField] private Sprite[] optionSprite;
    [SerializeField] private SoundMixerManager soundMixerManager;
    //[SerializeField] private GameObject displayPanel;
    [SerializeField] private AudioClip selectedClip;
    [SerializeField] private AudioClip pressedClip;

    private int currentIndex = 0;
    private float volumeStep = 0.05f;
    private float localLastMoveTime = 0f;

    void Start()
    {
        if (!PlayerPrefs.HasKey("language"))
        {
            PlayerPrefs.SetInt("language", 0);
        }

        if (!!PlayerPrefs.HasKey("fps"))
        {
            PlayerPrefs.SetInt("fps", 120);
        }

        soundMixerManager.InitializeVolumeSettings();
        musicOptions[0].musicFill.fillAmount = PlayerPrefs.GetFloat("masterVolume");
        musicOptions[1].musicFill.fillAmount = PlayerPrefs.GetFloat("musicVolume");
        musicOptions[2].musicFill.fillAmount = PlayerPrefs.GetFloat("soundFXVolume");
        UpdateMenu();
    }

    void Update()
    {
        if (InputManager.Instance.IsUpPressed(ref localLastMoveTime))
        {
            currentIndex = (currentIndex - 1 + musicOptions.Count) % musicOptions.Count;
            SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
            UpdateMenu();
        }
        else if (InputManager.Instance.IsDownPressed(ref localLastMoveTime))
        {
            currentIndex = (currentIndex + 1) % musicOptions.Count;
            SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
            UpdateMenu();
        }

        if (InputManager.Instance.IsLeftPressed(ref localLastMoveTime))
        {
            AdjustVolume(-volumeStep);
        }
        else if (InputManager.Instance.IsRightPressed(ref localLastMoveTime))
        {
            AdjustVolume(volumeStep);
        }

    }

    void UpdateMenu()
    {
        for (int i = 0; i < musicOptions.Count; i++)
        {
            MusicOption option = musicOptions[i];

            if (i == currentIndex)
            {
                option.optionText.color = new Color32(249, 168, 117, 255);
                option.musicBorder.sprite = optionSprite[0];
                option.musicFill.sprite = optionSprite[1];
                option.arrowLeft.sprite = optionSprite[2];
                option.arrowRight.sprite = optionSprite[2];
            }
            else
            {
                option.optionText.color = new Color32(102, 46, 72, 255);
                option.musicBorder.sprite = optionSprite[3];
                option.musicFill.sprite = optionSprite[4];
                option.arrowLeft.sprite = optionSprite[5];
                option.arrowRight.sprite = optionSprite[5];
            }
        }
    }

    void AdjustVolume(float change)
    {
        float newVolume = 0;

        switch (currentIndex)
        {
            case 0: // Master Volume
                newVolume = Mathf.Clamp(PlayerPrefs.GetFloat("masterVolume", 1.0f) + change, 0, 1);
                soundMixerManager.SetMasterVolume(newVolume);
                PlayerPrefs.SetFloat("masterVolume", newVolume);
                break;
            case 1: // Music Volume
                newVolume = Mathf.Clamp(PlayerPrefs.GetFloat("musicVolume", 1.0f) + change, 0, 1);
                soundMixerManager.SetMusicVolume(newVolume);
                PlayerPrefs.SetFloat("musicVolume", newVolume);
                break;
            case 2: // SFX Volume
                newVolume = Mathf.Clamp(PlayerPrefs.GetFloat("soundFXVolume", 1.0f) + change, 0, 1);
                soundMixerManager.SetSoundFXVolume(newVolume);
                PlayerPrefs.SetFloat("soundFXVolume", newVolume);
                break;
        }

        musicOptions[currentIndex].musicFill.fillAmount = newVolume;
        SoundFXManager.instance.PlaySoundFXClip(pressedClip, transform, false, 1);
    }

    
}

[System.Serializable]
public class tabOption
{
    public Image tabBorder;
    public Image symbolTab;   
}

[System.Serializable]
public class MusicOption
{
    public Image musicBorder;
    public Image musicFill;
    public Image arrowLeft;
    public Image arrowRight;
    public TextMeshProUGUI optionText;
}

[System.Serializable]
public class DisplayOption
{
    public Image displayorder;
    public Image arrowLeft;
    public Image arrowRight;
    public TextMeshProUGUI optionText;
}
