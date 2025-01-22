using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : EventObject
{
    [SerializeField] private AudioClip trap_clip;

    private Animator animator;
    private BoxCollider2D boxCollider;
    private bool isDestroy = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator.speed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyTrap();
    }

    void DestroyTrap()
    {
        if(EventManager.Instance.IsEventTriggered(eventID) && !isDestroy)
        {
            isDestroy = true;
            StartCoroutine(TrapDisable());
            boxCollider.enabled = false;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog") && !isDestroy)
        {
            StartCoroutine(TrapWorking());
            SoundFXManager.instance.PlaySoundFXClip(trap_clip, transform, false, 1.0f);
        }
    }

    IEnumerator TrapWorking()
    {
        animator.speed = 1f;
        animator.Play("Trap");
        yield return new WaitForSeconds(0.5f);
        animator.Play("Trap", -1, 0f);
        animator.speed = 0f;
        yield return new WaitForSeconds(0.5f);
        HeartManager.instance.HeartDecrease();
    }

    IEnumerator TrapDisable()
    {
        animator.speed = 1f;
        animator.Play("Trap", -1, 0f); 
        yield return new WaitForSeconds(0.5f);
        animator.speed = 0f;
    }
}
