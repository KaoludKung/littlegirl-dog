using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHtem : MonoBehaviour
{
    [SerializeField] int id;
    // Start is called before the first frame update
    void Start()
    {
        InventoryManager.Instance.RemoveItem(id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
