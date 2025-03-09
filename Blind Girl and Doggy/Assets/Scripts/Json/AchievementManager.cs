using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class AchievementItem
{
    public int id;
    public string achievementName;
    public string iconPath;
    public string description;
    public bool isCollected;

    public AchievementItem(int id, string name, string iconPath, string description, bool isCollected)
    {
        this.id = id;
        this.achievementName = name;
        this.iconPath = iconPath;
        this.description = description;
        this.isCollected = isCollected;
    }

    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>(iconPath);
    }
}


public class AchievementManager : JsonManager<AchievementData>
{
    public static AchievementManager Instance { get; private set; }
    private List<AchievementItem> items = new List<AchievementItem>();
   
    public List<AchievementItem> Items
    {
        get { return items; }
        set { items = value; }
    }

    public AchievementItem GetItemByID(int id)
    {
        foreach (AchievementItem item in items)
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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePaths("achievement.json");

        ///if (File.Exists(persistentPath)) > LoadInventory(persistentPath);
        if (File.Exists(persistentPath))
        {
            LoadAchievement(persistentPath);
        }
        else
        {
            if (File.Exists(streamingAssetsPath))
            {
                File.Copy(streamingAssetsPath, persistentPath);
                LoadAchievement(persistentPath);
                Debug.Log("Copied default achievement data to Persistent Data Path.");
            }
            else
            {
                Debug.LogError("Default achievement data not found in StreamingAssets.");
            }
        }
    }

    public void LoadAchievement(string path)
    {
        string jsonData = LoadJson(path);

        if (!string.IsNullOrEmpty(jsonData))
        {
            AchievementData loadedData = JsonUtility.FromJson<AchievementData>(jsonData);
            items = loadedData.items;
            Debug.Log("Achievement loaded from: " + jsonData);
        }
        else
        {
            Debug.LogError("Failed to load achievement data: No data found.");
        }
    }

    public void SaveAchievement()
    {
        string jsonData = JsonUtility.ToJson(new AchievementData(items), true);
        SaveJson(jsonData);

    }

    public void DeleteAchievement()
    {
        DeleteJson();

        if (File.Exists(persistentPath))
        {
            LoadAchievement(persistentPath);
        }
        else
        {
            if (File.Exists(streamingAssetsPath))
            {
                File.Copy(streamingAssetsPath, persistentPath);
                LoadAchievement(persistentPath);
                Debug.Log("Copied default inventory data to Persistent Data Path.");
            }
            else
            {
                Debug.LogError("Default inventory data not found in StreamingAssets.");
            }
        }
    }

    public bool GetAchievementIsCollected(int achievementId)
    {
        AchievementItem achievementToAdd = items.Find(item => item.id == achievementId);

        if (achievementToAdd != null)
        {
            return achievementToAdd.isCollected;
        }

        Debug.LogWarning($"Achievement with ID '{achievementId}' not found.");
        return false;

    }

    public void UpdateAchievementTrigger(int achievementId)
    {
        AchievementItem achievementToAdd = items.Find(item => item.id == achievementId);
        
        if (achievementToAdd != null)
        {
            if (!achievementToAdd.isCollected)
            {
                achievementToAdd.isCollected = true;
                Debug.Log("Update " + achievementToAdd.achievementName);
                SaveAchievement();
            }
            else
            {
                Debug.Log("Achievement already unlocks in data");
            }
        }

    }
}

[System.Serializable]
public class AchievementData
{
    public List<AchievementItem> items;

    public AchievementData(List<AchievementItem> items)
    {
        this.items = items;
    }
}
