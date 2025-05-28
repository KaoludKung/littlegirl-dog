using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.2f;
   
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

        // Smoothly adjust smoothTime based on FPS
        float targetSmoothTime = averageFPS <= 60 ? 0.35f : 0.2f;
        smoothTime = Mathf.Lerp(smoothTime, targetSmoothTime, Time.unscaledDeltaTime);

        // Debugging (Optional)
        //Debug.Log($"Average FPS: {averageFPS}, SmoothTime: {smoothTime}");
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            transform.position = smoothedPosition;
        }
    }
}
