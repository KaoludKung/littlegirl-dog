using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Translate : MonoBehaviour
{
    [SerializeField] private List<LocalizeElement> localizeElements;

    // Start is called before the first frame update
    void Start()
    {
        TranslateAllText();
    }

    public void TranslateAllText()
    {
        //If current language is Thai
        if(PlayerDataManager.Instance.GetLanguage() == 1)
        {
            FontChanger.Instance.ChangeAllTextFontsTH();
        }
        else
        {
            FontChanger.Instance.ChangeAllTextFontsEN();
        }
            

        for (int i = 0; i < localizeElements.Count; i++)
        {
            localizeElements[i].localizeText.text = LocalizationManager.Instance.GetText(localizeElements[i].localID, PlayerDataManager.Instance.GetLanguage()).Replace("\\n", "\n");
        }
    }
}

[System.Serializable]
public class LocalizeElement
{
    public TextMeshProUGUI  localizeText;
    public int localID;
}
