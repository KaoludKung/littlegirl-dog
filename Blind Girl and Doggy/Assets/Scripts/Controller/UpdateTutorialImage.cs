using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTutorialImage : MonoBehaviour
{
    private float checkInterval = 2.5f;
    private float timeSinceLastCheck = 0f;

    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private int maximum;
    [SerializeField] private int[] tutorialIndex;
    [SerializeField] private int[] controllerIndex;
    
    [SerializeField] private UISprite keyboardSprites;
    [SerializeField] private UISprite ps4Sprites;
    [SerializeField] private UISprite xboxSprites;

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
                    UpdateUI(2);
                    //Debug.Log("PS4 Controller connected.");
                }
                else if (joystickName.Contains("xbox") || joystickName.Contains("microsoft"))
                {
                    UpdateUI(3);
                    //Debug.Log("Xbox Controller connected.");
                }
            }
            else
            {
                UpdateUI(1);
                //Debug.Log("No controller connected.");
            }
        }
    }

    void UpdateUI(int controller)
    {
        UISprite selectedSprite = null;

        switch (controller)
        {
            case 1:
                selectedSprite = keyboardSprites;
                break;
            case 2:
                selectedSprite = ps4Sprites;
                break;
            case 3:
                selectedSprite = xboxSprites;
                break;
        }

        if (selectedSprite != null)
        {
            for (int i = 0; i < maximum; i++)
            {
                int tIndex = tutorialIndex[i];
                int cIndex = controllerIndex[i];
                tutorialManager.SetTutorialImage(tIndex, selectedSprite.imageController[cIndex]);
            }
        }
    }
}