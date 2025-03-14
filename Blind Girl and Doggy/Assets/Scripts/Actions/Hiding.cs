using System.Collections;
using UnityEngine;

public class Hiding : MonoBehaviour, Interactable
{
    public static Hiding instance;
    [SerializeField] private SpriteRenderer[] spriteRenderer;
    [SerializeField] private GameObject lightfly;
    [SerializeField] private AudioClip hidingClip;
    [SerializeField] private Sprite alertSprite;
    [SerializeField] private GameObject gameoverPanel;
    
    private Hunter hunter;
    private GirlController girlController;

    private string[] originalSortingLayer;
    private int[] originalSortingOrder;
    private bool isHidden = false;

    public bool IsHidden => isHidden;

    private void Awake()
    {
        hunter = FindObjectOfType<Hunter>();
        girlController = FindObjectOfType<GirlController>();

        originalSortingLayer = new string[spriteRenderer.Length];
        originalSortingOrder = new int[spriteRenderer.Length];
    }

    void Start()
    {
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            originalSortingLayer[i] = spriteRenderer[i].sortingLayerName;
            originalSortingOrder[i] = spriteRenderer[i].sortingOrder;
        }
    }

    private void Update()
    {
        if (InputManager.Instance.IsXPressed() && isHidden && !UIManager.Instance.IsAnyUIActive)
        {
            if(hunter != null && hunter.IsQuit)
            {
                Debug.Log("I'm vansihing.");
            }
            else
            {
                StartCoroutine(ToggleHide());
            }
        }

        if(gameoverPanel != null && gameoverPanel.activeSelf && isHidden)
        {
            StartCoroutine(ToggleHide());
        }
    }

    public void Interact()
    {
        if (!girlController.IsMoving && !girlController.IsBarking && girlController.isActive && !isHidden)
        {
            CharacterManager.Instance.SetIsActive(false);
            girlController.InteractionIcon.SetActive(false);
            StartCoroutine(ToggleHide());
            Debug.Log("YYY");
        }
        else
        {
            Debug.Log("Cannot perform Action A while moving or barking.");
        }
    }

    IEnumerator ToggleHide()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        if (hidingClip != null && gameoverPanel != null && !gameoverPanel.activeSelf)
        {
            SoundFXManager.instance.PlaySoundFXClip(hidingClip, transform, false, 1.0f);
        }

        if (isHidden)
        {
            isHidden = false;
            girlController.Animator.SetBool("isInteract", false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Character"), LayerMask.NameToLayer("Enemy"), false);

            for (int i = 0; i < spriteRenderer.Length; i++)
            {
                spriteRenderer[i].sortingLayerName = originalSortingLayer[i];
                spriteRenderer[i].sortingOrder = originalSortingOrder[i];
            }

            yield return new WaitForSecondsRealtime(0.3f);
            CharacterManager.Instance.SetIsActive(true);
            lightfly.SetActive(true);
            girlController.InteractionIcon.SetActive(true);

            //Debug.Log("Player exited hiding.");
        }
        else if (!isHidden)
        {
            girlController.Animator.SetBool("isInteract", true);
            yield return new WaitForSecondsRealtime(0.3f);
            isHidden = true;
            lightfly.SetActive(false);
            

            for (int i = 0; i < spriteRenderer.Length; i++)
            {
                spriteRenderer[i].sortingLayerName = "background";
                spriteRenderer[i].sortingOrder = -3;
            }

            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Character"), LayerMask.NameToLayer("Enemy"), true);

            //Debug.Log("Player is hiding.");
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
