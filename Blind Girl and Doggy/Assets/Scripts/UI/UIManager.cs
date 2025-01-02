using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool IsAnyUIActive => inventoryUI.isActive || noteUI.isActive || pauseManager.isActive;
    public static UIManager Instance { get; private set; }

    private InventoryUI inventoryUI;
    private NoteUI noteUI;
    private PauseManager pauseManager;

    private void Awake()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
        noteUI = FindObjectOfType<NoteUI>();
        pauseManager = FindObjectOfType<PauseManager>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }

    public void ToggleTimeScale(bool isPaused)
    {
        if (Time.timeScale != (isPaused ? 0.0f : 1.0f))
        {
            Time.timeScale = isPaused ? 0.0f : 1.0f;
        }
    }

}

