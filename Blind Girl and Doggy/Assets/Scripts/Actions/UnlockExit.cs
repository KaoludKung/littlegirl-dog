using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockExit : MonoBehaviour
{
    //0: use crowbar 1: use exit key
    [SerializeField] private int[] requireID;
    [SerializeField] private GameObject targetObject;

    private bool isUnlock = false;

    // Update is called once per frame
    void Update()
    {
        if (requireID.Length < 2)
        {
            Debug.LogWarning("RequireID must have at least 2 elements!");
            return;
        }

        if (!isUnlock && EventManager.Instance.IsEventTriggered(requireID[0]) && EventManager.Instance.IsEventTriggered(requireID[1]))
        {
            isUnlock = true;
            StartCoroutine(UnlockTarget());
        }
    }

    IEnumerator UnlockTarget()
    {
         yield return new WaitForSeconds(0.5f);
         targetObject.SetActive(true);
         Debug.Log("Exit Unlocked!");
    }
}
