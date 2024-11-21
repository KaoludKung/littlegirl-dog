using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : EventObject
{
    [SerializeField] private int nextEventID;
    [SerializeField] private PlayableDirector timeline;
    
    private GirlControl girlControl;
    private DogControl dogControl;

    private void Awake()
    {
        girlControl = FindObjectOfType<GirlControl>();
        dogControl = FindObjectOfType<DogControl>();
    }

    void Start()
    {
        dogControl.SetIsStart(false);
        girlControl.SetIsStart(false);

        timeline.stopped += OnTimelineStopped;
        StartTimeline();
    }

    void StartTimeline()
    {
        timeline.Play();
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        EventManager.Instance.UpdateEventDataTrigger(nextEventID, true);
        dogControl.SetIsStart(true);
        girlControl.SetIsStart(true);
        Debug.Log("Timeline stopped, isSomethingActive = false");
    }
}
