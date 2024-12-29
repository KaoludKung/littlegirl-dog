using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject popUp;
    [SerializeField] private GameObject backgroundNight;
    [SerializeField] private GameObject[] cutsceneObject; // [0] title name, [1] press z, [2] option grid, [3] credit 

    [SerializeField] private List<TitleOption> titleOptions;
    [SerializeField] private Sprite[] defaultIcon;
    [SerializeField] private Sprite[] selectedIcon;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] clips; // [0] select, [1] pressed, [2] glitch

    private Color32 defaultColor = new Color32(102, 46, 72, 255);
    private Color32 selectedColor = new Color32(249, 168, 117, 255);
    private int currentIndex = 0;
    private bool isPressed = false;
    private bool isAlert = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowIntro());
        UpdateMenu();

        if (PlayerDataManager.Instance.GetIsNewGame())
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPressed)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && cutsceneObject[2].activeSelf)
            {
                do
                {
                    currentIndex = (currentIndex + 1) % titleOptions.Count;
                } while (currentIndex == 1 && PlayerDataManager.Instance.GetIsNewGame()); // ???? "Continue" ??????? New Game
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateMenu();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && cutsceneObject[2].activeSelf)
            {
                do
                {
                    currentIndex = (currentIndex - 1 + titleOptions.Count) % titleOptions.Count;
                } while (currentIndex == 1 && PlayerDataManager.Instance.GetIsNewGame()); // ???? "Continue" ??????? New Game
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateMenu();
            }

            if (Input.GetKeyDown(KeyCode.Z) && cutsceneObject[2].activeSelf)
            {
                StartCoroutine(SelectOption());
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && isPressed)
        {
            if (controlPanel.activeSelf)
            {
                controlPanel.SetActive(false);
            }
            else if (settingPanel.activeSelf)
            {
                settingPanel.SetActive(false);
            }

            isPressed = false;
            SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);
        }

        if (isAlert)
        {
            if (Input.GetKeyDown(KeyCode.Z) && popUp.activeSelf)
            {
                popUp.SetActive(false);
                backgroundNight.SetActive(true);
                musicSource.clip = clips[2];
                musicSource.Play();
                StartCoroutine(MoveOn("Prologue", 1.5f));
                PlayerDataManager.Instance.UpdateNewGame(false);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                popUp.SetActive(false);
                isAlert = false;
            }
        }

       
       if (Input.GetKeyDown(KeyCode.Z) && cutsceneObject[1].activeSelf)
       {
           StartCoroutine(PressToStart());
       }
        
    }

    IEnumerator ShowIntro()
    {
        yield return new WaitForSeconds(2.5f);
        cutsceneObject[0].SetActive(true);
        musicSource.Play();
        yield return new WaitForSeconds(2.5f);
        cutsceneObject[1].SetActive(true);
    }

    IEnumerator PressToStart()
    {
        cutsceneObject[1].SetActive(false);
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);
        yield return new WaitForSeconds(clips[1].length);
        cutsceneObject[2].SetActive(true);
        cutsceneObject[3].SetActive(true);
    }

    IEnumerator MoveOn(string name, float extra = 0)
    {
        yield return new WaitForSeconds(clips[1].length + extra);
        SceneManager.instance.ChangeScene(name);
    }


    IEnumerator SelectOption()
    {
        isPressed = true;
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);

        for (int i = 0; i < titleOptions.Count; i++)
        {
            TitleOption option = titleOptions[i];

            if (i == currentIndex)
            {
                option.optionText.color = new Color(option.optionText.color.r, option.optionText.color.g, option.optionText.color.b, 0.5f);
                option.optionIcons.color = new Color(option.optionIcons.color.r, option.optionIcons.color.g, option.optionIcons.color.b, 0.5f);
                option.optionArrow.GetComponent<Image>().color = new Color(option.optionArrow.GetComponent<Image>().color.r, option.optionArrow.GetComponent<Image>().color.g, option.optionArrow.GetComponent<Image>().color.b, 0.5f);

                yield return new WaitForSeconds(0.5f);

                option.optionText.color = new Color(option.optionText.color.r, option.optionText.color.g, option.optionText.color.b, 1.0f);
                option.optionIcons.color = new Color(option.optionIcons.color.r, option.optionIcons.color.g, option.optionIcons.color.b, 1.0f);
                option.optionArrow.GetComponent<Image>().color = new Color(option.optionArrow.GetComponent<Image>().color.r, option.optionArrow.GetComponent<Image>().color.g, option.optionArrow.GetComponent<Image>().color.b, 1.0f);
            }
        }

        yield return new WaitForSeconds(0.3f);

        switch (currentIndex)
        {
            case 0:
                NewGame();
                break;
            case 1:
                if (!PlayerDataManager.Instance.GetIsNewGame()) // Prevent "Continue" if new game
                {
                    Continue();
                }
                else
                {
                    isPressed = false;
                }
                break;
            case 2:
                OpenControls();
                break;
            case 3:
                OpenSettings();
                break;
            case 4:
                QuitGame();
                break;
            default:
                break;
        }
    }


    void UpdateMenu()
    {
        // ???????????????????????????????????????
        if (currentIndex == 1 && PlayerDataManager.Instance.GetIsNewGame())
        {
            currentIndex = (currentIndex + 1) % titleOptions.Count; // ???????????????????
        }

        for (int i = 0; i < titleOptions.Count; i++)
        {
            TitleOption option = titleOptions[i];

            if (i == currentIndex)
            {
                if (i == 1 && PlayerDataManager.Instance.GetIsNewGame()) // Disable "Continue" if new game
                {
                    option.optionText.color = new Color(option.optionText.color.r, option.optionText.color.g, option.optionText.color.b, 0.5f);
                    option.optionIcons.color = new Color(option.optionIcons.color.r, option.optionIcons.color.g, option.optionIcons.color.b, 0.5f);
                    option.optionArrow.SetActive(false);
                }
                else
                {
                    option.optionText.color = selectedColor;
                    option.optionIcons.sprite = selectedIcon[i];
                    option.optionArrow.SetActive(true);
                }
            }
            else
            {
                if (i == 1 && PlayerDataManager.Instance.GetIsNewGame()) // Ensure "Continue" remains disabled
                {
                    option.optionText.color = new Color(option.optionText.color.r, option.optionText.color.g, option.optionText.color.b, 0.5f);
                    option.optionIcons.color = new Color(option.optionIcons.color.r, option.optionIcons.color.g, option.optionIcons.color.b, 0.5f);
                    option.optionArrow.SetActive(false);
                }
                else
                {
                    option.optionText.color = defaultColor;
                    option.optionIcons.sprite = defaultIcon[i];
                    option.optionArrow.SetActive(false);
                }
            }
        }
    }

    void NewGame()
    {
        if (PlayerDataManager.Instance.GetIsNewGame())
        {
            backgroundNight.SetActive(true);
            musicSource.clip = clips[2];
            musicSource.Play();
            StartCoroutine(MoveOn("Prologue", 1.5f));
            PlayerDataManager.Instance.UpdateNewGame(false);          
        }
        else
        {
            popUp.SetActive(true);
            isAlert = true;
        }    
    }

    void Continue()
    {
        if (!PlayerDataManager.Instance.GetIsNewGame())
        {
            string sceneName = PlayerDataManager.Instance.GetSceneName();
            StartCoroutine(MoveOn(sceneName));
            Debug.Log("Continue");
        }
    }

    void OpenControls()
    {
        controlPanel.SetActive(true);
    }

    void OpenSettings()
    {
        settingPanel.SetActive(true);
    }

    void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
        Debug.Log("Quit Game");
    }

}

[System.Serializable]
public class TitleOption
{
    public TextMeshProUGUI optionText;
    public Image optionIcons;
    public GameObject optionArrow;
    
}
