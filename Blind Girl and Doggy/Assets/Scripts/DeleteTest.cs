using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerDataManager.Instance.DeletePlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
