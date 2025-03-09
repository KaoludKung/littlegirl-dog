using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class AchievementUnlock : MonoBehaviour
{
    public static AchievementUnlock Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UnlockAchievement(int id)
    {
        if(!AchievementManager.Instance.GetAchievementIsCollected(id))
        {
            string achachievementName = "achievement_0" + id;
            AchievementManager.Instance.UpdateAchievementTrigger(id);
            //AchievementManager.Instance.SaveAchievement();

            if (SteamManager.Initialized)
            {
                SteamUserStats.SetAchievement(achachievementName);
                SteamUserStats.StoreStats();
            }

            Debug.Log("Achievement Unlock");
        }
    }
}
