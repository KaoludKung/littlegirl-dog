using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    [SerializeField] int id;

    // Start is called before the first frame update
    void Start()
    {
        InventoryManager.Instance.AddItem(id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
