using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0.5f;
    public float chaseSpeed = 1.0f;
    public float detectionRange = 1f;
    public float attackRange = 1f;
    public Transform playerTarget;
    public Transform[] patrolPoint;

    public AudioClip patrolSound;
    public AudioClip chaseSound;

    private int currentPatrolIndex = 0;
    private Transform targetPatrolPoint;

    private AudioSource audioSource;
    private bool isChasing = false;

    protected virtual void Start()
    {
        if (patrolPoint.Length > 0)
        {
            targetPatrolPoint = patrolPoint[currentPatrolIndex];
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = patrolSound;
    }

    protected virtual void Update()
    {
        if (isChasing)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    protected virtual void Patrol()
    {
        if (targetPatrolPoint == null) return;

        Vector2 patrolPosition = new Vector2(targetPatrolPoint.position.x, transform.position.y);
        FlipEnemy(patrolPosition);

        if (audioSource.clip != patrolSound)
        {
            audioSource.clip = patrolSound;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }

        transform.position = Vector2.MoveTowards(transform.position, patrolPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 2.0f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoint.Length;
            targetPatrolPoint = patrolPoint[currentPatrolIndex];
        }

        Vector2 directionToPlayer = (playerTarget.position - transform.position).normalized;
        float dotProduct = Vector2.Dot(directionToPlayer, transform.right);

        if (Vector2.Distance(transform.position, playerTarget.position) < detectionRange)
        {
            if ((dotProduct > 0 && transform.localScale.x > 0) || (dotProduct < 0 && transform.localScale.x < 0))
            {
                isChasing = true;
                audioSource.Stop();
            }
        }
    }



    protected virtual void Chase()
    {
        if (audioSource.clip != chaseSound)
        {
            audioSource.clip = chaseSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        Vector2 playerTargetPosition = new Vector2(playerTarget.position.x, transform.position.y);
        FlipEnemy(playerTargetPosition);
        transform.position = Vector3.MoveTowards(transform.position, playerTargetPosition, chaseSpeed * Time.deltaTime);

        if (Vector3.Distance(playerTarget.position, transform.position) <= attackRange)
        {
            Attack();
        }

        Hiding hiding = FindObjectOfType<Hiding>();
        if (hiding.IsHidden && Vector3.Distance(playerTarget.position, transform.position) > 10.0f)
        {
            isChasing = false;
            StartCoroutine(StopAudioAfterDelay(0.1f));
        }
    }

    protected virtual void Attack()
    {
        Debug.Log("Target caught! Game Over.");
    }

    protected virtual void FlipEnemy(Vector2 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
        else if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
    }

    private IEnumerator StopAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
    }
}
