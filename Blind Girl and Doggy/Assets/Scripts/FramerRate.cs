using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerRate : MonoBehaviour
{
    void Start()
    {
        SetTargetFrameRate();
    }

    void SetTargetFrameRate()
    {
        if (SystemInfo.processorFrequency >= 3000)
        {
            Application.targetFrameRate = 120;
            Debug.Log("Setting FPS to 120");
        }
        else if (SystemInfo.processorFrequency >= 2500)
        {
            Application.targetFrameRate = 60;
            Debug.Log("Setting FPS to 60");
        }
        else
        {
            Application.targetFrameRate = 30;
            Debug.Log("Setting FPS to 30");
        }
    }
}
