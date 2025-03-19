using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateImageController : MonoBehaviour
{
    private float checkInterval = 2.5f;
    private float timeSinceLastCheck = 0f;

    [SerializeField] private List<OriginalImage> originalImage;
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
            Debug.Log("Connected Joysticks Length: " + connectedJoysticks.Length);

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
                    UpdateUI(2); 
                    Debug.Log("PS4 Controller connected.");
                }
                else if (activeJoystickName.Contains("xbox") || activeJoystickName.Contains("microsoft"))
                {
                    UpdateUI(3); 
                    Debug.Log("Xbox Controller connected.");
                }
                else
                {
                    UpdateUI(3);
                    Debug.Log("Other Controller connected.");
                }
            }
            else
            {
                UpdateUI(1);
                Debug.Log("No controller connected.");
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
            for (int i = 0; i < originalImage.Count; i++)
            {
                int targetIndex = originalImage[i].targetImage;

                if (targetIndex >= 0 && targetIndex < selectedSprite.imageController.Length)
                {
                    originalImage[i].UIImage.sprite = selectedSprite.imageController[targetIndex];
                }
            }
        }
    }
}

[System.Serializable]
public class OriginalImage
{
    public Image UIImage;
    public int targetImage;
}

