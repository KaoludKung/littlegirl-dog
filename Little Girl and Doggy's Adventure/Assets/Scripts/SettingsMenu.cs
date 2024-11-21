using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown fullscreenDropdown;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundFXSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private AudioClip closeClip;
    [SerializeField] private SoundMixerManager soundMixerManager;

    private List<Resolution> resolutions = new List<Resolution>();
    private float mouseSensitivity;

    private void Start()
    {
        PopulateResolutionDropdown();
        PopulateFullscreenDropdown();
        LoadSettings();
        LoadScreenSettings();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenDropdown.onValueChanged.AddListener(SetFullscreenMode);
        masterSlider.onValueChanged.AddListener(value => soundMixerManager.SetMasterVolume(value));
        musicSlider.onValueChanged.AddListener(value => soundMixerManager.SetMusicVolume(value));
        soundFXSlider.onValueChanged.AddListener(value => soundMixerManager.SetSoundFXVolume(value));
        sensitivitySlider.onValueChanged.AddListener(UpdateMouseSensitivity);
        closeButton.onClick.AddListener(() => CloseSetting());
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 0.1f;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 0.1f;
    }

    private void UpdateMouseSensitivity(float value)
    {
        mouseSensitivity = Mathf.Max(value, 0.1f) * 20;
        PlayerPrefs.SetFloat("mouseSensitivity", value);
     //Debug.Log("Mouse Sensitivity: " + mouseSensitivity);
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("mouseSensitivity"))
        {
            float mouseValue = PlayerPrefs.GetFloat("mouseSensitivity");
            sensitivitySlider.value = mouseValue;
            mouseSensitivity = mouseValue;
        }
        else
        {
            sensitivitySlider.value = 5.0f;
        }

        if (PlayerPrefs.HasKey("masterVolume"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        if (PlayerPrefs.HasKey("soundFXVolume"))
        {
            soundFXSlider.value = PlayerPrefs.GetFloat("soundFXVolume");
        }

    }

    private void LoadScreenSettings()
    {
        if (PlayerPrefs.HasKey("resolutionIndex"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("resolutionIndex");
            if (resolutionIndex >= 0 && resolutionIndex < resolutions.Count)
            {
                resolutionDropdown.value = resolutionIndex;
                SetResolution(resolutionIndex);
            }
        }
        else
        {
            PlayerPrefs.SetInt("resolutionIndex", 1);
            SetResolution(1);
        }

        if (PlayerPrefs.HasKey("fullscreenMode"))
        {
            int fullscreenModeIndex = PlayerPrefs.GetInt("fullscreenMode");
            if (fullscreenModeIndex >= 0 && fullscreenModeIndex < fullscreenDropdown.options.Count)
            {
                fullscreenDropdown.value = fullscreenModeIndex;
                SetFullscreenMode(fullscreenModeIndex);
            }
        }
        else
        {
            PlayerPrefs.SetInt("fullscreenMode", 0);
            SetFullscreenMode(0); 
        }
    }

    private void PopulateResolutionDropdown()
    {
        resolutions.Clear();
        resolutionDropdown.ClearOptions();

        List<Resolution> customResolutions = new List<Resolution>
    {
        new Resolution { width = 2560, height = 1440 },
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 800, height = 600 }
    };

        resolutions.AddRange(customResolutions);

        List<string> options = new List<string>();
        foreach (Resolution resolution in customResolutions)
        {
            options.Add(resolution.width + " x " + resolution.height);
        }

        resolutionDropdown.AddOptions(options);
        int currentIndex = customResolutions.FindIndex(r => r.width == Screen.width && r.height == Screen.height);
        resolutionDropdown.value = (currentIndex >= 0) ? currentIndex : 1;
      //Debug.Log("Available Resolutions: " + options.Count);
    }


    private void PopulateFullscreenDropdown()
    {
        fullscreenDropdown.ClearOptions();
        List<string> options = new List<string> { "Fullscreen", "Windowed", "Borderless" };
        fullscreenDropdown.AddOptions(options);

        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                fullscreenDropdown.value = 0;
                break;
            case FullScreenMode.Windowed:
                fullscreenDropdown.value = 1;
                break;
            case FullScreenMode.FullScreenWindow:
                fullscreenDropdown.value = 2;
                break;
        }

        fullscreenDropdown.RefreshShownValue();

    }

    public void SetResolution(int index)
    {
        if (index < 0 || index >= resolutions.Count)
            return;

        Resolution selectedResolution = resolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionIndex", index);
        PlayerPrefs.Save();
     //Debug.Log($"Setting resolution to: {selectedResolution.width} x {selectedResolution.height}");
    }

    public void SetFullscreenMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }

        PlayerPrefs.SetInt("fullscreenMode", index);
        PlayerPrefs.Save();
      //Debug.Log("Fullscreen mode set to: " + fullscreenDropdown.options[index].text);
    }

    public void CloseSetting()
    {
        SoundFXManager.instance.PlaySoundFXClip(closeClip, transform, false, 1.0f);
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
}
