using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LocalizationData
{
    public List<LocalizationItem> items = new List<LocalizationItem>();
}

[System.Serializable]
public class LocalizationItem
{
    public int key;
    public LanguageData value;
}

[System.Serializable]
public class LanguageData
{
    public string en;
    public string th;
    public string tr;
    public string id;
}

public class LocalizationManager : JsonManager<LocalizationData>
{
    public static LocalizationManager Instance { get; private set; }
    private Dictionary<int, LanguageData> localizationDataDictionary = new Dictionary<int, LanguageData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
        InitializePaths("localization.json");

        if (File.Exists(persistentPath))
        {
            string persistentVersion = GetGameVersionFromJson(persistentPath);
            string streamingVersion = GetGameVersionFromJson(streamingAssetsPath);

            if (persistentVersion != streamingVersion && !string.IsNullOrEmpty(streamingVersion))
            {
                File.Copy(streamingAssetsPath, persistentPath, true);
                Debug.LogWarning("Updated localization data to the latest version.");
            }

            LoadLocalizationData(persistentPath);
        }
        else
        {
            if (File.Exists(streamingAssetsPath))
            {
                File.Copy(streamingAssetsPath, persistentPath);
                LoadLocalizationData(persistentPath);
                Debug.Log("Copied default localization data to Persistent Data Path.");
            }
            else
            {
                Debug.LogError("Default localization data not found in StreamingAssets.");
            }
        }

        // Test
        //Debug.Log(GetText(2, "th"));
    }

    private string GetGameVersionFromJson(string path)
    {
        if (File.Exists(path))
        {
            string jsonData = LoadJson(path);
            if (!string.IsNullOrEmpty(jsonData))
            {
                var temp = JsonUtility.FromJson<LocalizationData>(jsonData);
                return temp.items.Find(item => item.key == 11)?.value.en; // Assuming key 11 is for version
            }
        }
        return null;
    }

    public void LoadLocalizationData(string path)
    {
        string jsonData = LoadJson(path);

        if (!string.IsNullOrEmpty(jsonData))
        {
            var localizationData = JsonUtility.FromJson<LocalizationData>(jsonData);
            localizationDataDictionary.Clear();
            foreach (var item in localizationData.items)
            {
                localizationDataDictionary[item.key] = item.value;
            }
            Debug.Log("Loaded localization data successfully.");
        }
        else
        {
            Debug.LogError("Failed to load localization data: No data found.");
        }
    }

    public void SaveLocalizationData()
    {
        var localizationData = new LocalizationData
        {
            items = new List<LocalizationItem>()
        };

        foreach (var kvp in localizationDataDictionary)
        {
            localizationData.items.Add(new LocalizationItem { key = kvp.Key, value = kvp.Value });
        }

        string jsonData = JsonUtility.ToJson(localizationData, true);
        SaveJson(jsonData);
    }

    public void DeleteLocalizationData()
    {
        DeleteJson();
        InitializePaths("localization.json");
    }

    public string GetText(int key, int languageCode)
    {
        if (localizationDataDictionary.TryGetValue(key, out var languageData))
        {
            return languageCode switch
            {
                0 => languageData.en,
                1 => languageData.th,
                2 => languageData.tr,
                3 => languageData.id,
                _ => $"[Unknown Language: {languageCode}]"
            };
        }
        return $"[Key Not Found: {key}]";
    }
}

