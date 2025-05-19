using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBalloon : MonoBehaviour
{
    [Header("Floating Settings")]
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;

    [Header("Fly Away Settings")]
    [SerializeField] private float flySpeed = 0.25f;
    [SerializeField] private float targetY = 10f; 

    private bool safepointActive = false;
    private bool flyAwayTriggered = false;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (!safepointActive)
        {
            float newY = startPos.y + Mathf.Sin(Time.unscaledTime * Mathf.PI * 2f * frequency) * amplitude;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }
        else if (!flyAwayTriggered)
        {
            FlyAway();
        }
    }

    void FlyAway()
    {
        flyAwayTriggered = true;
        StartCoroutine(FlyToTarget());
    }

    private IEnumerator FlyToTarget()
    {
        while (transform.position.y < targetY)
        {
            transform.position += new Vector3(0, flySpeed * Time.deltaTime, 0);
            yield return null;
        }
    }

    public void SetSafePoint()
    {
        safepointActive = true;
    }
}
