using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerformAction : EventObject, Interactable
{
    [SerializeField] private int itemID;
    [SerializeField] private AudioClip actionClip;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private float increaseAmount = 0.1f; 
    [SerializeField] private float maxValue = 1f;
    [SerializeField] private ActionType actionType;
    [SerializeField] private ItemActionType itemActionType;

    private bool isComplete;
    private AudioSource actionSource;
    private InventoryItem inventoryItem;
    private Coroutine longActionCoroutine;
    private GirlControl girlControl;

    private void Awake()
    {
        actionSource = GetComponent<AudioSource>();
    }

    public AudioSource ActionSource
    {
        get { return actionSource; }
    }

    private void Start()
    {
        isComplete = false;
        inventoryItem = InventoryManager.Instance.GetItemByID(itemID);
        girlControl = FindObjectOfType<GirlControl>();

        if (progressSlider != null)
        {
            progressSlider.value = 0;
            progressSlider.gameObject.SetActive(false);
        }

    }

    public void Interact()
    {
        //if player has a item for action then the trigger unlock to player can action
        if (EventManager.Instance.IsEventTriggered(eventID) && girlControl != null && !girlControl.IsMoving && !girlControl.IsBarking)
        {
            if(actionType == ActionType.Short)
            {
                SoundFXManager.instance.PlaySoundFXClip(actionClip, transform, false, 1.0f);
                Debug.Log("Sound Short");
                HandleAction();
            }
            else
            {
                longActionCoroutine = StartCoroutine(PerformLongAction());
            }
           
        }
        else
        {
            Debug.Log("Cannot perform Action A while moving or barking.");
        }
    }

    void HandleAction()
    {
        if(itemActionType == ItemActionType.WithItem)
        {
            if(inventoryItem != null)
            {
                if (inventoryItem.isCollected)
                {
                    InventoryManager.Instance.RemoveItem(itemID);
                }
                else
                {
                    InventoryManager.Instance.AddItem(itemID);
                }

                StartCoroutine(FinalizeEvent());
            }
            else
            {
                Debug.LogError("Inventory item not found.");
            }
            
        }
        else
        {
            StartCoroutine(FinalizeEvent());
        }
    }

    IEnumerator FinalizeEvent()
    {
        //EventManager.Instance.UpdataEventDataExcuted(eventID, true);

        if (TriggerEventID != 0)
            EventManager.Instance.UpdateEventDataTrigger(TriggerEventID, true);

        if (actionType == ActionType.Short)
        {
            Debug.Log("Performing Short Action");
            yield return new WaitForSeconds(actionClip.length);
        }
        else
        {
            Debug.Log("Performing Long Action");
        }

        this.gameObject.SetActive(false);

    }

    private IEnumerator PerformLongAction()
    {
        if (progressSlider != null)
        {
            progressSlider.gameObject.SetActive(true);
            progressSlider.value = 0;
            SoundLoopManager.instance.PlayLoopSound(actionSource, actionClip, false, 1.0f);

            while (progressSlider.value < maxValue)
            {
                progressSlider.value += increaseAmount * Time.deltaTime;
                yield return null;
            }

            isComplete = true;
            StartCoroutine(SoundLoopManager.instance.StopLoopSound());
            progressSlider.gameObject.SetActive(false);
            HandleAction();

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && longActionCoroutine != null && !isComplete)
        {
            StopCoroutine(longActionCoroutine);
            if (progressSlider != null)
            {
                progressSlider.value = 0;
                progressSlider.gameObject.SetActive(false);
            }
        }
    }


    public enum ActionType
    {
        Short,
        Long
    }

    public enum ItemActionType
    {
        WithItem,
        WithoutItem
    }
}
