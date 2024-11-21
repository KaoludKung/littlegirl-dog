using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera[] cameras;
    //[SerializeField] private int cameraID;
    private int currentCameraIndex;

    void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        if (cameras.Length > 0)
        {
            currentCameraIndex = 0;
            cameras[currentCameraIndex].gameObject.SetActive(true);
        }
    }

    public void SwitchCamera(int index)
    {

        if (index < 0 || index >= cameras.Length)
        {
            Debug.LogWarning("Camera index out of range.");
            return;
        }

        cameras[currentCameraIndex].gameObject.SetActive(false);
        currentCameraIndex = index;
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }
}
