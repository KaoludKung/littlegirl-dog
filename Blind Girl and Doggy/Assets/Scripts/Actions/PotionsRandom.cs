using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionsRandom : MonoBehaviour
{
    [SerializeField] private List<PotionSet> potionSets;

    // Start is called before the first frame update
    void Start()
    {
        RandomizePotions();
    }

    private void RandomizePotions()
    {
        if (potionSets == null || potionSets.Count == 0)
        {
            Debug.LogWarning("PotionSets is empty or null!");
            return;
        }

        for (int i = 0; i < potionSets.Count; i++)
        {
            potionSets[i].potionSpot1.SetActive(false);
            potionSets[i].potionSpot2.SetActive(false);

            int potionSelected = Random.Range(0, 2);
            if (potionSelected == 0)
            {
                potionSets[i].potionSpot1.SetActive(true);
            }
            else
            {
                potionSets[i].potionSpot2.SetActive(true);
            }
        }
    }
}

[System.Serializable]
public class PotionSet
{
    public GameObject potionSpot1;
    public GameObject potionSpot2;
}
