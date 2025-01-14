using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPuzzle : MonoBehaviour
{
    [SerializeField] private Transform shortHand;
    [SerializeField] private Transform longHand;

    [SerializeField] private int shortHandTargetAngle = 90;
    [SerializeField] private int longHandTargetAngle = 180;
    [SerializeField] private AudioClip rotateClip;
    [SerializeField] private AudioClip unlockClip;

    private int angleStep = -30;

    public void RotateClockHand(Transform transform)
    {
        transform.Rotate(0, 0, angleStep);
        SoundFXManager.instance.PlaySoundFXClip(rotateClip, transform, false, 1.0f);
        CheckPuzzleCompletion();
    }

    private void CheckPuzzleCompletion()
    {
        if (Mathf.Approximately(shortHand.eulerAngles.z, shortHandTargetAngle) &&
            Mathf.Approximately(longHand.eulerAngles.z, longHandTargetAngle))
        {
            Unlock();
        }
    }

    private void Unlock()
    {
        Debug.Log("Puzzle Solved!");
        SoundFXManager.instance.PlaySoundFXClip(unlockClip, transform, false, 1.0f);
    }

    public void RotatetShortClockHand() => RotateClockHand(shortHand);
    public void RotatetlongClockHand() => RotateClockHand(longHand);

}
