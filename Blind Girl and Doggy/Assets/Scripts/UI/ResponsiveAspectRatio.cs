using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter))]
public class ResponsiveAspectRatio : MonoBehaviour
{
    private AspectRatioFitter aspectRatioFitter;
    private Vector2 lastScreenSize;

    void Start()
    {
        aspectRatioFitter = GetComponent<AspectRatioFitter>();
        lastScreenSize = new Vector2(Screen.width, Screen.height);
        AdjustAspectRatio();
    }

    void Update()
    {
        Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);

        if (currentScreenSize != lastScreenSize)
        {
            lastScreenSize = currentScreenSize;
            AdjustAspectRatio();
        }
    }

    void AdjustAspectRatio()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float currentAspect = screenWidth / screenHeight;

        aspectRatioFitter.aspectRatio = currentAspect;

        Debug.Log($"Screen Width: {screenWidth}, Screen Height: {screenHeight}, Aspect Ratio: {currentAspect}");
    }
}
