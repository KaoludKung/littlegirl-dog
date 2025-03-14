using System.Collections;
using UnityEngine;

public class CheatCode : MonoBehaviour
{
    [SerializeField] private CheatType cheatType;
    [SerializeField] private string cheatCode = "UP,UP,DOWN,DOWN,LEFT,RIGHT,LEFT,RIGHT,X,Z,";
    [SerializeField] private AudioClip unlockClips;
    private string currentInput = "";
    private bool Delay = false;
    private float localLastMoveTime = 0f;

    private void Start()
    {
        Debug.Log("You can type cheat code");
    }

    void Update()
    {
        if (!Delay)
        {
            if (InputManager.Instance.IsUpPressed(ref localLastMoveTime))
                StartCoroutine(AddInput("UP"));
            else if (InputManager.Instance.IsDownPressed(ref localLastMoveTime))
                StartCoroutine(AddInput("DOWN"));
            else if (InputManager.Instance.IsLeftPressed(ref localLastMoveTime))
                StartCoroutine(AddInput("LEFT"));
            else if (InputManager.Instance.IsRightPressed(ref localLastMoveTime))
                StartCoroutine(AddInput("RIGHT"));
            else if (InputManager.Instance.IsXPressed())
                StartCoroutine(AddInput("X"));
            else if (InputManager.Instance.IsZPressed())
                StartCoroutine(AddInput("Z"));
            else if (InputManager.Instance.IsEnterPressed())
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
