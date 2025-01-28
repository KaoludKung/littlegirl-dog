using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private AudioSource typeSource;
    [SerializeField] private AudioClip typeClip;
    [SerializeField] private string fullText;
    [SerializeField] private bool isReset = false;

    public float typingSpeed = 0.25f;
    private string currentText = "";

    private void Start()
    {
        typeSource.clip = typeClip;
        StartTyping(fullText);
    }


    public void StartTyping(string text)
    {
        typeSource.loop = true;
        fullText = text;
        currentText = "";
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        typeSource.Play();

        for (int i = 0; i < fullText.Length; i++)
        {
            
            currentText += fullText[i];
            textMeshPro.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }

        typeSource.loop = false;
        yield return new WaitForSeconds(3.0f);

        if(!isReset)
            textMeshPro.text = "";
    }
}
