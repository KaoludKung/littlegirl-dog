using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private List<MusicOption> musicOptions;
    [SerializeField] private Sprite[] optionSprite;
    [SerializeField] private SoundMixerManager soundMixerManager;
    //[SerializeField] private GameObject displayPanel;
    [SerializeField] private AudioClip selectedClip;
    [SerializeField] private AudioClip pressedClip;

    private int currentIndex = 0;
    private float volumeStep = 0.05f;

    void Start()
    {
        soundMixerManager.InitializeVolumeSettings();
        musicOptions[0].musicFill.fillAmount = PlayerPrefs.GetFloat("masterVolume");
        musicOptions[1].musicFill.fillAmount = PlayerPrefs.GetFloat("musicVolume");
        musicOptions[2].musicFill.fillAmount = PlayerPrefs.GetFloat("soundFXVolume");
        UpdateMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex = (currentIndex - 1 + musicOptions.Count) % musicOptions.Count;
            SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
            UpdateMenu();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = (currentIndex + 1) % musicOptions.Count;
            SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AdjustVolume(-volumeStep);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AdjustVolume(volumeStep);
        }

    }

    /*
    IEnumerator SwapTab()
    {
        SoundFXManager.instance.PlaySoundFXClip(pressedClip, transform, false, 1);
        yield return new WaitForSeconds(pressedClip.length);
        displayPanel.SetActive(true);
        gameObject.SetActive(false);
    }*/

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
public class MusicOption
{
    public Image musicBorder;
    public Image musicFill;
    public Image arrowLeft;
    public Image arrowRight;
    public TextMeshProUGUI optionText;
}
