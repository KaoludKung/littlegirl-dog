using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }
    private GirlController girlController;
    private DogController dogController;

    private void Awake()
    {
        girlController = FindObjectOfType<GirlController>();
        dogController = FindObjectOfType<DogController>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetIsActive(bool i)
    {
        if (girlController != null && dogController != null)
        {
            girlController.SetIsActive(i);
            dogController.SetIsActive(i);
        }
    }

    public void SoundPause() 
    { 
        if(girlController != null && dogController != null)
        {
            girlController.GetComponent<AudioSource>().Pause();
            dogController.GetComponent<AudioSource>().Pause();
        }
    }

    public void SoundUnPause()
    {
        if (girlController != null && dogController != null)
        {
            girlController.GetComponent<AudioSource>().UnPause();
            dogController.GetComponent<AudioSource>().UnPause();
        }
    }
}
