using System.Collections;
using UnityEngine;

public class CheatCode : MonoBehaviour
{
    [SerializeField] private CheatType cheatType;
    [SerializeField] private string cheatCode = "UP,UP,DOWN,DOWN,LEFT,RIGHT,LEFT,RIGHT,X,Z,";
    [SerializeField] private AudioClip unlockClips;
    private string currentInput = "";
    private bool Delay = false;

    private void Start()
    {
        Debug.Log("You can type cheat code");
    }

    void Update()
    {
        if (!Delay)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                StartCoroutine(AddInput("UP"));
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                StartCoroutine(AddInput("DOWN"));
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                StartCoroutine(AddInput("LEFT"));
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                StartCoroutine(AddInput("RIGHT"));
            else if (Input.GetKeyDown(KeyCode.X))
                StartCoroutine(AddInput("X"));
            else if (Input.GetKeyDown(KeyCode.Z))
                StartCoroutine(AddInput("Z"));
            else if (Input.GetKeyDown(KeyCode.Return))
                StartCoroutine(AddInput("START"));
        }
    }

    IEnumerator AddInput(string key)
    {
        Delay = true;

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

        yield return new WaitForSecondsRealtime(0.25f);
        Delay = false;
    }

    void ActivateCheat()
    {
        Debug.Log("Cheat Activated!");

        if (cheatType == CheatType.Hint)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Hints");
        }
        else if(cheatType == CheatType.Unlock && !PlayerDataManager.Instance.GetIsSecret())
        {
            PlayerDataManager.Instance.UpdateSecrets(true);
            PlayerDataManager.Instance.SavePlayerData();

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
