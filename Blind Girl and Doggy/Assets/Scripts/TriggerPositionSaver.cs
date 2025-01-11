using UnityEngine;

public class TriggerPositionSaver : MonoBehaviour
{
    private Vector3 positionAtTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // ???????????? tag ???? "Player"
        {
            positionAtTrigger = other.transform.position;

            Debug.Log("Player entered trigger at position: " + positionAtTrigger);
        }
    }

    public Vector3 GetPositionAtTrigger()
    {
        return positionAtTrigger;
    }
}
