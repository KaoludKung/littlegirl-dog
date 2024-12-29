using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplaySetting : MonoBehaviour
{
    [SerializeField] private List<DisplayOption> displayOptions;
    [SerializeField] private Sprite[] optionSprite;
    [SerializeField] private GameObject musicPanel;
    [SerializeField] private AudioClip selectedClip;
    [SerializeField] private AudioClip pressedClip;

    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
             currentIndex = (currentIndex - 1 + displayOptions.Count) % displayOptions.Count;
             SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
             UpdateMenu();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
             currentIndex = (currentIndex + 1) % displayOptions.Count;
             SoundFXManager.instance.PlaySoundFXClip(selectedClip, transform, false, 1);
             UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {

        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            musicPanel.SetActive(true);
            gameObject.SetActive(false);
        }

    }

    void UpdateMenu()
    {
        for (int i = 0; i < displayOptions.Count; i++)
        {
            DisplayOption option = displayOptions[i];

            if (i == currentIndex)
            {
                option.optionText.color = new Color32(249, 168, 117, 255);
                option.displayText.color = new Color32(226, 135, 89, 255);
                option.displayBorder.sprite = optionSprite[0];
                option.arrowLeft.sprite = optionSprite[1];
                option.arrowRight.sprite = optionSprite[1];
            }
            else
            {
                option.optionText.color = new Color32(102, 46, 72, 255);
                option.displayText.color = new Color32(124, 63, 88, 255);
                option.displayBorder.sprite = optionSprite[2];
                option.arrowLeft.sprite = optionSprite[3];
                option.arrowRight.sprite = optionSprite[3];
            }
        }
    }
}

[System.Serializable]
public class DisplayOption
{
    public Image displayBorder;
    public Image arrowLeft;
    public Image arrowRight;
    public TextMeshProUGUI optionText;
    public TextMeshProUGUI displayText;
}