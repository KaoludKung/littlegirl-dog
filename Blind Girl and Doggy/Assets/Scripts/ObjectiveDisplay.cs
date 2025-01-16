using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveDisplay : EventObject
{
    [SerializeField] private string objectiveName = "Objective:";
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private GameObject uiManager;
    [SerializeField] bool isEnable = false;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (isEnable)
        {
            uiManager.SetActive(true);
        }

        objectiveText.text = "";
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(TypeObjectiveText());
    }

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


    private IEnumerator TypeObjectiveText()
    {
        audioSource.Play();

        for (int i = 0; i <= objectiveName.Length; i++)
        {
            objectiveText.text = objectiveName.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }

        audioSource.Stop();
        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);
        yield return new WaitForSeconds(3.5f);
        objectiveText.text = "";
        
    }
}
