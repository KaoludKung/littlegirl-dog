using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI a_text;

    // Start is called before the first frame update
    void Start()
    {
        a_text.text = "";
    }

    public void ActionDisplay(string result)
    {
        a_text.text = result;
        StartCoroutine(ResetDisplay());
    }

    IEnumerator ResetDisplay()
    {
        yield return new WaitForSeconds(3.5f);
        a_text.text = "";
    }
}
