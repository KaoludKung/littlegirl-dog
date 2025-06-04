using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WiggleText : MonoBehaviour
{
    [SerializeField] private float amplitude = 5f;
    [SerializeField] private float frequency = 2f;
    [SerializeField] private float fontMinTH;

    private TextMeshProUGUI wiggleText;
    private float fontMinEN;
    private Vector3 originalPosition;

    void Start()
    {
        wiggleText = GetComponent<TextMeshProUGUI>();
        fontMinEN = wiggleText.fontSizeMin;
        wiggleText.fontSizeMin = PlayerDataManager.Instance.GetLanguage() == 1 ? fontMinTH : fontMinEN;
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.unscaledTime * Mathf.PI * 2f * frequency) * amplitude;
        transform.localPosition = originalPosition + new Vector3(0, offset, 0);
    }
}
