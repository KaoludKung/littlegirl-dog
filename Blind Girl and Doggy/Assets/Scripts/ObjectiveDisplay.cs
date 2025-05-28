using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveDisplay : EventObject
{
    [SerializeField] private int objectiveID;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private GameObject uiManager;
    [SerializeField] bool isEnable = false;

    private string objectiveData;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        objectiveText.text = "";
        objectiveText.fontSizeMax = PlayerDataManager.Instance.GetLanguage() == 1 ? 42 : 48;
        objectiveData = LocalizationManager.Instance.GetText(57, PlayerDataManager.Instance.GetLanguage()) + LocalizationManager.Instance.GetText(objectiveID, PlayerDataManager.Instance.GetLanguage());
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(TypeObjectiveText());
    }

    /*
    private void Update()
    {
        if (UIManager.Instance.IsAnyUIActive)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }
    */

    private IEnumerator TypeObjectiveText()
    {
        audioSource.Play();

        for (int i = 0; i <= objectiveData.Length; i++)
        {
            objectiveText.text = objectiveData.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }

        audioSource.Stop();
        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);
        yield return new WaitForSeconds(3.0f);
        objectiveText.text = "";
        
        if (isEnable)
        {
            CharacterManager.Instance.SetIsActive(true);
            CharacterManager.Instance.SetActiveUIPlayer(true);
            uiManager.SetActive(true);
        }

    }
}
