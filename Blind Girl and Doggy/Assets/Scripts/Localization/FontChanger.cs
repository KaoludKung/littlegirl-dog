using UnityEngine;
using TMPro;

public class FontChanger : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset thFont;
    [SerializeField] private TMP_FontAsset enFont;
    public static FontChanger Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeAllTextFontsTH()
    {
        TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>(true);

        foreach (var text in allTexts)
        {
            text.font = thFont;
        }
    }

    public void ChangeAllTextFontsEN()
    {
        TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>(true);

        foreach (var text in allTexts)
        {
            text.font = enFont;
        }
    }

    public void ChangeSpecificFont(TextMeshProUGUI text, int language)
    {
        text.font = language == 1 ? thFont : enFont;
    }
}
