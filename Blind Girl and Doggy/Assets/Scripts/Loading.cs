using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class Loading : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textLoading;
    //private string baseText = "Loading";
    private bool isLoading = true;
    private string sceneName;

    private string streamingAssetsPathA;
    private string streamingAssetsPathB;
    private string streamingAssetsPathC;
    private string streamingAssetsPathD;
    private string streamingAssetsPathE;

    private void Awake()
    {
        /// streamingAssetsPath = persistentDataPath
        Time.timeScale = 1;
        streamingAssetsPathA = Path.Combine(Application.persistentDataPath, "playerData.json");
        streamingAssetsPathB = Path.Combine(Application.persistentDataPath, "eventData.json");
        streamingAssetsPathC = Path.Combine(Application.persistentDataPath, "note.json");
        streamingAssetsPathD = Path.Combine(Application.persistentDataPath, "inventory.json");
        streamingAssetsPathE = Path.Combine(Application.persistentDataPath, "achievement.json");

        PlayerDataManager.Instance.LoadPlayerData(streamingAssetsPathA);
        EventManager.Instance.LoadEventData(streamingAssetsPathB);
        NoteManager.Instance.LoadNote(streamingAssetsPathC);
        InventoryManager.Instance.LoadInventory(streamingAssetsPathD);
        AchievementManager.Instance.LoadAchievement(streamingAssetsPathE);
    }

    // Start is called before the first frame update
    void Start()
    {
        textLoading.fontSizeMin = PlayerDataManager.Instance.GetLanguage() == 1 ? 52 : 72;

        sceneName = PlayerDataManager.Instance.GetSceneName();
        Debug.Log(sceneName);
        StartCoroutine(LoadSceneASycn());
    }

    private IEnumerator LoadSceneASycn()
    {
        float startTime = Time.time;
        AsyncOperation loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;
        StartCoroutine(TypeLoadingText());

        while (!loadOperation.isDone)
        {
            if (loadOperation.progress >= 0.9f)
            {
                if (Time.time - startTime >= 5.0f)
                {
                    isLoading = false;
                    loadOperation.allowSceneActivation = true;
                }
            }
            yield return null;
        }    
    }

    private IEnumerator TypeLoadingText()
    {
        while (isLoading)
        {
            for (int i = 0; i <= LocalizationManager.Instance.GetText(24, PlayerDataManager.Instance.GetLanguage()).Length; i++)
            {
                textLoading.text = LocalizationManager.Instance.GetText(24, PlayerDataManager.Instance.GetLanguage()).Substring(0, i);
                yield return new WaitForSeconds(0.3f);
            }

            for (int i = 0; i < 3; i++)
            {
                textLoading.text += ".";
                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(0.5f);
            textLoading.text = "";       
        }

    }
}

