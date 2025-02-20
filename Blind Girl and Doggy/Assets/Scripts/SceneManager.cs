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

    public void ChangeScene(string sceneName, bool all = true)
    {
        PlayerDataManager.Instance.UpdateSceneName(sceneName);
        PlayerDataManager.Instance.SavePlayerData();
        //EventManager.Instance.SaveEventData();

        if (all)
        {
            InventoryManager.Instance.SaveInventory();
            NoteManager.Instance.SaveNote();
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");  
    }

    public void ChangeSceneImmediately(string sceneName, bool player = true ,bool all = true)
    {
        PlayerDataManager.Instance.UpdateSceneName(sceneName);

        if(player)
            PlayerDataManager.Instance.SavePlayerData();

        //EventManager.Instance.SaveEventData();

        if (all)
        {
            InventoryManager.Instance.SaveInventory();
            NoteManager.Instance.SaveNote();
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

}
