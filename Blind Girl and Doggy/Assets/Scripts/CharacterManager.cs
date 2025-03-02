using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }
    [SerializeField] private GameObject staminaBar;
    [SerializeField] private GameObject heartIcon;

    private GirlController girlController;
    private DogController dogController;
    private Monster monster;


    private void Awake()
    {
        girlController = FindObjectOfType<GirlController>();
        dogController = FindObjectOfType<DogController>();
        monster = FindObjectOfType<Monster>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CheckIsActive()
    {
        if(girlController.isActive || dogController.isActive)
        {
            girlController.SetIsActive(false);
            dogController.SetIsActive(false);
        }
    }

    public void StopMoving()
    {
        girlController.SetIsMoving(false);
        girlController.Animator.SetBool("isWalk", false);
    }

    public void SetIsActive(bool i)
    {
        if (girlController != null && dogController != null)
        {
            girlController.SetIsActive(i);
            dogController.SetIsActive(i);
        }     
    }

    public void SetActiveUIPlayer(bool u)
    {
        if (staminaBar != null)
        {
            staminaBar.SetActive(u);
            //Debug.Log("It's real.");
        }

        if(heartIcon != null)
        {
            heartIcon.SetActive(u);
            //.Log("It's real.");
        }
    }

    public void SoundPause() 
    { 
        if(girlController != null && dogController != null)
        {
            girlController.GetComponent<AudioSource>().Pause();
            dogController.GetComponent<AudioSource>().Pause();
        }

        if(monster != null)
        {
            monster.GetComponent<AudioSource>().Pause();
        }
    }

    public void SoundUnPause()
    {
        if (girlController != null && dogController != null)
        {
            girlController.GetComponent<AudioSource>().UnPause();
            dogController.GetComponent<AudioSource>().UnPause();
        }

        if (monster != null)
        {
            monster.GetComponent<AudioSource>().UnPause();
        }
    }

    public void SoundStop()
    {
        if (girlController != null && dogController != null)
        {
            girlController.GetComponent<AudioSource>().Stop();
            dogController.GetComponent<AudioSource>().Stop();
        }
    }

    public void PauseAllSound()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in allAudioSources)
        {
            audio.Pause();
        }
    }

    public void UnpauseAllSound()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in allAudioSources)
        {
            audio.UnPause();
        }
    }

    public bool isHiding()
    {
        Hiding[] allHidingObjects = FindObjectsOfType<Hiding>();
        bool anyHidden = false;

        foreach (Hiding hiding in allHidingObjects)
        {
            if (hiding.IsHidden)
            {
                anyHidden = true;
                break;
            }
        }

        return anyHidden;
    }

    public bool isHidingT()
    {
        Teleport[] allHidingObjects = FindObjectsOfType<Teleport>();
        bool anyHidden = false;

        foreach (Teleport hiding in allHidingObjects)
        {
            if (hiding.IsHiding && hiding.canHide)
            {
                anyHidden = true;
                break;
            }
        }

        return anyHidden;
    }
}
