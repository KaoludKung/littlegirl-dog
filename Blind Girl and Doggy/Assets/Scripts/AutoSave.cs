using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    [SerializeField] GameObject autoText;
    [SerializeField] GameObject triggerObject;

    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(autoSave());
    }

    IEnumerator autoSave()
    {
        yield return new WaitForSeconds(3.0f);
        autoText.SetActive(false);
        triggerObject.SetActive(true);
    }       

}
