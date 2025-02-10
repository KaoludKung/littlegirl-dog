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
    private string baseText = "It's time to relax!!!";

   
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        StartCoroutine(TypePauseText());
    }

    // Update is called once per frame
    void Update()
    {
        if (pausePanel.activeSelf)
        {
            CharacterManager.Instance.SetIsActive(false);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
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
            if (Input.GetKeyDown(KeyCode.LeftArrow) && pausePanel.activeSelf)
            {
                currentIndex = (currentIndex - 1 + pauseOptions.Count) % pauseOptions.Count;
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateMenu();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && pausePanel.activeSelf)
            {
                currentIndex = (currentIndex + 1) % pauseOptions.Count;
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateMenu();
            }

            if (Input.GetKeyDown(KeyCode.Z) && pausePanel.activeSelf)
            {
                StartCoroutine(SelectOption());
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && isPressed)
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

        if (isAlert && Input.GetKeyDown(KeyCode.Z) && popUp.activeSelf)
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

            for (int i = 0; i <= baseText.Length; i++)
            {
                relaxText.text = baseText.Substring(0, i);
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
