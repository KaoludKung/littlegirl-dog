using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpots : MonoBehaviour
{
    [SerializeField] private List<SpotSet> spotSets;
    [SerializeField] private Vector3[] randomPointX;

    void Start()
    {
        List<Vector3> availablePositions = new List<Vector3>(randomPointX);

        for (int i = 0; i < spotSets.Count; i++)
        {
            SpotSet spotSet = spotSets[i];

            if (availablePositions.Count > 0)
            {
                int randomIndex = Random.Range(0, availablePositions.Count);
                Vector3 randomPosition = availablePositions[randomIndex];

                Vector3 spotPosition = spotSet.spot.transform.position;
                spotPosition.x = randomPosition.x;  
                spotSet.spot.transform.position = spotPosition;

                Vector3 hiddenObjectPosition = spotSet.hiddenObject.transform.position;
                hiddenObjectPosition.x = randomPosition.x; 
                spotSet.hiddenObject.transform.position = hiddenObjectPosition;

                availablePositions.RemoveAt(randomIndex);
            }
        }
    }
}

[System.Serializable]
public class SpotSet
{
    public GameObject spot;
    public GameObject hiddenObject;
}
