using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    private Animator radioAnimator;
    private AudioSource radioSource;
    private Hunter hunter;
    private bool isPlay = true;
    public AudioSource RadioSource => radioSource;

    // Start is called before the first frame update
    void Start()
    {
        hunter = FindObjectOfType<Hunter>();
        radioAnimator = GetComponent<Animator>();
        radioSource = GetComponent<AudioSource>();
        radioAnimator.speed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventManager.Instance.IsEventTriggered(65) && isPlay && hunter.CurrentState == HunterState.Radio)
        {
            StartCoroutine(PlayRadio());
        }

        if (EventManager.Instance.IsEventTriggered(86))
        {
            StopAllCoroutines();

            radioSource.Stop();
            radioAnimator.Play("radio", -1, 0f);
            radioAnimator.speed = 0f;

            hunter.setHunterState(HunterState.Final);
        }
    }

    IEnumerator PlayRadio()
    {
        isPlay = false;
        hunter.setHunterState(HunterState.None);
        radioAnimator.speed = 1f;

        if (!radioSource.isPlaying)
        {
            radioSource.Play();
        }

        int r = Random.Range(60, 115);

        yield return new WaitForSeconds(r);

        if(!EventManager.Instance.IsEventTriggered(86))
        {
            int p = Random.Range(1, 4);
            hunter.SetPattrenIndex(p);
            Debug.Log("Patrol Round: " + p);

            yield return null;

            hunter.setHunterState(HunterState.Patrol);
        }

        radioSource.Stop();
        radioAnimator.Play("radio", -1, 0f);
        radioAnimator.speed = 0f;
    }

    public void SetIsPlay(bool p)
    {
        isPlay = p;
    }
}
