using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skip : MonoBehaviour
{
    [SerializeField] GameObject skipObject;
    [SerializeField] string sceneName;
    private InventoryItem applePieItem;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SkipAppear());
        applePieItem = InventoryManager.Instance.GetItemByID(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(InputManager.Instance.IsShiftPressed() && skipObject.activeSelf)
        {
            if (sceneName != "Mansion" && sceneName != "MansionError")
            {
                //Normal Load Scene
                SceneManager.instance.ChangeScene(sceneName);
            }
            else
            {
                string sceneNameEnding = applePieItem.isCollected ? "Mansion" : "MansionError";
                SceneManager.instance.ChangeScene(sceneNameEnding);
            }

        }
    }

    IEnumerator SkipAppear()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        skipObject.SetActive(true);
    }
}
