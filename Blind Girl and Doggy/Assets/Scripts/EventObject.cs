using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    [SerializeField] protected int eventID;
    [SerializeField] protected int TriggerEventID;

    public int GetEventId()
    {
        return eventID;
    }
}
