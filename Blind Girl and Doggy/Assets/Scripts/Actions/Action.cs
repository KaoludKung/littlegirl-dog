using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Action : EventObject, Interactable
{
    [SerializeField] private ActionType actionType;
    [SerializeField] private int actionResultID;
    [SerializeField] private int itemID = 0;
    [SerializeField] private Sprite alertSprite;
    [SerializeField] private AudioClip actionClips;

    // special options
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private Image progressFill;
    [SerializeField] private float progressSpeed = 0.1f;
    [SerializeField] private string sceneName = "None";

    // unlock character controller
    [SerializeField] private bool playerEnable = true;
 
    // if that action more than 2 action such as use and pick up a item
    [SerializeField] private bool UseAndGet = false;
    [SerializeField] private int itemGetID = 0;

    private bool isAdd = false;
    private GirlController girlController;
    private NoteItem noteItem;
    private InventoryItem inventoryItem;
    private InventoryUI inventoryUI;
    private ActionText actionText;
    private AudioSource AudioSource;
    private string actionResult =  "";

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

    void Update()
    {
        if (!isComplete && progressBar != null && (girlController.IsMoving || (gameoverPanel != null && gameoverPanel.activeSelf)))
        {
            SetInteractionAnimation(false);
            StopAllCoroutines();
            girlController.SetIsInteract(false);

            if (AudioSource != null && AudioSource.isPlaying)
                AudioSource.Stop();

            if (progressFill != null)
            {
                progressFill.fillAmount = 0;
                progressBar.gameObject.SetActive(false);
            }
        }
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
                    PickUp();
                    break;
                case ActionType.UseItemShort:
                    StartCoroutine(UseAndGet ? UseGetItemShort() : UseItemShort());
                    break;
                case ActionType.UseItemLong:
                    StartCoroutine(UseAndGet ? UseGetItemLong() : UseItemLong());
                    break;
                case ActionType.ActiveNoAnimation:
                    StartCoroutine(ActiveEventNoAnimation());
                    break;
                case ActionType.ActiveEvent:
                    StartCoroutine(ActiveEvent());
                    break;
                case ActionType.TakeNote:
                    TakeNote();
                    break;
                case ActionType.Exit:
                    Exit();
                    break;
                default:
                    break;
            }
        }
    }

    void ResetAction()
    {
        SetInteractionAnimation(false);
        actionText.ActionDisplay(LocalizationManager.Instance.GetText(95, PlayerDataManager.Instance.GetLanguage()));
        CharacterManager.Instance.SetIsActive(true);
        girlController.SetIsInteract(false);

        if (progressBar != null && progressFill != null)
        {
            progressBar.SetActive(false);
            isComplete = false;
        }
    }

    private void SetInteractionAnimation(bool state)
    {
        if (girlController.Animator != null)
        {
            girlController.Animator.SetBool("isInteract", state);
        }
    }

    //Play Sound, Play/Stop Aniamtion
    IEnumerator ExcuteAction(bool exit = false)
    {
        SetInteractionAnimation(true);
        SoundFXManager.instance.PlaySoundFXClip(actionClips, transform, false, 1.0f);
        yield return new WaitForSeconds(actionClips.length);
        SetInteractionAnimation(false);

        StartCoroutine(FinalizeAction());

        if (exit)
        {
            yield return null;
            SceneManager.instance.ChangeScene(sceneName);
        }
    }


    void PickUp()
    {
        if (inventoryItem != null)
        {
            if (!inventoryItem.isCollected && InventoryUI.CollectedItems.Count < 6)
            {
                StartCoroutine(ExcuteAction());
                InventoryManager.Instance.AddItem(itemID);
            }
            else
            {
                ResetAction();
            }
        }
        
    }

    IEnumerator UseItemShort()
    {
        if (inventoryItem != null)
        {
            if (inventoryItem.isCollected)
            {
                StartCoroutine(ExcuteAction());
                InventoryManager.Instance.RemoveItem(itemID);
            }
            else
            {
                Debug.Log("There is not a item in inventory");
            }
        }

        yield return null;
        
    }

    IEnumerator UseGetItemShort()
    {
        if (inventoryItem != null && inventoryItem.isCollected)
        {
            if (InventoryUI.CollectedItems.Count < 7)
            {
                StartCoroutine(ExcuteAction());
                InventoryManager.Instance.RemoveItem(itemID);
                InventoryManager.Instance.AddItem(itemGetID);
            }
            else
            {
                ResetAction();
            }
        }
        else
        {
            Debug.Log("There is not a item in inventory");
        }

        yield return null;
    }

    IEnumerator UseItemLong()
    {
        AudioSource.clip = actionClips;
        CharacterManager.Instance.SetIsActive(true);
        progressBar.SetActive(true);
        SetInteractionAnimation(true);
        progressFill.fillAmount = 0;

        AudioSource.Play();

        while (progressFill.fillAmount < 1)
        {
            progressFill.fillAmount += progressSpeed * Time.deltaTime;
            yield return null;
        }

        isComplete = true;
        AudioSource.Stop();

        if (inventoryItem != null && isComplete)
        {
            if (inventoryItem.isCollected)
            {
                InventoryManager.Instance.RemoveItem(itemID);
                progressBar.SetActive(false);
                SetInteractionAnimation(false);
                StartCoroutine(FinalizeAction());
            }
            else if (!inventoryItem.isCollected && InventoryUI.CollectedItems.Count < 6)
            {
                InventoryManager.Instance.AddItem(itemID);
                progressBar.SetActive(false);
                SetInteractionAnimation(false);
                StartCoroutine(FinalizeAction());
            }
            else
            {
                ResetAction();
            }
        }
    }

    IEnumerator UseGetItemLong()
    {
        AudioSource.clip = actionClips;
        CharacterManager.Instance.SetIsActive(true);
        progressBar.SetActive(true);
        SetInteractionAnimation(true);
        progressFill.fillAmount = 0;

        AudioSource.Play();

        while (progressFill.fillAmount < 1)
        {
            progressFill.fillAmount += progressSpeed * Time.deltaTime;
            yield return null;
        }

        isComplete = true;
        AudioSource.Stop();

        if (inventoryItem != null && inventoryItem.isCollected && isComplete)
        {
            if (InventoryUI.CollectedItems.Count < 7)
            {
                //protect character move while process
                CharacterManager.Instance.SetIsActive(false);
                //girlController.ResetCurrentInteractable();

                InventoryManager.Instance.RemoveItem(itemID);
                progressBar.SetActive(false);
                SetInteractionAnimation(false);
                InventoryManager.Instance.AddItem(itemGetID);
                StartCoroutine(FinalizeAction());

                
            }
            else
            {
                ResetAction();
            }
        }
    }

    void TakeNote()
    {
        if (!noteItem.isCollected)
        {
            NoteManager.Instance.AddItem(itemID);
            StartCoroutine(ExcuteAction());
        }
    }
        
    IEnumerator ActiveEventNoAnimation()
    {
        if(actionClips != null)
            SoundFXManager.instance.PlaySoundFXClip(actionClips, transform, false, 1.0f);

        StartCoroutine(FinalizeAction());
        yield return null;
    }

    IEnumerator ActiveEvent()
    {
        SetInteractionAnimation(true);

        if (actionClips != null)
            SoundFXManager.instance.PlaySoundFXClip(actionClips, transform, false, 1.0f);

        StartCoroutine(FinalizeAction());
        yield return new WaitForSeconds(1.5f);
        SetInteractionAnimation(false);
    }

    void Exit()
    {
        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);
        StartCoroutine(ExcuteAction(true));
    }

    //After finish the action
    IEnumerator FinalizeAction()
    {
        EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);
        actionResult = LocalizationManager.Instance.GetText(actionResultID, PlayerDataManager.Instance.GetLanguage());
        actionText.ActionDisplay(actionResult);

        yield return new WaitForSeconds(0.15f);

        if (playerEnable)
        {
            CharacterManager.Instance.SetIsActive(true);
        }

        girlController.SetIsInteract(false);
        //girlController.ResetCurrentInteractable();

        yield return null;

        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAdd)
        {
            isAdd = true;
            girlController.AddInteractSprite(alertSprite);
            //girlController.InteractionIcon.GetComponent<SpriteRenderer>().sprite = alertSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isComplete)
        {
            SetInteractionAnimation(false);
            StopAllCoroutines();
            girlController.SetIsInteract(false);
            
            if(AudioSource != null && AudioSource.isPlaying)
                AudioSource.Stop();

            if (progressBar != null && progressFill != null)
            {
                progressFill.fillAmount = 0;
                progressBar.gameObject.SetActive(false);
            }
        }

        if(collision.CompareTag("Player") && isAdd)
        {
            isAdd = false;
        }
    }

    public void SetActionText(string newText)
    {
        actionResult = newText;
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