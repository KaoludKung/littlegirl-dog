using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : EventObject
{
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject uiManagerObject;
   
    void Start()
    {
        if (uiManagerObject != null)
        {
            uiManagerObject.SetActive(false);
            CharacterManager.Instance.SetIsActive(false);
        }
    
        timeline.stopped += OnTimelineStopped;
        StartTimeline();
    }

    void StartTimeline()
    {
        timeline.Play();
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);
        
        if(uiManagerObject != null)
        {
            uiManagerObject.SetActive(true);
            CharacterManager.Instance.SetIsActive(true);
        }

    }
}
