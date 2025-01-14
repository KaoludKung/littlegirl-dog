using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] int eventID;
    [SerializeField] float duration = 0;
    [SerializeField] GameObject triggerGameObject;
    [SerializeField] GameObject gameooverPanel;
    private bool isActive = false;

    private void Update()
    {
        if(isActive && gameooverPanel.activeSelf)
        {
            isActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog") || collision.CompareTag("Player"))
        {
            if (!isActive && EventManager.Instance.IsEventTriggered(eventID))
            {
                isActive = true;
                StartCoroutine(ActiveEvent());
            }
        }
    }

    IEnumerator ActiveEvent()
    {
        yield return new WaitForSeconds(duration);

        if (isActive)
        {
            triggerGameObject.SetActive(true);
        }

        yield return null;
        Destroy(gameObject);
        
    }
}
