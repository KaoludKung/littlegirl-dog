using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SteamManager.Initialized)
        {
            //string name = SteamFriends.GetPersonaName();
            //Debug.Log(name);

            //SteamUserStats.ClearAchievement("achievement_01");
            //SteamUserStats.StoreStats();

            //SteamUserStats.SetAchievement("achievement_01");
            //SteamUserStats.StoreStats();
            //Debug.Log("Achievement Unlock");
        }
    }
}
