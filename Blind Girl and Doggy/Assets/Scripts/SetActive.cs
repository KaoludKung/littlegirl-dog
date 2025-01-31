using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    [SerializeField] private SetType setType;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float duration = 0;
    [SerializeField] private int cameraID = 0;

    private CameraSwitcher cameraSwitcher;

    private void Awake()
    {
        cameraSwitcher = FindObjectOfType<CameraSwitcher>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActiveNow());
    }

    IEnumerator ActiveNow()
    {
        yield return new WaitForSeconds(duration);

        if (setType == SetType.setactive && targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }

        if (setType == SetType.switchcamera)
        {
            cameraSwitcher.SwitchCamera(cameraID);
        }
    }

}

public enum SetType
{
    setactive,
    switchcamera
}
