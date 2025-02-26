using System.Collections;
using UnityEngine;

public class CheatCode : MonoBehaviour
{
    [SerializeField] private CheatType cheatType;
    [SerializeField] private string cheatCode = "UP,UP,DOWN,DOWN,LEFT,RIGHT,LEFT,RIGHT,X,Z,";
    [SerializeField] private AudioClip unlockClips;
    private string currentInput = "";
    private float inputDelay = 0.2f;

    private void Start()
    {
        Debug.Log("You can type cheat code");
    }

    void Update()
    {
        if (Time.time % inputDelay < 0.1f)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                AddInput("UP");
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                AddInput("DOWN");
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                AddInput("LEFT");
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                AddInput("RIGHT");
            else if (Input.GetKeyDown(KeyCode.X))
                AddInput("X");
            else if (Input.GetKeyDown(KeyCode.Z))
                AddInput("Z");
            else if (Input.GetKeyDown(KeyCode.Return))
                AddInput("START");
        }
    }

    void AddInput(string key)
    {
        currentInput += key + ",";
        Debug.Log(currentInput);

        if (currentInput == cheatCode)
        {
            ActivateCheat();
            currentInput = "";
        }
        else if (!cheatCode.StartsWith(currentInput))
        {
            currentInput = "";
            Debug.Log("Reset Try Again");
        }
    }

    void ActivateCheat()
    {
        Debug.Log("Cheat Activated!");

        if (cheatType == CheatType.Hint)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Hints");
        }
        else if(cheatType == CheatType.Unlock && PlayerPrefs.GetInt("UnlockSecret") == 0)
        {
            PlayerPrefs.SetInt("UnlockSecret", 1);

            if(unlockClips != null)
                SoundFXManager.instance.PlaySoundFXClip(unlockClips, transform, false, 0.7f);
        }
        else
        {
            Debug.Log("Nothing");
        }
    }
}

public enum CheatType
{
    Hint,
    Unlock
}
