using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Loading : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textLoading;
    private string baseText = "Loading";
    private bool isLoading = true;
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
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
            for (int i = 0; i <= baseText.Length; i++)
            {
                textLoading.text = baseText.Substring(0, i);
                yield return new WaitForSeconds(0.3f);
            }

            for (int i = 0; i < 3; i++)
            {
                textLoading.text += "."; // ???????????????
                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(0.5f);
            textLoading.text = "";       
        }

    }
}

