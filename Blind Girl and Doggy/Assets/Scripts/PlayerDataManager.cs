using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public Vector3 girlPosition;
    public Vector3 dogPosition;
    public string sceneName;
    public int hearts;

    public PlayerData(Vector3 girlPosition, Vector3 dogPosition, string sceneName, int hearts)
    {
        this.girlPosition = girlPosition;
        this.dogPosition = dogPosition;
        this.sceneName = sceneName;
        this.hearts = hearts;
    }
}

public class PlayerDataManager : JsonManager<PlayerData>
{
    public static PlayerDataManager Instance { get; private set; }
    private PlayerData playerData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializePaths("playerData.json");

        if (File.Exists(persistentPath))
        {
            LoadPlayerData(persistentPath);
        }
        else
        {
            if (File.Exists(streamingAssetsPath))
            {
                File.Copy(streamingAssetsPath, persistentPath);
                LoadPlayerData(persistentPath);
                Debug.Log("Copied default player data to Persistent Data Path.");
            }
            else
            {
                Debug.LogError("Default player data not found in StreamingAssets.");
            }
        }

    }

    public void LoadPlayerData(string path)
    {
        string jsonData = LoadJson(path);

        if (!string.IsNullOrEmpty(jsonData))
        {
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            Debug.Log("Loaded player data: " + jsonData);
        }
        else
        {
            Debug.LogError("Failed to load player data: No data found.");
        }
    }

    public void SavePlayerData()
    {
        string jsonData = JsonUtility.ToJson(playerData, true);
        SaveJson(jsonData);
    }

    public string GetSceneName()
    {
        return playerData != null ? playerData.sceneName : string.Empty;
    }

    public Vector3 GetGirlPosition()
    {
        return playerData != null ? playerData.girlPosition : Vector3.zero;
    }

    public Vector3 GetDogPosition()
    {
        return playerData != null ? playerData.dogPosition : Vector3.zero;
    }

    public int GetHearts()
    {
        return playerData != null ? playerData.hearts : 0;
    }

    public void UpdateGirlPosition(Vector3 newPosition)
    {
        if (playerData != null)
        {
            playerData.girlPosition = newPosition;
            Debug.Log($"Updated girl's position to: {newPosition}");
            SavePlayerData();
        }
    }

    public void UpdateDogPosition(Vector3 newPosition)
    {
        if (playerData != null)
        {
            playerData.dogPosition = newPosition;
            Debug.Log($"Updated dog's position to: {newPosition}");
            SavePlayerData();
        }
    }

    public void UpdateSceneName(string newSceneName)
    {
        if (playerData != null)
        {
            playerData.sceneName = newSceneName;
            Debug.Log($"Updated scene name to: {newSceneName}");
            SavePlayerData();
        }
    }

    public void UpdateHearts(int newHearts)
    {
        if (playerData != null)
        {
            playerData.hearts = newHearts;
            Debug.Log($"Updated hearts to: {newHearts}");
            SavePlayerData();
        }
    }

    public PlayerData GetPlayerData() => playerData;
}
