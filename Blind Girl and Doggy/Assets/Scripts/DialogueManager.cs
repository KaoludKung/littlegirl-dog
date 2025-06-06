using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : EventObject
{
    [SerializeField] private float durationUnlock = 0;
    [SerializeField] private Conversation conversation;
    [SerializeField] private DialogueOption dialogueOption;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject uiManagerObject;
    [SerializeField] private Animator[] animators;
    [SerializeField] List<ItemActive> itemActives;
    [SerializeField] private bool resetAnimation = false;
    [SerializeField] private bool playerEnable = false;

    private float speed;
    private int index;
    private int charIndex;
    private bool started;
    private bool waitForNext;
    private bool isWriting;
    private bool isSkipping;

    private void Awake()
    {
        Time.timeScale = 0;

        if (PlayerPrefs.HasKey("speedText"))
        {
            speed = PlayerPrefs.GetFloat("speedText");
        }
        else
        {
            PlayerPrefs.SetFloat("speedText", 0.05f);
            speed = PlayerPrefs.GetFloat("speedText");
        }

        if (speed == 0.05)
        {
            dialogueUI.speedText.text = LocalizationManager.Instance.GetText(25, PlayerDataManager.Instance.GetLanguage());
        }
        else
        {
            dialogueUI.speedText.text = LocalizationManager.Instance.GetText(26, PlayerDataManager.Instance.GetLanguage());
        }

        dialogueUI.ResizeText();
    }


    void Start()
    {
        CharacterManager.Instance.SetIsActive(false);
        CharacterManager.Instance.SetActiveUIPlayer(false);
        CharacterManager.Instance.StopMoving();

        if (uiManagerObject != null)
            uiManagerObject.SetActive(false);

        if (!started && !EventManager.Instance.IsEventTriggered(eventID))
            return;

        started = true;
        GetDialogue(0);

    }

    void Update()
    {
        if (InputManager.Instance.IsEnterPressed())
        {
            if (started)
            {
                if (speed == 0.05f)
                {
                    speed = 0.015f;
                    PlayerPrefs.SetFloat("speedText", speed);
                    //speed = PlayerPrefs.GetFloat("speedText");
                    dialogueUI.speedText.text = LocalizationManager.Instance.GetText(26, PlayerDataManager.Instance.GetLanguage());
                }
                else
                {
                    speed = 0.05f;
                    PlayerPrefs.SetFloat("speedText", speed);
                    //speed = PlayerPrefs.GetFloat("speedText");
                    dialogueUI.speedText.text = LocalizationManager.Instance.GetText(25, PlayerDataManager.Instance.GetLanguage());
                }

                //Debug.Log("Speed:" + speed);
            }
        }


        if (InputManager.Instance.IsZPressed())
        {
            if (isWriting)
            {
                isSkipping = true;
            }
            else if (waitForNext)
            {
                dialogueUI.InitializeText();
                dialogueUI.ToggleArrow(false);
                waitForNext = false;
                index++;

                if (index < conversation.lines.Length)
                {
                    GetDialogue(index);
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    private void GetDialogue(int i)
    {
        index = i;
        charIndex = 0;
        isWriting = true;
        isSkipping = false;

        //use english'name before 
        string currentName = conversation.lines[i].character.fullname;
        string currentText = conversation.lines[i].text;


        Sprite currentSprite = conversation.lines[i].character.portrait;

        SetActiveItem(i);

        if (dialogueOption == DialogueOption.FullDisplay)
        {
            foreach (Animator animator in animators)
            {
                if (animator != null && animator.gameObject.name == currentName)
                {
                    animator.SetInteger(conversation.lines[i].character.animationCondition, conversation.lines[i].character.conditionID);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("No Animation");
        }

        //check character's name after played animation
        currentName = PlayerDataManager.Instance.GetLanguage() == 0 ? conversation.lines[i].character.fullname : conversation.lines[i].character.translationsName[PlayerDataManager.Instance.GetLanguage() - 1];

        dialogueUI.ShowDialogue(currentName, currentText, currentSprite, dialogueOption);
        StartCoroutine(Writing());
    }

    void SetActiveItem(int i)
    {
        if (itemActives.Count > 0)
        {
            for (int j = 0; j < itemActives.Count; j++)
            {
                ItemActive option = itemActives[j];

                if (i == option.targetIndex)
                {
                    option.gameObjectActive.SetActive(true);
                }
            }
        }
    }

    IEnumerator Writing()
    {
        dialogueUI.PlaySound(conversation.lines[index].character);

        string lineLanguage = "";

        switch (PlayerDataManager.Instance.GetLanguage())
        {
            case 0:
                lineLanguage = conversation.lines[index].text;
                break;
            case 1:
                lineLanguage = conversation.lines[index].textTH;
                break;
            case 2:
                lineLanguage = conversation.lines[index].textTR;
                break;
        }

        while (charIndex < lineLanguage.Length)
        {
            if (isSkipping)
            {
                dialogueUI.DialogueText.text = lineLanguage;
                break;
            }

            dialogueUI.DialogueText.text += lineLanguage[charIndex];
            charIndex++;

            yield return new WaitForSecondsRealtime(speed);
        }

        dialogueUI.StopSound();
        dialogueUI.ToggleArrow(true);

        yield return new WaitForSecondsRealtime(0.5f);

        isWriting = false;
        waitForNext = true;
    }

    private void EndDialogue()
    {
        started = false;
        waitForNext = false;
        StopAllCoroutines();
        dialogueUI.TogglePanel(false);
        dialogueUI.InitializeText();
        StartCoroutine(UnlockAndContinue());
    }

    private IEnumerator UnlockAndContinue()
    {
        yield return new WaitForSecondsRealtime(durationUnlock);
        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);
        Time.timeScale = 1;

        if (resetAnimation)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                animators[i].SetInteger("IdleVariant", 0);
            }
        }

        if (playerEnable)
        {
            yield return new WaitForSecondsRealtime(1.0f);

            if (uiManagerObject != null)
                uiManagerObject.SetActive(true);

            CharacterManager.Instance.SetIsActive(true);
            CharacterManager.Instance.SetActiveUIPlayer(true);
        }

    }
}

public enum DialogueOption
{
    TextOnly,
    FullDisplay
}

[System.Serializable]
public class ItemActive
{
    public GameObject gameObjectActive;
    public int targetIndex;
}