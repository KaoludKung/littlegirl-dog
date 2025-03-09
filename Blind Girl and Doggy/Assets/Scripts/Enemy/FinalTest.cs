using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.UpdateEventDataTrigger(86, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
