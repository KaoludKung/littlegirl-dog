using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int a = 20;

        switch (a)
        {
            case 4 or 5 or 6:
                Debug.Log("This works!");
                break;
            default:
                Debug.Log("Something else");
                break;
        }
    }
}
