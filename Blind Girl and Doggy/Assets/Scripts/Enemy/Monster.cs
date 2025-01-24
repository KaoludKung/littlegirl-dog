using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float chaseSpeed = 1.0f;
    [SerializeField] private float detectionRange = 1f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Transform[] patrolPoint;
    [SerializeField] private AudioClip[] monsterClips;

    private bool isChasing = false;

    private int currentPatrolIndex = 0;
    private Transform targetPatrolPoint;
    private AudioSource enemySource;

    // Start is called before the first frame update
    void Start()
    {
        if (patrolPoint.Length > 0)
        {
            targetPatrolPoint = patrolPoint[currentPatrolIndex];
        }

        enemySource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            Chasing();
        }
        else
        {
            Patrol();
        }
    }

    void Stalker()
    {

    }

    void Patrol()
    {
        if (targetPatrolPoint == null) return;

        Vector2 patrolPosition = new Vector2(targetPatrolPoint.position.x, transform.position.y);
        FlipEnemy(patrolPosition);

        enemySource.clip = monsterClips[0];

        if (!enemySource.isPlaying)
        {
            enemySource.loop = true;
            enemySource.Play();
        }
        else
        {
            enemySource.Stop();
            enemySource.loop = true;
            enemySource.Play();
        }

        transform.position = Vector2.MoveTowards(transform.position, patrolPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 5.0f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoint.Length;
            targetPatrolPoint = patrolPoint[currentPatrolIndex];
        }
    }

    void Chasing()
    {

    }

    protected virtual void FlipEnemy(Vector2 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-0.9f, 0.9f, 0.9f);
        }
        else if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog") || collision.CompareTag("Player"))
        {
            Debug.Log("Catch me!!!");
        }
    }

}
