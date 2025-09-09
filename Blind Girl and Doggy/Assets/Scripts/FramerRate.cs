using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerRate : MonoBehaviour
{
    void Start()
    {
        SetFullScreenMode();
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

    void SetFullScreenMode()
    {
        if (!PlayerPrefs.HasKey("FullScreen"))
        {
            PlayerPrefs.SetInt("FullScreen", 0);
        }

        Screen.fullScreen = PlayerPrefs.GetInt("FullScreen") == 0 ? true : false;
    }
}
