using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private List<tabOption> tabOptions;
    [SerializeField] private List<MusicOption> musicOptions;
    [SerializeField] private List<DisplayOption> displayOptions;
    [SerializeField] private Sprite[] optionSprite;
    [SerializeField] private SoundMixerManager soundMixerManager;
    [SerializeField] private GameObject displayPanel;
    [SerializeField] private AudioClip selectedClip;
    [SerializeField] private AudioClip pressedClip;

    private int currentIndexM = 0;
    private int currentIndexD = 0;
    private int currentIndexTab = 0;
    private int currentLanguage;
    private float volumeStep = 0.05f;
    private float localLastMoveTime = 0f;
    private string language;

    void InitializecurrentLanguage()
    {
        currentLanguage = PlayerDataManager.Instance.GetLanguage();
        Debug.Log("Current L: " + currentLanguage);

        switch (currentLanguage)
        {
            case 0:
                language = LocalizationManager.Instance.GetText(100, PlayerDataManager.Instance.GetLanguage());
                break;
            case 1:
                language = LocalizationManager.Instance.GetText(101, PlayerDataManager.Instance.GetLanguage());
                break;
            case 2:
                language = LocalizationManager.Instance.GetText(102, PlayerDataManager.Instance.GetLanguage());
                break;
        }
    }


    void Start()
    {
        if (!PlayerPrefs.HasKey("FPS"))
        {
            PlayerPrefs.SetInt("FPS", 120);
        }

        InitializecurrentLanguage();

        soundMixerManager.InitializeVolumeSettings();
        musicOptions[0].musicFill.fillAmount = PlayerPrefs.GetFloat("masterVolume");
        musicOptions[1].musicFill.fillAmount = PlayerPrefs.GetFloat("musicVolume");
        musicOptions[2].musicFill.fillAmount = PlayerPrefs.GetFloat("soundFXVolume");

        displayOptions[0].optionText.text = language;
        displayOptions[1].optionText.text = PlayerPrefs.GetInt("FPS").ToString();

        UpdateTab();
        UpdateMenuSound();
        UpdateMenuDisplay();
    }

    void Update()
    {
        if (InputManager.Instance.IsQPressed())
        {
            currentIndexTab = (currentIndexTab - 1 + tabOptions.Count) % tabOptions.Count;
            SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
            UpdateTab();
        }
        else if (InputManager.Instance.IsEPressed())
        {
            currentIndexTab = (currentIndexTab + 1) % tabOptions.Count;
            SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
            UpdateTab();
        }


        if (InputManager.Instance.IsUpPressed(ref localLastMoveTime))
        {
            if (!displayPanel.activeSelf)
            {
                currentIndexM = (currentIndexM - 1 + musicOptions.Count) % musicOptions.Count;
                SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
                UpdateMenuSound();
            }
            else
            {
                currentIndexD = (currentIndexD - 1 + displayOptions.Count) % displayOptions.Count;
                SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
                UpdateMenuDisplay();
            }
        }
        else if (InputManager.Instance.IsDownPressed(ref localLastMoveTime))
        {
            if (!displayPanel.activeSelf)
            {
                currentIndexM = (currentIndexM + 1) % musicOptions.Count;
                SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
                UpdateMenuSound();
            }
            else
            {
                currentIndexD = (currentIndexD + 1) % displayOptions.Count;
                SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
                UpdateMenuDisplay();
            }   
        }

        if (InputManager.Instance.IsLeftPressed(ref localLastMoveTime))
        {
            if (!displayPanel.activeSelf)
            {
                AdjustVolume(-volumeStep);
            }
            else if (displayPanel.activeSelf && currentIndexD == 1)
            {
                AdjustFPS(-30);
            }
            else if (displayPanel.activeSelf && currentIndexD == 0)
            {
                ChangeLanguage(-1);
            }


        }
        else if (InputManager.Instance.IsRightPressed(ref localLastMoveTime))
        {
            if (!displayPanel.activeSelf)
            {
                AdjustVolume(volumeStep);
            }else if(displayPanel.activeSelf && currentIndexD == 1)
            {
                AdjustFPS(30);
            }
            else if (displayPanel.activeSelf && currentIndexD == 0)
            {
                ChangeLanguage(1);
            }
        }

    }

    void UpdateTab()
    {
        if (currentIndexTab == 0)
        {
            tabOptions[0].tabBorder.sprite = optionSprite[7];
            tabOptions[1].tabBorder.sprite = optionSprite[6];

            tabOptions[0].symbolTab.sprite = optionSprite[9];
            tabOptions[1].symbolTab.sprite = optionSprite[10];
            displayPanel.SetActive(false);
        }
        else
        {
            tabOptions[0].tabBorder.sprite = optionSprite[6];
            tabOptions[1].tabBorder.sprite = optionSprite[7];

            tabOptions[0].symbolTab.sprite = optionSprite[8];
            tabOptions[1].symbolTab.sprite = optionSprite[11];
            displayPanel.SetActive(true);
        }
    }

    void UpdateMenuSound()
    {
        for (int i = 0; i < musicOptions.Count; i++)
        {
            MusicOption option = musicOptions[i];

            if (i == currentIndexM)
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

    void UpdateMenuDisplay()
    {
        for (int i = 0; i < displayOptions.Count; i++)
        {
            DisplayOption option = displayOptions[i];

            if (i == currentIndexD)
            {
                option.optionTitle.color = new Color32(249, 168, 117, 255);
                option.optionText.color = new Color32(249, 168, 117, 255);
                option.displayBorder.sprite = optionSprite[0];
                option.arrowLeft.sprite = optionSprite[2];
                option.arrowRight.sprite = optionSprite[2];
            }
            else { 
                option.optionTitle.color = new Color32(102, 46, 72, 255);
                option.optionText.color = new Color32(102, 46, 72, 255);
                option.displayBorder.sprite = optionSprite[3];
                option.arrowLeft.sprite = optionSprite[5];
                option.arrowRight.sprite = optionSprite[5];
            }
        }
    }

    void ChangeLanguage(int value)
    {
        currentLanguage = Mathf.Clamp(currentLanguage += value, 0, 2);

        SoundFXManager.instance.PlaySoundFXClip(pressedClip, transform, false, 1);
        PlayerDataManager.Instance.UpdateLanguage(currentLanguage);
        PlayerDataManager.Instance.SavePlayerData();

        switch (currentLanguage)
        {
            case 0:
                language = LocalizationManager.Instance.GetText(100, PlayerDataManager.Instance.GetLanguage());
                break;
            case 1:
                language = LocalizationManager.Instance.GetText(101, PlayerDataManager.Instance.GetLanguage());
                break;
            case 2:
                language = LocalizationManager.Instance.GetText(102, PlayerDataManager.Instance.GetLanguage());
                break;
        }

        //FontChanger.Instance.ChangeSpecificFont(displayOptions[0].optionText, currentLanguage);
        displayOptions[0].optionText.text = language;

        Translate[] objects = FindObjectsOfType<Translate>();

        foreach (Translate obj in objects)
        {
            obj.TranslateAllText();
        }

    }

    void AdjustFPS(int limit)
    {
        int currentFPS = PlayerPrefs.GetInt("FPS");
        currentFPS = Mathf.Clamp(currentFPS += limit, 30, 150);

        Application.targetFrameRate = currentFPS;
        PlayerPrefs.SetInt("FPS", currentFPS);
        displayOptions[1].optionText.text = currentFPS.ToString();
        SoundFXManager.instance.PlaySoundFXClip(pressedClip, transform, false, 1);
    }

    void AdjustVolume(float change)
    {
        float newVolume = 0;

        switch (currentIndexM)
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

        musicOptions[currentIndexM].musicFill.fillAmount = newVolume;
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
    public Image displayBorder;
    public Image arrowLeft;
    public Image arrowRight;
    public TextMeshProUGUI optionTitle;
    public TextMeshProUGUI optionText;
}
