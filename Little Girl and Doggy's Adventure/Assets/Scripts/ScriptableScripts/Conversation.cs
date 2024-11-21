using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Conversation")]
public class Conversation : ScriptableObject
{
    public string titleName;
    public Line[] lines;
}

[System.Serializable]
public struct Line
{
    public Character character;
    [TextArea(2,5)]
    public string text;
}
