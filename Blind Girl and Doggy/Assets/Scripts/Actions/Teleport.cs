using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour, Interactable
{
    [SerializeField] private TeleportOption teleportOption;
    [SerializeField] private GameObject[] objects;
    [SerializeField] private int cameraID;
    [SerializeField] private Vector2 girl_TeleportPosition;
    [SerializeField] private Vector2 dog_TeleportPosition;
    [SerializeField] private AudioClip teleportClip;
    [SerializeField] private Sprite alertSprite;
    [SerializeField] public bool canHide = true;

    private GirlController girlController;
    private CameraSwitcher cameraSwitcher;

    private bool isAdd = false;
    private bool isHiding;
    public bool IsHiding => isHiding;

    // Start is called before the first frame update
    void Start()
    {
        girlController = FindObjectOfType<GirlController>();
        cameraSwitcher = FindObjectOfType<CameraSwitcher>();
    }

    public void Interact()
    {
        //girlControl != null && !girlControl.IsMoving && !girlControl.IsBarking
        if (!girlController.IsMoving && !girlController.IsBarking)
        {
            CharacterManager.Instance.SetIsActive(false);
            StartCoroutine(HandleTeleport());
        }
        else
        {
            Debug.Log("Cannot perform Action A while moving or barking.");
        }
    }

    private IEnumerator HandleTeleport()
    {
        girlController.Animator.SetBool("isInteract", true);

        if (EventManager.Instance.IsEventTriggered(88))
        {
            canHide = true;
        }

        if (teleportOption == TeleportOption.PlaySound)
        {
            SoundFXManager.instance.PlaySoundFXClip(teleportClip, transform, false, 1.0f);
            yield return new WaitForSeconds(teleportClip.length); 
        }

        objects[0].transform.position = girl_TeleportPosition;
        objects[1].transform.position = dog_TeleportPosition;

        cameraSwitcher.SwitchCamera(cameraID);
        Debug.Log("Performing Action Teleport");

        girlController.Animator.SetBool("isInteract", false);
        isHiding = true;

        yield return new WaitForSecondsRealtime(0.5f);
        CharacterManager.Instance.SetIsActive(true);
        isHiding = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAdd)
        {
            isAdd = true;
            girlController.AddInteractSprite(alertSprite);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAdd)
        {
            isAdd = false;
        }
    }

    enum TeleportOption
    {
        PlaySound,
        NoSound
    }
}
