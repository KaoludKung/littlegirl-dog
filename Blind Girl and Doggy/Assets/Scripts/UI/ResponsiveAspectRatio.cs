using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter))]
public class ResponsiveAspectRatio : MonoBehaviour
{
    private AspectRatioFitter aspectRatioFitter;

    void Start()
    {
        aspectRatioFitter = GetComponent<AspectRatioFitter>();
        AdjustAspectRatio();
    }

    void AdjustAspectRatio()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float currentAspect = screenWidth / screenHeight;

        aspectRatioFitter.aspectRatio = currentAspect;

        Debug.Log($"Screen Width: {screenWidth}, Screen Height: {screenHeight}, Aspect Ratio: {currentAspect}");
    }

    void Update()
    {
        if (Screen.width != aspectRatioFitter.aspectRatio * Screen.height)
        {
            AdjustAspectRatio();
        }
    }
}
