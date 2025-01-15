using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Action : EventObject, Interactable
{
    [SerializeField] ActionType actionType;
    [SerializeField] string actionResult;
    [SerializeField] int itemID = 0;
    [SerializeField] Sprite alertSprite;
    [SerializeField] AudioClip actionClips;
    [SerializeField] GameObject progressBar;
    [SerializeField] Image progressFill;
    [SerializeField] float progressSpeed = 0.1f;
    [SerializeField] string sceneName = "None";

    private GirlController girlController;
    private NoteItem noteItem;
    private InventoryItem inventoryItem;
    private InventoryUI inventoryUI;
    private ActionText actionText;
    private AudioSource AudioSource;

    public InventoryUI InventoryUI
    {
        get
        {
            if (inventoryUI == null || !inventoryUI.gameObject.activeInHierarchy)
            {
                inventoryUI = FindObjectOfType<InventoryUI>();
            }
            return inventoryUI;
        }
    }


    private bool isComplete = false;

    private void Awake()
    {
        girlController = FindObjectOfType<GirlController>();
        actionText = FindObjectOfType<ActionText>();
        AudioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryItem = InventoryManager.Instance.GetItemByID(itemID);
        noteItem = NoteManager.Instance.GetItemByID(itemID);
    }

    public void Interact()
    {
        if (EventManager.Instance.IsEventTriggered(eventID) && !girlController.IsMoving && !girlController.IsBarking)
        {
            girlController.SetIsInteract(true);
            girlController.InteractionIcon.SetActive(false);
            CharacterManager.Instance.SetIsActive(false);

            switch (actionType)
            {
                case ActionType.PickUp:
                    StartCoroutine(PickUp());
                    break;
                case ActionType.UseItemShort:
                    StartCoroutine(UseItemShort());
                    break;
                case ActionType.UseItemLong:
                    StartCoroutine(UseItemLong());
                    break;
                case ActionType.ActiveNoAnimation:
                    StartCoroutine(ActiveEventNoAnimation());
                    break;
                case ActionType.ActiveEvent:
                    StartCoroutine(ActiveEvent());
                    break;
                case ActionType.TakeNote:
                    StartCoroutine(TakeNote());
                    break;
                case ActionType.Exit:
                    StartCoroutine(Exit());
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator PickUp()
    {
        if(inventoryItem != null)
        {
            if (!inventoryItem.isCollected && InventoryUI.CollectedItems.Count < 7)
            {
                girlController.Animator.SetBool("isInteract", true);
                SoundFXManager.instance.PlaySoundFXClip(actionClips, transform, false, 1.0f);
                yield return new WaitForSeconds(actionClips.length);
                girlController.Animator.SetBool("isInteract", false);
                InventoryManager.Instance.AddItem(itemID);
                StartCoroutine(FinalizeAction());
            }
            else
            {
                girlController.Animator.SetBool("isInteract", true);
                actionText.ActionDisplay("Oops, Your inventory is full.");
                CharacterManager.Instance.SetIsActive(true);
                girlController.Animator.SetBool("isInteract", false);
                girlController.SetIsInteract(false);

            }
        }
        
    }

    IEnumerator UseItemShort()
    {
        if (inventoryItem != null)
        {
            if (inventoryItem.isCollected)
            {
                girlController.Animator.SetBool("isInteract", true);
                SoundFXManager.instance.PlaySoundFXClip(actionClips, transform, false, 1.0f);
                yield return new WaitForSeconds(actionClips.length);
                girlController.Animator.SetBool("isInteract", false);
                InventoryManager.Instance.RemoveItem(itemID);
                StartCoroutine(FinalizeAction());
            }
            else
            {
                Debug.Log("There is not a item in inventory");
            }
        }
        
    }

    IEnumerator UseItemLong()
    {
        CharacterManager.Instance.SetIsActive(true);
        progressBar.SetActive(true);
        girlController.Animator.SetBool("isInteract", true);
        progressFill.fillAmount = 0;

        AudioSource.Play();
        while (progressFill.fillAmount < 1)
        {
            progressFill.fillAmount += progressSpeed * Time.deltaTime;
            yield return null;
        }


        isComplete = true;
        AudioSource.Stop();

        if (inventoryItem != null)
        {
            if (inventoryItem.isCollected)
            {
                InventoryManager.Instance.RemoveItem(itemID);
                progressBar.SetActive(false);
                girlController.Animator.SetBool("isInteract", false);
                StartCoroutine(FinalizeAction());
            }
            else if (!inventoryItem.isCollected && InventoryUI.CollectedItems.Count < 7)
            {
                InventoryManager.Instance.AddItem(itemID);
                progressBar.SetActive(false);
                girlController.Animator.SetBool("isInteract", false);
                StartCoroutine(FinalizeAction());
            }
            else
            {
                girlController.Animator.SetBool("isInteract", true);
                actionText.ActionDisplay("Oops, Your inventory is full.");
                progressBar.SetActive(false);
                isComplete = false;
                CharacterManager.Instance.SetIsActive(true);
                girlController.Animator.SetBool("isInteract", false);
                girlController.SetIsInteract(false);
            }
        }
    }  

    IEnumerator TakeNote()
    {
        if (!noteItem.isCollected)
        {
            NoteManager.Instance.AddItem(itemID);
            girlController.Animator.SetBool("isInteract", true);
            SoundFXManager.instance.PlaySoundFXClip(actionClips, transform, false, 1.0f);
            yield return new WaitForSeconds(actionClips.length);
            girlController.Animator.SetBool("isInteract", false);
            StartCoroutine(FinalizeAction());
        }
    }
        
    IEnumerator ActiveEventNoAnimation()
    {
        if(actionClips != null)
            SoundFXManager.instance.PlaySoundFXClip(actionClips, transform, false, 1.0f);

        StartCoroutine(FinalizeAction());
        yield return new WaitForSeconds(1.5f);
    }


    IEnumerator ActiveEvent()
    {
        girlController.Animator.SetBool("isInteract", true);

        if(actionClips != null)
            SoundFXManager.instance.PlaySoundFXClip(actionClips, transform, false, 1.0f);

        StartCoroutine(FinalizeAction());
        yield return new WaitForSeconds(1.5f);
        girlController.Animator.SetBool("isInteract", false);
    }

    IEnumerator Exit()
    {
        girlController.Animator.SetBool("isInteract", true);
        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);
        SoundFXManager.instance.PlaySoundFXClip(actionClips, transform, false, 1.0f);
        yield return new WaitForSeconds(actionClips.length);
        girlController.Animator.SetBool("isInteract", false);
        SceneManager.instance.ChangeScene(sceneName);
    }

    IEnumerator FinalizeAction()
    {
        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);
        actionText.ActionDisplay(actionResult);
        //actionText.text = actionResult;

        yield return new WaitForSeconds(0.2f);
        CharacterManager.Instance.SetIsActive(true);
        girlController.SetIsInteract(false);

        yield return null;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            girlController.InteractionIcon.GetComponent<SpriteRenderer>().sprite = alertSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isComplete)
        {
            girlController.Animator.SetBool("isInteract", false);
            StopAllCoroutines();
            girlController.SetIsInteract(false);
            
            if(AudioSource != null)
                AudioSource.Stop();

            if (progressFill != null)
            {
                progressFill.fillAmount = 0;
                progressBar.gameObject.SetActive(false);
            }
        }
    }

}

public enum ActionType
{
    PickUp,
    UseItemShort,
    UseItemLong,
    ActiveNoAnimation,
    ActiveEvent,
    TakeNote,
    Exit
}