using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Memory : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI memoryText;
    [SerializeField] string[] memoryLines;
    [SerializeField] GameObject[] memoryVisuals;
    [SerializeField] AudioClip[] memoryClips;

    private AudioSource memoryAudioSource;
    private int memoryIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        memoryAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TypePauseText()
    {
        memoryText.text = "";

        for (int i = 0; i <= memoryLines[memoryIndex].Length; i++)
        {
            memoryText.text = memoryLines[memoryIndex].Substring(0, i);
            yield return new WaitForSecondsRealtime(0.2f);
        }

        yield return new WaitForSecondsRealtime(0.5f);
    }
}
