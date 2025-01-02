using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueShow : MonoBehaviour
{
    [SerializeField] private DialogueOption dialogueOption;
    [SerializeField] private int eventID;
    [SerializeField] private int nexteventID;
    
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image dialogueSprite;
    [SerializeField] private float speed;
    [SerializeField] private AudioSource typingSound;
    public Conversation conversation;

    private int index;
    private int charIndex;
    private bool started;
    private bool waitForNext;

    private void Awake()
    {
        TogglePanel(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!started && !EventManager.Instance.IsEventTriggered(eventID))
            return;

        started = true;
        TogglePanel(true);
        GetDialogue(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!started || !EventManager.Instance.IsEventTriggered(eventID))
            return;

        if (waitForNext && Input.GetMouseButtonDown(0))
        {
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

    private void TogglePanel(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    private void GetDialogue(int i)
    {
        index = i;
        charIndex = 0;
        dialogueText.text = string.Empty;

        if (dialogueOption == DialogueOption.FullDisplay)
        {
            dialogueName.text = conversation.lines[i].character.fullname;
            dialogueSprite.sprite = conversation.lines[i].character.portrait;
        }
        else
        {
            dialogueName.text = string.Empty;
            dialogueSprite.sprite = null;
        }

        StartCoroutine(Writing());
    }

    IEnumerator Writing()
    {
        if (typingSound != null && !typingSound.isPlaying && charIndex == 0)
        {
            typingSound.Play();
        }

        yield return new WaitForSeconds(speed);

        string currentDialogue = conversation.lines[index].text;

        dialogueText.text += currentDialogue[charIndex];

        charIndex++;

        if (charIndex < currentDialogue.Length)
        {
            yield return new WaitForSeconds(speed);
            StartCoroutine(Writing());
        }
        else
        {
            waitForNext = true;

            if (typingSound != null && typingSound.isPlaying)
            {
                typingSound.Stop();
            }
        }

    }

    public void EndDialogue()
    {
        started = false;
        waitForNext = false;
        StopAllCoroutines();
        TogglePanel(false);
        EventManager.Instance.UpdateEventDataTrigger(nexteventID, true);
    }

}


