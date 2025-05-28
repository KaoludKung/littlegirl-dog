using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Gameover Elements")]
    [SerializeField] private List<GameOverOption> gameoverOptions;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Image gameOverTitle;
    [SerializeField] private Sprite[] gameoverSprite;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private Transform[] character;
    
    [SerializeField] private string sceneName;
    [SerializeField] private int cameraID = 0;

    [Header("Hint Settings")]
    [SerializeField] private TextMeshProUGUI hintText;
    //[SerializeField] private string[] hintsList;

    public static GameOverManager instance;
    private DogController dogController;
    private GirlController girlController;
    private Monster monster;
    private Hunter hunter;
    private CameraSwitcher cameraSwitcher;
    private int currentIndex = 0;
    private bool isPressed = false;
    public bool isActive { get; private set; }
    private float localLastMoveTime = 0f;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateMenu();
        girlController = FindObjectOfType<GirlController>();
        dogController = FindObjectOfType<DogController>();
        cameraSwitcher = FindObjectOfType<CameraSwitcher>();
        monster = FindObjectOfType<Monster>();
        hunter = FindObjectOfType<Hunter>();

    }

    // Update is called once per frame
    void Update()
    {
        if(gameOverPanel.activeSelf)
            CharacterManager.Instance.SetIsActive(false);

        if (PlayerDataManager.Instance.GetHearts() == 0)
        {
            gameoverOptions[0].gameoverText.text = LocalizationManager.Instance.GetText(43, PlayerDataManager.Instance.GetLanguage());
            gameOverTitle.sprite = gameoverSprite[3];
        }
        else
        {
            gameoverOptions[0].gameoverText.text = LocalizationManager.Instance.GetText(44, PlayerDataManager.Instance.GetLanguage());
            gameOverTitle.sprite = gameoverSprite[4];
        }

        if (!isPressed)
        {
            if (InputManager.Instance.IsUpPressed(ref localLastMoveTime) && gameOverPanel.activeSelf)
            {
                currentIndex = (currentIndex - 1 + gameoverOptions.Count) % gameoverOptions.Count;
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateMenu();
            }
            else if (InputManager.Instance.IsDownPressed(ref localLastMoveTime) && gameOverPanel.activeSelf)
            {
                currentIndex = (currentIndex + 1) % gameoverOptions.Count;
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateMenu();
            }

            if (InputManager.Instance.IsZPressed() && gameOverPanel.activeSelf)
            {
                StartCoroutine(SelectOption());
            }
        }
        
    }

    void UpdateMenu()
    {
        for (int i = 0; i < gameoverOptions.Count; i++)
        {
            GameOverOption option = gameoverOptions[i];

            if (i == currentIndex)
            {
                option.buttonImage.sprite = gameoverSprite[1];
            }
            else
            {
                option.buttonImage.sprite = gameoverSprite[0];
            }
        }
    }

    IEnumerator SelectOption()
    {
        isPressed = true;
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);

        for (int i = 0; i < gameoverOptions.Count; i++)
        {
            GameOverOption option = gameoverOptions[i];

            if (i == currentIndex)
            {
                option.buttonImage.sprite = gameoverSprite[2];

                yield return new WaitForSecondsRealtime(0.5f);

                option.buttonImage.sprite = gameoverSprite[2];
            }
        }

        yield return new WaitForSecondsRealtime(0.3f);


        if(PlayerDataManager.Instance.GetHearts() == 0)
        {
            switch (currentIndex)
            {
                case 0:
                    StartCoroutine(Retry());
                    break;
                case 1:
                    StartCoroutine(ExitToMenu());
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (currentIndex)
            {
                case 0:
                    StartCoroutine(Respawn());
                    break;
                case 1:
                    StartCoroutine(ExitToMenu());
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator Retry()
    {
        yield return new WaitForSecondsRealtime(clips[1].length);
        Time.timeScale = 1;
        SceneManager.instance.ChangeScene(sceneName, false);

    }

    IEnumerator Respawn()
    {
        Time.timeScale = 1;

        if (dogController != null)
            dogController.MaxStamina();

        CharacterManager.Instance.StopMoving();
        yield return new WaitForSecondsRealtime(0.2f);

        dogController.SetNewClip(null, true);
        girlController.SetNewClip(null, true);

        character[0].position = PlayerDataManager.Instance.GetDogPosition();
        character[1].position = PlayerDataManager.Instance.GetGirlPosition();

        if (character != null && character.Length > 2 && character[2] != null)
        {
            character[2].position = new Vector3(-5.97f,1.20f, 0f);
        }

        if (cameraSwitcher != null)
            cameraSwitcher.SwitchCamera(cameraID);

        yield return new WaitForSeconds(1.2f);
        isPressed = false;

        foreach (AnimatorControllerParameter parameter in girlController.Animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                girlController.Animator.SetBool(parameter.name, false);
            }
        }

        foreach (AnimatorControllerParameter parameter in dogController.Animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                dogController.Animator.SetBool(parameter.name, false);
            }
        }

        dogController.Animator.SetInteger("BarkType", 0);

        girlController.SetIsInteract(false);
        girlController.ResetCurrentInteractable();

        yield return new WaitForSeconds(0.5f);
        CharacterManager.Instance.SetIsActive(true);
        isActive = false;
        CharacterManager.Instance.UnpauseAllSound();

        if (monster != null)
        {
            monster.SetIsKill(false);
        }

        if (hunter != null && EventManager.Instance.IsEventTriggered(88))
        {
            hunter.ResetHunter(true);
        }else if(hunter != null)
        {
            hunter.ResetHunter(false);
        }

        hintText.text = "";
        OpenPanel();
    }

    void SpawnHints()
    {
        int random = Random.Range(1, 6);

        if(random == 4)
        {
            int hints = Random.Range(0, 2);
            hintText.text = hints == 0 ? LocalizationManager.Instance.GetText(45, PlayerDataManager.Instance.GetLanguage()) : LocalizationManager.Instance.GetText(46, PlayerDataManager.Instance.GetLanguage());
        }

    }

    IEnumerator ExitToMenu()
    {
        yield return new WaitForSecondsRealtime(clips[1].length + 0.5f);
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }

    public void OpenPanel()
    {
        for(int i=0; i< gameoverOptions.Count; i++)
        {
            gameoverOptions[i].gameoverText.fontSizeMax = PlayerDataManager.Instance.GetLanguage() == 1 ? 50 : 60;
        }

        hintText.fontSizeMax = PlayerDataManager.Instance.GetLanguage() == 1 ? 25 : 30;

        if (!gameOverPanel.activeSelf)
        {
            CharacterManager.Instance.SetIsActive(false);
            CharacterManager.Instance.SoundStop();
            CharacterManager.Instance.PauseAllSound();
            SpawnHints();
            Time.timeScale = 0;
        }

        gameOverPanel.SetActive(!gameOverPanel.activeSelf);
    }

    public void SetIsActive(bool g)
    {
        isActive = g;
    }
}

[System.Serializable]
public class GameOverOption
{
    public TextMeshProUGUI gameoverText;
    public Image buttonImage;

}
