using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doll : MonoBehaviour, Interactable
{
    [SerializeField] private Sprite alertSprite;

    private Animator d_Animator;
    private AudioSource d_Audiosource;
    private GirlController girlController;
    private ActionText actionText;
    private BoxCollider2D boxCollider;
    private bool isSaying = false;

    // Start is called before the first frame update
    void Start()
    {
        d_Audiosource = GetComponent<AudioSource>();
        d_Animator = GetComponent<Animator>();
        girlController = FindObjectOfType<GirlController>();
        actionText = FindObjectOfType<ActionText>();
        boxCollider = GetComponent<BoxCollider2D>();
        d_Animator.speed = 0;
        boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventManager.Instance.IsEventTriggered(79) && !isSaying)
        {
            Working();
            StartCoroutine(Unlock());
        }
    }

    void Working()
    {
        d_Animator.speed = 1;

        if (!d_Audiosource.isPlaying)
        {
            d_Audiosource.Play();
        }
    }

    IEnumerator Unlock()
    {
        gameObject.tag = "Interactable";
        yield return new WaitForSeconds(0.3f);
        boxCollider.enabled = true;
    }

    IEnumerator Saying()
    {
        girlController.SetIsInteract(true);
        girlController.InteractionIcon.SetActive(false);
        CharacterManager.Instance.SetIsActive(false);
        d_Animator.SetBool("isSaying", true);
        actionText.ActionDisplay("The answer will appear when the hexagon, circle, and nonagon are aligned.");

        yield return new WaitForSeconds(3.0f);

        girlController.SetIsInteract(false);
        girlController.InteractionIcon.SetActive(true);
        CharacterManager.Instance.SetIsActive(true);
        d_Animator.SetBool("isSaying", false);
        isSaying = false;
    }

    public void Interact()
    {
        if (EventManager.Instance.IsEventTriggered(79) && !isSaying)
        {
            isSaying = true;
            StartCoroutine(Saying());
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            girlController.AddInteractSprite(alertSprite);
            //girlController.InteractionIcon.GetComponent<SpriteRenderer>().sprite = alertSprite;
        }
    }

}
