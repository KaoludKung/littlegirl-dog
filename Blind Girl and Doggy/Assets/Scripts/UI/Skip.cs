using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour
{
    [SerializeField] GameObject skipObject;
    [SerializeField] string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SkipAppear());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) && skipObject.activeSelf)
        {
            SceneManager.instance.ChangeScene(sceneName);
        }
    }

    IEnumerator SkipAppear()
    {
        yield return new WaitForSeconds(5.0f);
        skipObject.SetActive(true);
    }
}
