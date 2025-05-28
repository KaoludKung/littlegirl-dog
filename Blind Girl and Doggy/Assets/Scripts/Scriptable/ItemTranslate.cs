using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New itemTranslate", menuName = "ItemTranslate")]
public class ItemTranslate : ScriptableObject
{
    public translation[] itemList;
}

[System.Serializable]
public struct translation
{
    public string[] nameTranslation;
    [TextArea(1, 1)]
    public string[] descriptionTranslation;
}

