using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Sprite emptySprite;
    [SerializeField] private AudioClip clickClip;

    private InventoryManager inventory;

    private void Awake()
    {
        inventory = FindObjectOfType<InventoryManager>();
    }

    void Start()
    {
        Initialize();
        inventory = FindObjectOfType<InventoryManager>();
        UpdateInventoryUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {

        }
    }

    void UpdateItemMenu()
    {

    }


    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        SoundFXManager.instance.PlaySoundFXClip(clickClip, transform, false, 1.0f);

        if (inventoryPanel.activeSelf)
        {
            UpdateInventoryUI();
        }
    }

    public void UpdateInventoryUI()
    {
        foreach (Transform child in itemSlotContainer)
        {
            Destroy(child.gameObject);
        }

        List<InventoryItem> collectedItems = new List<InventoryItem>();

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

            if (i < collectedItems.Count)
            {
                InventoryItem item = collectedItems[i];
                Button itemButton = itemSlot.GetComponent<Button>();
                var itemImage = itemSlot.transform.Find("item_image").GetComponent<Image>();

                if (itemImage != null)
                    itemImage.sprite = item.GetIcon();

                if (itemButton != null)
                    itemButton.onClick.AddListener(() => ShowItemDescription(item));
            }
            else
            {
                var itemImage = itemSlot.transform.Find("item_image").GetComponent<Image>();
                if (itemImage != null)
                    itemImage.sprite = emptySprite;

                Button itemButton = itemSlot.GetComponent<Button>();
                if (itemButton != null)
                {
                    itemButton.onClick.RemoveAllListeners();
                    itemButton.transition = Selectable.Transition.None;
                }
            }
        }
    }


    private void ShowItemDescription(InventoryItem item)
    {
        SoundFXManager.instance.PlaySoundFXClip(clickClip, transform, false, 1.0f);

        if (itemImage != null)
            itemImage.sprite = item.GetIcon();

        itemNameText.text = item.itemName;
        descriptionText.text = item.description;

        Debug.Log($"Showing description for: {item.itemName}");
    }

    void Initialize()
    {
        itemImage.sprite = emptySprite;
        itemNameText.text = "";
        descriptionText.text = "";
        //inventoryPanel.SetActive(false);
    }
   
}
