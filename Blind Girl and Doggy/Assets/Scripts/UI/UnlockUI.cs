using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockUI : MonoBehaviour
{
    [SerializeField] Unlockoption unlockoption;
    
    private NoteUI noteUI;
    private InventoryUI inventoryUI;

    private void Awake()
    {
        noteUI = FindObjectOfType<NoteUI>();
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (unlockoption)
        {
            case Unlockoption.inventory:
                inventoryUI.Unlock(true);
                break;
            case Unlockoption.note:
                noteUI.Unlock(true);
                break;
            default:
                break;
        }
    }

}

public enum Unlockoption
{
    inventory,
    note
}
