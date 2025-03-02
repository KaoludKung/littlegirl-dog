using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementDisplay : MonoBehaviour
{
    [SerializeField] GameObject[] achIcons;

    // Start is called before the first frame update
    void Start()
    {
        DisplayAchievementIcon();
    }

    private void DisplayAchievementIcon()
    {
        Debug.Log("Ach Display");

        for(int i = 0; i < achIcons.Length; i++)
        {
            if (AchievementManager.Instance.GetAchievementIsCollected(i + 1))
            {
                achIcons[i].SetActive(true);
            }
            else
            {
                achIcons[i].SetActive(false);
            }
        }
    }
}
