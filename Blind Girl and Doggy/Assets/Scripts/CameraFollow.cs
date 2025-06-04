using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 offset = new Vector3(0.2f, 0f, 0f);
    private Vector3 velocity = Vector3.zero;
    private float averageFPS;
    private float updateInterval = 3.0f;
    private float timeAccumulator;
    private int frames;

    void Start()
    {
        timeAccumulator = 0f;
        frames = 0;
    }

    void Update()
    {
        timeAccumulator += Time.unscaledDeltaTime;
        frames++;

        if (timeAccumulator >= updateInterval)
        {
            averageFPS = frames / timeAccumulator;
            timeAccumulator = 0f;
            frames = 0;
        }

        // Adjust smoothTime based on FPS
        float targetSmoothTime = averageFPS <= 60 ? 0.25f : 0.20f;
        smoothTime = Mathf.Lerp(smoothTime, targetSmoothTime, Time.unscaledDeltaTime);
    }

    void LateUpdate() 
    {
        if (target != null)
        {
            // Follow only the X axis
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            transform.position = smoothedPosition;
        }
    }
}
