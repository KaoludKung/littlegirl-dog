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
    public bool isNewGame;
    public bool isSpined;
    public bool secrets;
    public int hearts;
    public int deathCount;
    public int language;

    public PlayerData(Vector3 girlPosition, Vector3 dogPosition, string sceneName,bool isNewGame, bool isSpined, bool secret, int hearts, int deathCount, int language)
    {
        this.girlPosition = girlPosition;
        this.dogPosition = dogPosition;
        this.sceneName = sceneName;
        this.isNewGame = isNewGame;
        this.isSpined = isSpined;
        this.secrets = secret;
        this.hearts = hearts;
        this.deathCount = deathCount;
        this.language = language;
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
            string rawJson = File.ReadAllText(persistentPath);

            if (!rawJson.Contains("\"deathCount\""))
            {
                Debug.LogWarning("Existing save file lacks 'deathCount'. Adding default value (0).");

                LoadPlayerData(persistentPath);
                if (playerData != null)
                {
                    playerData.deathCount = 0;
                    playerData.language = 0;
                    SavePlayerData();
                }
            }
            else
            {
                LoadPlayerData(persistentPath);
            }
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

    public void DeletePlayerData()
    {
        DeleteJson();

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

        //InitializePaths("playerData.json");
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

    public bool GetIsNewGame()
    {
        return playerData != null ? playerData.isNewGame : false;
    }

    public bool GetIsSpined()
    {
        return playerData != null ? playerData.isSpined : false;
    }

    public bool GetIsSecret()
    {
        return playerData != null ? playerData.secrets : false;
    }

    public int GetHearts()
    {
        return playerData != null ? playerData.hearts : 0;
    }

    public int GetDeathCount()
    {
        return playerData != null ? playerData.deathCount : 0;
    }

    public int GetLanguage()
    {
        return playerData != null ? playerData.language : 0;
    }

    public void UpdateGirlPosition(Vector3 newPosition)
    {
        if (playerData != null)
        {
            playerData.girlPosition = newPosition;
            Debug.Log($"Updated girl's position to: {newPosition}");
            //SavePlayerData();
        }
    }

    public void UpdateDogPosition(Vector3 newPosition)
    {
        if (playerData != null)
        {
            playerData.dogPosition = newPosition;
            Debug.Log($"Updated dog's position to: {newPosition}");
            //SavePlayerData();
        }
    }

    public void UpdateSceneName(string newSceneName)
    {
        if (playerData != null)
        {
            playerData.sceneName = newSceneName;
            Debug.Log($"Updated scene name to: {newSceneName}");
            //SavePlayerData();
        }
    }

    public void UpdateNewGame(bool isNewGame)
    {
        if (playerData != null)
        {
            playerData.isNewGame = isNewGame;
            Debug.Log($"Updated newgame to: {isNewGame}");
            //SavePlayerData();
        }
    }

    public void UpdateIsSpined(bool isSpined)
    {
        if (playerData != null)
        {
            playerData.isSpined = isSpined;
            Debug.Log($"Updated isSpined to: {isSpined}");
            //SavePlayerData();
        }
    }

    public void UpdateSecrets(bool secret)
    {
        if (playerData != null)
        {
            playerData.secrets = secret;
            Debug.Log($"Updated secrets to: {secret}");
            //SavePlayerData();
        }
    }

    public void UpdateHearts(int newHearts)
    {
        if (playerData != null)
        {
            playerData.hearts = newHearts;
            Debug.Log($"Updated hearts to: {newHearts}");
            //SavePlayerData();
        }
    }

    public void UpdateDeathCount(int death)
    {
        if (playerData != null)
        {
            playerData.deathCount = death;
            Debug.Log($"Updated deathCount to: {death}");
            //SavePlayerData();
        }
    }

    public void UpdateLanguage(int language)
    {
        if (playerData != null)
        {
            playerData.language = language;
            Debug.Log($"Updated Language to: {language}");
            //SavePlayerData();
        }
    }

    public PlayerData GetPlayerData() => playerData;
}
