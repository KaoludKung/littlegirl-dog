using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DogControl : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private AudioClip walkClip;
    private  AudioSource walkSource;
    private Vector3 targetPosition;
    private bool isMoving;
    public bool IsMoving => isMoving;
    private bool isWalkingSoundPlaying;
    private bool isStart;

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
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Character"), LayerMask.NameToLayer("Character"), true);
        targetPosition = transform.position;
        isWalkingSoundPlaying = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && isStart)
        {
            SetTargetPosition();
        }

        if (isMoving)
        {
            MoveCharacter();
        }

    }

    void SetTargetPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition = new Vector3(mousePos.x, transform.position.y, transform.position.z);

        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
        else if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }

        isMoving = true;

        if (!isWalkingSoundPlaying)
        {
            isWalkingSoundPlaying = true;
            StartCoroutine(PlayWalkSound());
        }
    }

    void MoveCharacter()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            isMoving = false;
            isWalkingSoundPlaying = false;
            StartCoroutine(SoundLoopManager.instance.StopLoopSound());
        }
    }

    IEnumerator PlayWalkSound()
    {
        if (walkClip != null)
        {
            SoundLoopManager.instance.PlayLoopSound(walkSource,walkClip, true, 1.0f, 5.0f, 20.0f);

            while (isMoving)
            {
                yield return null;
            }

            StartCoroutine(SoundLoopManager.instance.StopLoopSound());
            isWalkingSoundPlaying = false; 
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isMoving = false;
            Vector3 collisionNormal = collision.contacts[0].normal;
            Vector3 newPosition = transform.position + (collisionNormal * 0.8f);

            transform.position = newPosition;
            targetPosition = transform.position;

            isWalkingSoundPlaying = false;
            StartCoroutine(SoundLoopManager.instance.StopLoopSound());

            Debug.Log("Collided with Wall, movement stopped.");
        }
    }

    public void SetIsStart(bool value)
    {
        isStart = value;
    }

}
