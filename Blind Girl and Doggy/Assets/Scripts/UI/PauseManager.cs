using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject popUp;
    [SerializeField] private TextMeshProUGUI relaxText;
    [SerializeField] private List<PauseOption> pauseOptions;
    [SerializeField] private Sprite[] pauseSprite;
    [SerializeField] private AudioClip[] clips;

    public bool isActive { get; private set; }
    private bool isPaused = false;
    private bool isPressed = false;
    private bool isAlert = false;
    private int currentIndex = 0;
    //private string baseText = "It's time to relax!!!";

    private float localLastMoveTime = 0f;
    private int currentLanguage = -1;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        UpdateFontSizes();
        StartCoroutine(TypePauseText());
    }

    private void UpdateFontSizes()
    {
        for (int i = 0; i < pauseOptions.Count; i++)
        {
            pauseOptions[i].buttonText.fontSizeMax = currentLanguage == 1 ? 42 : 60;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pausePanel.activeSelf)
        {
            CharacterManager.Instance.SetIsActive(false);

            int language = PlayerDataManager.Instance.GetLanguage();
            if (language != currentLanguage)
            {
                currentLanguage = language;
                UpdateFontSizes();
            }
        }

        if (InputManager.Instance.IsEnterPressed())
        {
            if (!isPaused && !pausePanel.activeSelf && !UIManager.Instance.IsAnyUIActive)           
            {
                isActive = true;
                CharacterManager.Instance.SetIsActive(false);
                PauseGame();
            }      
        }

        if (!isPressed && isPaused)
        {
            if (InputManager.Instance.IsLeftPressed(ref localLastMoveTime) && pausePanel.activeSelf)
            {
                currentIndex = (currentIndex - 1 + pauseOptions.Count) % pauseOptions.Count;
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateMenu();
            }
            else if (InputManager.Instance.IsRightPressed(ref localLastMoveTime) && pausePanel.activeSelf)
            {
                currentIndex = (currentIndex + 1) % pauseOptions.Count;
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateMenu();
            }

            if (InputManager.Instance.IsZPressed() && pausePanel.activeSelf)
            {
                StartCoroutine(SelectOption());
            }
        }

        if (InputManager.Instance.IsXPressed() && isPressed)
        {
            if (settingPanel.activeSelf)
            {
                settingPanel.SetActive(false);
            }else if (popUp.activeSelf)
            {
                popUp.SetActive(false);
            }

            isPressed = false;
            SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);
        }

        if (isAlert && InputManager.Instance.IsZPressed() && popUp.activeSelf)
        {
            StartCoroutine(ExitToMenu());
        }

    }

    IEnumerator SelectOption()
    {
        isPressed = true;
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);

        for (int i = 0; i < pauseOptions.Count; i++)
        {
            PauseOption option = pauseOptions[i];

            if (i == currentIndex)
            {
                option.buttonImage.sprite = pauseSprite[2];

                yield return new WaitForSecondsRealtime(0.5f);

                option.buttonImage.sprite = pauseSprite[1];
            }
        }

        yield return new WaitForSecondsRealtime(0.3f);

        switch (currentIndex)
        {
            case 0:
                StartCoroutine(ResumeGame());
                break;
            case 1:
                OpenSetting();
                break;
            case 2:
                OpenExitPopUp();
                break;
            default:
                break;
        }
    }

    void PauseGame()
    {
        CharacterManager.Instance.SoundPause();
        isPaused = !isPaused;
        pausePanel.SetActive(!pausePanel.activeSelf);
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);
        UIManager.Instance.ToggleTimeScale(true);

        StopAllCoroutines();

        if (isPaused)
        {
            StartCoroutine(TypePauseText());
        }
        else
        {
            relaxText.text = "";
        }
    }

    void UpdateMenu()
    {
       for (int i = 0; i < pauseOptions.Count; i++)
       {
           PauseOption option = pauseOptions[i];

           if (i == currentIndex)
           {
               //option.buttonText.color = new Color32(249, 168, 117, 255);
               option.buttonImage.sprite = pauseSprite[1];
           }
           else
           {
               option.buttonText.color = new Color32(235, 107, 111, 255);
               option.buttonImage.sprite = pauseSprite[0];
           }
       }
       
    }

    IEnumerator ResumeGame()
    {
        isPaused = !isPaused;
        isPressed = false;
        yield return new WaitForSecondsRealtime(clips[1].length);
        pausePanel.SetActive(!pausePanel.activeSelf);
        UIManager.Instance.ToggleTimeScale(false);
        isActive = false;
        yield return new WaitForSecondsRealtime(0.3f);
        CharacterManager.Instance.SoundUnPause();

        if (!CharacterManager.Instance.isHiding())
            CharacterManager.Instance.SetIsActive(true);

    }

    void OpenSetting()
    {
        settingPanel.SetActive(true);
    }

    void OpenExitPopUp()
    {
        isAlert = true;
        popUp.SetActive(true);
    }

    IEnumerator ExitToMenu()
    {
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);
        yield return new WaitForSecondsRealtime(clips[1].length + 0.5f);
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    private IEnumerator TypePauseText()
    {
        while (isPaused) 
        {
            relaxText.text = "";

            for (int i = 0; i <= LocalizationManager.Instance.GetText(32, PlayerDataManager.Instance.GetLanguage()).Length; i++)
            {
                relaxText.text = LocalizationManager.Instance.GetText(32, PlayerDataManager.Instance.GetLanguage()).Substring(0, i);
                yield return new WaitForSecondsRealtime(0.2f);
            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

}

[System.Serializable]
public class PauseOption
{
    public Image buttonImage;
    public TextMeshProUGUI buttonText;
}
