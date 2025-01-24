using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite[] slotSelect;
    [SerializeField] private AudioClip[] clips; // 0: selected, 1: pressed
    [SerializeField] private bool isUnlock = true;
    
    private InventoryManager inventory;
   
    public bool isActive { get; private set; }

    private int currentIndex = 0;
    private const int columns = 3; // columns in Inventory
    private const int rows = 2;    // rows in Inventory
    private List<GameObject> itemSlots = new List<GameObject>();
    private List<InventoryItem> collectedItems = new List<InventoryItem>(); 
    
    public List<InventoryItem> CollectedItems => collectedItems;

    private void Awake()
    {
        inventory = FindObjectOfType<InventoryManager>();
  
    }

    void Start()
    {
        Initialize();
        UpdateInventoryUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (!inventoryPanel.activeSelf && isUnlock &&!UIManager.Instance.IsAnyUIActive)
            {
                isActive = true;
                CharacterManager.Instance.SoundPause();
                CharacterManager.Instance.SetIsActive(false);
                SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1.0f);
                StartCoroutine(ToggleInventory());
                UIManager.Instance.ToggleTimeScale(true);
            }     
        }

        if(Input.GetKeyDown(KeyCode.X) && inventoryPanel.activeSelf)
        {
            SoundFXManager.instance.PlaySoundFXClip(clips[1], transform, false, 1.0f);
            StartCoroutine(ToggleInventory());
            StartCoroutine(Delay());
        }

        if (inventoryPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) MoveHighlight(-1, 0);
            if (Input.GetKeyDown(KeyCode.RightArrow)) MoveHighlight(1, 0);
            if (Input.GetKeyDown(KeyCode.UpArrow)) MoveHighlight(0, -1);
            if (Input.GetKeyDown(KeyCode.DownArrow)) MoveHighlight(0, 1);
        }
    }

    void MoveHighlight(int x, int y)
    {
        int newRow = currentIndex / columns + y;
        int newColumn = currentIndex % columns + x;

        if (newRow >= 0 && newRow < rows && newColumn >= 0 && newColumn < columns)
        {
            int newIndex = newRow * columns + newColumn;
            if (newIndex < itemSlots.Count)
            {
                currentIndex = newIndex;
                UpdateItemMenu();
                SoundFXManager.instance.PlaySoundFXClip(clips[0], transform, false, 1.0f);
            }
        }
    }

    public void UpdateInventoryUI()
    {
        foreach (Transform child in itemSlotContainer)
        {
            Destroy(child.gameObject);
        }
        itemSlots.Clear();

        collectedItems.Clear();
        foreach (var item in inventory.Items)
        {
            if (item.isCollected)
            {
                collectedItems.Add(item);
            }
        }

        for (int i = 0; i < inventory.MaxItems; i++)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, itemSlotContainer);
            itemSlots.Add(itemSlot);

            if (i < collectedItems.Count)
            {
                InventoryItem item = collectedItems[i];
                var itemImage = itemSlot.transform.Find("item_image").GetComponent<Image>();

                if (itemImage != null)
                    itemImage.sprite = item.GetIcon();
            }
            else
            {
                var itemImage = itemSlot.transform.Find("item_image").GetComponent<Image>();
                if (itemImage != null)
                    itemImage.sprite = emptySprite;
            }
        }

        currentIndex = 0; // Reset highlight to first slot
        UpdateItemMenu();
    }

    void UpdateItemMenu()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            var slot = itemSlots[i];
            var slotImage = slot.GetComponent<Image>();

            if (i == currentIndex)
            {
                slotImage.sprite = slotSelect[1];

                if (i < collectedItems.Count)
                {
                    ShowItemDescription(collectedItems[i]);
                }
                else
                {
                    ClearItemDescription();
                }
            }
            else
            {
                slotImage.sprite = slotSelect[0];
            }
        }
    }

    private void ShowItemDescription(InventoryItem item)
    {
        if (itemImage != null)
            itemImage.sprite = item.GetIcon();

        itemNameText.text = item.itemName;
        descriptionText.text = item.description;
    }

    private void ClearItemDescription()
    {
        itemImage.sprite = emptySprite;
        itemNameText.text = "";
        descriptionText.text = "";
    }

    void Initialize()
    {
        itemImage.sprite = emptySprite;
        itemNameText.text = "";
        descriptionText.text = "";
    }

    IEnumerator ToggleInventory()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        
        if (inventoryPanel.activeSelf)
        {
            UpdateInventoryUI();
        }

    }

    IEnumerator Delay()
    {
        if (isActive)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            isActive = false;
            UIManager.Instance.ToggleTimeScale(false);
            CharacterManager.Instance.SoundUnPause();

            if (Hiding.instance?.IsHidden != true)
            {
                CharacterManager.Instance.SetIsActive(true);
            }

            //CharacterManager.Instance.SetIsActive(true);
        }
    }

    public void Unlock(bool n)
    {
        isUnlock = n;
    }
}


