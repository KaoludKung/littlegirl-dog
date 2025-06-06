using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class GirlController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distance = 2.0f;
    [SerializeField] private float detectRange = 30.0f;
    [SerializeField] private float volume = 1.0f;
    [SerializeField] private AudioClip barkClip;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private Transform dogTransform;
    [SerializeField] private DogController dogControlller;
    [SerializeField] private GameObject interactionIcon;
    [SerializeField] private TextMeshProUGUI tooFar;
    [SerializeField] private Image progressFill;
    [SerializeField] private GameObject progressSlider;

    private bool hasSound = false;
    private bool isBarking;
    private bool isMoving;
    private bool isInteract;
    public bool isActive { get; private set; }

    private AudioClip originalClip;
    private AudioSource walkSource;
    private Animator animator;
    private Vector3 targetPosition;
    private Vector3 originalScale;
    private bool isWalkingSoundPlaying;
    private List<Interactable> interactablesInRange = new List<Interactable>();
    private List<Sprite> interactionSprite = new List<Sprite>();

    public bool HasSound => hasSound;
    public bool IsBarking => isBarking;
    public bool IsMoving => isMoving;
    public Animator Animator => animator;
    public GameObject InteractionIcon => interactionIcon;

    private void Awake()
    {
        isActive = false;
        walkSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        walkSource.clip = walkClip;
        originalClip = walkClip;
        tooFar.text = "";

    }

    
    void Start()
    {
        isMoving = false;
        isBarking = false;
        isWalkingSoundPlaying = false;
        originalScale = transform.localScale;

        if (interactionIcon != null)
        {
            interactionIcon.SetActive(false);
        }

    }

    void Update()
    {
        if (InputManager.Instance.IsZPressed() && isActive)
        {
            float distance = Vector3.Distance(transform.position, dogTransform.position);

            if (!isBarking && !dogControlller.IsMoving)
            {
                if (dogControlller.staminaFill.fillAmount > 0.11 && distance <= detectRange)
                {
                    StartCoroutine(HandleMove());
                }
                else if (dogControlller.staminaFill.fillAmount < 0.11)
                {
                    tooFar.text = LocalizationManager.Instance.GetText(53, PlayerDataManager.Instance.GetLanguage());
                    StartCoroutine(DogSad());
                    StartCoroutine(TooFar());
                }
                else
                {
                    tooFar.text = LocalizationManager.Instance.GetText(54, PlayerDataManager.Instance.GetLanguage());
                    StartCoroutine(DogSad());
                    StartCoroutine(TooFar());
                }
            }
            else
            {
                tooFar.text = LocalizationManager.Instance.GetText(55, PlayerDataManager.Instance.GetLanguage());
                StartCoroutine(TooFar());
            }
        }

        if (InputManager.Instance.IsXPressed() && isActive)
        {
            if (!dogControlller.IsMoving)
            {
                float distance = Vector3.Distance(transform.position, dogTransform.position);
                float detectRangeForAction = detectRange > 28 ? detectRange - 20.0f : 8f;

                if (interactablesInRange.Count > 0 && !isMoving && !isInteract && dogControlller.staminaFill.fillAmount > 0.13 && distance <= detectRangeForAction)
                {
                    //Debug.Log("Performing action with: " + currentInteractable);
                    Interactable interactable = interactablesInRange[0];
                    interactable.Interact();
                    StartCoroutine(BarkTwice());
                }
                else if (interactablesInRange.Count > 0 && !isMoving && !isInteract && dogControlller.staminaFill.fillAmount < 0.13 && distance <= detectRangeForAction)
                {
                    tooFar.text = LocalizationManager.Instance.GetText(53, PlayerDataManager.Instance.GetLanguage());
                    StartCoroutine(DogSad());
                    StartCoroutine(TooFar());
                }else if (interactablesInRange.Count > 0 && !isMoving && !isInteract && dogControlller.staminaFill.fillAmount > 0.13 && distance > detectRangeForAction)
                {
                    tooFar.text = LocalizationManager.Instance.GetText(54, PlayerDataManager.Instance.GetLanguage());
                    StartCoroutine(DogSad());
                    StartCoroutine(TooFar());
                }
                else
                {
                    tooFar.text = LocalizationManager.Instance.GetText(56, PlayerDataManager.Instance.GetLanguage());
                    StartCoroutine(TooFar());
                }
            }
            else
            {
                tooFar.text = LocalizationManager.Instance.GetText(55, PlayerDataManager.Instance.GetLanguage());
                StartCoroutine(TooFar());
            }
        }
        

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    IEnumerator DogSad()
    {
        dogControlller.Animator.SetInteger("IdleVariant", 2);
        yield return new WaitForSeconds(1.2f);
        dogControlller.Animator.SetInteger("IdleVariant", 0);
    }

    IEnumerator TooFar()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        tooFar.text = "";
    }

    IEnumerator BarkTwice()
    {
        int i = 0;
        dogControlller.staminaFill.fillAmount -= 0.13f;
        
        hasSound = true;
        yield return new WaitForSecondsRealtime(0.1f);
        hasSound = false;

        while (i < 2)
        {
            dogControlller.Animator.SetInteger("BarkType", 1);
            SoundFXManager.instance.PlaySoundFXClip(barkClip, dogTransform, true, volume, 20.0f, 60.0f);
            yield return new WaitForSecondsRealtime(barkClip.length);
            dogControlller.Animator.SetInteger("BarkType", 0);
            i++;
        }     
    }

    IEnumerator HandleMove()
    {
        isBarking = true;
        hasSound = true;
        targetPosition = dogControlller.transform.position;
        dogControlller.staminaFill.fillAmount -= 0.11f;
        SoundFXManager.instance.PlaySoundFXClip(barkClip, dogTransform, true, volume, 20.0f, 60.0f);
        dogControlller.Animator.SetInteger("BarkType", 1);
        yield return new WaitForSeconds(barkClip.length);
        dogControlller.Animator.SetInteger("BarkType", 0);

        yield return new WaitForSeconds(0.1f);
        hasSound = false;

        if (!isMoving)
        {
            isMoving = true;
            
            if (!isWalkingSoundPlaying)
            {
                isWalkingSoundPlaying = true;
                StartCoroutine(PlayWalkSound());
            }
        }
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
            animator.SetBool("isWalk", true);
        }
        else
        {
            isMoving = false;
            walkSource.loop = false;
            animator.SetBool("isWalk", false);
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
            isBarking = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("isInteract: " + isInteract);

        if (other.CompareTag("Interactable") && !isInteract)
        {
            //Debug.Log("Entering Interactable: " + other.gameObject.name);
            Interactable interactable = other.GetComponent<Interactable>();

            if (interactable != null && !interactablesInRange.Contains(interactable))
            {
                interactablesInRange.Add(interactable);
                StartCoroutine(UpdateInteractionIcon());
                //UpdateInteractionIcon();
            }

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            Interactable interactable = other.GetComponent<Interactable>();

            if (interactable != null && interactablesInRange.Contains(interactable))
            {
                interactablesInRange.Remove(interactable);

                if(interactionSprite.Count > 0)
                {
                    interactionSprite.Remove(interactionSprite[0]);
                }

                if (this != null && gameObject.activeInHierarchy)
                {
                    StartCoroutine(UpdateInteractionIcon());
                }
                //UpdateInteractionIcon();
            }

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

    public void SetNewClip(AudioClip newClip, bool useOriginal = false)
    {
        walkSource.clip = useOriginal ? originalClip : newClip;
    }

    public void SetIsMoving(bool value)
    {
        isMoving = value;
    }

    public void SetIsActive(bool value)
    {
        isActive = value;
    }

    public void SetIsInteract(bool value)
    {
        isInteract = value;
    }

    public void SetIcon(bool value)
    {
        interactionIcon.SetActive(value);
    }

    public void AddInteractSprite(Sprite icon)
    {
        interactionSprite.Add(icon);
    }

    public void ResetCurrentInteractable()
    {
        if (interactablesInRange.Count > 0)
        {
            interactablesInRange.Clear();
            interactionSprite.Clear();
            StartCoroutine(UpdateInteractionIcon());
        }        
    }

    public void AdjustSpeed(float value)
    {
        speed += value;
    }

    IEnumerator UpdateInteractionIcon()
    {
        yield return new WaitForSeconds(0.1f);

        if (interactablesInRange.Count > 0 && interactionSprite.Count > 0)
        {
            interactionIcon.GetComponent<SpriteRenderer>().sprite = interactionSprite[0];
            interactionIcon.SetActive(true);
        }
        else
        {
            interactionIcon.SetActive(false);
        }
    }
}
