using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : EventObject
{
    [SerializeField] int nextEventID;
    [SerializeField] string objectiveName;
    [SerializeField] int[] actionID;
    [SerializeField] GameObject[] actionObjects;

    [SerializeField] ObjectiveType objectiveType;

    [SerializeField] int cameraID;
    [SerializeField] bool isSwitch;

    private TextMeshProUGUI objectiveText;
    private CameraSwitcher cameraSwitcher;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        cameraSwitcher = FindObjectOfType<CameraSwitcher>();

        GameObject textObject = GameObject.Find("text_objective");

        if (textObject != null)
            objectiveText = textObject.GetComponent<TextMeshProUGUI>();

        objectiveText.canvasRenderer.SetAlpha(0.0f);
        StartCoroutine(ShowObjective());

        if (isSwitch)
        {
            cameraSwitcher.SwitchCamera(cameraID);
        }

        ///If objective type is complterequried, you must set actionobject that you cleart it
        for (int i = 0; i < actionID.Length; i++)
        {
            EventManager.Instance.UpdateEventDataTrigger(actionID[i], true);
            actionObjects[i].gameObject.SetActive(true);
        }

        StartCoroutine(CheckRequired());

    }

    IEnumerator CheckRequired()
    {
        if (objectiveType == ObjectiveType.CompletionRequired)
        {
            count = 0;

            /*
            for (int i = 0; i < actionID.Length; i++)
            {
                bool hasExecuted = EventManager.Instance.IsHasExcuted(actionID[i]);

                Debug.Log($"ActionID: {actionID[i]}, HasExecuted: {hasExecuted}");

                if (hasExecuted)
                {
                    count++;
                }
            }*/

            Debug.Log($"Count: {count}, Required: {actionID.Length}");

            if (count == actionID.Length)
            {
                EventManager.Instance.UpdateEventDataTrigger(nextEventID, true);
            }
        }

        yield return new WaitForSeconds(3.5f);
        StartCoroutine(CheckRequired());
    }

    IEnumerator ShowObjective()
    {
        objectiveText.text = objectiveName;
        objectiveText.CrossFadeAlpha(1f, 3.5f, false);
        
        yield return new WaitForSeconds(3.5f);

        objectiveText.CrossFadeAlpha(0f, 3.5f, false);

        if(objectiveType == ObjectiveType.DisplayOnly)
            EventManager.Instance.UpdateEventDataTrigger(nextEventID, true);
    }

    public enum ObjectiveType
    {
        CompletionRequired,
        DisplayOnly
    }
}
