using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private int minRandomtime;
    [SerializeField] private int maxRandomtime;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private bool spitalBlend;

    private GameOverManager gameover;

    // Start is called before the first frame update
    void Start()
    {
        gameover = FindObjectOfType<GameOverManager>();
        StartCoroutine(PlayRandomSound());
    }

    IEnumerator PlayRandomSound()
    {
        while (true)
        {
            int r = Random.Range(minRandomtime, maxRandomtime);
            yield return new WaitForSeconds(r);

            if (gameover == null || !gameover.isActive)
            {
                SoundFXManager.instance.PlayRandomSoundFXClip(clips, transform, spitalBlend, 1.0f, minDistance, maxDistance);
            }
        }
    }
}
