using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : EventObject
{
    [SerializeField] string sceneName;
    [SerializeField] float times;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitSoundAndLoadScene());
    }

    private IEnumerator WaitSoundAndLoadScene()
    {
        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);
        yield return new WaitForSeconds(times);
        SceneManager.instance.ChangeScene(sceneName);
    }
}
