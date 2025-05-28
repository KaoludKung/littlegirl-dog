using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI typingText;
    [SerializeField] private AudioSource typeSource;
    [SerializeField] private AudioClip typeClip;
    [SerializeField] private int dataID;
    //[SerializeField] private string fullText;
    [SerializeField] private bool isReset = false;

    public float typingSpeed = 0.25f;
    private string currentText = "";
    private string data;

    private void Start()
    {
        typeSource.clip = typeClip;
        typingText.fontSizeMax = PlayerDataManager.Instance.GetLanguage() == 1 ? 42 : 48;
        StartTyping(LocalizationManager.Instance.GetText(dataID, PlayerDataManager.Instance.GetLanguage()));
    }


    public void StartTyping(string text)
    {
        typeSource.loop = true;
        data = text;
        currentText = "";
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        typeSource.Play();

        for (int i = 0; i < data.Length; i++)
        {
            
            currentText += data[i];
            typingText.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }

        typeSource.loop = false;
        yield return new WaitForSeconds(3.0f);

        if(!isReset)
            typingText.text = "";
    }
}
