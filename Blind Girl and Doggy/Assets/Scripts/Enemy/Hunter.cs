using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Hunter : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float berserkSpeed = 4.0f;
    [SerializeField] private Transform sleepPoint;
    [SerializeField] private Transform[] patrolPoint;
    [SerializeField] private Vector3[] warpPoint;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private Light2D globalLight;
    [SerializeField] Transform shadow;

    private Radio radio;
    private GirlController girlcontroller;

    private GameObject player;
    private GameObject dog;

    private Animator h_animator;
    private AudioSource enemySource;
    private Dictionary<string, AudioClip> hunterClips;

    private int currentPatrolIndex = 0;
    private Transform targetPatrolPoint;
    private int pattrenIndex = 0;
    private int patrolCount = 0;

    private float lastPlayerY;
    private HunterState currentState;
    
    private bool isKill = true;
    private bool isFound = false;
    private bool isWarp = false;
    private bool isWarping = false;
    private bool isQuit = false;
    private bool isTriggerFinal = false;

    public HunterState CurrentState => currentState;
    public bool IsQuit => isQuit;


    private void Awake()
    {
        currentState = HunterState.Radio;

        hunterClips = new Dictionary<string, AudioClip>()
        {
            { "walk", clips[0] },
            { "chasing", clips[1] },
            { "laughing", clips[2] },
            { "scream", clips[3] }
        };

        player = GameObject.FindWithTag("Player");
        dog = GameObject.FindWithTag("Dog");
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPlayerY = player.transform.position.y;
        radio = FindObjectOfType<Radio>();
        girlcontroller = FindObjectOfType<GirlController>();

        if (patrolPoint.Length > 0)
        {
            targetPatrolPoint = patrolPoint[currentPatrolIndex];
        }

        h_animator = GetComponent<Animator>();
        SetAnimatorState("isSleep");
        currentState = HunterState.Radio;
        enemySource = GetComponent<AudioSource>();
        shadow.localPosition = new Vector3(0.85f, -2.89f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        float distanceToDog = Vector2.Distance(transform.position, dog.transform.position);

        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector2 directionToDog = (dog.transform.position - transform.position).normalized;
        bool isFacingLeft = transform.localScale.x > 0;

        bool isPlayerOnCorrectSide = (isFacingLeft && directionToPlayer.x < 0) || (!isFacingLeft && directionToPlayer.x > 0);
        bool isDogOnCorrectSide = (isFacingLeft && directionToDog.x < 0) || (!isFacingLeft && directionToDog.x > 0);


        if ((distanceToPlayer < 13.0f && isPlayerOnCorrectSide) || (distanceToDog < 13.0f && isDogOnCorrectSide))
        {
            if ((currentState == HunterState.Patrol || currentState == HunterState.Sleep || currentState == HunterState.Final) && !isFound && !CharacterManager.Instance.isHiding())
            {
                isFound = true;
                isKill = false;
                SoundFXManager.instance.PlaySoundFXClip(hunterClips["laughing"], transform, false, 1.0f);
                currentState = HunterState.None;
                currentState = HunterState.Chasing;
            }
        }else if((distanceToPlayer < 17.0f) || (distanceToDog < 17.0f))
        {
            if ((currentState == HunterState.Patrol || currentState == HunterState.Sleep || currentState == HunterState.Final) && girlcontroller.HasSound && !isFound && !CharacterManager.Instance.isHiding())
            {
                isFound = true;
                isKill = false;
                SoundFXManager.instance.PlaySoundFXClip(hunterClips["laughing"], transform, false, 1.0f);
                currentState = HunterState.None;
                currentState = HunterState.Chasing;
            }
        }

        if (currentState == HunterState.Chasing)
        {
            if((CharacterManager.Instance.isHiding() || CharacterManager.Instance.isHidingT()))
            {
                if (EventManager.Instance.IsEventTriggered(88))
                {
                    currentState = HunterState.Final;
                }
                else
                {
                    currentState = HunterState.Quit;
                }
            }
        }

        float currentPlayerY = player.transform.position.y;
        if (currentState == HunterState.Chasing && !isWarping && (currentPlayerY != lastPlayerY))
        {
            if (Mathf.Abs(currentPlayerY - transform.position.y) > 1.0f && !EventManager.Instance.IsEventTriggered(88))
            {
                StartCoroutine(WarpToTarget());
            }
            else if (Mathf.Abs(currentPlayerY - transform.position.y) > 1.0f && EventManager.Instance.IsEventTriggered(88))
            {
                currentState = HunterState.Final;
            }
        }

        lastPlayerY = currentPlayerY;

        switch (currentState)
        {
            case HunterState.Sleep:
                SleepTime();
                break;
            case HunterState.Patrol:
                shadow.localPosition = new Vector3(-0.09f, -2.89f, 0f);
                Patrol();
                break;
            case HunterState.Chasing:
                Chasing();
                break;
            case HunterState.Quit:
                StartCoroutine(StopChasing());
                break;
            case HunterState.Final:
                StartCoroutine(FinalPatrol());
                break;
            default:
                break;
        }

    }

    void PlaySound(string clips)
    {
        if (!UIManager.Instance.IsAnyUIActive)
        {
            if (enemySource.isPlaying && enemySource.clip == hunterClips[clips]) return;

            enemySource.Stop();
            enemySource.clip = hunterClips[clips];
            enemySource.Play();
        }
        else
        {
            enemySource.Stop();
        }
    }


    void Chasing()
    {
        isKill = false;
        SetAnimatorState("isChasing");
        PlaySound("chasing");
        GameObject target = player;

        /*
        if (dog != null && Vector2.Distance(transform.position, dog.transform.position) < Vector2.Distance(transform.position, player.transform.position))
        {
            target = dog;
        }*/

        Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
        FlipEnemy(targetPosition);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, berserkSpeed * Time.deltaTime);
    }

    //If you hide or enter the room in time while being hunted
    IEnumerator StopChasing()
    {
        isQuit = true;
        currentState = HunterState.None;
        CharacterManager.Instance.SetIsActive(false);

        if (enemySource.isPlaying)
        {
            enemySource.Stop();
        }

        SetAnimatorState("isSleep", false);
        h_animator.SetInteger("IdleVariant", 1);

        for(int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(1.2f);
            globalLight.intensity = i%2 == 0 ? 0.0f : 0.1f;

            if(i == 3)
                transform.position = new Vector3(54.22f, 0f, 0f);
        }

        yield return new WaitForSeconds(1.2f);
        h_animator.SetInteger("IdleVariant", 0);
        globalLight.intensity = 0.1f;
        ResetHunter(false);
        isQuit = false;

        if(!CharacterManager.Instance.isHiding())
            CharacterManager.Instance.SetIsActive(true);
    }

    IEnumerator WarpToTarget()
    {
        isWarping = true;
        yield return new WaitForSeconds(10.0f);

        if(currentState == HunterState.Chasing)
            transform.position = warpPoint[transform.position.y == 0f ? 0 : 1];

        yield return new WaitForSeconds(0.1f);

        if(currentState == HunterState.Sleep)
            transform.position = new Vector3(54.22f, 0f, 0f);

        yield return new WaitForSeconds(0.1f);

        isWarping = false; 
    }

    void Patrol()
    {
        SetAnimatorState("isWalk");
        PlaySound("walk");

        if (targetPatrolPoint == null) return;

        if (isWarp)
        {
            isWarp = false;
            StartCoroutine(WarpToNextFloor());
        }

        Vector2 patrolPosition = new Vector2(targetPatrolPoint.position.x, transform.position.y);
        FlipEnemy(patrolPosition);
        transform.position = Vector2.MoveTowards(transform.position, patrolPosition, speed * Time.deltaTime);


        if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 3.0f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoint.Length;
            targetPatrolPoint = patrolPoint[currentPatrolIndex];
            patrolCount += 1;
            isWarp = true;

            if ((pattrenIndex == 1 && patrolCount == 1) || (pattrenIndex == 2 && patrolCount == 5) || (pattrenIndex == 3 && patrolCount == 10))
            {
                currentState = HunterState.Sleep;
            }
        }
    }

    IEnumerator FinalPatrol()
    {
        // First Time
        if (!isTriggerFinal)
        {
            isTriggerFinal = true;
            currentState = HunterState.None;
            CharacterManager.Instance.SetIsActive(false);

            if (enemySource.isPlaying)
            {
                enemySource.Stop();
            }

            SetAnimatorState("isSleep", false);
            h_animator.SetInteger("IdleVariant", 1);

            for (int i = 0; i < 4; i++)
            {
                yield return new WaitForSeconds(1.2f);
                globalLight.intensity = i % 2 == 0 ? 0.0f : 0.1f;

                if(i == 1)
                {
                    //SoundFXManager.instance.PlaySoundFXClip(hunterClips["scream"], transform, false, 0.6f);
                }

                if (i == 3)
                {
                    transform.position = new Vector3(54.22f, 0f, 0f);
                    globalLight.intensity = 0f;
                }
            }

            ResetHunter(true);
            EventManager.Instance.UpdateEventDataTrigger(87, true);
            // Dialogue Before FinalPatrol
        }

        if (EventManager.Instance.IsEventTriggered(88))
        {
            SetAnimatorState("isChasing");
            PlaySound("walk");

            Vector2 patrolPosition = new Vector2(targetPatrolPoint.position.x, transform.position.y);
            FlipEnemy(patrolPosition);
            transform.position = Vector2.MoveTowards(transform.position, patrolPosition, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPatrolPoint.position) < 3.0f)
            {
                currentPatrolIndex = (currentPatrolIndex == 0) ? 4 : 0;
                targetPatrolPoint = patrolPoint[currentPatrolIndex];
            }
        }
    }

    IEnumerator WarpToNextFloor()
    {
        yield return new WaitForSeconds(0.1f);

        if ((currentPatrolIndex == 2 || currentPatrolIndex == 4) && currentState == HunterState.Patrol)
        {
            yield return new WaitForSeconds(1.5f);
            transform.position = warpPoint[currentPatrolIndex == 2 ? 0 : 1];
        }
    }

    void SleepTime()
    {
        Vector2 sleepPosition = new Vector2(sleepPoint.position.x, transform.position.y);
        FlipEnemy(sleepPosition);
        transform.position = Vector2.MoveTowards(transform.position, sleepPosition, speed * Time.deltaTime);
      
        if (Vector2.Distance(transform.position, sleepPosition) < 0.1f)
        {
            shadow.localPosition = new Vector3(0.85f, -2.89f, 0f);

            if (enemySource.isPlaying)
            {
                enemySource.Stop();
            }

            ResetHunter(false);
        }

    }

    public void ResetHunter(bool isFinal = false)
    {
        isKill = true;
        isFound = false;
        isWarp = false;
        isWarping = false;
        isQuit = false;

        currentPatrolIndex = 0;
        pattrenIndex = 0;
        patrolCount = 0;

        targetPatrolPoint = patrolPoint[currentPatrolIndex];
        Debug.Log("Current Patrol Point: " + currentPatrolIndex);

        if (!isFinal)
        {
            transform.position = new Vector3(54.22f, 0f, 0f);
            SetAnimatorState("isSleep");
            currentState = HunterState.Radio;
            radio.SetIsPlay(true);
        }
        else
        {
            transform.position = new Vector3(54.22f, 0f, 0f);
            currentState = HunterState.Final;
        }

    }

    private void FlipEnemy(Vector2 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void SetAnimatorState(string state, bool s = true)
    {
        foreach (AnimatorControllerParameter parameter in h_animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                h_animator.SetBool(parameter.name, false);
            }
        }

        h_animator.SetInteger("IdleVariant", 0);
        h_animator.SetBool(state, s);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Time.deltaTime != 0)
        {
            if (!isKill)
            {
                girlcontroller.SetIsMoving(false);
                girlcontroller.Animator.SetBool("isWalk", false);
                girlcontroller.Animator.SetBool("isDeath", true);
            }
        }

        if ((collision.CompareTag("Player") || collision.CompareTag("Dog")) && Time.deltaTime != 0)
        {
            if (!isKill)
            {
                currentState = HunterState.None;

                if (enemySource.isPlaying)
                {
                    enemySource.Stop();
                }

                isKill = true;
                Invoke("DecreaseHeart", 0.15f);
            }
        }

    }

    private void DecreaseHeart()
    {
        HeartManager.instance.HeartDecrease();
    }

    public void SetPattrenIndex(int p)
    {
        pattrenIndex = p;
    }


    public void setHunterState(HunterState state)
    {
        currentState = state;
    }

}

public enum HunterState { Radio, Sleep, Patrol, Chasing, Quit , Final ,None}
