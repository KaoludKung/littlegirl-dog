using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Intro : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] introText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoToTitle());
    }

    IEnumerator GoToTitle()
    {
        yield return new WaitForSeconds(6.0f);

        introText[0].text = "Player Tips!!!";
        //introText[1].enableWordWrapping = true;
        //introText[1].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1500f);
        introText[1].text = "Put on headphones to experience more immersive sound and enhance the excitement of your gameplay!";

        yield return new WaitForSeconds(5.0f);

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Title");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
}
