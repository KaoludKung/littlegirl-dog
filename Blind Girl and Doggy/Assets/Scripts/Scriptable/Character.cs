using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public string fullname;
    public Sprite portrait;
    public AudioClip typing;
    public Animator animation;
    public string animationName;
    public string AnimationID;
}
