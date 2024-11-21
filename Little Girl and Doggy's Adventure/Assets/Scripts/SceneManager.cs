using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : EventObject
{
    [SerializeField] private string sceneName;
    [SerializeField] private AudioClip sceneClip;
    [SerializeField] private SceneOption sceneOption;
    public static SceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (EventManager.Instance.IsEventTriggered(eventID))
        {
            ChangeScene(sceneName,sceneOption);
        }
        else
        {
            Debug.Log("Load Scene Failed");
        }
    }

    public void ChangeScene(string sceneName, SceneOption option)
    {
        PlayerDataManager.Instance.UpdateSceneName(sceneName);
        PlayerDataManager.Instance.SavePlayerData();
 
        if (sceneOption != SceneOption.WithSound)
        {
            StartCoroutine(WaitSoundAndLoadScene());
        }
        else
        {
            EventManager.Instance.SaveEventData();
            InventoryManager.Instance.SaveInventory();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
        }
        
    }

    private IEnumerator WaitSoundAndLoadScene()
    {
        SoundFXManager.instance.PlaySoundFXClip(sceneClip, transform, false, 1.0f);
        yield return new WaitForSeconds(sceneClip.length);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }

    public enum SceneOption
    {
        WithSound,
        WithoutSound
    }
}
