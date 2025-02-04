using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    private Animator radioAnimator;
    private AudioSource radioSource;

    // Start is called before the first frame update
    void Start()
    {
        radioAnimator = GetComponent<Animator>();
        radioSource = GetComponent<AudioSource>();
        radioAnimator.speed = 0f;

        StartCoroutine(PlayRadio());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator PlayRadio()
    {
        radioAnimator.speed = 1f;
       
        if (!radioSource.isPlaying)
        {
            radioSource.Play();
        }

        int r = Random.Range(30, 45);

        yield return new WaitForSeconds(r);

        radioSource.Stop();
        radioAnimator.speed = 0f;

        StartCoroutine(PlayRadio());
    }
}
