using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DogController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private AudioClip walkClip;
    public Image staminaFill;

    public bool isActive { get; private set; }

    private AudioSource walkSource;
    private Animator animator;
    private bool isMoving;
    private bool isWalkingSoundPlaying;
    private bool isRegeneratingStamina = false;

    public Animator Animator => animator;
    public bool IsMoving => isMoving;


    private void Awake()
    {
        isActive = true;
        walkSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        walkSource.clip = walkClip;
    }


    void Start()
    {
        isMoving = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Character"), LayerMask.NameToLayer("Character"), true);
        isWalkingSoundPlaying = false;
    }

    void Update()
    {
        if (!isRegeneratingStamina && staminaFill.fillAmount < 1.0f)
        {
            StartCoroutine(IncreaseStamina());
            isRegeneratingStamina = true;
        }

        if (isActive)
        {
            MoveCharacterWithKeyboard();
        }
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


    IEnumerator IncreaseStamina()
    {
        while (staminaFill.fillAmount < 1.0f)
        {
            yield return new WaitForSeconds(3.0f);
            staminaFill.fillAmount += 0.1f;
        }
        isRegeneratingStamina = false;
    }


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

    public void SetIsActive(bool value)
    {
        isActive = value;
    }

}
