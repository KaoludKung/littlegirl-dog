using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    [SerializeField] GameObject autoText;
    [SerializeField] GameObject triggerObject;
    private string previousScene;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("PreviousScene"))
        {
            PlayerPrefs.SetString("PreviousScene", "None");
        }

        previousScene = PlayerPrefs.GetString("PreviousScene");
        Debug.Log("Previous Scene: " + previousScene);

        StartCoroutine(autoSave());
    }

    IEnumerator autoSave()
    {
        if(previousScene != PlayerDataManager.Instance.GetSceneName())
        {
            PlayerPrefs.SetString("PreviousScene", PlayerDataManager.Instance.GetSceneName());
            autoText.SetActive(true);
            yield return new WaitForSeconds(3.0f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        autoText.SetActive(false);
        triggerObject.SetActive(true);
    }       

}
