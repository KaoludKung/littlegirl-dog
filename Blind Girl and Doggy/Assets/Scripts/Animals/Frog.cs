using System.Collections;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [SerializeField] private AudioClip ribbit;
    [SerializeField] private int minInterval = 60;
    [SerializeField] private int maxInterval = 120;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlayAnimationAndSoundRandomly());
    }

    private IEnumerator PlayAnimationAndSoundRandomly()
    {
        float randomInterval = Random.Range(minInterval, maxInterval);
        yield return new WaitForSeconds(randomInterval);

        SoundFXManager.instance.PlaySoundFXClip(ribbit, transform, true, 1.0f, 5, 10);
        animator.SetBool("isSmile", true);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetBool("isSmile", false);
        StartCoroutine(PlayAnimationAndSoundRandomly());

    }
}
