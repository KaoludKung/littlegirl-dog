using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float berserkSpeed = 3.5f;
    [SerializeField] private GameObject endOfmonster;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Transform[] patrolPoint;
    [SerializeField] private AudioClip[] monsterClips;

    private MonsterState currentState;
    private int currentPatrolIndex = 0;
    private Transform targetPatrolPoint;

    private float patrolSoundTimer = 0f;
    private float roarSoundTimer = 0f;

    private bool hasPlayedSound = false;
    private bool isHunting = false;
    private bool isKill = false;
    
    private AudioSource enemySource;
    private GirlController girlController;

    private void Awake()
    {
        currentState = MonsterState.Stalker;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (patrolPoint.Length > 0)
        {
            targetPatrolPoint = patrolPoint[currentPatrolIndex];
        }

        girlController = FindObjectOfType<GirlController>();
        enemySource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventManager.Instance.IsEventTriggered(41) && !isHunting)
        {
            isHunting = true;
            currentState = MonsterState.Patrol;
        }

        if (endOfmonster.activeSelf)
            currentState = MonsterState.RunAway;

        switch (currentState)
        {
            case MonsterState.Stalker:
                Stalker();
                break;
            case MonsterState.Patrol:
                Patrol();
                break;
            case MonsterState.RunAway:
                RunAway();
                break;
            default:
                break;
        }

    }


    void Stalker()
    {
        float stopChasingRange = 30.0f;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (girlController.IsMoving && distanceToPlayer > stopChasingRange)
        {
            transform.position = new Vector2(transform.position.x + 5f * Time.deltaTime, transform.position.y);

            if (!hasPlayedSound)
            {
                enemySource.clip = monsterClips[0];
                enemySource.loop = true;
                enemySource.Play();
                hasPlayedSound = true;
            }
        }
        else
        {
            if (hasPlayedSound)
            {
                enemySource.Stop();
                hasPlayedSound = false;
            }
        }
    }

    void Patrol()
    {
        patrolSoundTimer += Time.deltaTime;
        roarSoundTimer += Time.deltaTime;

        if (patrolSoundTimer >= 1)
        {
            patrolSoundTimer = 0f;
            enemySource.spatialBlend = 1.0f;
            enemySource.clip = monsterClips[1];
            enemySource.loop = false;
            enemySource.Play();
        }

        if (roarSoundTimer >= 10)
        {
            roarSoundTimer = 0f;
            int r = Random.Range(0, 3);
            Debug.Log(r);
            
            if(r == 1 || r == 2)
                SoundFXManager.instance.PlaySoundFXClip(monsterClips[2], transform, true, 0.8f, 2.5f, 40f);
        }

        if (EventManager.Instance.IsEventTriggered(42))
        {
            if (targetPatrolPoint == null) return;

            Vector2 patrolPosition = new Vector2(targetPatrolPoint.position.x, transform.position.y);
            FlipEnemy(patrolPosition);

            transform.position = Vector2.MoveTowards(transform.position, patrolPosition, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 5.0f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoint.Length;
                targetPatrolPoint = patrolPoint[currentPatrolIndex];
            }
        }
    }

    void RunAway()
    {
        isKill = true;
        enemySource.Stop();

        Vector2 patrolPosition = new Vector2(6.46f, transform.position.y);
        FlipEnemy(patrolPosition);
        transform.position = Vector2.MoveTowards(transform.position, patrolPosition, speed * Time.deltaTime);
    }


    private void FlipEnemy(Vector2 targetPosition)
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
            if (!isKill)
            {
                isKill = true;
                HeartManager.instance.HeartDecrease();
            }
        }

        if (collision.CompareTag("Player"))
        {
            if (!isKill)
            {
                girlController.SetIsMoving(false);
                girlController.Animator.SetBool("isWalk", false);
                girlController.Animator.SetBool("isDeath", true);
            }
        }
    }

    public void SetIsKill(bool k)
    {
        isKill = k;
    }

}

public enum MonsterState {Stalker, Patrol, RunAway ,None }
