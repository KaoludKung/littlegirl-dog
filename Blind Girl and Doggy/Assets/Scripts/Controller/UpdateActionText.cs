using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateActionText : MonoBehaviour
{
    private float checkInterval = 2.5f;
    private float timeSinceLastCheck = 0f;

    [SerializeField] Action[] action;
    //0: keyboard 1: xbox 2: ps4
    [SerializeField] string[] newText;

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
                //Debug.Log("Controller connected: " + joystickName);

                if (joystickName.Contains("wireless controller") || joystickName.Contains("sony"))
                {
                    UpdateNewActionText(2);
                    //Debug.Log("PS4 Controller connected.");
                }
                else if (joystickName.Contains("xbox") || joystickName.Contains("microsoft"))
                {
                    UpdateNewActionText(1);
                    //Debug.Log("Xbox Controller connected.");
                }
            }
            else
            {
                UpdateNewActionText(0);
                //Debug.Log("No controller connected.");
            }
        }
    }

    void UpdateNewActionText(int controller)
    {
        for(int i = 0; i < action.Length; i++)
        {
            if (action[i] != null)
                action[i].SetActionText(newText[controller]);
        }
    }
}
