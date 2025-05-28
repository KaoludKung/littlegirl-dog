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
        if (!PlayerPrefs.HasKey("FPS"))
        {
            PlayerPrefs.SetInt("FPS", 120);
        }

        Application.targetFrameRate = PlayerPrefs.GetInt("FPS");
        Debug.Log("Setting FPS to " + PlayerPrefs.GetInt("FPS"));
      
    }
}
