using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : EventObject
{
    [SerializeField] string sceneName;
    [SerializeField] GameObject endScreen;
    [SerializeField] float times;
    [SerializeField] bool isLoading = true;

    void Start()
    {
        StartCoroutine(WaitSoundAndLoadScene());
    }

    private IEnumerator WaitSoundAndLoadScene()
    {
        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);

        if(endScreen != null)
        {
            endScreen.SetActive(true);
            yield return new WaitForSeconds(2.2f);
        }

        yield return new WaitForSeconds(times);

        if (isLoading)
        {
            SceneManager.instance.ChangeScene(sceneName);
        }
        else
        {
            SceneManager.instance.ChangeSceneImmediately(sceneName, isLoading, isLoading);
            Debug.Log("No Save Data");
        }
    }
}

