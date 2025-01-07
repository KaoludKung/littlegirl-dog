using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : EventObject
{
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject uiManagerObject;
    
    private GirlControl girlControl;
    private DogControl dogControl;

    private void Awake()
    {
        girlControl = FindObjectOfType<GirlControl>();
        dogControl = FindObjectOfType<DogControl>();
    }

    void Start()
    {
        if (dogControl != null && girlControl != null && uiManagerObject != null)
        {
            uiManagerObject.SetActive(false);
            dogControl.SetIsStart(false);
            girlControl.SetIsStart(false);
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
        
        if(dogControl != null && girlControl != null && uiManagerObject != null)
        {
            uiManagerObject.SetActive(true);
            dogControl.SetIsStart(true);
            girlControl.SetIsStart(true);
        }

    }
}
