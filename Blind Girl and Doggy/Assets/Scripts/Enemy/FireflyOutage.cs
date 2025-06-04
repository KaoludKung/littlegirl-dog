using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireflyOutage : MonoBehaviour
{
    [SerializeField] private Light2D fireFlyLight;
    [SerializeField] private int maxOutages;
    [SerializeField] private int minTime = 10;
    [SerializeField] private int maxTime = 20;

    private const int OutageTriggerValue = 2;
    private const int OutageChanceRange = 5;
    private const float FlickerDuration = 0.4f;
    private const float OutageDuration = 15f;

    private int currentOutages = 0;

    void Start()
    {
        StartCoroutine(RandomTriggerOutage());
    }

    private IEnumerator RandomTriggerOutage()
    {
        while (currentOutages < maxOutages)
        {
            int randomTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(randomTime);

            if (Random.Range(1, OutageChanceRange) == OutageTriggerValue)
            {
                yield return StartCoroutine(HandleOutage());
            }
        }
    }

    private IEnumerator HandleOutage()
    {
        yield return StartCoroutine(FlickerLight());
        yield return new WaitForSeconds(OutageDuration);
        fireFlyLight.intensity = 1;
        currentOutages++;
    }

    private IEnumerator FlickerLight()
    {
        for (int i = 0; i < 6; i++)
        {
            fireFlyLight.intensity = i % 2 == 0 ? 1 : 0;
            yield return new WaitForSeconds(FlickerDuration);
        }
    }
}
