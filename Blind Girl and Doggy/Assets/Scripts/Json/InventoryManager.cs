using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class InventoryManager : JsonManager<InventoryData>
{
    public static InventoryManager Instance { get; private set; }
    private List<InventoryItem> items = new List<InventoryItem>();
    private int maxItems = 6;
    private InventoryUI inventoryUI;
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


    public List<InventoryItem> Items
    {
        get { return items; }
        set { items = value; }
    }

    public int MaxItems
    {
        get { return maxItems; }
        private set { maxItems = value; } 
    }

    public InventoryItem GetItemByID(int id)
    {
        foreach (InventoryItem item in items)
        {
            if (item.id == id) 
            {
                return item;
            }
        }
        return null;
    }


    private void Awake()
    {
        //inventoryUI = FindObjectOfType<InventoryUI>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePaths("inventory.json");

        ///if (File.Exists(persistentPath)) > LoadInventory(persistentPath);
        if (File.Exists(persistentPath))
        {
            LoadInventory(persistentPath);
        }
        else
        {
            if (File.Exists(streamingAssetsPath))
            {
                File.Copy(streamingAssetsPath, persistentPath);
                LoadInventory(persistentPath);
                Debug.Log("Copied default inventory data to Persistent Data Path.");
            }
            else
            {
                Debug.LogError("Default inventory data not found in StreamingAssets.");
            }
        }
    }

    public void LoadInventory(string path)
    {
        string jsonData = LoadJson(path);

        if (!string.IsNullOrEmpty(jsonData))
        {
            InventoryData loadedData = JsonUtility.FromJson<InventoryData>(jsonData);
            items = loadedData.items;
            Debug.Log("Inventory loaded from: " + jsonData);
        }
        else
        {
            Debug.LogError("Failed to load inventory data: No data found.");
        }
    }

    public void SaveInventory()
    {
        string jsonData = JsonUtility.ToJson(new InventoryData(items), true);
        SaveJson(jsonData);

    }

    public void DeleteInventory()
    {
        DeleteJson();
        //InitializePaths("inventory.json");
    }

    public void AddItem(int itemId)
    {
        InventoryItem itemToAdd = items.Find(item => item.id == itemId);

        if (itemToAdd != null)
        {
            if (!itemToAdd.isCollected)
            {
                itemToAdd.isCollected = true;
                Debug.Log("Item collected: " + itemToAdd.itemName);
                InventoryUI.UpdateInventoryUI();
                //SaveInventory();
            }
            else
            {
                Debug.Log("Item already exists in inventory");
            }
        }
        else
        {
            Debug.Log("Item not found in inventory data");
        }
    }

    public void RemoveItem(int itemId)
    {
        InventoryItem itemToRemove = items.Find(item => item.id == itemId);
        if (itemToRemove != null)
        {
            if (itemToRemove.isCollected)
            {
                itemToRemove.isCollected = false;
                InventoryUI.UpdateInventoryUI();
                Debug.Log("Remove a item from inventory");
                //SaveInventory();
            }
        }
        else
        {
            Debug.Log("Item not found in inventory");
        }
    }


}

[System.Serializable]
public class InventoryData
{
    public List<InventoryItem> items;

    public InventoryData(List<InventoryItem> items)
    {
        this.items = items;
    }
}
