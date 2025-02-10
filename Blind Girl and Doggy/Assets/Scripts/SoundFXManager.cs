using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    public AudioSource soundFXPrefab;
    //private AudioSource loopAudioSource;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, bool spatialBlend, float volume, float? minDistance = null, float? maxDistance = null)
    {
        AudioSource audioSource = Instantiate(soundFXPrefab, spawnTransform.position, Quaternion.identity);
        audioSource.transform.SetParent(spawnTransform);
        audioSource.clip = audioClip;
        audioSource.volume = volume;

        if (spatialBlend)
        {
            audioSource.spatialBlend = 1.0f;
            audioSource.minDistance = minDistance ?? 1f;
            audioSource.maxDistance = maxDistance ?? 10f;
        }
        else
        {
            audioSource.spatialBlend = 0.0f;
        }

        audioSource.Play();
        float clipLength = audioSource.clip.length;
        //Destroy(audioSource.gameObject, clipLength);
        StartCoroutine(DestroyAudioSource(audioSource.gameObject, clipLength));
    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, bool spatialBlend, bool linaerRoller, float volume, float? minDistance = null, float? maxDistance = null)
    {
        int random = Random.Range(0, audioClip.Length);

        AudioSource audioSource = Instantiate(soundFXPrefab, spawnTransform.position, Quaternion.identity);
        audioSource.transform.SetParent(spawnTransform);
        audioSource.clip = audioClip[random];
        audioSource.volume = volume;

        if (linaerRoller)
        {
            audioSource.rolloffMode = AudioRolloffMode.Linear;
        }

        if (spatialBlend)
        {
            audioSource.spatialBlend = 1.0f;
            audioSource.minDistance = minDistance ?? 1f;
            audioSource.maxDistance = maxDistance ?? 10f;
        }
        else
        {
            audioSource.spatialBlend = 0.0f;
        }

        audioSource.Play();
        float clipLength = audioSource.clip.length;
        StartCoroutine(DestroyAudioSource(audioSource.gameObject,clipLength));
        //Destroy(audioSource.gameObject, clipLength);
    }

    IEnumerator DestroyAudioSource(GameObject Object, float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Destroy(Object);
    }

}
