using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Choice : EventObject
{
    [SerializeField] List<ChoiceOption> choiceOptions;
    [SerializeField] int[] triggerID;
    [SerializeField] Sprite[] chioceSprites;
    [SerializeField] AudioClip[] clips;
    [SerializeField] GameObject choicePanel;

    private int currentIndex = 0;
    private bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        choicePanel.SetActive(true);
        UpdateChoice();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && choicePanel.activeSelf && !isPressed)
        {
            currentIndex = (currentIndex - 1 + choiceOptions.Count) % choiceOptions.Count;
            SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
            UpdateChoice();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && choicePanel.activeSelf && !isPressed)
        {
            currentIndex = (currentIndex + 1) % choiceOptions.Count;
            SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
            UpdateChoice();
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isPressed)
        {
            StartCoroutine(SelectChoice());
        }
    }

    void UpdateChoice()
    {
        for (int i = 0; i < choiceOptions.Count; i++)
        {
            ChoiceOption option = choiceOptions[i];

            if (i == currentIndex)
            {
                option.buttonImage.sprite = chioceSprites[1];
            }
            else
            {
                option.buttonImage.sprite = chioceSprites[0];
            }
        }
    }

    IEnumerator SelectChoice()
    {
        isPressed = true;
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);

        for (int i = 0; i < choiceOptions.Count; i++)
        {
            ChoiceOption option = choiceOptions[i];

            if (i == currentIndex)
            {
                option.buttonImage.sprite = chioceSprites[2];

                yield return new WaitForSecondsRealtime(0.5f);

                option.buttonImage.sprite = chioceSprites[1];

                yield return new WaitForSeconds(0.5f);

                choicePanel.SetActive(false);

                EventManager.Instance.UpdateEventDataTrigger(triggerID[i], true);
            }
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }
}

[System.Serializable]
public class ChoiceOption
{
    public Image buttonImage;
    public TextMeshProUGUI buttonText;
}
