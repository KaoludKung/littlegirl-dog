using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleText : MonoBehaviour
{
    [SerializeField] private float amplitude = 5f;
    [SerializeField] private float frequency = 2f; 

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.unscaledTime * Mathf.PI * 2f * frequency) * amplitude;
        transform.localPosition = originalPosition + new Vector3(0, offset, 0);
    }
}
