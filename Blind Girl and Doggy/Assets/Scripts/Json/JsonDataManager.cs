using System;
using System.IO;
using UnityEngine;

public abstract class JsonManager<T> : MonoBehaviour 
{
    protected string savePath;
    protected string persistentPath;
    protected string streamingAssetsPath;

    protected void InitializePaths(string fileName)
    {
        savePath = Path.Combine(Application.persistentDataPath, fileName);
        persistentPath = savePath;
        streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, fileName);
    }

    protected string LoadJson(string path)
    {
        try
        {
            return File.ReadAllText(path);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load JSON data: " + ex.Message);
            return null;
        }
    }

    protected void SaveJson(string jsonData)
    {
        try
        {
            File.WriteAllText(savePath, jsonData);
            Debug.Log("Data saved to: " + savePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save JSON data: " + ex.Message);
        }
    }

    protected void DeleteJson()
    {
        try
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log("JSON file deleted successfully: " + savePath);
            }
            else
            {
                Debug.LogWarning("No JSON file found to delete at: " + savePath);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to delete JSON file: " + ex.Message);
        }
    }

}

