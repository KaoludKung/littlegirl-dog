using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActivator : MonoBehaviour
{
    [SerializeField] private float checkInterval;
    [SerializeField] private List<GameObject> eventObjects;
    //[SerializeField] private float activationDelay = 1f; // ?????????????

    private void Start()
    {
        StartCoroutine(CheckAndActivateEvents());
    }

    private IEnumerator CheckAndActivateEvents()
    {
        while (true)
        {
            Debug.Log("Checking events...");

            EventManager eventManager = EventManager.Instance;
            List<EventData> events = eventManager.GetEventList();
            bool allActivated = true;

            eventObjects.RemoveAll(obj => obj == null);

            foreach (EventData eventData in events)
            {
                if (eventData.trigger)
                {
                    foreach (GameObject obj in eventObjects)
                    {
                        EventObject eventObject = obj.GetComponent<EventObject>();
                        if (eventObject != null && eventObject.GetEventId() == eventData.id)
                        {
                            StartCoroutine(ActivateObjectWithDelay(obj)); 
                        }
                    }
                }
            }

            foreach (GameObject obj in eventObjects)
            {
                if (!obj.activeSelf)
                {
                    allActivated = false;
                    break;
                }
            }

            if (allActivated)
            {
                Debug.Log("All event objects have been activated. Stopping EventActivator.");
                yield break;
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private IEnumerator ActivateObjectWithDelay(GameObject obj)
    {
        yield return new WaitForSeconds(0.3f); 
        obj.SetActive(true);
        //Debug.Log($"Activated object: {obj.name}");
    }
}
