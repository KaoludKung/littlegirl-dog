using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundFXSlider;
    [SerializeField] private SoundMixerManager soundMixerManager;

    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float masterVolume = PlayerPrefs.GetFloat("masterVolume");
            masterSlider.value = masterVolume;
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("musicVolume");
            musicSlider.value = musicVolume;
        }

        if (PlayerPrefs.HasKey("soundFXVolume"))
        {
            float soundFXVolume = PlayerPrefs.GetFloat("soundFXVolume");
            soundFXSlider.value = soundFXVolume;
        }

        masterSlider.onValueChanged.AddListener(delegate { soundMixerManager.SetMasterVolume(masterSlider.value); });
        musicSlider.onValueChanged.AddListener(delegate { soundMixerManager.SetMusicVolume(musicSlider.value); });
        soundFXSlider.onValueChanged.AddListener(delegate { soundMixerManager.SetSoundFXVolume(soundFXSlider.value); });
    }
}
