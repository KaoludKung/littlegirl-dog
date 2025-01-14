using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private int  minRandomtime;
    [SerializeField] private int maxRandomtime;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private bool spitalBlend;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayRandomSound());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopAllCoroutines();
        }

    }

    IEnumerator PlayRandomSound()
    {
        int r = Random.Range(minRandomtime, maxRandomtime);
        yield return new WaitForSeconds(r);
        SoundFXManager.instance.PlayRandomSoundFXClip(clips, transform, spitalBlend, 1.0f, minDistance, maxDistance);

        yield return new WaitForSeconds(3.5f);
        StartCoroutine(PlayRandomSound());
    }
}
