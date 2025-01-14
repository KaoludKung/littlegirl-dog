using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoToTitle());
    }

    IEnumerator GoToTitle()
    {
        yield return new WaitForSeconds(5.0f);

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Title");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
}
