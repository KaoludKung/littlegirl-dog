using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class LockPuzzle : MonoBehaviour, Interactable
{
    [SerializeField] private GameObject lockPanel;
    [SerializeField] Sprite alertSprite;
    [SerializeField] private TextMeshProUGUI[] digitText;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private int[] answerDigits = { 6, 0, 9 };
    [SerializeField] private GameObject Gitch;

    private string[] originaText;
    private int[] digits = new int[3];
    private int currentIndex = 0;
    private Color32 defaultColor = new Color32(102, 46, 72, 255);
    private Color32 selectedColor = new Color32(249, 168, 117, 255);

    private GirlController girlController;
    private ActionText actionText;
    public bool isActive { get; private set; }

    private void Start()
    {
        girlController = FindObjectOfType<GirlController>();
        actionText = FindObjectOfType<ActionText>();
        originaText = new string[digitText.Length];
        UpdateNumberPosition();
    }

    private void Update()
    {
        if (lockPanel.activeSelf && isActive)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AdjustDigit(currentIndex, 1);
            }else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                AdjustDigit(currentIndex, -1);
            }


            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentIndex = (currentIndex - 1 + digitText.Length) % digitText.Length;
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateNumberPosition();
            }else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentIndex = (currentIndex + 1) % digitText.Length;
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
                UpdateNumberPosition();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(Delay(true));
            }

        }

    }

    IEnumerator Delay(bool sound)
    {
        if (isActive)
        {
            lockPanel.SetActive(false);
            isActive = false;

            if (sound)
            {
                SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);
            }

            yield return new WaitForSeconds(0.3f);
            girlController.SetIsInteract(false);
            girlController.InteractionIcon.SetActive(true);
            CharacterManager.Instance.SetIsActive(true);
        }
    }

    public void UpdateNumberPosition()
    {
        for (int i = 0; i < digits.Length; i++)
        {
            originaText[i] = Regex.Replace(digitText[i].text, "<.*?>", string.Empty);

            if (i == currentIndex)
            {
                digitText[i].color = selectedColor;
                digitText[i].text = $"<u>{originaText[i]}</u>";
            }
            else
            {
                digitText[i].color = defaultColor;
                digitText[i].text = originaText[i];
            }
        }
    }


    public void AdjustDigit(int digitIndex, int direction)
    {
        if (digitIndex < 0 || digitIndex > 2) return;

        digits[digitIndex] = (digits[digitIndex] + direction + 10) % 10;
        SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1.0f);
        UpdateDigitText(digitIndex);
        CheckCode();
    }

    private void UpdateDigitText(int digitIndex)
    {
        switch (digitIndex)
        {
            case 0:
                digitText[0].text = $"<u>{digits[0].ToString()}</u>";
                break;
            case 1:
                digitText[1].text = $"<u>{digits[1].ToString()}</u>";
                break;
            case 2:
                digitText[2].text = $"<u>{digits[2].ToString()}</u>";
                break;
        }
    }

    private void CheckCode()
    {
        if (digits[0] == answerDigits[0] && digits[1] == answerDigits[1] && digits[2] == answerDigits[2])
        {
            StartCoroutine(Unlock());
        }
    }

    IEnumerator Unlock()
    {
        if (EventManager.Instance.IsEventTriggered(79))
        {
            Debug.Log("Unlocked!");
            SoundFXManager.instance.PlaySoundFXClip(clips[2], transform, false, 1.0f);
            yield return new WaitForSeconds(clips[2].length);
            InventoryManager.Instance.AddItem(13);
            actionText.ActionDisplay("Margarete has gotten the flower coin.");
            StartCoroutine(Delay(false));
            
            yield return new WaitForSeconds(0.3f);
            if (gameObject != null)
            {
                Destroy(gameObject);
            }

        }
        else
        {
            CharacterManager.Instance.PauseAllSound();
            Gitch.SetActive(true);
            SoundFXManager.instance.PlaySoundFXClip(clips[3], transform, false, 1.0f);
            yield return new WaitForSeconds(clips[3].length);
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

    public void Interact()
    {
        if(!lockPanel.activeSelf && !UIManager.Instance.IsAnyUIActive)
        {
            girlController.SetIsInteract(true);
            girlController.InteractionIcon.SetActive(false);
            CharacterManager.Instance.SetIsActive(false);

            StartCoroutine(Open());
        }
    }

    IEnumerator Open()
    {
        isActive = true;
        yield return new WaitForSeconds(0.1f);
        lockPanel.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            girlController.AddInteractSprite(alertSprite);
            //girlController.InteractionIcon.GetComponent<SpriteRenderer>().sprite = alertSprite;
        }
    }
}
