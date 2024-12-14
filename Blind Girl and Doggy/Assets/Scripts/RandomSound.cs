using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    private AudioSource audioSoure;
    public AudioClip[] clips;
    private int clipIndex;

    private int randomTime;
    public int minRandomtime;
    public int maxRandomtime;

    // Start is called before the first frame update
    void Start()
    {
        audioSoure = gameObject.GetComponent<AudioSource>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(PlaySound());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
        }

    }

    IEnumerator PlaySound()
    {
        randomTime = Random.Range(minRandomtime, maxRandomtime);
        yield return new WaitForSeconds(randomTime);
        Debug.Log("Time: " + randomTime);

        clipIndex = Random.Range(0, clips.Length - 1);
        audioSoure.volume = Random.Range(0.4f, 0.6f);
        audioSoure.PlayOneShot(clips[clipIndex], 1f);

        StartCoroutine(PlaySound());

    }
}
