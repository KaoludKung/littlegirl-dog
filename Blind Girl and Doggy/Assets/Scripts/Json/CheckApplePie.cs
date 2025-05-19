using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckApplePie : MonoBehaviour
{
    //0: Bad Ending and Very Bad Ending 1: True Ending
    [SerializeField] GameObject[] Endings;
    private InventoryItem applePieItem;

    // Start is called before the first frame update
    void Start()
    {
        //id 1 = Apple Pie
        applePieItem = InventoryManager.Instance.GetItemByID(1);

        if (applePieItem.isCollected)
        {
            Endings[0].SetActive(true);
            Debug.Log("Mansion");
        }
        else
        {
            Endings[1].SetActive(true);
            Debug.Log("Mansion Error");
        }
    }    
}
