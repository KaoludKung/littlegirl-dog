using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class UpdateTutorialImage : MonoBehaviour
{
    private float checkInterval = 1.0f;
    private float timeSinceLastCheck = 0f;

    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private List<TutorialController> tutorialTargets;


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
                    UpdateTutorialDetail(2);
                }
                else if (activeJoystickName.Contains("xbox") || activeJoystickName.Contains("microsoft"))
                {
                    UpdateTutorialDetail(1);
                }
                else
                {
                    UpdateTutorialDetail(1);
                }
            }
            else
            {
                UpdateTutorialDetail(0);
            }
        }
    }

    void UpdateTutorialDetail(int controller)
    {
        for (int i = 0; i < tutorialTargets.Count; i++)
        {
            if (tutorialTargets[i].tutorialIndex == tutorialManager.GetCurrentIndex())
            {
                //Get Current Tutorial Detail
                string originalText = LocalizationManager.Instance.GetText(tutorialManager.GetTutorialIndex(), PlayerDataManager.Instance.GetLanguage());
                string updatedText = Regex.Replace(originalText, tutorialTargets[i].Pattern, tutorialTargets[i].controllerIcon[controller]);
                
                //Update Button in Detail According current controller
                tutorialManager.SetTutorialDetail(updatedText);
            }
        }
    }
}


[System.Serializable]
public class TutorialController
{
    public int tutorialIndex;
    public string Pattern;
    //0: keyborad 1: xbox 2:ps4
    public string[] controllerIcon;
}