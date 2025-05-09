using UnityEngine;
using TMPro;

public class FontChanger : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset newFont;

    private void Start()
    {
        ChangeAllTextFonts();
    }

    private void ChangeAllTextFonts()
    {
        TextMeshProUGUI[] allTexts = FindObjectsOfType<TextMeshProUGUI>(true);

        foreach (var text in allTexts)
        {
            text.font = newFont;
        }
    }
}
