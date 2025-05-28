using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Intro : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] introText;
    private bool isSkip = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoToTitle());
    }

    private void Update()
    {
        if(!isSkip && (InputManager.Instance.IsZPressed() || InputManager.Instance.IsXPressed() || InputManager.Instance.IsEnterPressed()))
        {
            isSkip = true;
            StartCoroutine(SkipToTitle());    
        }
    }

    IEnumerator GoToTitle()
    {
        yield return new WaitForSeconds(5.0f);

        introText[0].text = LocalizationManager.Instance.GetText(3, PlayerDataManager.Instance.GetLanguage()).Replace("\\n", "\n");
        introText[1].enableWordWrapping = true;
        introText[1].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1500f);
        introText[1].text = LocalizationManager.Instance.GetText(4, PlayerDataManager.Instance.GetLanguage()).Replace("\\n", "\n");

        yield return new WaitForSeconds(5.0f);

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Title");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator SkipToTitle()
    {
        yield return new WaitForSeconds(1.0f);

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Title");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
