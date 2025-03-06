using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementObject : MonoBehaviour
{
    [SerializeField] int achievementId;
    [SerializeField] float times = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Active());
    }

    IEnumerator Active()
    {
        yield return new WaitForSecondsRealtime(times);
        AchievementUnlock.Instance.UnlockAchievement(achievementId);
    }

}
