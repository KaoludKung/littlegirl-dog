using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    private float currentFPS;
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
            currentFPS = frames / timeAccumulator;
            frames = 0;
            timeAccumulator = 0f;

            if (currentFPS <= 30)
            {
                smoothTime = 0.35f;
            }
            else 
            {
                smoothTime = 0.2f;
            }
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            transform.position = smoothedPosition;
        }
    }
}
