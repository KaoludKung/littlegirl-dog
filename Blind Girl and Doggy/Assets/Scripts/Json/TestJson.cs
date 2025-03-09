using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJson : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AchievementManager.Instance.UpdateAchievementTrigger(7);
        //NoteManager.Instance.SaveNote();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
