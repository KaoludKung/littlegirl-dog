using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPuzzle : EventObject, Interactable
{
    [SerializeField] GameObject clockPanel;
    [SerializeField] GameObject shortHandA;
    [SerializeField] private Transform shortHand;
    [SerializeField] private Transform longHand;
    [SerializeField] private Sprite alertSprite;

    [SerializeField] private int shortHandTargetAngle = 90;
    [SerializeField] private int longHandTargetAngle = 180;
    [SerializeField] private AudioClip[] clips;

    private int angleStep = 30;
    private GirlController girlController;
    private ActionText actionText;
    private bool rotateEnable = true;

    public bool isActive { get; private set; }

    private void Start()
    {
        girlController = FindObjectOfType<GirlController>();
        actionText = FindObjectOfType<ActionText>();
        shortHand.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (EventManager.Instance.IsEventTriggered(85))
        {
            shortHand.gameObject.SetActive(true);
        }


        if (clockPanel.activeSelf && isActive && rotateEnable)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && shortHand.gameObject.activeSelf)
            {
                RotateClockHand(shortHand, angleStep);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && shortHand.gameObject.activeSelf)
            {
                RotateClockHand(shortHand, -angleStep);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RotateClockHand(longHand, -angleStep);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                RotateClockHand(longHand, angleStep);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(Delay(true));
            }
        }

    }

    IEnumerator Delay(bool sound)
    {
        if (isActive)
        {
            clockPanel.SetActive(false);
            isActive = false;

            if (sound)
            {
                SoundFXManager.instance.PlaySoundFXClip(clips[2], transform, false, 1);
            }

            yield return new WaitForSeconds(0.3f);
            girlController.SetIsInteract(false);
            girlController.InteractionIcon.SetActive(true);
            CharacterManager.Instance.SetIsActive(true);
        }
    }

    public void RotateClockHand(Transform transform , int angleStep)
    {
        transform.Rotate(0, 0, angleStep);
        SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1.0f);
        CheckPuzzleCompletion();
    }

    private void CheckPuzzleCompletion()
    {
        if (Mathf.Approximately(shortHand.eulerAngles.z, shortHandTargetAngle) && Mathf.Approximately(longHand.eulerAngles.z, longHandTargetAngle))
        {
            StartCoroutine(Unlock());
        }
    }

    IEnumerator Unlock()
    {
        rotateEnable = false;
        yield return null;
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1.0f);
        InventoryManager.Instance.AddItem(14);
        actionText.ActionDisplay("Margarete has gotten the flower coin.");
        StartCoroutine(Delay(false));

        yield return new WaitForSeconds(0.3f);
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        //80: pick up ladder to unlock
        if (EventManager.Instance.IsEventTriggered(80) && !clockPanel.activeSelf)
        {
            if (EventManager.Instance.IsEventTriggered(85))
            {
                InventoryManager.Instance.RemoveItem(10);
                shortHandA.SetActive(true);
            }

            girlController.SetIsInteract(true);
            girlController.InteractionIcon.SetActive(false);
            CharacterManager.Instance.SetIsActive(false);

            StartCoroutine(Open());
        }
    }

    IEnumerator Open()
    {
        isActive = true;
        yield return new WaitForSeconds(0.1f);
        clockPanel.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            girlController.AddInteractSprite(alertSprite);
            //girlController.InteractionIcon.GetComponent<SpriteRenderer>().sprite = alertSprite;
        }
    }
}
