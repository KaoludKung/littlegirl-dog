using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public string fullname;
    public Sprite portrait;
    public AudioClip typing;
    //public AnimatorController animatorController;
    public string animationCondition;
    public int conditionID;
}
