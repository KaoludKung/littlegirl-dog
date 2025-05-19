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
            string achievementName = id < 10 ? "achievement_0" + id : "achievement_" + id;
            AchievementManager.Instance.UpdateAchievementTrigger(id);
            //AchievementManager.Instance.SaveAchievement();

            if (SteamManager.Initialized)
            {
                SteamUserStats.SetAchievement(achievementName);
                SteamUserStats.StoreStats();
            }

            Debug.Log("Achievement Unlock");
        }
    }
}
