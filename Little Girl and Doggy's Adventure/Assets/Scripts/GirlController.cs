using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GirlController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distance = 3f;
    [SerializeField] private AudioClip barkClip;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private Transform dogTransform;
    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private DogController dogControlller;
    [SerializeField] private Slider progressSlider;

    private bool isBarking;
    private bool isMoving;

    private AudioSource walkSource;
    private Vector3 targetPosition;
    private Vector3 originalScale;
    private bool isWalkingSoundPlaying;
    private Interactable currentInteractable;

    public bool IsMoving => isMoving;
    public bool IsBarking => isBarking;
    public AudioSource WalkSource => walkSource;

    private void Awake()
    {
        //SetIsStart(true);
        walkSource = GetComponent<AudioSource>();
        walkSource.clip = walkClip;
    }

    
    void Start()
    {
        isMoving = false;
        isBarking = false;
        isWalkingSoundPlaying = false;
        originalScale = transform.localScale;

        if (barkClip == null)
        {
            Debug.LogError("barkSource is not assigned.");
        }

        if (interactionIcon != null)
        {
            interactionIcon.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && !isBarking && !isMoving && dogControlller.staminaSlider.value > 0)
        {
            if (barkClip != null)
            {
                StartCoroutine(HandleClickAndMove());
            } 
        }

        if (Input.GetMouseButtonDown(2) && !EventSystem.current.IsPointerOverGameObject() && !isBarking && !isMoving)
        {
            if (currentInteractable != null)
            {
                Debug.Log("Performing action with: " + currentInteractable);
                currentInteractable.Interact();
            }
            else
            {
                Debug.Log("Bruh");
            }
        }

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    void LateUpdate()
    {
        if (progressSlider != null)
        {
            if (transform.localScale.x < 0)
            {
                progressSlider.transform.localScale = new Vector3(-Mathf.Abs(progressSlider.transform.localScale.x),
                                                                  progressSlider.transform.localScale.y,
                                                                  progressSlider.transform.localScale.z);
            }
            else
            {
                progressSlider.transform.localScale = new Vector3(Mathf.Abs(progressSlider.transform.localScale.x),
                                                                  progressSlider.transform.localScale.y,
                                                                  progressSlider.transform.localScale.z);
            }
        }
    }

    IEnumerator HandleClickAndMove()
    {
        isBarking = true;
        targetPosition = dogControlller.transform.position;
        dogControlller.staminaSlider.value -= 0.2f;
        SoundFXManager.instance.PlaySoundFXClip(barkClip, dogTransform, true, 1.0f, 20.0f, 60.0f);
        yield return new WaitForSeconds(barkClip.length);

        if (!isMoving)
        {
            isMoving = true;

            if (!isWalkingSoundPlaying)
            {
                isWalkingSoundPlaying = true;
                StartCoroutine(PlayWalkSound());
            }
        }

        isBarking = false;
    }

    void MoveTowardsTarget()
    {
        if (Vector3.Distance(transform.position, targetPosition) >= distance)
        {
            if (targetPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            }
            else if (targetPosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
            walkSource.loop = false;
        }
    }

    IEnumerator PlayWalkSound()
    {
        if (walkClip != null)
        {
            walkSource.loop = true;
            walkSource.Play();

            while (isMoving)
            {
                yield return null;
            }

            walkSource.loop = false;
            isWalkingSoundPlaying = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            Debug.Log("Entering Interactable: " + other.gameObject.name);
            currentInteractable = other.GetComponent<Interactable>();

            if (currentInteractable != null)
            {
                Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
                Debug.Log("Cursor changed.");
                interactionIcon.gameObject.SetActive(true);
                Debug.Log("Interaction icon shown.");
            }
            else
            {
                Debug.Log("Current interactable is null or interaction icon is not set.");
                Debug.Log("Current interactable is null.");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (interactionIcon != null)
            {
                interactionIcon.gameObject.SetActive(false);
                Debug.Log("Interaction icon hidden.");
            }
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            currentInteractable = null;
        }
    }

    /*
    public void SetIsStart(bool value)
    {
        isStart = value;
    }*/
}
