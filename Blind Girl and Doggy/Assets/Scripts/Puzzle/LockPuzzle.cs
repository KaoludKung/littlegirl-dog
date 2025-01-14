using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockPuzzle : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI digitText1;
    [SerializeField] private TextMeshProUGUI digitText2;
    [SerializeField] private TextMeshProUGUI digitText3;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip unlockClip;

    private int[] digits = new int[3];

    [SerializeField] private int[] answerDigits = { 4, 5, 7 }; 

    public void AdjustDigit(int digitIndex, int direction)
    {
        if (digitIndex < 0 || digitIndex > 2) return;

        digits[digitIndex] = (digits[digitIndex] + direction + 10) % 10;
        SoundFXManager.instance.PlaySoundFXClip(clickClip, transform, false, 1.0f);
        UpdateDigitText(digitIndex);
        CheckCode();
    }

    private void UpdateDigitText(int digitIndex)
    {
        switch (digitIndex)
        {
            case 0:
                digitText1.text = digits[0].ToString();
                break;
            case 1:
                digitText2.text = digits[1].ToString();
                break;
            case 2:
                digitText3.text = digits[2].ToString();
                break;
        }
    }

    private void CheckCode()
    {
        if (digits[0] == answerDigits[0] && digits[1] == answerDigits[1] && digits[2] == answerDigits[2])
        {
            Unlock();
        }
    }

    private void Unlock()
    {
        Debug.Log("Unlocked!");
        SoundFXManager.instance.PlaySoundFXClip(unlockClip, transform, false, 1.0f);
    }

    public void IncreaseDigit1() => AdjustDigit(0, 1);
    public void DecreaseDigit1() => AdjustDigit(0, -1);
    public void IncreaseDigit2() => AdjustDigit(1, 1);
    public void DecreaseDigit2() => AdjustDigit(1, -1);
    public void IncreaseDigit3() => AdjustDigit(2, 1);
    public void DecreaseDigit3() => AdjustDigit(2, -1);
}
