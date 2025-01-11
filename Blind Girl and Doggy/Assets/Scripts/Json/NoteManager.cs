using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


[System.Serializable]
public class NoteItem
{
    public int id;
    public string noteName;
    public string noteDate;
    public string noteDetail;
    public bool isCollected;

    public NoteItem(int id, string name, string date, string detail, bool isCollected)
    {
        this.id = id;
        this.noteName = name;
        this.noteDate = date;
        this.noteDetail = detail;
        this.isCollected = isCollected;
    }

}

public class NoteManager : JsonManager<NoteData>
{
    public static NoteManager Instance { get; private set; }
    private List<NoteItem> items = new List<NoteItem>();
    private int maxItems = 9;
    private NoteUI noteUI;
    public NoteUI NoteUI
    {
        get
        {
            if (noteUI == null || !noteUI.gameObject.activeInHierarchy)
            {
                noteUI = FindObjectOfType<NoteUI>();
            }
            return noteUI;
        }
    }


    public List<NoteItem> Items
    {
        get { return items; }
        set { items = value; }
    }

    public int MaxItems
    {
        get { return maxItems; }
        private set { maxItems = value; }
    }

    public NoteItem GetItemByID(int id)
    {
        foreach (NoteItem item in items)
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
        //noteUI = FindObjectOfType<NoteUI>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializePaths("note.json");

        ///if (File.Exists(persistentPath)) > LoadInventory(persistentPath);
        if (File.Exists(persistentPath))
        {
            LoadNote(persistentPath);
        }
        else
        {
            if (File.Exists(streamingAssetsPath))
            {
                File.Copy(streamingAssetsPath, persistentPath);
                LoadNote(persistentPath);
                Debug.Log("Copied default note data to Persistent Data Path.");
            }
            else
            {
                Debug.LogError("Default note data not found in StreamingAssets.");
            }
        }
    }

    public void LoadNote(string path)
    {
        string jsonData = LoadJson(path);

        if (!string.IsNullOrEmpty(jsonData))
        {
            NoteData loadedData = JsonUtility.FromJson<NoteData>(jsonData);
            items = loadedData.items;
            Debug.Log("Note loaded from: " + jsonData);
        }
        else
        {
            Debug.LogError("Failed to load note data: No data found.");
        }
    }

    public void SaveNote()
    {
        string jsonData = JsonUtility.ToJson(new NoteData(items), true);
        SaveJson(jsonData);

    }

    public void DeleteNote()
    {
        DeleteJson();
        //InitializePaths("inventory.json");
    }

    public void AddItem(int itemId)
    {
        NoteItem itemToAdd = items.Find(item => item.id == itemId);

        if (itemToAdd != null)
        {
            if (!itemToAdd.isCollected)
            {
                itemToAdd.isCollected = true;
                Debug.Log("Item collected: " + itemToAdd.noteName);
                NoteUI.UpdateNoteUI();
                //SaveInventory();
            }
            else
            {
                Debug.Log("Item already exists in note inventory");
            }
        }
        else
        {
            Debug.Log("Item not found in note data");
        }
    }

   
}

[System.Serializable]
public class NoteData
{
    public List<NoteItem> items;

    public NoteData(List<NoteItem> items)
    {
        this.items = items;
    }
}
