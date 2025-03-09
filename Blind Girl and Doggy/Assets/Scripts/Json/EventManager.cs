using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class EventData
{
    public int id;
    public string nameEvent;
    public string type;
    public bool trigger;
    //public bool hasExecuted;

    public EventData(int id, string nameEvent, string type, bool trigger = false)
    {
        this.id = id;
        this.nameEvent = nameEvent;
        this.type = type;
        this.trigger = trigger;
        //this.hasExecuted = hasExecuted;
    }
}

public class EventManager : JsonManager<EventData>
{
    public static EventManager Instance { get; private set; }
    private List<EventData> eventList = new List<EventData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializePaths("eventData.json");
        eventList = new List<EventData>();

        ///if (File.Exists(persistentPath)) > LoadInventory(persistentPath);
        if (File.Exists(persistentPath))
        {
            LoadEventData(persistentPath);
        }
        else
        {
            if (File.Exists(streamingAssetsPath))
            {
                File.Copy(streamingAssetsPath, persistentPath);
                LoadEventData(persistentPath);
                Debug.Log("Copied default event data to Persistent Data Path.");
            }
            else
            {
                Debug.LogError("Default event data not found in StreamingAssets.");
            }
        }

    }

    public void LoadEventData(string path)
    {
        string jsonData = LoadJson(path);

        if (!string.IsNullOrEmpty(jsonData))
        {
            eventList = JsonUtility.FromJson<EventDataListWrapper>(jsonData).eventList;
            Debug.Log("Loaded event data: " + jsonData);
        }
        else
        {
            Debug.LogError("Failed to load event data: No data found.");
        }
    }

    public void SaveEventData()
    {
        string jsonData = JsonUtility.ToJson(new EventDataListWrapper { eventList = eventList }, true);
        SaveJson(jsonData);
    }

    public void DeleteEventData()
    {
        DeleteJson();
        InitializePaths("inventory.json");
    }

    public List<EventData> GetEventList() => eventList;

    public void UpdateEventDataTrigger(int eventId, bool newTriggerStatus)
    {
        EventData eventData = eventList.Find(e => e.id == eventId);
        if (eventData != null)
        {
            eventData.trigger = newTriggerStatus;
            //Debug.Log($"Event '{eventData.nameEvent}' updated to: {newTriggerStatus}");
            //SaveEventData();
        }
        else
        {
            Debug.LogWarning($"Event with ID '{eventId}' not found.");
        }
    }

    public bool IsEventTriggered(int eventId)
    {
        EventData eventData = eventList.Find(e => e.id == eventId);

        if (eventData != null)
        {
            return eventData.trigger;
        }
 
        Debug.LogWarning($"Event with ID '{eventId}' not found.");
        return false;
        
    }
}

[Serializable]
public class EventDataListWrapper
{
    public List<EventData> eventList;
}
