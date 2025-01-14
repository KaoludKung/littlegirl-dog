using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spots : MonoBehaviour, Interactable
{
    [SerializeField] GameObject secretItem;
    [SerializeField] AudioClip digging;
    private DogController dogController;
    private bool isDigging;
  
    // Start is called before the first frame update
    void Start()
    {
        dogController = FindAnyObjectByType<DogController>();
    }

    public void Interact()
    {
        if (!isDigging)
        {
            isDigging = true;
            CharacterManager.Instance.SetIsActive(false);
            StartCoroutine(Digging());
        }
    }

    IEnumerator Digging()
    {
        dogController.Animator.SetBool("isDig", true);
        SoundFXManager.instance.PlaySoundFXClip(digging, transform, false, 1.0f);
        
        yield return new WaitForSeconds(3.0f);
        dogController.Animator.SetBool("isDig", false);
        CharacterManager.Instance.SetIsActive(true);
        secretItem.SetActive(true);
        Destroy(gameObject);
    }

}
