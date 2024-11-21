using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : EventObject
{
    [SerializeField] private int nextEventID;
    [SerializeField] private float speed;
    [SerializeField] private float durationUnlock = 0;

    [SerializeField] private Conversation conversation;
    [SerializeField] private DialogueOption dialogueOption;

    private int index;
    private int charIndex;
    private bool started;
    private bool waitForNext;
    private bool isWriting;
    private bool isSkipping;

    private GirlControl girlControl;
    private DogControl dogControl;
    private DialogueUI dialogueUI;

    void Start()
    {
        girlControl = FindObjectOfType<GirlControl>();
        dogControl = FindObjectOfType<DogControl>();
        dialogueUI = FindObjectOfType<DialogueUI>();

        if (dogControl != null && girlControl != null)
        {
            dogControl.SetIsStart(false);
            girlControl.SetIsStart(false);
        }

        if (!started && !EventManager.Instance.IsEventTriggered(eventID))
            return;

        started = true;
        GetDialogue(0);
    }

    void Update()
    {
        if (!started || !EventManager.Instance.IsEventTriggered(eventID))
            return;

        if (Input.GetMouseButtonDown(0))
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

        string currentName = conversation.lines[i].character.fullname;
        string currentText = conversation.lines[i].text;
        Sprite currentSprite = conversation.lines[i].character.portrait;

        dialogueUI.ShowDialogue(currentName, currentText, currentSprite, dialogueOption);
        StartCoroutine(Writing());
    }

    IEnumerator Writing()
    {
        dialogueUI.PlaySound(conversation.lines[index].character);

        while (charIndex < conversation.lines[index].text.Length)
        {
            if (isSkipping)
            {
                dialogueUI.DialogueText.text = conversation.lines[index].text;
                break;
            }

            dialogueUI.DialogueText.text += conversation.lines[index].text[charIndex];
            charIndex++;

            yield return new WaitForSeconds(speed);
        }

        dialogueUI.StopSound();
        dialogueUI.ToggleArrow(true);
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

        Invoke("UnlockMovement", durationUnlock);
    }

    private void UnlockMovement()
    {
        EventManager.Instance.UpdateEventDataTrigger(nextEventID, true);

        if (dogControl != null && girlControl != null)
        {
            dogControl.SetIsStart(true);
            girlControl.SetIsStart(true);
        }
    }
}



public enum DialogueOption
{
    TextOnly,
    FullDisplay
}
