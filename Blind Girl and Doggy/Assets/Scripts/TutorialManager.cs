using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : EventObject
{
    
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;

    // 0: next 1: previous 2: exit
    [SerializeField] private GameObject[] tutorialIcon;
    [SerializeField] private int[] tutorialDataID;
    [SerializeField] private Sprite[] tutorialImage;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private GameObject uiManagerObject;
    [SerializeField] private bool playerEnable = false;

    private int currentIndex = 0;
    private DogController dogController;
    private float localLastMoveTime = 0f;


    void Start()
    {
        dogController = FindAnyObjectByType<DogController>();

        CharacterManager.Instance.SetIsActive(false);
        CharacterManager.Instance.SetActiveUIPlayer(false);
        CharacterManager.Instance.SoundPause();

        if (uiManagerObject != null)
            uiManagerObject.SetActive(false);

        tutorialIcon[0].SetActive(false);
        tutorialPanel.SetActive(true);
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);
        UpdateMenu();
    }

    void Update()
    {
        if (tutorialPanel.activeSelf)
        {
            uiManagerObject.SetActive(false);
        }

        if (InputManager.Instance.IsLeftPressed(ref localLastMoveTime) && tutorialPanel.activeSelf && currentIndex != 0)
        {
            currentIndex = (currentIndex - 1);
            SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
            UpdateMenu();
        }
        else if (InputManager.Instance.IsRightPressed(ref localLastMoveTime) && tutorialPanel.activeSelf && currentIndex != tutorialImage.Length - 1)
        {
            currentIndex = (currentIndex + 1);
            SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1);
            UpdateMenu();
        }

        if (InputManager.Instance.IsXPressed() && tutorialPanel.activeSelf && tutorialIcon[2].activeSelf)
        {
            StartCoroutine(ClosePopUp());
        }
    }

    IEnumerator ClosePopUp()
    {
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1);
        tutorialPanel.SetActive(false);
        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);

        yield return new WaitForSeconds(0.5f);

        if (playerEnable)
        {
            dogController.Animator.SetBool("isWalk", false);
            
            CharacterManager.Instance.SetIsActive(true);
            CharacterManager.Instance.SetActiveUIPlayer(true);
            CharacterManager.Instance.SoundUnPause();
            
            if (uiManagerObject != null)
                uiManagerObject.SetActive(true);
        }

        yield return null;
        Destroy(gameObject);
    }

    public void UpdateMenu()
    {
        for (int i = 0; i < tutorialImage.Length; i++)
        {
            if(i == currentIndex)
            {
                tutorialPanel.GetComponent<Image>().sprite = tutorialImage[i];
                tutorialText.text = LocalizationManager.Instance.GetText(tutorialDataID[i], PlayerDataManager.Instance.GetLanguage()).Replace("\\n", "\n");
            }
        }

        if (currentIndex == 0)
        {
            tutorialIcon[0].SetActive(false);
            tutorialIcon[1].SetActive(true);
            tutorialIcon[2].SetActive(false);
        }
        else if (currentIndex == tutorialImage.Length - 1)
        {
            tutorialIcon[0].SetActive(true);
            tutorialIcon[1].SetActive(false);
            tutorialIcon[2].SetActive(true);
        }
        else
        {
            tutorialIcon[0].SetActive(true);
            tutorialIcon[1].SetActive(true);
            tutorialIcon[2].SetActive(false);
        }

    }

    public void SetTutorialImage(int index, Sprite s)
    {
        tutorialImage[index] = s;
    }

}


[System.Serializable]
public class ControllerUI
{
    public string keyboardButton;
    public string ps4Button;
    public string xBoxButton;
}

