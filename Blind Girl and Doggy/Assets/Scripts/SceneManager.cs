using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour 
{
    //[SerializeField] private string sceneName;
    //[SerializeField] private AudioClip sceneClip;
    //[SerializeField] private SceneOption sceneOption;
    public static SceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeScene(string sceneName)
    {
        PlayerDataManager.Instance.UpdateSceneName(sceneName);
        InventoryManager.Instance.SaveInventory();
        PlayerDataManager.Instance.SavePlayerData();
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
