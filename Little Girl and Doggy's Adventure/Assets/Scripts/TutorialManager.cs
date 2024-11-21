using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : EventObject
{
    [SerializeField] private int nextEventID;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private AudioClip closeSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void ClosePopUp()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        SoundFXManager.instance.PlaySoundFXClip(closeSound, transform, false, 1.0f);
        EventManager.Instance.UpdateEventDataTrigger(nextEventID, true);
    }

   
}
