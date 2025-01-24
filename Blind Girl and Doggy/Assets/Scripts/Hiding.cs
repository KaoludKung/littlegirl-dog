using System.Collections;
using UnityEngine;

public class Hiding : MonoBehaviour, Interactable
{
    public static Hiding instance;
    [SerializeField] private SpriteRenderer[] spriteRenderer;
    [SerializeField] private GameObject lightfly;
    [SerializeField] private AudioClip hidingClip;
    [SerializeField] private Sprite alertSprite;

    private GirlController girlController;

    private string[] originalSortingLayer;
    private int[] originalSortingOrder;
    private bool isHidden = false;

    public bool IsHidden => isHidden;

    private void Awake()
    {
        instance = this;
        girlController = FindObjectOfType<GirlController>();

        // ???????????? array ?????????? spriteRenderer.Length
        originalSortingLayer = new string[spriteRenderer.Length];
        originalSortingOrder = new int[spriteRenderer.Length];
    }

    void Start()
    {
        // ????????? sortingLayer ??? sortingOrder ??? SpriteRenderer
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            originalSortingLayer[i] = spriteRenderer[i].sortingLayerName;
            originalSortingOrder[i] = spriteRenderer[i].sortingOrder;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isHidden && !UIManager.Instance.IsAnyUIActive)
        {
            StartCoroutine(ToggleHide());
        }
    }

    public void Interact()
    {
        if (!girlController.IsMoving && !girlController.IsBarking)
        {
            CharacterManager.Instance.SetIsActive(false);
            girlController.InteractionIcon.SetActive(false);
            StartCoroutine(ToggleHide());
        }
        else
        {
            Debug.Log("Cannot perform Action A while moving or barking.");
        }
    }

    IEnumerator ToggleHide()
    {
        yield return new WaitForSeconds(0.2f);
        if (hidingClip != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(hidingClip, transform, false, 1.0f);
        }

        if (isHidden)
        {
            isHidden = false;
            girlController.Animator.SetBool("isInteract", false);

            for (int i = 0; i < spriteRenderer.Length; i++)
            {
                spriteRenderer[i].sortingLayerName = originalSortingLayer[i];
                spriteRenderer[i].sortingOrder = originalSortingOrder[i];
            }

            yield return new WaitForSeconds(0.3f);
            CharacterManager.Instance.SetIsActive(true);
            lightfly.SetActive(true);
            girlController.InteractionIcon.SetActive(true);

            //Debug.Log("Player exited hiding.");
        }
        else
        {
            girlController.Animator.SetBool("isInteract", true);
            yield return new WaitForSeconds(0.3f);
            lightfly.SetActive(false);
            isHidden = true;

            for (int i = 0; i < spriteRenderer.Length; i++)
            {
                spriteRenderer[i].sortingLayerName = "background";
                spriteRenderer[i].sortingOrder = -3;
            }

            //Debug.Log("Player is hiding.");
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            girlController.InteractionIcon.GetComponent<SpriteRenderer>().sprite = alertSprite;
        }
    }
}
