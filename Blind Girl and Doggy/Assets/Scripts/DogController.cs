using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DogController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private AudioClip walkClip;
    public Slider staminaSlider;

    private AudioSource walkSource;
    private EventObject currentEventObject;

    private bool isStart;
    private bool isMoving;
    private bool isWalkingSoundPlaying;
    private bool isRegeneratingStamina = false;
    private bool isInTrigger = false;

    public AudioSource WalkSource => walkSource;
    public bool IsMoving => isMoving;
    private Animator animator;
    
    private void Awake()
    {
        SetIsStart(true);
        walkSource = GetComponent<AudioSource>();
        walkSource.clip = walkClip;
    }


    void Start()
    {
        isMoving = false;
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Character"), LayerMask.NameToLayer("Character"), true);
        isWalkingSoundPlaying = false;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isMoving && isInTrigger && currentEventObject != null && Input.GetKeyDown(KeyCode.W))
        {
            //EventManager.Instance.UpdateEventDataTrigger(currentEventObject.GetEventId() + 1, true);
            //EventManager.Instance.UpdateEventDataTrigger(currentEventObject.GetEventId(), true);
            //EventManager.Instance.UpdataEventDataExcuted(currentEventObject.GetEventId(), true);
            SoundFXManager.instance.PlaySoundFXClip(walkClip, transform, false, 1.0f);
            Debug.Log("Event ID: " + currentEventObject.GetEventId());
        }

        //!isRegeneratingStamina && staminaSlider.value < staminaSlider.maxValue
        if (!isRegeneratingStamina)
        {
            //StartCoroutine(IncreaseStamina());
            isRegeneratingStamina = true;
        }

        if (isStart)
        {
            MoveCharacterWithKeyboard();
        }
    }

    void LateUpdate()
    {
        if (staminaSlider != null)
        {
            if (transform.localScale.x < 0)
            {
                staminaSlider.transform.localScale = new Vector3(-Mathf.Abs(staminaSlider.transform.localScale.x),
                                                                  staminaSlider.transform.localScale.y,
                                                                  staminaSlider.transform.localScale.z);
            }
            else
            {
                staminaSlider.transform.localScale = new Vector3(Mathf.Abs(staminaSlider.transform.localScale.x),
                                                                  staminaSlider.transform.localScale.y,
                                                                  staminaSlider.transform.localScale.z);
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    void MoveCharacterWithKeyboard()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0)
        {
            isMoving = true;
            transform.position += new Vector3(horizontal * speed * Time.deltaTime, 0, 0);
            animator.SetBool("isWalk", true);

            if (horizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (horizontal > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (!isWalkingSoundPlaying)
            {
                isWalkingSoundPlaying = true;
                StartCoroutine(PlayWalkSound());
            }
        }
        else
        {
            isMoving = false;
            animator.SetBool("isWalk", false);

            if (isWalkingSoundPlaying)
            {
                isWalkingSoundPlaying = false;
                walkSource.loop = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentEventObject = other.GetComponent<EventObject>();
        if (currentEventObject != null)
        {
            isInTrigger = true;
            Debug.Log("Entered Trigger with Event ID: " + currentEventObject.GetEventId());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<EventObject>() != null)
        {
            isInTrigger = false;
            currentEventObject = null;
            Debug.Log("Exited Trigger");
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

    /*
    IEnumerator IncreaseStamina()
    {

        while (staminaSlider.value < staminaSlider.maxValue)
        {
            yield return new WaitForSeconds(8.0f);
            staminaSlider.value += 0.1f;
        }
        isRegeneratingStamina = false;
    }
    */

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isMoving = false;
            isWalkingSoundPlaying = false;

            if (walkSource.isPlaying)
            {
                walkSource.loop = false;
            }

            Debug.Log("Collided with Wall, movement stopped and sound stopped.");
        }
    }

    public void SetIsStart(bool value)
    {
        isStart = value;
    }


}
