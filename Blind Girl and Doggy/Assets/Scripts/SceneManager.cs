using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour 
{
    public static SceneManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void ChangeScene(string sceneName)
    {
        PlayerDataManager.Instance.UpdateSceneName(sceneName);
        //EventManager.Instance.SaveEventData();
        InventoryManager.Instance.SaveInventory();
        PlayerDataManager.Instance.SavePlayerData();
        NoteManager.Instance.SaveNote();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");  
    }

    /*
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
    */
}
