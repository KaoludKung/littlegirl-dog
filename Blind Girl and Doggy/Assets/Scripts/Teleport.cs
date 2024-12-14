using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, Interactable
{
    [SerializeField] private TeleportOption teleportOption;
    [SerializeField] private GameObject theGirl;
    [SerializeField] private GameObject theDog;
    [SerializeField] private int cameraID;
    [SerializeField] private Vector2 girl_TeleportPosition;
    [SerializeField] private Vector2 dog_TeleportPosition;

    [SerializeField] private AudioSource teleportSound;
    

    private GirlControl girlControl;
    private CameraSwitcher cameraSwitcher;

    // Start is called before the first frame update
    void Start()
    {
        girlControl = FindObjectOfType<GirlControl>();
        cameraSwitcher = FindObjectOfType<CameraSwitcher>();
    }

    public void Interact()
    {
        if (girlControl != null && !girlControl.IsMoving && !girlControl.IsBarking)
        {
            StartCoroutine(HandleTeleport());
        }
        else
        {
            Debug.Log("Cannot perform Action A while moving or barking.");
        }
    }

    private IEnumerator HandleTeleport()
    {

        if (teleportSound != null && teleportOption == TeleportOption.PlaySound)
        {
            teleportSound.Play();
            yield return new WaitForSeconds(1.5f); 
        }

  
        theGirl.transform.position = girl_TeleportPosition;
        theDog.transform.position = dog_TeleportPosition;

        cameraSwitcher.SwitchCamera(cameraID);
        Debug.Log("Performing Action Teleport");
    }

    enum TeleportOption
    {
        PlaySound,
        NoSound
    }
}
