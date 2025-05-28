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
    [SerializeField] private int longHandTargetAngle = 210;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private GameObject gameoverPanel;

    private int angleStep = 30;
    private GirlController girlController;
    private ActionText actionText;
    private bool rotateEnable = true;
    private bool isRotating = false;
    private float localLastMoveTime = 0f;

    public bool isActive { get; private set; }

    private void Start()
    {
        girlController = FindObjectOfType<GirlController>();
        actionText = FindObjectOfType<ActionText>();
        shortHand.gameObject.SetActive(false);
        shortHand.localEulerAngles = new Vector3(0, 0, -30);
    }

    void CancelWhileDeath()
    {
        isActive = false;
        clockPanel.SetActive(false);
    }

    private void Update()
    {
        if (gameoverPanel.activeSelf)
        {
            Invoke("CancelWhileDeath", 0.5f);
        }

        if (EventManager.Instance.IsEventTriggered(85))
        {
            shortHand.gameObject.SetActive(true);
        }

        if (clockPanel.activeSelf && isActive && rotateEnable && !isRotating)
        {
            if (InputManager.Instance.IsLeftPressed(ref localLastMoveTime) && shortHand.gameObject.activeSelf)
            {
                RotateClockHand(shortHand, angleStep);
            }
            else if (InputManager.Instance.IsRightPressed(ref localLastMoveTime) && shortHand.gameObject.activeSelf)
            {
                RotateClockHand(shortHand, -angleStep);
            }

            if (InputManager.Instance.IsUpPressed(ref localLastMoveTime))
            {
                RotateClockHand(longHand, -angleStep);
            }
            else if (InputManager.Instance.IsDownPressed(ref localLastMoveTime))
            {
                RotateClockHand(longHand, angleStep);
            }

            if (InputManager.Instance.IsXPressed())
            {
                StartCoroutine(Delay(true, 0.3f));
            }
        }
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle < 0)
            angle += 360;
        return angle;
    }

    IEnumerator Delay(bool sound, float time)
    {
        if (isActive)
        {
            clockPanel.SetActive(false);
            isActive = false;

            if (sound)
            {
                SoundFXManager.instance.PlaySoundFXClip(clips[2], transform, false, 1);
                girlController.InteractionIcon.SetActive(true);
            }

            yield return new WaitForSeconds(time);
            girlController.SetIsInteract(false);
            CharacterManager.Instance.SetIsActive(true);
        }
    }

    public void RotateClockHand(Transform handTransform, int angleStep)
    {
        float currentAngle = handTransform.eulerAngles.z;
        float newAngle = NormalizeAngle(currentAngle + angleStep);

        Vector3 newRotation = handTransform.eulerAngles;
        newRotation.z = newAngle;
        handTransform.eulerAngles = newRotation;

        SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1.0f);
        StartCoroutine(Rotating());
        CheckPuzzleCompletion();
    }

    private void CheckPuzzleCompletion()
    {
        if (Mathf.Approximately(shortHand.eulerAngles.z, shortHandTargetAngle))
            Debug.Log("Correct short hand");

        if (Mathf.Approximately(longHand.eulerAngles.z, longHandTargetAngle))
            Debug.Log("Correct long hand");

        if (Mathf.Approximately(shortHand.eulerAngles.z, shortHandTargetAngle) && Mathf.Approximately(longHand.eulerAngles.z, longHandTargetAngle))
        {
            StartCoroutine(Unlock());
        }
    }

    IEnumerator Rotating()
    {
        isRotating = true;
        yield return new WaitForSeconds(0.5f);
        isRotating = false;
    }

    IEnumerator Unlock()
    {
        yield return new WaitForSeconds(0.5f);
        girlController.SetIcon(false);
        rotateEnable = false;
        isRotating = true;
        yield return null;
        SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1.0f);
        InventoryManager.Instance.AddItem(14);
        actionText.ActionDisplay(LocalizationManager.Instance.GetText(88, PlayerDataManager.Instance.GetLanguage()));
        StartCoroutine(Delay(false, 2.0f));
        
        yield return new WaitForSeconds(2.0f);
        yield return null;

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
        }
    }
}
