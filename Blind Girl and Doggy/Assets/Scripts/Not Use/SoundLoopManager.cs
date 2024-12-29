using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLoopManager : MonoBehaviour
{
    public static SoundLoopManager instance;
    private AudioSource loopAudioSource;

    private void Awake()
    {
        instance = this;
    }

    public void PlayLoopSound(AudioSource audioSource,AudioClip audioClip, bool spatialBlend, float volume, float? minDistance = null, float? maxDistance = null)
    {
        loopAudioSource = audioSource;

        if (loopAudioSource != null && loopAudioSource.isPlaying)
        {
            StopCoroutine("StopLoopSound");
            loopAudioSource.Stop();
        }

        loopAudioSource.clip = audioClip;
        loopAudioSource.volume = volume;
        loopAudioSource.loop = true;

        if (spatialBlend)
        {
            loopAudioSource.spatialBlend = 1.0f;
            loopAudioSource.minDistance = minDistance ?? 1f;
            loopAudioSource.maxDistance = maxDistance ?? 10f;
        }
        else
        {
            loopAudioSource.spatialBlend = 0.0f;
        }

        loopAudioSource.Play();
    }

    /*
    public void PauseLoopSound()
    {
        if (loopAudioSource != null)
        {
            if (PauseMenu.isPaused && loopAudioSource.isPlaying)
            {
                loopAudioSource.Pause();
                Debug.Log("Pause Sound");
            }
        }
    }


    public void UnPauseLoopSound()
    {
        if (loopAudioSource != null)
        {
            if (!PauseMenu.isPaused && !loopAudioSource.isPlaying)
            {
                loopAudioSource.UnPause();
                Debug.Log("Unpause Sound");
            }
        }
    }
    */

    public IEnumerator StopLoopSound(float fadeOutTime = 0.1f)
    {
        if (loopAudioSource != null && loopAudioSource.isPlaying)
        {
            float startVolume = loopAudioSource.volume;

            for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
            {
                loopAudioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeOutTime);
                yield return null;
            }

            loopAudioSource.Stop();
        }
    }
}
