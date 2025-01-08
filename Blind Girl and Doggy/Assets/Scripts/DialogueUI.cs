using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image dialogueSprite;
    [SerializeField] private Image arrowSprite;
    [SerializeField] private AudioSource typingSound;

    private bool isPlayingOneShot = false;

    public TextMeshProUGUI DialogueText
    {
        get { return dialogueText; }
        set { dialogueText = value; }
    }

    private void Awake()
    {
        TogglePanel(false);
        InitializeText();
    }

    public void TogglePanel(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void ToggleArrow(bool show)
    {
        arrowSprite.gameObject.SetActive(show);
    }

    public void ShowDialogue(string name, string text, Sprite sprite, DialogueOption option)
    {
        if (option == DialogueOption.FullDisplay)
        {
            dialogueName.text = name;
            dialogueSprite.sprite = sprite;
        }
        else
        {
            dialogueName.text = string.Empty;
            dialogueSprite.sprite = null;
        }

        TogglePanel(true);
    }

    public void InitializeText()
    {
        dialogueName.text = "";
        dialogueText.text = "";
    }

    private IEnumerator ResetOneShotFlag()
    {
        yield return new WaitForSeconds(typingSound.clip.length);
        isPlayingOneShot = false;
    }

    public void PlaySound(Character character)
    {
        if (typingSound != null)
        {
            typingSound.clip = character.typing;

            if (character.fullname == "Dog")
            {
                if (!isPlayingOneShot)
                {
                    isPlayingOneShot = true;
                    typingSound.PlayOneShot(character.typing);
                    Debug.Log("One Shot");
                    StartCoroutine(ResetOneShotFlag());
                }
            }
            else
            {
                if (!typingSound.isPlaying)
                {
                    typingSound.Play();
                    Debug.Log("Loop");
                }
            }
        }
    }

    public void StopSound()
    {
        if (typingSound != null && typingSound.isPlaying)
        {
            StartCoroutine(FadeOutSound());
        }
    }

    private IEnumerator FadeOutSound()
    {
        float startVolume = typingSound.volume;

        while (typingSound.volume > 0)
        {
            typingSound.volume -= startVolume * Time.deltaTime / 0.5f;
            yield return null;
        }

        typingSound.Stop();
        typingSound.volume = startVolume;
    }



}
