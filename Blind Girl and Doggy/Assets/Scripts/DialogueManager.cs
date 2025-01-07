using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Animations;

public class DialogueManager : EventObject
{
    [SerializeField] private float speed = 0.05f;
    [SerializeField] private float durationUnlock = 0;
    [SerializeField] private Conversation conversation;
    [SerializeField] private DialogueOption dialogueOption;
    [SerializeField] private Animator[] animators;

    private int index;
    private int charIndex;
    private bool started;
    private bool waitForNext;
    private bool isWriting;
    private bool isSkipping;

    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject uiManagerObject;

    //private GirlControl girlControl;
    //private DogControl dogControl;

    void Start()
    {
        if (uiManagerObject != null)
            uiManagerObject.SetActive(false);

        //girlControl = FindObjectOfType<GirlControl>();
        //dogControl = FindObjectOfType<DogControl>();
        //dialogueUI = FindObjectOfType<DialogueUI>();

        /*
        if (dogControl != null && girlControl != null)
        {
            dogControl.SetIsStart(false);
            girlControl.SetIsStart(false);
        }*/

        if (!started && !EventManager.Instance.IsEventTriggered(eventID))
            return;

        started = true;
        GetDialogue(0);

    }

    void Update()
    {
        /*
        if (!started || !EventManager.Instance.IsEventTriggered(eventID))
            return;
        */

        if (Input.GetKeyDown(KeyCode.Z))
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

        if(dialogueOption == DialogueOption.FullDisplay)
        {
            for (i = 0; i < animators.Length; i++)
            {
                if (animators[i] != null && animators[i].gameObject.name == currentName)
                {
                    animators[i].SetInteger(conversation.lines[i].character.animationCondition, conversation.lines[i].character.conditionID);
                }
            }
        }
        else
        {
            Debug.Log("No Animation");
        }
       

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
        StartCoroutine(UnlockAndContinue());
    }

    
    private IEnumerator UnlockAndContinue()
    {
        yield return new WaitForSeconds(durationUnlock);

        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);

        yield return new WaitForSeconds(2.5f);

        if (uiManagerObject != null)
            uiManagerObject.SetActive(true);

        /*
        if (dogControl != null && girlControl != null)
        {
            dogControl.SetIsStart(true);
            girlControl.SetIsStart(true);
        }*/
    }
}


public enum DialogueOption
{
    TextOnly,
    FullDisplay
}
