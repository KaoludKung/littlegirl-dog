using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FixingText : MonoBehaviour
{
    [Header("System Texts")]
    [SerializeField] private TextMeshProUGUI fixingText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("System Strings")]
    [SerializeField] private string[] baseText;
    [SerializeField] private AudioSource fixSoure;
    //[SerializeField] private float[] timeFixing;

    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartFixing());
    }

    IEnumerator StartFixing()
    {
        yield return new WaitForSeconds(5.0f);
        fixingText.gameObject.SetActive(true);
        statusText.gameObject.SetActive(true);
        nameText.gameObject.SetActive(true);

        StartCoroutine(TypeFixingText());
    }

    private IEnumerator TypeFixingText()
    {
        fixSoure.Play();

        while (currentIndex < baseText.Length)
        {
            

            for (int i = 0; i <= baseText[currentIndex].Length; i++)
            {
                fixingText.text = baseText[currentIndex].Substring(0, i);
                yield return new WaitForSeconds(0.25f);
            }

            for (int i = 0; i < 7; i++)
            {
                fixingText.text += ".";
                yield return new WaitForSeconds(0.25f);
            }

            yield return new WaitForSeconds(0.5f);
            currentIndex++;
            fixingText.text = "";
        }

        StartCoroutine(EndFixing());
    }

    IEnumerator EndFixing()
    {
        if (fixSoure.isPlaying)
            fixSoure.Stop();

        fixingText.gameObject.SetActive(false);
        statusText.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);

        yield return new WaitForSeconds(3.0f);
        EventManager.Instance.UpdateEventDataTrigger(603, true);
    }
}
