using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ???????? Image ??? Sprite

public class ControllerTest : MonoBehaviour
{
    private float checkInterval = 2.5f;
    private float timeSinceLastCheck = 0f;

    void Update()
    {
        timeSinceLastCheck += Time.unscaledDeltaTime;

        if (timeSinceLastCheck >= checkInterval)
        {
            timeSinceLastCheck = 0f;

            string[] connectedJoysticks = Input.GetJoystickNames();

            if (connectedJoysticks.Length > 0 && !string.IsNullOrEmpty(connectedJoysticks[0]))
            {
                string joystickName = connectedJoysticks[0].Trim().ToLower();
                Debug.Log("Controller connected: " + joystickName);

                if (joystickName.Contains("wireless controller") || joystickName.Contains("sony"))
                {
                    Debug.Log("PS4 Controller connected.");
                }
                else if (joystickName.Contains("xbox") || joystickName.Contains("microsoft"))
                {
                    Debug.Log("Xbox Controller connected.");
                }
            }
            else
            {
                Debug.Log("No controller connected.");
            }
        }
    }

    
}


