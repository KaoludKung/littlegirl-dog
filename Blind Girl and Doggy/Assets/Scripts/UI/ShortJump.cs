using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShortJump : MonoBehaviour
{
    [SerializeField] private GameObject shortJumpImage; 
    [SerializeField] private int maxJumpscares = 3;
    [SerializeField] private int loopColor = 5;
    [SerializeField] private int minTime = 10;  
    [SerializeField] private int maxTime = 20;
    [SerializeField] private Color[] jumpScareColors;

    private int currentCount = 0;
    private Image shortJumpImageComponent;

    private void Start()
    {
        shortJumpImageComponent = shortJumpImage.GetComponent<Image>();
        StartCoroutine(JumpscareLoop());
    }

    private IEnumerator JumpscareLoop()
    {
        while (currentCount < maxJumpscares)
        {
            int randomTime = Random.Range(minTime, maxTime);
            Debug.Log(randomTime);
            yield return new WaitForSeconds(randomTime);

            if (UIManager.Instance != null && !UIManager.Instance.IsAnyUIActive)
            {
                currentCount++;
                StartCoroutine(ShowJumpscare());
            }
        }
    }

    private IEnumerator ShowJumpscare()
    {
        shortJumpImage.SetActive(true);

        for(int i = 0; i < loopColor; i++)
        {
            shortJumpImageComponent.color = jumpScareColors[i % 2];
            yield return new WaitForSecondsRealtime(0.3f);
        }

        shortJumpImage.SetActive(false); 
    }
}
