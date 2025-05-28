using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New noteTranslate", menuName = "NoteTranslate")]
public class NoteTranslate : ScriptableObject
{
    public noteTranslation[] noteList;
}

[System.Serializable]
public struct noteTranslation
{
    public string[] nameTranslation;
    [TextArea(2, 5)]
    public string[] dateTranslation;
    [TextArea(5, 5)]
    public string[] descriptionTranslation;
}

