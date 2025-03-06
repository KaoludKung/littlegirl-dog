using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllNotes : MonoBehaviour
{
    private List<NoteItem> allNotes = new List<NoteItem>();
    private NoteManager note;
    private int collectCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        note = FindObjectOfType<NoteManager>();
        allNotes = new List<NoteItem>(note.Items);
        Invoke("CheckNoteCount", 1.0f);
    }

    void CheckNoteCount()
    {
        for (int i = 0; i < allNotes.Count; i++)
        {
            if (allNotes[i].isCollected == true)
            {
                collectCount++;
            }
        }

        Debug.Log("Note Collectd: " + collectCount);

        if (collectCount == 9)
        {
            AchievementUnlock.Instance.UnlockAchievement(3);
        }
    }

}
