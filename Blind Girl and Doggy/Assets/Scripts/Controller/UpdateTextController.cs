using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateTextController : MonoBehaviour
{
    private float checkInterval = 2.2f;
    private float timeSinceLastCheck = 0f;

    [SerializeField] private TextMeshProUGUI[] original_Text;
    [SerializeField] private string[] keyboard_Text;
    [SerializeField] private string[] xbox_Text;
    [SerializeField] private string[] ps4_Text;

    // Update is called once per frame
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
                    UpdateText(2);
                    //Debug.Log("PS4 Controller connected.");
                }
                else if (activeJoystickName.Contains("xbox") || activeJoystickName.Contains("microsoft"))
                {
                    UpdateText(3);
                    //Debug.Log("Xbox Controller connected.");
                }
                else
                {
                    UpdateText(3);
                }
            }
            else
            {
                UpdateText(1);
                //Debug.Log("No controller connected.");
            }
        }
    }

    void UpdateText(int controller)
    {
        string[] selectedText = null;

        switch (controller)
        {
            case 1: 
                selectedText = keyboard_Text;
                break;
            case 2: 
                selectedText = ps4_Text;
                break;
            case 3:
                selectedText = xbox_Text;
                break;
        }

        if (selectedText != null)
        {
            for (int i = 0; i < original_Text.Length; i++)
            {
                if (i < selectedText.Length)
                {
                    original_Text[i].text = selectedText[i];
                }
            }
        }
    }
}
