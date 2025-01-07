using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.SaveEventData();
        PlayerDataManager.Instance.SavePlayerData();
        InventoryManager.Instance.SaveInventory();
        NoteManager.Instance.SaveNote();
    }

}
