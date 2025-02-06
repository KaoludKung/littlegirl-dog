using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool IsAnyUIActive => inventoryUI.isActive == true || noteUI.isActive == true || pauseManager.isActive == true || GameOverManager?.isActive == true || LockPuzzle?.isActive == true;
    public static UIManager Instance { get; private set; }

    private InventoryUI inventoryUI;
    private NoteUI noteUI;
    private PauseManager pauseManager;
    private GameOverManager GameOverManager;
    private LockPuzzle LockPuzzle;

    private void Awake()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
        noteUI = FindObjectOfType<NoteUI>();
        pauseManager = FindObjectOfType<PauseManager>();
        GameOverManager = FindObjectOfType<GameOverManager>();
        LockPuzzle = FindObjectOfType<LockPuzzle>();

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

