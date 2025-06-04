using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DogController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private GameObject toofar;
    [SerializeField] private Image dogIcon;
    [SerializeField] private Sprite[] dogSprite;
    public Image staminaFill;
    public bool isActive { get; private set; }

    private AudioClip originalClip;
    private AudioSource walkSource;
    private Animator animator;
    private bool isMoving;
    private bool isDigging;
    private bool isRegeneratingStamina = false;
    private Interactable currentInteractable;

    public Animator Animator => animator;
    public bool IsMoving => isMoving;


    private void Awake()
    {
        isActive = false;
        isDigging = false;
        walkSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        walkSource.clip = walkClip;
        originalClip = walkClip;
    }


    void Start()
    {
        isMoving = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Character"), LayerMask.NameToLayer("Character"), true);
    }

    void Update()
    {
        ChangeIcon();

        if (!isRegeneratingStamina && staminaFill.fillAmount < 1.0f)
        {
            StartCoroutine(IncreaseStamina());
            isRegeneratingStamina = true;
        }

        if (isActive)
        {
            MoveCharacterWithKeyboard();
        }

        if (InputManager.Instance.IsQPressed() && isActive)
        {
            if (currentInteractable != null && !isMoving && isDigging)
            {
                currentInteractable.Interact();
            }
            else
            {
                Debug.Log("Bruh");
            }
        }
    }

    void ChangeIcon()
    {
        if(staminaFill.fillAmount != 0.0f)
        {
            dogIcon.sprite = dogSprite[0];
        }
        else
        {
            dogIcon.sprite = dogSprite[1];
        }
    }

    void MoveCharacterWithKeyboard()
    {
        float horizontal = 0f;

        if (Input.GetKey(KeyCode.LeftArrow) || InputManager.Instance.IsWalkingLeft())
        {
            horizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || InputManager.Instance.IsWalkingRight())
        {
            horizontal = 1f;
        }


        if (horizontal != 0)
        {
            isMoving = true;
            transform.position += new Vector3(horizontal * speed * Time.deltaTime, 0, 0);
            animator.SetInteger("IdleVariant", 0);
            animator.SetBool("isWalk", true);

            if (horizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (horizontal > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (walkSource.clip != null && !walkSource.isPlaying)
            {
                walkSource.loop = true;
                walkSource.Play();
            }
        }
        else
        {
            isMoving = false;
            animator.SetBool("isWalk", false);

            if (walkSource.isPlaying)
            {
                walkSource.loop = false;
            }
        }
    }

    public void StopFootStep()
    {
        if (walkSource.isPlaying)
        {
            walkSource.loop = false;
        }
    }

    IEnumerator IncreaseStamina()
    {
        while (staminaFill.fillAmount < 1.0f)
        {
            float waitTime = staminaFill.fillAmount < 0.30f ? 1.0f : 0.75f;
            yield return new WaitForSeconds(waitTime);
            staminaFill.fillAmount += 0.01075f;
        }
        isRegeneratingStamina = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spots"))
        {
            isDigging = true;
            currentInteractable = collision.GetComponent<Interactable>();

            if (currentInteractable != null)
            {
                Debug.Log("There is spots");
            }
            else
            {
                Debug.Log("There is not spots");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Spots"))
        {
            isDigging = false;
            //Debug.Log("Bye");
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isMoving = false;

            if (walkSource.isPlaying)
            {
                walkSource.loop = false;
            }

            Debug.Log("Collided with Wall, movement stopped and sound stopped.");
        }
    }

    void LateUpdate()
    {
        if (toofar != null)
        {
            if (transform.localScale.x < 0)
            {
                toofar.transform.localScale = new Vector3(-Mathf.Abs(toofar.transform.localScale.x),
                                                                  toofar.transform.localScale.y,
                                                                  toofar.transform.localScale.z);
            }
            else
            {
                toofar.transform.localScale = new Vector3(Mathf.Abs(toofar.transform.localScale.x),
                                                                  toofar.transform.localScale.y,
                                                                  toofar.transform.localScale.z);
            }
        }
    }

    public void SetNewClip(AudioClip newClip, bool useOriginal = false)
    {
        walkSource.clip = useOriginal ? originalClip : newClip;
    }


    public void SetIsActive(bool value)
    {
        isActive = value;
    }

    public void SetIsDigging(bool value)
    {
        isDigging = value;
    }

    public void StopSound()
    {
        if (walkSource.isPlaying)
        {
            walkSource.Stop();
        }
    }

    public void AdjustSpeed(float value)
    {
        speed += value;
    }

    public void AdjustStamina(float value)
    {
        staminaFill.fillAmount = Mathf.Clamp(staminaFill.fillAmount + value, 0f, 1f);
    }

    public void MaxStamina()
    {
        if(staminaFill.fillAmount < 1.0f)
        {
            staminaFill.fillAmount = 1.0f;
        }
    }

}
