using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateTextController : MonoBehaviour
{
    private float checkInterval = 2.5f;
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

            if (connectedJoysticks.Length > 0 && !string.IsNullOrEmpty(connectedJoysticks[0]))
            {
                string joystickName = connectedJoysticks[0].Trim().ToLower();
                //Debug.Log("Controller connected: " + joystickName);

                if (joystickName.Contains("wireless controller") || joystickName.Contains("sony"))
                {
                    UpdateText(2);
                    //Debug.Log("PS4 Controller connected.");
                }
                else if (joystickName.Contains("xbox") || joystickName.Contains("microsoft"))
                {
                    UpdateText(3);
                    //Debug.Log("Xbox Controller connected.");
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
