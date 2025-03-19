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
            //Debug.Log("Connected Joysticks Length: " + connectedJoysticks.Length);

            string activeJoystickName = null;

            foreach (string joystick in connectedJoysticks)
            {
                if (!string.IsNullOrEmpty(joystick))
                {
                    activeJoystickName = joystick.Trim().ToLower();
                    break;
                }
            }

            if (!string.IsNullOrEmpty(activeJoystickName))
            {
                
                if (activeJoystickName.Contains("wireless controller") || activeJoystickName.Contains("sony") || activeJoystickName.Contains("playstation"))
                {
                    UpdateNewActionText(2);
                    //Debug.Log("PS4 Controller connected.");
                }
                else if (activeJoystickName.Contains("xbox") || activeJoystickName.Contains("microsoft"))
                {
                    UpdateNewActionText(1);
                    //Debug.Log("Xbox Controller connected.");
                }
                else
                {
                    UpdateNewActionText(1);
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
