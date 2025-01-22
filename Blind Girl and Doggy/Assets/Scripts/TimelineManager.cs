using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : EventObject
{
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject uiManagerObject;
    [SerializeField] private bool playerEnable = false;

    private DogController dogController;

    private void Awake()
    {
        dogController = FindObjectOfType<DogController>();
        dogController.Animator.SetBool("isWalk", false);
        dogController.StopFootStep();
    }

    void Start()
    {
        CharacterManager.Instance.SetIsActive(false);
        CharacterManager.Instance.SetActiveUIPlayer(false);
       
        if (uiManagerObject != null)
        {
            uiManagerObject.SetActive(false);
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
        StartCoroutine(UnlockPlayer());
       
    }

    private IEnumerator UnlockPlayer()
    {
        if (playerEnable)
        {
            yield return new WaitForSeconds(1.5f);

            CharacterManager.Instance.SetIsActive(true);
            CharacterManager.Instance.SetActiveUIPlayer(true);

            if (uiManagerObject != null)
            {
                uiManagerObject.SetActive(true);
            }
        }
    }
}
