using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class UpdateTitleText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private int titleID;
    //0: keyboard 1: xbox 2: ps4
    [SerializeField] private string[] newText;
    //@"Shift|View|Share"
    [SerializeField] private string pattern;

    private float checkInterval = 2.5f;
    private float timeSinceLastCheck = 0f;

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
                    UpdateNewTitleText(2);
                    //Debug.Log("PS4 Controller connected.");
                }
                else if (activeJoystickName.Contains("xbox") || activeJoystickName.Contains("microsoft"))
                {
                    UpdateNewTitleText(1);
                    //Debug.Log("Xbox Controller connected.");
                }
                else
                {
                    UpdateNewTitleText(1);
                }
            }
            else
            {
                UpdateNewTitleText(0);
                //Debug.Log("No controller connected.");
            }
        }
    }

    void UpdateNewTitleText(int controller)
    {
        string originalText = LocalizationManager.Instance.GetText(titleID, PlayerDataManager.Instance.GetLanguage());
        string updatedText = Regex.Replace(originalText, pattern, newText[controller]);

        titleText.fontSizeMin = PlayerDataManager.Instance.GetLanguage() == 1 ? 42 : 54;
        titleText.text = updatedText;         
    }
}
