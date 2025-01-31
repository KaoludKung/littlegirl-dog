using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] randomObject;

    // Start is called before the first frame update
    void Start()
    {
        RandomActive();
    }

    private void RandomActive()
    {
        int r = Random.Range(0, randomObject.Length);
        randomObject[r].SetActive(true);
    }
}
