using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public GameObject treePrefab;
    public int numberOfTrees = 10;
    public Vector3 areaSize = new Vector3(100f, 0f, 100f);

    void Start()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(3.5f, 5f),
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            );

            int s = (int)Random.Range(1.0f, 1.0f);
            Vector3 randomScale = new Vector3(s,s,1f);

            float randomRotation = Random.Range(-10f, 10f);

            GameObject tree = Instantiate(treePrefab, randomPosition, Quaternion.Euler(0, randomRotation, 0));
            tree.transform.localScale = randomScale;
        }
    }
}
