using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    [SerializeField] private SetType setType;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private int cameraID = 0;

    private CameraSwitcher cameraSwitcher;

    private void Awake()
    {
        cameraSwitcher = FindObjectOfType<CameraSwitcher>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(setType == SetType.setactive)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }

        if(setType == SetType.switchcamera)
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
