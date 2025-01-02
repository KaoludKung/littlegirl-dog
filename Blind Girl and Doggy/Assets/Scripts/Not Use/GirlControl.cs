using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GirlControl : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distance = 3f;
    [SerializeField] private AudioClip barkClip;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private Transform dogTransform;
    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private DogControl dogControl;
    [SerializeField] private Slider progressSlider;

    private bool isBarking;
    private bool isMoving;
    private bool isStart;

    public bool IsMoving => isMoving;
    public bool IsBarking => isBarking;

    private AudioSource walkSource;
    private Vector3 targetPosition;
    private Vector3 originalScale;
    private bool isWalkingSoundPlaying;
    private Interactable currentInteractable;

    private void Awake()
    {
        SetIsStart(true);
        walkSource = GetComponent<AudioSource>();
    }

    public AudioSource WalkSource
    {
        get { return walkSource; }
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
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && !isBarking && !isMoving && isStart)
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
        targetPosition = dogControl.transform.position;
        SoundFXManager.instance.PlaySoundFXClip(barkClip, dogTransform, true, 1.0f, 20.0f, 60.0f);
        yield return new WaitForSeconds(barkClip.length);

        isMoving = true;

        if (!isWalkingSoundPlaying)
        {
            isWalkingSoundPlaying = true;
            StartCoroutine(PlayWalkSound());
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
            StartCoroutine(SoundLoopManager.instance.StopLoopSound());
        }
    }

    IEnumerator PlayWalkSound()
    {
        if (walkClip != null)
        {
            SoundLoopManager.instance.PlayLoopSound(walkSource, walkClip, true, 1.0f, 5.0f, 20.0f);

            while (isMoving)
            {
                yield return null;
            }

            StartCoroutine(SoundLoopManager.instance.StopLoopSound());
            isWalkingSoundPlaying = false;
        }    
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            Debug.Log("Entering Interactable: " + other.gameObject.name);
            currentInteractable = other.GetComponent<Interactable>();

            if (currentInteractable != null && interactionIcon != null)
            {
                interactionIcon.gameObject.SetActive(true);
                Debug.Log("Interaction icon shown.");
            }
            else
            {
                Debug.Log("Current interactable is null or interaction icon is not set.");
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
            currentInteractable = null;
        }
    }


    public void SetIsStart(bool value)
    {
        isStart = value;
    }
}
