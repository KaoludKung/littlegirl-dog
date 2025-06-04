using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject popUp;

    [Header("Objects In Scene")]
    [SerializeField] private GameObject[] SpriteInScene; // [0] dog, [1] girl, [2] picture (true end)
    [SerializeField] private GameObject[] backgroundNight;
    [SerializeField] private GameObject[] cutsceneObject; // [0] title name, [1] press z, [2] option grid, [3] credit 
    [SerializeField] private Animator[] animator;

    [Header("Title Menu Settings")]
    [SerializeField] private List<TitleOption> titleOptions;
    [SerializeField] private Sprite[] defaultIcon;
    [SerializeField] private Sprite[] selectedIcon;

    [Header("Sound Settings")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] clips; // [0] select, [1] pressed, [2] glitch

    private Color32 defaultColor = new Color32(102, 46, 72, 255);
    private Color32 selectedColor = new Color32(249, 168, 117, 255);
    private int currentIndex = 0;
    private int currentLanguage = 0;
    private bool isPressed = false;
    private bool isAlert = false;

    private float localLastMoveTime = 0f;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void StartWalking()
    {
        Time.timeScale = 1;

        for(int i = 0; i < animator.Length; i++)
        {
            if (animator[i].gameObject.activeSelf)
            {
                animator[i].SetBool("isWalk", true);
            }
        }
    }

    private void ShowSprite()
    {
        //Get a true ending
        if (AchievementManager.Instance.GetAchievementIsCollected(10))
        {
            SpriteInScene[2].SetActive(true);
        }
        else
        {
            for(int i = 0; i < SpriteInScene.Length - 1; i++)
            {
                SpriteInScene[i].SetActive(true);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowSprite();

        if (PlayerDataManager.Instance.GetIsSecret())
            PlayerDataManager.Instance.UpdateSecrets(false);

        Invoke(nameof(StartWalking), 0.3f);
        StartCoroutine(ShowIntro());
        UpdateMenu();
        musicSource.PlayDelayed(1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPressed)
        {
            if (InputManager.Instance.IsRightPressed(ref localLastMoveTime) && cutsceneObject[2].activeSelf)
            {
                do
                {
                    currentIndex = (currentIndex + 1) % titleOptions.Count;
                } while (currentIndex == 1 && PlayerDataManager.Instance.GetIsNewGame()); // ???? "Continue" ??????? New Game
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateMenu();
            }
            else if (InputManager.Instance.IsLeftPressed(ref localLastMoveTime) && cutsceneObject[2].activeSelf)
            {
                do
                {
                    currentIndex = (currentIndex - 1 + titleOptions.Count) % titleOptions.Count;
                } while (currentIndex == 1 && PlayerDataManager.Instance.GetIsNewGame()); // ???? "Continue" ??????? New Game
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateMenu();
            }

            if (InputManager.Instance.IsZPressed() && cutsceneObject[2].activeSelf)
            {
                StartCoroutine(SelectOption());
            }
        }

        if (InputManager.Instance.IsXPressed() && isPressed)
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
            if (InputManager.Instance.IsZPressed() && popUp.activeSelf)
            {
                popUp.SetActive(false);
                backgroundNight[0].SetActive(true);
                backgroundNight[1].SetActive(true);
                musicSource.clip = clips[2];
                musicSource.Play();

                currentLanguage = PlayerDataManager.Instance.GetLanguage();
                PlayerDataManager.Instance.DeletePlayerData();
                InventoryManager.Instance.DeleteInventory();
                NoteManager.Instance.DeleteNote();
               
                StartCoroutine(MoveOn("Prologue", 2.0f));
                PlayerDataManager.Instance.UpdateNewGame(false);
                PlayerDataManager.Instance.UpdateLanguage(currentLanguage);
            }
            else if (InputManager.Instance.IsXPressed())
            {
                popUp.SetActive(false);
                isAlert = false;
            }
        }

       
       if (InputManager.Instance.IsZPressed() && cutsceneObject[1].activeSelf)
       {
           StartCoroutine(PressToStart());
       }
        
    }

    IEnumerator ShowIntro()
    {
        yield return new WaitForSeconds(1.5f);
        cutsceneObject[0].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        cutsceneObject[1].SetActive(true);
        //cutsceneObject[4].SetActive(true);
        //cutsceneObject[5].SetActive(true);
    }

    IEnumerator PressToStart()
    {
        cutsceneObject[1].SetActive(false);
        //cutsceneObject[4].SetActive(false);
        //cutsceneObject[5].SetActive(false);
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
        if (currentIndex == 1 && PlayerDataManager.Instance.GetIsNewGame())
        {
            currentIndex = (currentIndex + 1) % titleOptions.Count;
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
            backgroundNight[0].SetActive(true);
            backgroundNight[1].SetActive(true);
            musicSource.clip = clips[2];
            musicSource.Play();
            
            StartCoroutine(MoveOn("Prologue", 1.5f));
            currentLanguage = PlayerDataManager.Instance.GetLanguage();
            PlayerDataManager.Instance.UpdateLanguage(currentLanguage);
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
        //UnityEditor.EditorApplication.isPlaying = false;
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
