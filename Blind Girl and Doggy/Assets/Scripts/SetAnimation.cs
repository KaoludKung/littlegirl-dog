using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimation : MonoBehaviour
{
    [SerializeField] Type types;
    [SerializeField] Animator targetAnimator;
    [SerializeField] string parameterName;
    [SerializeField] int animationID;
    [SerializeField] bool value;

    // Start is called before the first frame update
    void Start()
    {
        SetAnimator();
    }

    public void SetAnimator()
    {
        if(types == Type.boolean)
        {
            targetAnimator.SetBool(parameterName, value);
        }else if (types == Type.number)
        {
            targetAnimator.SetInteger(parameterName, animationID);
        }
    }
}

public enum Type
{
    number,
    boolean
}
